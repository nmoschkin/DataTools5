// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DiskApi
//         Native Disk Serivces.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using DataTools.Memory;
using DataTools.Text;
using DataTools.Hardware.Disk;
using Microsoft.VisualStudio.OLE.Interop;
using DataTools.Hardware.Disk.Partitions;
using DataTools.Hardware.Disk.Partitions.Mbr;
using DataTools.Hardware.Disk.Partitions.Gpt;

namespace DataTools.Hardware.Native
{
    [SecurityCritical()]
    internal static class DiskApi
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int METHOD_BUFFERED = 0;
        public const int METHOD_IN_DIRECT = 1;
        public const int METHOD_OUT_DIRECT = 2;
        public const int METHOD_NEITHER = 3;
        public const int IOCTL_STORAGE_BASE = 0x2D;
        public const int FILE_ANY_ACCESS = 0;
        public const int FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;
        public const int FILE_READ_ACCESS = 1;    // file & pipe
        public const int FILE_WRITE_ACCESS = 2;    // file & pipe

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CTL_CODE
        {
            public uint Value;

            public uint DeviceType
            {
                get
                {
                    return (uint)(Value & 0xFFFF0000L) >> 16;
                }
            }

            public uint Method
            {
                get
                {
                    return Value & 3;
                }
            }

            public CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
            {
                Value = DeviceType << 16 | Access << 14 | Function << 2 | Method;
            }

            public override string ToString()
            {
                return Value.ToString();
            }

            public static explicit operator CTL_CODE(uint operand)
            {
                CTL_CODE c;
                c.Value = operand;
                return c;
            }

            public static implicit operator uint(CTL_CODE operand)
            {
                return operand.Value;
            }
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public readonly static CTL_CODE IOCTL_STORAGE_CHECK_VERIFY = new CTL_CODE(IOCTL_STORAGE_BASE, 0x200U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_CHECK_VERIFY2 = new CTL_CODE(IOCTL_STORAGE_BASE, 0x200U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_MEDIA_REMOVAL = new CTL_CODE(IOCTL_STORAGE_BASE, 0x201U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_EJECT_MEDIA = new CTL_CODE(IOCTL_STORAGE_BASE, 0x202U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_LOAD_MEDIA = new CTL_CODE(IOCTL_STORAGE_BASE, 0x203U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_LOAD_MEDIA2 = new CTL_CODE(IOCTL_STORAGE_BASE, 0x203U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_RESERVE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x204U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_RELEASE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x205U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_FIND_NEW_DEVICES = new CTL_CODE(IOCTL_STORAGE_BASE, 0x206U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_EJECTION_CONTROL = new CTL_CODE(IOCTL_STORAGE_BASE, 0x250U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_MCN_CONTROL = new CTL_CODE(IOCTL_STORAGE_BASE, 0x251U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_MEDIA_TYPES = new CTL_CODE(IOCTL_STORAGE_BASE, 0x300U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_MEDIA_TYPES_EX = new CTL_CODE(IOCTL_STORAGE_BASE, 0x301U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_MEDIA_SERIAL_NUMBER = new CTL_CODE(IOCTL_STORAGE_BASE, 0x304U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_HOTPLUG_INFO = new CTL_CODE(IOCTL_STORAGE_BASE, 0x305U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_SET_HOTPLUG_INFO = new CTL_CODE(IOCTL_STORAGE_BASE, 0x306U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_RESET_BUS = new CTL_CODE(IOCTL_STORAGE_BASE, 0x400U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_RESET_DEVICE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x401U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_BREAK_RESERVATION = new CTL_CODE(IOCTL_STORAGE_BASE, 0x405U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_PERSISTENT_RESERVE_IN = new CTL_CODE(IOCTL_STORAGE_BASE, 0x406U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_PERSISTENT_RESERVE_OUT = new CTL_CODE(IOCTL_STORAGE_BASE, 0x407U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_DEVICE_NUMBER = new CTL_CODE(IOCTL_STORAGE_BASE, 0x420U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_PREDICT_FAILURE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x440U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_FAILURE_PREDICTION_CONFIG = new CTL_CODE(IOCTL_STORAGE_BASE, 0x441U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_READ_CAPACITY = new CTL_CODE(IOCTL_STORAGE_BASE, 0x450U, METHOD_BUFFERED, FILE_READ_ACCESS);

        //
        // IOCTLs &H0463 to &H0468 reserved for dependent disk support.
        //


        //
        // IOCTLs &H0470 to &H047f reserved for device and stack telemetry interfaces
        //

        public readonly static CTL_CODE IOCTL_STORAGE_GET_DEVICE_TELEMETRY = new CTL_CODE(IOCTL_STORAGE_BASE, 0x470U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_DEVICE_TELEMETRY_NOTIFY = new CTL_CODE(IOCTL_STORAGE_BASE, 0x471U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_DEVICE_TELEMETRY_QUERY_CAPS = new CTL_CODE(IOCTL_STORAGE_BASE, 0x472U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_DEVICE_TELEMETRY_RAW = new CTL_CODE(IOCTL_STORAGE_BASE, 0x473U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_QUERY_PROPERTY = new CTL_CODE(IOCTL_STORAGE_BASE, 0x500U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_MANAGE_DATA_SET_ATTRIBUTES = new CTL_CODE(IOCTL_STORAGE_BASE, 0x501U, METHOD_BUFFERED, FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_LB_PROVISIONING_MAP_RESOURCES = new CTL_CODE(IOCTL_STORAGE_BASE, 0x502U, METHOD_BUFFERED, FILE_READ_ACCESS);

        //
        // IOCTLs &H0503 to &H0580 reserved for Enhanced Storage devices.
        //


        //
        // IOCTLs for bandwidth contracts on storage devices
        // (Move this to ntddsfio if we decide to use a new base)
        //

        public readonly static CTL_CODE IOCTL_STORAGE_GET_BC_PROPERTIES = new CTL_CODE(IOCTL_STORAGE_BASE, 0x600U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_ALLOCATE_BC_STREAM = new CTL_CODE(IOCTL_STORAGE_BASE, 0x601U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_FREE_BC_STREAM = new CTL_CODE(IOCTL_STORAGE_BASE, 0x602U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);

        //
        // IOCTL to check for priority support
        //
        public readonly static CTL_CODE IOCTL_STORAGE_CHECK_PRIORITY_HINT_SUPPORT = new CTL_CODE(IOCTL_STORAGE_BASE, 0x620U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //
        // IOCTL for data integrity check support
        //

        public readonly static CTL_CODE IOCTL_STORAGE_START_DATA_INTEGRITY_CHECK = new CTL_CODE(IOCTL_STORAGE_BASE, 0x621U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_STOP_DATA_INTEGRITY_CHECK = new CTL_CODE(IOCTL_STORAGE_BASE, 0x622U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);

        // begin_winioctl

        //
        // IOCTLs &H0643 to &H0655 reserved for VHD disk support.
        //

        //
        // IOCTL to support Idle Power Management, including Device Wake
        //
        public readonly static CTL_CODE IOCTL_STORAGE_ENABLE_IDLE_POWER = new CTL_CODE(IOCTL_STORAGE_BASE, 0x720U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_GET_IDLE_POWERUP_REASON = new CTL_CODE(IOCTL_STORAGE_BASE, 0x721U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //
        // IOCTLs to allow class drivers to acquire and release active references on
        // a unit.  These should only be used if the class driver previously sent a
        // successful IOCTL_STORAGE_ENABLE_IDLE_POWER request to the port driver.
        //
        public readonly static CTL_CODE IOCTL_STORAGE_POWER_ACTIVE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x722U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_STORAGE_POWER_IDLE = new CTL_CODE(IOCTL_STORAGE_BASE, 0x723U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //
        // This IOCTL indicates that the physical device has triggered some sort of event.
        //
        public readonly static CTL_CODE IOCTL_STORAGE_EVENT_NOTIFICATION = new CTL_CODE(IOCTL_STORAGE_BASE, 0x724U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public const int IOCTL_VOLUME_BASE = 86; // Asc("V")
        public const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = IOCTL_VOLUME_BASE << 16 | FILE_ANY_ACCESS << 14 | 0 << 2 | METHOD_BUFFERED;


        //
        // IoControlCode values for disk devices.
        //

        public const int IOCTL_DISK_BASE = 7;
        public readonly static CTL_CODE IOCTL_DISK_GET_DRIVE_GEOMETRY = new CTL_CODE(IOCTL_DISK_BASE, 0x0U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_PARTITION_INFO = new CTL_CODE(IOCTL_DISK_BASE, 0x1U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SET_PARTITION_INFO = new CTL_CODE(IOCTL_DISK_BASE, 0x2U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_DRIVE_LAYOUT = new CTL_CODE(IOCTL_DISK_BASE, 0x3U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SET_DRIVE_LAYOUT = new CTL_CODE(IOCTL_DISK_BASE, 0x4U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_VERIFY = new CTL_CODE(IOCTL_DISK_BASE, 0x5U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_FORMAT_TRACKS = new CTL_CODE(IOCTL_DISK_BASE, 0x6U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_REASSIGN_BLOCKS = new CTL_CODE(IOCTL_DISK_BASE, 0x7U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_PERFORMANCE = new CTL_CODE(IOCTL_DISK_BASE, 0x8U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_IS_WRITABLE = new CTL_CODE(IOCTL_DISK_BASE, 0x9U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_LOGGING = new CTL_CODE(IOCTL_DISK_BASE, 0xAU, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_FORMAT_TRACKS_EX = new CTL_CODE(IOCTL_DISK_BASE, 0xBU, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_HISTOGRAM_STRUCTURE = new CTL_CODE(IOCTL_DISK_BASE, 0xCU, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_HISTOGRAM_DATA = new CTL_CODE(IOCTL_DISK_BASE, 0xDU, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_HISTOGRAM_RESET = new CTL_CODE(IOCTL_DISK_BASE, 0xEU, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_REQUEST_STRUCTURE = new CTL_CODE(IOCTL_DISK_BASE, 0xFU, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_REQUEST_DATA = new CTL_CODE(IOCTL_DISK_BASE, 0x10U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_PERFORMANCE_OFF = new CTL_CODE(IOCTL_DISK_BASE, 0x18U, METHOD_BUFFERED, FILE_ANY_ACCESS);



        //if(_WIN32_WINNT >= &H0400)
        public readonly static CTL_CODE IOCTL_DISK_CONTROLLER_NUMBER = new CTL_CODE(IOCTL_DISK_BASE, 0x11U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //
        // IOCTL support for SMART drive fault prediction.
        //

        public readonly static CTL_CODE SMART_GET_VERSION = new CTL_CODE(IOCTL_DISK_BASE, 0x20U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE SMART_SEND_DRIVE_COMMAND = new CTL_CODE(IOCTL_DISK_BASE, 0x21U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE SMART_RCV_DRIVE_DATA = new CTL_CODE(IOCTL_DISK_BASE, 0x22U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);

        //endif /* _WIN32_WINNT >= &H0400 */

        //if (_WIN32_WINNT >= &H500)

        //
        // New IOCTLs for GUID Partition tabled disks.
        //

        public readonly static CTL_CODE IOCTL_DISK_GET_PARTITION_INFO_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x12U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SET_PARTITION_INFO_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x13U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_DRIVE_LAYOUT_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x14U, METHOD_BUFFERED, FILE_ANY_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SET_DRIVE_LAYOUT_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x15U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_CREATE_DISK = new CTL_CODE(IOCTL_DISK_BASE, 0x16U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_LENGTH_INFO = new CTL_CODE(IOCTL_DISK_BASE, 0x17U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_DRIVE_GEOMETRY_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x28U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //endif /* _WIN32_WINNT >= &H0500 */


        //if (_WIN32_WINNT >= &H0502)

        //
        // New IOCTL for disk devices that support 8 byte LBA
        //
        public readonly static CTL_CODE IOCTL_DISK_REASSIGN_BLOCKS_EX = new CTL_CODE(IOCTL_DISK_BASE, 0x29U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);

        //End If ''_WIN32_WINNT >= &H0502

        //if(_WIN32_WINNT >= &H0500)
        public readonly static CTL_CODE IOCTL_DISK_UPDATE_DRIVE_SIZE = new CTL_CODE(IOCTL_DISK_BASE, 0x32U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GROW_PARTITION = new CTL_CODE(IOCTL_DISK_BASE, 0x34U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_CACHE_INFORMATION = new CTL_CODE(IOCTL_DISK_BASE, 0x35U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SET_CACHE_INFORMATION = new CTL_CODE(IOCTL_DISK_BASE, 0x36U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        //If (NTDDI_VERSION < NTDDI_WS03) Then
        public readonly static CTL_CODE IOCTL_DISK_GET_WRITE_CACHE_STATE = new CTL_CODE(IOCTL_DISK_BASE, 0x37U, METHOD_BUFFERED, FILE_READ_ACCESS);
        //Else
        public readonly static CTL_CODE OBSOLETE_DISK_GET_WRITE_CACHE_STATE = new CTL_CODE(IOCTL_DISK_BASE, 0x37U, METHOD_BUFFERED, FILE_READ_ACCESS);
        //End If
        public readonly static CTL_CODE IOCTL_DISK_DELETE_DRIVE_LAYOUT = new CTL_CODE(IOCTL_DISK_BASE, 0x40U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);

        //
        // Called to flush cached information that the driver may have about this
        // device's characteristics.  Not all drivers cache characteristics, and not
        // cached properties can be flushed.  This simply serves as an update to the
        // driver that it may want to do an expensive reexamination of the device's
        // characteristics now (fixed media size, partition table, etc...)
        //

        public readonly static CTL_CODE IOCTL_DISK_UPDATE_PROPERTIES = new CTL_CODE(IOCTL_DISK_BASE, 0x50U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //
        //  Special IOCTLs needed to support PC-98 machines in Japan
        //

        public readonly static CTL_CODE IOCTL_DISK_FORMAT_DRIVE = new CTL_CODE(IOCTL_DISK_BASE, 0xF3U, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_SENSE_DEVICE = new CTL_CODE(IOCTL_DISK_BASE, 0xF8U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //endif /* _WIN32_WINNT >= &H0500 */

        //
        // The following device control codes are common for all class drivers.  The
        // functions codes defined here must match all of the other class drivers.
        //
        // Warning: these codes will be replaced in the future by equivalent
        // IOCTL_STORAGE codes
        //

        public readonly static CTL_CODE IOCTL_DISK_CHECK_VERIFY = new CTL_CODE(IOCTL_DISK_BASE, 0x200U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_MEDIA_REMOVAL = new CTL_CODE(IOCTL_DISK_BASE, 0x201U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_EJECT_MEDIA = new CTL_CODE(IOCTL_DISK_BASE, 0x202U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_LOAD_MEDIA = new CTL_CODE(IOCTL_DISK_BASE, 0x203U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_RESERVE = new CTL_CODE(IOCTL_DISK_BASE, 0x204U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_RELEASE = new CTL_CODE(IOCTL_DISK_BASE, 0x205U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_FIND_NEW_DEVICES = new CTL_CODE(IOCTL_DISK_BASE, 0x206U, METHOD_BUFFERED, FILE_READ_ACCESS);
        public readonly static CTL_CODE IOCTL_DISK_GET_MEDIA_TYPES = new CTL_CODE(IOCTL_DISK_BASE, 0x300U, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Storage device number information.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STORAGE_DEVICE_NUMBER
        {
            public uint DeviceType;
            public uint DeviceNumber;
            public uint PartitionNumber;
        }

        [DllImport("kernel32.dll")]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, IntPtr lpOverlapped);
        [DllImport("kernel32.dll")]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref STORAGE_DEVICE_NUMBER lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, IntPtr lpOverlapped);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int ERROR_MORE_DATA = 234;
        public const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

        /// <summary>
        /// Describes a volume disk extent.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_EXTENT
        {
            [MarshalAs(UnmanagedType.I4)]
            public int DiskNumber;
            public int Space;
            [MarshalAs(UnmanagedType.I8)]
            public long StartingOffset;
            [MarshalAs(UnmanagedType.I8)]
            public long ExtentLength;
        }

        /// <summary>
        /// Describes volume disk extents.
        /// </summary>
        /// <remarks></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct VOLUME_DISK_EXTENTS
        {
            [MarshalAs(UnmanagedType.I4)]
            public int NumberOfExtents;
            public int Space;
            public DISK_EXTENT[] Extents;

            public static VOLUME_DISK_EXTENTS FromPtr(IntPtr ptr)
            {
                VOLUME_DISK_EXTENTS FromPtrRet = default;
                var ve = new VOLUME_DISK_EXTENTS();
                int cb = Marshal.SizeOf<DISK_EXTENT>();
                MemPtr m = ptr;
                int i;
                ve.NumberOfExtents = m.IntAt(0L);
                ve.Space = m.IntAt(1L);
                ve.Extents = new DISK_EXTENT[ve.NumberOfExtents];
                m = m + 8;
                var loopTo = ve.NumberOfExtents - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    ve.Extents[i] = m.ToStruct<DISK_EXTENT>();
                    m = m + cb;
                }

                FromPtrRet = ve;
                return FromPtrRet;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationW")]

        public static extern bool GetVolumeInformation([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName, IntPtr lpVolumeNameBuffer, int nVolumeNameSize, ref uint lpVolumeSerialNumber, ref int lpMaximumComponentLength, ref FileSystemFlags lpFileSystemFlags, IntPtr lpFileSystemNameBuffer, int nFileSystemNameSize);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationByHandleW")]

        public static extern bool GetVolumeInformationByHandle(IntPtr hFile, IntPtr lpVolumeNameBuffer, int nVolumeNameSize, ref uint lpVolumeSerialNumber, ref int lpMaximumComponentLength, ref FileSystemFlags lpFileSystemFlags, IntPtr lpFileSystemNameBuffer, int nFileSystemNameSize);
        [DllImport("kernel32.dll", EntryPoint = "GetVolumePathNamesForVolumeNameW")]

        public static extern bool GetVolumePathNamesForVolumeName([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, IntPtr lpszVolumePathNames, int cchBufferLength, ref int lpcchReturnLength);


        /// <summary>
        /// Get volume disk extents for volumes that may or may not span more than one physical drive.
        /// </summary>
        /// <param name="devicePath">The device path of the volume.</param>
        /// <returns>An array of DiskExtent structures.</returns>
        /// <remarks></remarks>
        public static DiskExtent[] GetDiskExtentsFor(string devicePath)
        {
            DiskExtent[] deOut = null;
            MemPtr inBuff = new MemPtr();
            int inSize;
            IntPtr file;
            int h = 0;
            var de = new DISK_EXTENT();
            var ve = new VOLUME_DISK_EXTENTS();
            bool r;
            file = FileApi.CreateFile(devicePath, FileApi.GENERIC_READ, FileApi.FILE_SHARE_READ | FileApi.FILE_SHARE_WRITE, IntPtr.Zero, FileApi.OPEN_EXISTING, FileApi.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
            if (file == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            uint arb = 0;

            inSize = Marshal.SizeOf(de) + Marshal.SizeOf(ve);
            inBuff.Length = inSize;
            r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, inBuff, (uint)inSize, ref arb, IntPtr.Zero);

            if (!r && PInvoke.GetLastError() == ERROR_MORE_DATA)
            {
                inBuff.Length = inSize * inBuff.IntAt(0L);
                r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, inBuff, (uint)inSize, ref arb, IntPtr.Zero);
            }

            if (!r)
            {
                inBuff.Free();
                PInvoke.CloseHandle(file);
                return null;
            }

            PInvoke.CloseHandle(file);
            ve = VOLUME_DISK_EXTENTS.FromPtr(inBuff);
            inBuff.Free();
            h = 0;
            deOut = new DiskExtent[ve.Extents.Length];
            foreach (var currentDe in ve.Extents)
            {
                de = currentDe;
                deOut[h].PhysicalDevice = de.DiskNumber;
                deOut[h].Space = de.Space;
                deOut[h].Size = de.ExtentLength;
                deOut[h].Offset = de.StartingOffset;
                h += 1;
            }

            return deOut;
        }

        /// <summary>
        /// Populates a DiskDeviceInfo object with extended volume information.
        /// </summary>
        /// <param name="disk">The disk device object to populate.</param>
        /// <param name="handle">Optional handle to an open disk.</param>
        /// <remarks></remarks>
        public static void PopulateVolumeInfo(DiskDeviceInfo disk, IntPtr handle = default)
        {
            int pLen = (FileApi.MAX_PATH + 1) * 2;
            
            MemPtr mm1 = new MemPtr();
            MemPtr mm2 = new MemPtr();

            int mc = 0;
            
            mm1.Alloc(pLen);
            mm2.Alloc(pLen);
            
            mm1.ZeroMemory();
            mm2.ZeroMemory();
            
            string pp = new string('\0', 1024);
            FileApi.GetVolumeNameForVolumeMountPoint(disk.DevicePath + @"\", mm1, 1024U);

            // just get rid of the extra nulls (they like to stick around).

            disk.Type = StorageType.Volume;
            disk.VolumeGuidPath = (string)mm1;
            disk.VolumePaths = GetVolumePaths((string)mm1);

            mm1.ZeroMemory();

            if (handle == IntPtr.Zero || handle == DevProp.INVALID_HANDLE_VALUE)
            {
                string arglpRootPathName = disk.VolumeGuidPath;
                uint arglpVolumeSerialNumber = disk.SerialNumber;
                var arglpFileSystemFlags = disk.VolumeFlags;

                DiskApi.GetVolumeInformation(arglpRootPathName, mm1, pLen, ref arglpVolumeSerialNumber, ref mc, ref arglpFileSystemFlags, mm2, pLen);

                disk.VolumeGuidPath = arglpRootPathName;
                disk.SerialNumber = arglpVolumeSerialNumber;
                disk.VolumeFlags = arglpFileSystemFlags;
            }
            else
            {
                uint arglpVolumeSerialNumber1 = disk.SerialNumber;
                var arglpFileSystemFlags1 = disk.VolumeFlags;

                GetVolumeInformationByHandle(handle, mm1, pLen, ref arglpVolumeSerialNumber1, ref mc, ref arglpFileSystemFlags1, mm2, pLen);

                disk.SerialNumber = arglpVolumeSerialNumber1;
                disk.VolumeFlags = arglpFileSystemFlags1;
            }

            disk.DiskExtents = GetDiskExtentsFor(disk.DevicePath);
            disk.FriendlyName = (string)mm1;
            disk.FileSystem = (string)mm2;

            var a = default(int);
            var b = default(int);
            var c = default(int);
            var d = default(int);

            string arglpRootPathName1 = disk.VolumeGuidPath;

            FileApi.GetDiskFreeSpace(arglpRootPathName1, ref a, ref b, ref c, ref d);

            disk.VolumeGuidPath = arglpRootPathName1;
            disk.SectorSize = b;

            mm1.Free();
            mm2.Free();
        }

        /// <summary>
        /// Get all mount points for a volume.
        /// </summary>
        /// <param name="path">Volume Guid Path.</param>
        /// <returns>An array of strings that represent mount points.</returns>
        /// <remarks></remarks>
        public static string[] GetVolumePaths(string path)
        {
            string[] GetVolumePathsRet = default;
            int cc = 1024;
            int retc = 0;
            var mm = new MemPtr();
            bool r;
            mm.Alloc(cc);

            r = DiskApi.GetVolumePathNamesForVolumeName(path, mm.Handle, cc, ref retc);
            if (!r)
                return null;
            if (retc > 1024)
            {
                mm.ReAlloc(retc);
                r = DiskApi.GetVolumePathNamesForVolumeName(path, mm, retc, ref retc);
            }

            GetVolumePathsRet = mm.GetStringArray(0L);
            mm.Free();
            return GetVolumePathsRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

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
            DiskApi.DeviceIoControl(hfile, DiskApi.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, 0, mm.Handle, l, ref cb, IntPtr.Zero);
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
        public static bool ReadRawGptDisk(DiskDeviceInfo inf, ref RAW_GPT_DISK gptInfo)
        {


            // Demand Administrator for accessing a raw disk.
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            // Dim principalPerm As New PrincipalPermission(Nothing, "Administrators")
            // principalPerm.Demand()


            var hfile = FileApi.CreateFile(inf.DevicePath, FileApi.GENERIC_READ | FileApi.GENERIC_WRITE, FileApi.FILE_SHARE_READ | FileApi.FILE_SHARE_WRITE, IntPtr.Zero, FileApi.OPEN_EXISTING, FileApi.FILE_FLAG_NO_BUFFERING | FileApi.FILE_FLAG_RANDOM_ACCESS, IntPtr.Zero);
            if (hfile == DevProp.INVALID_HANDLE_VALUE)
                return false;
            DISK_GEOMETRY_EX geo = default;

            // get the disk geometry to retrieve the sector (LBA) size.
            if (!DiskGeometry(hfile, ref geo))
            {
                PInvoke.CloseHandle(hfile);
                return false;
            }

            // sector size (usually 512 bytes)
            uint bps = geo.Geometry.BytesPerSector;
            uint br = 0U;
            long lp = 0L;
            long lp2 = 0L;
            var mm = new MemPtr(bps * 2L);
            FileApi.SetFilePointerEx(hfile, 0L, ref lp, FileApi.FilePointerMoveMethod.Begin);
            FileApi.ReadFile(hfile, mm.Handle, bps * 2, ref br, IntPtr.Zero);
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
                FileApi.SetFilePointerEx(hfile, (uint)(bps * gpt.PartitionEntryLBA), ref lr, FileApi.FilePointerMoveMethod.Begin);
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
                FileApi.ReadFile(hfile, mm.Handle, (uint)lp, ref br, IntPtr.Zero);

                // check the partition table CRC.
                if (mm.CalculateCrc32() == gpt.PartitionArrayCRC32)
                {
                    // disk is valid.

                    lp = (uint)Marshal.SizeOf<RAW_GPT_PARTITION>();
                    br = 0U;
                    int i;
                    int c = (int)gpt.NumberOfPartitions - 1;
                    gpp = new RAW_GPT_PARTITION[c + 1];

                    // populate the drive information.
                    var loopTo = c;
                    for (i = 0; i <= loopTo; i++)
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
            PInvoke.CloseHandle(hfile);

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
                hfile = FileApi.CreateFile(devicePath, FileApi.GENERIC_READ, FileApi.FILE_SHARE_READ | FileApi.FILE_SHARE_WRITE, IntPtr.Zero, FileApi.OPEN_EXISTING, 0, IntPtr.Zero);
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
                succeed = DiskApi.DeviceIoControl(hfile, DiskApi.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, IntPtr.Zero, 0U, pex.Handle, (uint)pex.Length, ref cb, IntPtr.Zero);
                if (!succeed)
                {
                    int xErr = PInvoke.GetLastError();
                    if (xErr != DiskApi.ERROR_MORE_DATA & xErr != DiskApi.ERROR_INSUFFICIENT_BUFFER)
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
                    PInvoke.CloseHandle(hfile);
                return null;
            }

            lay = pex.ToStruct<DRIVE_LAYOUT_INFORMATION_EX>();
            pexBegin.Handle = pex.Handle + 48;
            c = (int)lay.ParititionCount - 1;
            pOut = new PARTITION_INFORMATION_EX[c + 1];
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                pOut[i] = pexBegin.ToStruct<PARTITION_INFORMATION_EX>();
                pexBegin = pexBegin + pexLen;
            }

            pex.Free();
            if (!hf)
                PInvoke.CloseHandle(hfile);
            layInfo = lay;
            return pOut;
        }
    }
}