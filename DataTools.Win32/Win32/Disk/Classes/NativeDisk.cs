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
    [SecurityCritical()]
    internal static class NativeDisk
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int METHOD_BUFFERED = 0;
        public const int METHOD_IN_DIRECT = 1;
        public const int METHOD_OUT_DIRECT = 2;
        public const int METHOD_NEITHER = 3;
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
        public const int IOCTL_STORAGE_BASE = 0x2D;

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
                var c = ve.NumberOfExtents;
                for (i = 0; i < c; i++)
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
            file = IO.CreateFile(devicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
            if (file == DevProp.INVALID_HANDLE_VALUE)
            {
                return null;
            }

            uint arb = 0;

            inSize = Marshal.SizeOf(de) + Marshal.SizeOf(ve);
            inBuff.Length = inSize;
            r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, inBuff, (uint)inSize, ref arb, IntPtr.Zero);

            if (!r && User32.GetLastError() == ERROR_MORE_DATA)
            {
                inBuff.Length = inSize * inBuff.IntAt(0L);
                r = DeviceIoControl(file, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, inBuff, (uint)inSize, ref arb, IntPtr.Zero);
            }

            if (!r)
            {
                inBuff.Free();
                User32.CloseHandle(file);
                return null;
            }

            User32.CloseHandle(file);
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

            r = NativeDisk.GetVolumePathNamesForVolumeName(path, mm.Handle, cc, ref retc);
            if (!r)
                return null;
            if (retc > 1024)
            {
                mm.ReAlloc(retc);
                r = NativeDisk.GetVolumePathNamesForVolumeName(path, mm, retc, ref retc);
            }

            GetVolumePathsRet = mm.GetStringArray(0L);
            mm.Free();
            return GetVolumePathsRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}
