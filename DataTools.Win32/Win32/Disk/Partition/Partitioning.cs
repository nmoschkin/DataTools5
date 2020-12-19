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
using DataTools.Win32.Disk.Partition.Mbr;
using DataTools.Win32.Disk.Partition.Gpt;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Disk.Partition
{
    [SecurityCritical()]
    internal static class Partitioning
    {

        /// <summary>
        /// Windows system MBR partition information structure.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct PARTITION_INFORMATION_MBR
        {
            public byte PartitionType;
            [MarshalAs(UnmanagedType.Bool)]
            public bool BootIndicator;
            [MarshalAs(UnmanagedType.Bool)]
            public bool RecognizedPartition;
            public uint HiddenSectors;

            /// <summary>
            /// Returns a list of all partition types that were found for the current partition code.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PartitionCodeInfo[] PartitionCodes
            {
                get
                {
                    return PartitionCodeInfo.FindByCode(PartitionType);
                }
            }

            /// <summary>
            /// Returns the strongest partition type match for Windows NT from the available partition types for the current partition code.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PartitionCodeInfo PartitionCode
            {
                get
                {
                    var c = PartitionCodes;
                    if (c is object)
                    {
                        return c[0];
                    }

                    return null;
                }
            }

            public override string ToString()
            {
                var c = PartitionCodes;
                if (c is object)
                {
                    return c[0].Description;
                }

                return c[0].PartitionID.ToString();
            }
        }

        /// <summary>
        /// Windows system GPT partition information structure.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct PARTITION_INFORMATION_GPT
        {
            public Guid PartitionType;
            public Guid PartitionId;
            public GptPartitionAttributes Attributes;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 36)]
            private char[] _Name;

            /// <summary>
            /// Returns the name of this partition.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name
            {
                get
                {
                    return new string(_Name).Trim('\0');
                }
            }

            /// <summary>
            /// Returns the partition code information for this structure (if any).
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public GptCodeInfo PartitionCode
            {
                get
                {
                    return GptCodeInfo.FindByCode(PartitionType);
                }
            }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Name))
                    return Name;
                var c = PartitionCode;
                if (c is object)
                    return c.Name;
                return null;
            }
        }

        /// <summary>
        /// Contains extended partition information for any kind of disk device with a partition table.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct PARTITION_INFORMATION_EX
        {
            public PartitionStyle PartitionStyle;
            public long StartingOffset;
            public long PartitionLength;
            public uint PartitionNumber;
            [MarshalAs(UnmanagedType.Bool)]
            public bool RewritePartition;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 112)]
            private byte[] _PartitionInfo;

            /// <summary>
            /// Returns the Mbr structure or nothing if this partition is not an MBR partition.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PARTITION_INFORMATION_MBR Mbr
            {
                get
                {
                    if (PartitionStyle == PartitionStyle.Gpt)
                        return default;
                    SafePtr mm = (SafePtr)_PartitionInfo;
                    return mm.ToStruct<PARTITION_INFORMATION_MBR>();
                }
            }

            /// <summary>
            /// Returns the Gpt structure or nothing if this partition is not a GPT partition.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public PARTITION_INFORMATION_GPT Gpt
            {
                get
                {
                    if (PartitionStyle == PartitionStyle.Mbr)
                        return default;
                    SafePtr mm = (SafePtr)_PartitionInfo;
                    return mm.ToStruct<PARTITION_INFORMATION_GPT>();
                }
            }

            public override string ToString()
            {
                string ToStringRet = default;
                if (StartingOffset == 0L && PartitionLength == 0L)
                    return null;
                string fs = TextTools.PrintFriendlySize(PartitionLength);
                if (PartitionStyle == PartitionStyle.Mbr)
                {
                    ToStringRet = Mbr.ToString() + " [" + fs + "]";
                }
                else
                {
                    ToStringRet = Gpt.ToString() + " [" + fs + "]";
                }

                return ToStringRet;
            }
        }

        /// <summary>
        /// Drive layout information for an MBR partition table.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DRIVE_LAYOUT_INFORMATION_MBR
        {
            public uint Signature;
        }

        /// <summary>
        /// Drive layout information for a GPT partition table.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DRIVE_LAYOUT_INFORMATION_GPT
        {
            public Guid DiskId;
            public long StartingUsableOffset;
            public long UsableLength;
            public uint MaxPartitionCount;
        }

        /// <summary>
        /// Windows system disk drive partition layout information for any kind of disk.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DRIVE_LAYOUT_INFORMATION_EX
        {
            public PartitionStyle PartitionStyle;
            public uint ParititionCount;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
            private byte[] _LayoutInfo;

            public DRIVE_LAYOUT_INFORMATION_MBR Mbr
            {
                get
                {
                    SafePtr mm = (SafePtr)_LayoutInfo;
                    return mm.ToStruct<DRIVE_LAYOUT_INFORMATION_MBR>();
                }
            }

            public DRIVE_LAYOUT_INFORMATION_GPT Gpt
            {
                get
                {
                    SafePtr mm = (SafePtr)_LayoutInfo;
                    return mm.ToStruct<DRIVE_LAYOUT_INFORMATION_GPT>();
                }
            }
        }

        /// <summary>
        /// Enumerates all the partitions on a physical device.
        /// </summary>
        /// <param name="devicePath">The disk device path to query.</param>
        /// <param name="hfile">Optional valid disk handle.</param>
        /// <param name="layInfo">Optionally receives the layout information.</param>
        /// <returns>An array of PARTITION_INFORMATION_EX structures.</returns>
        /// <remarks></remarks>
        public static PARTITION_INFORMATION_EX[] GetPartitions(string devicePath, IntPtr hfile, ref DRIVE_LAYOUT_INFORMATION_EX layInfo)
        {
            bool hf = false;
            if (hfile != IntPtr.Zero)
            {
                hf = true;
            }
            else
            {
                hfile = IO.CreateFile(devicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, 0, IntPtr.Zero);
            }

            var pex = new MemPtr();
            var pexBegin = new MemPtr();
            PARTITION_INFORMATION_EX[] pOut = null;
            DRIVE_LAYOUT_INFORMATION_EX lay;
            int pexLen = Marshal.SizeOf<PARTITION_INFORMATION_EX>();
            int i;
            int c;
            uint cb = 0U;
            int sbs = 32768;
            bool succeed = false;
            if (hfile == DevProp.INVALID_HANDLE_VALUE)
                return null;
            do
            {
                pex.ReAlloc(sbs);
                succeed = NativeDisk.DeviceIoControl(hfile, NativeDisk.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, IntPtr.Zero, 0U, pex.Handle, (uint)pex.Length, ref cb, IntPtr.Zero);
                if (!succeed)
                {
                    int xErr = User32.GetLastError();
                    if (xErr != NativeDisk.ERROR_MORE_DATA & xErr != NativeDisk.ERROR_INSUFFICIENT_BUFFER)
                    {
                        string s = NativeError.Message;
                        sbs = -1;
                        break;
                    }
                }

                sbs *= 2;
            }
            while (!succeed);
            if (sbs == -1)
            {
                pex.Free();
                if (!hf)
                    User32.CloseHandle(hfile);
                return null;
            }

            lay = pex.ToStruct<DRIVE_LAYOUT_INFORMATION_EX>();
            pexBegin.Handle = pex.Handle + 48;
            c = (int)lay.ParititionCount;
            pOut = new PARTITION_INFORMATION_EX[c + 1];

            for (i = 0; i < c; i++)
            {
                pOut[i] = pexBegin.ToStruct<PARTITION_INFORMATION_EX>();
                pexBegin = pexBegin + pexLen;
            }

            pex.Free();
            if (!hf)
                User32.CloseHandle(hfile);
            layInfo = lay;
            return pOut;
        }
    }
}
