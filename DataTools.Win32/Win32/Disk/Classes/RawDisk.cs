// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DiskApi
//         Native Disk Serivces.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using DataTools.Text;
using DataTools.Win32.Disk.Partition;
using DataTools.Win32.Disk.Partition.Mbr;
using DataTools.Win32.Disk.Partition.Gpt;
using DataTools.Win32.Memory;

namespace DataTools.Win32
{
    /// <summary>
    /// Raw disk access.  Currently not exposed.
    /// </summary>
    /// <remarks></remarks>
    [SecurityCritical()]
    internal static class RawDisk
    {

        /// <summary>
        /// Disk Geometry Media Types
        /// </summary>
        /// <remarks></remarks>
        public enum MEDIA_TYPE
        {
            Unknown = 0x0,
            F5_1Pt2_512 = 0x1,
            F3_1Pt44_512 = 0x2,
            F3_2Pt88_512 = 0x3,
            F3_20Pt8_512 = 0x4,
            F3_720_512 = 0x5,
            F5_360_512 = 0x6,
            F5_320_512 = 0x7,
            F5_320_1024 = 0x8,
            F5_180_512 = 0x9,
            F5_160_512 = 0xA,
            RemovableMedia = 0xB,
            FixedMedia = 0xC,
            F3_120M_512 = 0xD,
            F3_640_512 = 0xE,
            F5_640_512 = 0xF,
            F5_720_512 = 0x10,
            F3_1Pt2_512 = 0x11,
            F3_1Pt23_1024 = 0x12,
            F5_1Pt23_1024 = 0x13,
            F3_128Mb_512 = 0x14,
            F3_230Mb_512 = 0x15,
            F8_256_128 = 0x16,
            F3_200Mb_512 = 0x17,
            F3_240M_512 = 0x18,
            F3_32M_512 = 0x19
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY
        {
            public long Cylinders;
            public MEDIA_TYPE MediaType;
            public uint TracksPerCylinder;
            public uint SectorsPerTrack;
            public uint BytesPerSector;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY_EX
        {
            public DISK_GEOMETRY Geometry;
            public ulong DiskSize;
            public byte Data;
        }

        /// <summary>
        /// Retrieves the disk geometry of the specified disk.
        /// </summary>
        /// <param name="hfile">Handle to a valid, open disk.</param>
        /// <param name="geo">Receives the disk geometry information.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool DiskGeometry(IntPtr hfile, ref DISK_GEOMETRY_EX geo)
        {
            if (hfile == DevProp.INVALID_HANDLE_VALUE)
                return false;
            MemPtr mm = new MemPtr();
            uint l = 0U;
            uint cb = 0U;
            l = (uint)Marshal.SizeOf<DISK_GEOMETRY_EX>();
            mm.Alloc(l);
            NativeDisk.DeviceIoControl(hfile, NativeDisk.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, 0, mm.Handle, l, ref cb, IntPtr.Zero);
            geo = mm.ToStruct<DISK_GEOMETRY_EX>();
            mm.Free();
            return true;
        }

        /// <summary>
        /// Retrieve the partition table of a GPT-layout disk, manually.
        /// Must be Administrator.
        /// </summary>
        /// <param name="inf">DiskDeviceInfo object to the physical disk to read.</param>
        /// <param name="gptInfo">Receives the drive layout information.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public static bool ReadRawGptDisk(string devicePath, ref RAW_GPT_DISK gptInfo)
        {


            // Demand Administrator for accessing a raw disk.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            // Dim principalPerm As New PrincipalPermission(Nothing, "Administrators")
            // principalPerm.Demand()


            var hfile = IO.CreateFile(devicePath, IO.GENERIC_READ | IO.GENERIC_WRITE, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_FLAG_NO_BUFFERING | IO.FILE_FLAG_RANDOM_ACCESS, IntPtr.Zero);
            if (hfile == DevProp.INVALID_HANDLE_VALUE)
                return false;
            DISK_GEOMETRY_EX geo = default;

            // get the disk geometry to retrieve the sector (LBA) size.
            if (!DiskGeometry(hfile, ref geo))
            {
                User32.CloseHandle(hfile);
                return false;
            }

            // sector size (usually 512 bytes)
            uint bps = geo.Geometry.BytesPerSector;
            uint br = 0U;
            long lp = 0L;
            long lp2 = 0L;
            var mm = new MemPtr(bps * 2L);
            IO.SetFilePointerEx(hfile, 0L, ref lp, IO.FilePointerMoveMethod.Begin);
            IO.ReadFile(hfile, mm.Handle, bps * 2, ref br, IntPtr.Zero);
            var mbr = new RAW_MBR();
            var gpt = new RAW_GPT_HEADER();
            RAW_GPT_PARTITION[] gpp = null;

            // read the master boot record.
            mbr = mm.ToStructAt<RAW_MBR>(446L);

            // read the GPT structure header.
            gpt = mm.ToStructAt<RAW_GPT_HEADER>(bps);

            // check the partition header CRC.
            if (gpt.IsValid)
            {
                long lr = br;

                // seek to the LBA of the partition information.
                IO.SetFilePointerEx(hfile, (uint)(bps * gpt.PartitionEntryLBA), ref lr, IO.FilePointerMoveMethod.Begin);
                br = (uint)lr;

                // calculate the size of the partition table buffer.
                lp = gpt.NumberOfPartitions * gpt.PartitionEntryLength;

                // byte align to the sector size.
                if (lp % bps != 0L)
                {
                    lp += bps - lp % bps;
                }

                // bump up the memory pointer.
                mm.ReAlloc(lp);
                mm.ZeroMemory();

                // read the partition information into the pointer.
                IO.ReadFile(hfile, mm.Handle, (uint)lp, ref br, IntPtr.Zero);

                // check the partition table CRC.
                if (mm.CalculateCrc32() == gpt.PartitionArrayCRC32)
                {
                    // disk is valid.

                    lp = (uint)Marshal.SizeOf<RAW_GPT_PARTITION>();
                    br = 0U;
                    int i;
                    int c = (int)gpt.NumberOfPartitions;

                    gpp = new RAW_GPT_PARTITION[c + 1];

                    // populate the drive information.
                    for (i = 0; i < c; i++)
                    {
                        gpp[i] = mm.ToStructAt<RAW_GPT_PARTITION>(lp2);

                        // break on empty GUID, we are past the last partition.
                        if (gpp[i].PartitionTypeGuid == Guid.Empty)
                            break;
                        lp2 += lp;
                    }

                    // trim off excess records from the array.
                    if (i < c)
                    {
                        if (i == 0)
                        {
                            gpp = Array.Empty<RAW_GPT_PARTITION>();
                        }
                        else
                        {
                            Array.Resize(ref gpp, i);
                        }
                    }
                }
            }

            // free the resources.
            mm.Free();
            User32.CloseHandle(hfile);

            // if gpp is nothing then some error occurred somewhere and we did not succeed.
            if (gpp is null)
                return false;

            // create a new RAW_GPT_DISK structure.
            gptInfo = new RAW_GPT_DISK();
            gptInfo.Header = gpt;
            gptInfo.Partitions = gpp;

            // we have succeeded.
            return true;
        }

        /// <summary>
        /// Raw disk Master Boot Record entry.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAW_MBR
        {
            public byte BootIndicator;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 3)]
            private byte[] _startingChs;
            public byte PartitionType;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 3)]
            private byte[] _endingChs;
            public uint StartingLBA;
            public uint SizeInLBA;

            public int StartingCHS
            {
                get
                {
                    int StartingCHSRet = default;
                    var mm = new MemPtr(4L);
                    Marshal.Copy(_startingChs, 0, mm.Handle, 3);
                    StartingCHSRet = mm.IntAt(0L);
                    mm.Free();
                    return StartingCHSRet;
                }
            }

            public int EndingCHS
            {
                get
                {
                    int EndingCHSRet = default;
                    var mm = new MemPtr(4L);
                    Marshal.Copy(_endingChs, 0, mm.Handle, 3);
                    EndingCHSRet = mm.IntAt(0L);
                    mm.Free();
                    return EndingCHSRet;
                }
            }
        }

        /// <summary>
        /// Raw disk GPT partition table header.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAW_GPT_HEADER
        {
            public ulong Signature;
            public uint Revision;
            public uint HeaderSize;
            public uint HeaderCRC32;
            public uint Reserved;
            public ulong MyLBA;
            public ulong AlternateLBA;
            public ulong FirstUsableLBA;
            public ulong MaxUsableLBA;
            public Guid DiskGuid;
            public ulong PartitionEntryLBA;
            public uint NumberOfPartitions;
            public uint PartitionEntryLength;
            public uint PartitionArrayCRC32;

            /// <summary>
            /// True if this structure contains a CRC-32 valid GPT partition header.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool IsValid
            {
                get
                {
                    bool IsValidRet = default;
                    IsValidRet = Validate();
                    return IsValidRet;
                }
            }

            /// <summary>
            /// Validate the header and CRC-32 of this structure.
            /// </summary>
            /// <returns>True if the structure is valid.</returns>
            /// <remarks></remarks>
            public bool Validate()
            {
                bool ValidateRet = default;
                var mm = new MemPtr();
                mm.FromStruct(this);
                mm.UIntAt(4L) = 0U;

                // validate the crc and the signature moniker 
                ValidateRet = HeaderCRC32 == mm.CalculateCrc32() && Signature == 0x5452415020494645;
                mm.Free();
                return ValidateRet;
            }
        }

        /// <summary>
        /// Raw GPT partition information.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RAW_GPT_PARTITION
        {
            public Guid PartitionTypeGuid;
            public Guid UniquePartitionGuid;
            public ulong StartingLBA;
            public ulong EndingLBA;
            public GptPartitionAttributes Attributes;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 36)]
            private char[] _Name;

            /// <summary>
            /// Returns the name of this partition (if any).
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return (new string(_Name)).Trim('\0');
                }
            }

            /// <summary>
            /// Retrieve the partition code information for this partition type (if any).
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public GptCodeInfo PartitionCode
            {
                get
                {
                    return GptCodeInfo.FindByCode(PartitionTypeGuid);
                }
            }

            /// <summary>
            /// Converts this object into its string representation.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public override string ToString()
            {
                string ToStringRet = default;
                if (!string.IsNullOrEmpty(Name))
                {
                    ToStringRet = Name;
                }
                else if (PartitionCode is object)
                {
                    ToStringRet = PartitionCode.ToString();
                }
                else
                {
                    ToStringRet = UniquePartitionGuid.ToString("B");
                }

                return ToStringRet;
            }
        }

        /// <summary>
        /// Contains an entire raw GPT disk layout.
        /// </summary>
        /// <remarks></remarks>
        public struct RAW_GPT_DISK
        {
            public RAW_GPT_HEADER Header;
            public RAW_GPT_PARTITION[] Partitions;
        }
    }
}
