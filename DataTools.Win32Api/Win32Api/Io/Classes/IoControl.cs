using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.ApplicationServices;

namespace DataTools.Win32Api
{
    internal static class IoControl
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int FILE_DEVICE_BEEP = 0x1;
        public const int FILE_DEVICE_CD_ROM = 0x2;
        public const int FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x3;
        public const int FILE_DEVICE_CONTROLLER = 0x4;
        public const int FILE_DEVICE_DATALINK = 0x5;
        public const int FILE_DEVICE_DFS = 0x6;
        public const int FILE_DEVICE_DISK = 0x7;
        public const int FILE_DEVICE_DISK_FILE_SYSTEM = 0x8;
        public const int FILE_DEVICE_FILE_SYSTEM = 0x9;
        public const int FILE_DEVICE_INPORT_PORT = 0xA;
        public const int FILE_DEVICE_KEYBOARD = 0xB;
        public const int FILE_DEVICE_MAILSLOT = 0xC;
        public const int FILE_DEVICE_MIDI_IN = 0xD;
        public const int FILE_DEVICE_MIDI_OUT = 0xE;
        public const int FILE_DEVICE_MOUSE = 0xF;
        public const int FILE_DEVICE_MULTI_UNC_PROVIDER = 0x10;
        public const int FILE_DEVICE_NAMED_PIPE = 0x11;
        public const int FILE_DEVICE_NETWORK = 0x12;
        public const int FILE_DEVICE_NETWORK_BROWSER = 0x13;
        public const int FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x14;
        public const int FILE_DEVICE_NULL = 0x15;
        public const int FILE_DEVICE_PARALLEL_PORT = 0x16;
        public const int FILE_DEVICE_PHYSICAL_NETCARD = 0x17;
        public const int FILE_DEVICE_PRINTER = 0x18;
        public const int FILE_DEVICE_SCANNER = 0x19;
        public const int FILE_DEVICE_SERIAL_MOUSE_PORT = 0x1A;
        public const int FILE_DEVICE_SERIAL_PORT = 0x1B;
        public const int FILE_DEVICE_SCREEN = 0x1C;
        public const int FILE_DEVICE_SOUND = 0x1D;
        public const int FILE_DEVICE_STREAMS = 0x1E;
        public const int FILE_DEVICE_TAPE = 0x1F;
        public const int FILE_DEVICE_TAPE_FILE_SYSTEM = 0x20;
        public const int FILE_DEVICE_TRANSPORT = 0x21;
        public const int FILE_DEVICE_UNKNOWN = 0x22;
        public const int FILE_DEVICE_VIDEO = 0x23;
        public const int FILE_DEVICE_VIRTUAL_DISK = 0x24;
        public const int FILE_DEVICE_WAVE_IN = 0x25;
        public const int FILE_DEVICE_WAVE_OUT = 0x26;
        public const int FILE_DEVICE_8042_PORT = 0x27;
        public const int FILE_DEVICE_NETWORK_REDIRECTOR = 0x28;
        public const int FILE_DEVICE_BATTERY = 0x29;
        public const int FILE_DEVICE_BUS_EXTENDER = 0x2A;
        public const int FILE_DEVICE_MODEM = 0x2B;
        public const int FILE_DEVICE_VDM = 0x2C;
        public const int FILE_DEVICE_MASS_STORAGE = 0x2D;
        public const int FILE_DEVICE_SMB = 0x2E;
        public const int FILE_DEVICE_KS = 0x2F;
        public const int FILE_DEVICE_CHANGER = 0x30;
        public const int FILE_DEVICE_SMARTCARD = 0x31;
        public const int FILE_DEVICE_ACPI = 0x32;
        public const int FILE_DEVICE_DVD = 0x33;
        public const int FILE_DEVICE_FULLSCREEN_VIDEO = 0x34;
        public const int FILE_DEVICE_DFS_FILE_SYSTEM = 0x35;
        public const int FILE_DEVICE_DFS_VOLUME = 0x36;
        public const int FILE_DEVICE_SERENUM = 0x37;
        public const int FILE_DEVICE_TERMSRV = 0x38;
        public const int FILE_DEVICE_KSEC = 0x39;
        public const int FILE_DEVICE_FIPS = 0x3A;
        public const int FILE_DEVICE_INFINIBAND = 0x3B;
        public const int FILE_DEVICE_VMBUS = 0x3E;
        public const int FILE_DEVICE_CRYPT_PROVIDER = 0x3F;
        public const int FILE_DEVICE_WPD = 0x40;
        public const int FILE_DEVICE_BLUETOOTH = 0x41;
        public const int FILE_DEVICE_MT_COMPOSITE = 0x42;
        public const int FILE_DEVICE_MT_TRANSPORT = 0x43;
        public const int FILE_DEVICE_BIOMETRIC = 0x44;
        public const int FILE_DEVICE_PMI = 0x45;
        public const int FILE_DEVICE_EHSTOR = 0x46;
        public const int FILE_DEVICE_DEVAPI = 0x47;
        public const int FILE_DEVICE_GPIO = 0x48;
        public const int FILE_DEVICE_USBEX = 0x49;
        public const int FILE_DEVICE_CONSOLE = 0x50;
        public const int FILE_DEVICE_NFP = 0x51;
        public const int FILE_DEVICE_SYSENV = 0x52;
        public const int FILE_DEVICE_VIRTUAL_BLOCK = 0x53;
        public const int FILE_DEVICE_POINT_OF_SERVICE = 0x54;
        public const int FILE_DEVICE_AVIO = 0x99;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        //
        // The following is a list of the native file system fsctls followed by
        // additional network file system fsctls.  Some values have been
        // decommissioned.
        //

        public readonly static CTL_CODE FSCTL_REQUEST_OPLOCK_LEVEL_1 = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 0U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_REQUEST_OPLOCK_LEVEL_2 = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 1U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_REQUEST_BATCH_OPLOCK = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 2U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_OPLOCK_BREAK_ACKNOWLEDGE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 3U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_OPBATCH_ACK_CLOSE_PENDING = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 4U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_OPLOCK_BREAK_NOTIFY = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 5U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_LOCK_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 6U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_UNLOCK_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 7U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_DISMOUNT_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 8U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                              9
        public readonly static CTL_CODE FSCTL_IS_VOLUME_MOUNTED = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 10U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_IS_PATHNAME_VALID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 11U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // PATHNAME_BUFFER,
        public readonly static CTL_CODE FSCTL_MARK_VOLUME_DIRTY = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 12U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             13
        public readonly static CTL_CODE FSCTL_QUERY_RETRIEVAL_POINTERS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 14U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_GET_COMPRESSION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 15U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_SET_COMPRESSION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 16U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA | IO.FILE_WRITE_DATA);

        // decommissioned fsctl value                                             17
        // decommissioned fsctl value                                             18
        public readonly static CTL_CODE FSCTL_SET_BOOTLOADER_ACCESSED = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 19U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);
        public static CTL_CODE FSCTL_MARK_AS_SYSTEM_HIVE = FSCTL_SET_BOOTLOADER_ACCESSED;
        public readonly static CTL_CODE FSCTL_OPLOCK_BREAK_ACK_NO_2 = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 20U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_INVALIDATE_VOLUMES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 21U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_QUERY_FAT_BPB = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 22U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // FSCTL_QUERY_FAT_BPB_BUFFER
        public readonly static CTL_CODE FSCTL_REQUEST_FILTER_OPLOCK = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 23U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_FILESYSTEM_GET_STATISTICS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 24U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // FILESYSTEM_STATISTICS

        // if  (_WIN32_WINNT >= _WIN32_WINNT_NT4)
        public readonly static CTL_CODE FSCTL_GET_NTFS_VOLUME_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 25U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // NTFS_VOLUME_DATA_BUFFER
        public readonly static CTL_CODE FSCTL_GET_NTFS_FILE_RECORD = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 26U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // NTFS_FILE_RECORD_INPUT_BUFFER, NTFS_FILE_RECORD_OUTPUT_BUFFER
        public readonly static CTL_CODE FSCTL_GET_VOLUME_BITMAP = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 27U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // STARTING_LCN_INPUT_BUFFER, VOLUME_BITMAP_BUFFER
        public readonly static CTL_CODE FSCTL_GET_RETRIEVAL_POINTERS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 28U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // STARTING_VCN_INPUT_BUFFER, RETRIEVAL_POINTERS_BUFFER
        public readonly static CTL_CODE FSCTL_MOVE_FILE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 29U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // MOVE_FILE_DATA,
        public readonly static CTL_CODE FSCTL_IS_VOLUME_DIRTY = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 30U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             31
        public readonly static CTL_CODE FSCTL_ALLOW_EXTENDED_DASD_IO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 32U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_NT4 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
        // decommissioned fsctl value                                             33
        // decommissioned fsctl value                                             34
        public readonly static CTL_CODE FSCTL_FIND_FILES_BY_SID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 35U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             36
        // decommissioned fsctl value                                             37
        public readonly static CTL_CODE FSCTL_SET_OBJECT_ID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 38U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static CTL_CODE FSCTL_GET_OBJECT_ID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 39U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static CTL_CODE FSCTL_DELETE_OBJECT_ID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 40U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE FSCTL_SET_REPARSE_POINT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 41U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER,
        public readonly static CTL_CODE FSCTL_GET_REPARSE_POINT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 42U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // REPARSE_DATA_BUFFER
        public readonly static CTL_CODE FSCTL_DELETE_REPARSE_POINT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 43U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER,
        public readonly static CTL_CODE FSCTL_ENUM_USN_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 44U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // MFT_ENUM_DATA,
        public readonly static CTL_CODE FSCTL_SECURITY_ID_CHECK = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 45U, IO.METHOD_NEITHER, IO.FILE_READ_DATA);  // BULK_SECURITY_TEST_DATA,
        public readonly static CTL_CODE FSCTL_READ_USN_JOURNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 46U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // READ_USN_JOURNAL_DATA, USN
        public readonly static CTL_CODE FSCTL_SET_OBJECT_ID_EXTENDED = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 47U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE FSCTL_CREATE_OR_GET_OBJECT_ID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 48U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // FILE_OBJECTID_BUFFER
        public readonly static CTL_CODE FSCTL_SET_SPARSE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 49U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE FSCTL_SET_ZERO_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 50U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // FILE_ZERO_DATA_INFORMATION,
        public readonly static CTL_CODE FSCTL_QUERY_ALLOCATED_RANGES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 51U, IO.METHOD_NEITHER, IO.FILE_READ_DATA);  // FILE_ALLOCATED_RANGE_BUFFER, FileApi.FILE_ALLOCATED_RANGE_BUFFER
        public readonly static CTL_CODE FSCTL_ENABLE_UPGRADE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 52U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA);
        public readonly static CTL_CODE FSCTL_SET_ENCRYPTION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 53U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // ENCRYPTION_BUFFER, DECRYPTION_STATUS_BUFFER
        public readonly static CTL_CODE FSCTL_ENCRYPTION_FSCTL_IO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 54U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_WRITE_RAW_ENCRYPTED = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 55U, IO.METHOD_NEITHER, IO.FILE_SPECIAL_ACCESS); // ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO
        public readonly static CTL_CODE FSCTL_READ_RAW_ENCRYPTED = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 56U, IO.METHOD_NEITHER, IO.FILE_SPECIAL_ACCESS); // REQUEST_RAW_ENCRYPTED_DATA, ENCRYPTED_DATA_INFO, EXTENDED_ENCRYPTED_DATA_INFO
        public readonly static CTL_CODE FSCTL_CREATE_USN_JOURNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 57U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // CREATE_USN_JOURNAL_DATA,
        public readonly static CTL_CODE FSCTL_READ_FILE_USN_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 58U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // Read the Usn Record for a file
        public readonly static CTL_CODE FSCTL_WRITE_USN_CLOSE_RECORD = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 59U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // Generate Close Usn Record
        public readonly static CTL_CODE FSCTL_EXTEND_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 60U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_QUERY_USN_JOURNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 61U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_DELETE_USN_JOURNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 62U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_MARK_HANDLE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 63U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_SIS_COPYFILE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 64U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_SIS_LINK_FILES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 65U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA | IO.FILE_WRITE_DATA);

        // decommissional fsctl value                                             66
        // decommissioned fsctl value                                             67
        // decommissioned fsctl value                                             68
        public readonly static CTL_CODE FSCTL_RECALL_FILE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 69U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                             70
        public readonly static CTL_CODE FSCTL_READ_FROM_PLEX = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 71U, IO.METHOD_OUT_DIRECT, IO.FILE_READ_DATA);
        public readonly static CTL_CODE FSCTL_FILE_PREFETCH = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 72U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // FILE_PREFETCH

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN2K */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static CTL_CODE FSCTL_MAKE_MEDIA_COMPATIBLE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 76U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // UDFS R/W
        public readonly static CTL_CODE FSCTL_SET_DEFECT_MANAGEMENT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 77U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // UDFS R/W
        public readonly static CTL_CODE FSCTL_QUERY_SPARING_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 78U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // UDFS R/W
        public readonly static CTL_CODE FSCTL_QUERY_ON_DISK_VOLUME_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 79U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // C/UDFS
        public readonly static CTL_CODE FSCTL_SET_VOLUME_COMPRESSION_STATE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 80U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // VOLUME_COMPRESSION_STATE

        // decommissioned fsctl value                                                 80
        public readonly static CTL_CODE FSCTL_TXFS_MODIFY_RM = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 81U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_QUERY_RM_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 82U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA);  // TxF

        // decommissioned fsctl value                                                 83
        public readonly static CTL_CODE FSCTL_TXFS_ROLLFORWARD_REDO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 84U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_ROLLFORWARD_UNDO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 85U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_START_RM = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 86U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_SHUTDOWN_RM = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 87U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_READ_BACKUP_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 88U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA);  // TxF
        public readonly static CTL_CODE FSCTL_TXFS_WRITE_BACKUP_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 89U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_CREATE_SECONDARY_RM = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 90U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_GET_METADATA_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 91U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA);  // TxF
        public readonly static CTL_CODE FSCTL_TXFS_GET_TRANSACTED_VERSION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 92U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA);  // TxF

        // decommissioned fsctl value                                                 93
        public readonly static CTL_CODE FSCTL_TXFS_SAVEPOINT_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 94U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_CREATE_MINIVERSION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 95U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA); // TxF

        // decommissioned fsctl value                                                 96
        // decommissioned fsctl value                                                 97
        // decommissioned fsctl value                                                 98
        public readonly static CTL_CODE FSCTL_TXFS_TRANSACTION_ACTIVE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 99U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA);  // TxF
        public readonly static CTL_CODE FSCTL_SET_ZERO_ON_DEALLOCATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 101U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE FSCTL_SET_REPAIR = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 102U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_GET_REPAIR = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 103U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_WAIT_FOR_REPAIR = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 104U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // decommissioned fsctl value                                                 105
        public readonly static CTL_CODE FSCTL_INITIATE_REPAIR = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 106U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSC_INTERNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 107U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // CSC internal implementation
        public readonly static CTL_CODE FSCTL_SHRINK_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 108U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS); // SHRINK_VOLUME_INFORMATION
        public readonly static CTL_CODE FSCTL_SET_SHORT_NAME_BEHAVIOR = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 109U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_DFSR_SET_GHOST_HANDLE_STATE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 110U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        //
        //  Values 111 - 119 are reserved for FSRM.
        //

        public readonly static CTL_CODE FSCTL_TXFS_LIST_TRANSACTION_LOCKED_FILES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 120U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA); // TxF
        public readonly static CTL_CODE FSCTL_TXFS_LIST_TRANSACTIONS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 121U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA); // TxF
        public readonly static CTL_CODE FSCTL_QUERY_PAGEFILE_ENCRYPTION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 122U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static CTL_CODE FSCTL_RESET_VOLUME_ALLOCATION_HINTS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 123U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static CTL_CODE FSCTL_QUERY_DEPENDENT_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 124U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);    // Dependency File System Filter
        public readonly static CTL_CODE FSCTL_SD_GLOBAL_CHANGE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 125U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // Query/Change NTFS Security Descriptors

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_VISTA)
        public readonly static CTL_CODE FSCTL_TXFS_READ_BACKUP_INFORMATION2 = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 126U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // TxF

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_VISTA */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static CTL_CODE FSCTL_LOOKUP_STREAM_FROM_CLUSTER = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 127U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_TXFS_WRITE_BACKUP_INFORMATION2 = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 128U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // TxF
        public readonly static CTL_CODE FSCTL_FILE_TYPE_NOTIFICATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 129U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static CTL_CODE FSCTL_FILE_LEVEL_TRIM = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 130U, IO.METHOD_BUFFERED, IO.FILE_WRITE_DATA);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        //
        //  Values 131 - 139 are reserved for FSRM.
        //

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static CTL_CODE FSCTL_GET_BOOT_AREA_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 140U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // BOOT_AREA_INFO
        public readonly static CTL_CODE FSCTL_GET_RETRIEVAL_POINTER_BASE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 141U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // RETRIEVAL_POINTER_BASE
        public readonly static CTL_CODE FSCTL_SET_PERSISTENT_VOLUME_STATE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 142U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);  // FILE_FS_PERSISTENT_VOLUME_INFORMATION
        public readonly static CTL_CODE FSCTL_QUERY_PERSISTENT_VOLUME_STATE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 143U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);  // FILE_FS_PERSISTENT_VOLUME_INFORMATION
        public readonly static CTL_CODE FSCTL_REQUEST_OPLOCK = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 144U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_TUNNEL_REQUEST = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 145U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // CSV_TUNNEL_REQUEST
        public readonly static CTL_CODE FSCTL_IS_CSV_FILE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 146U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // IS_CSV_FILE
        public readonly static CTL_CODE FSCTL_QUERY_FILE_SYSTEM_RECOGNITION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 147U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); //
        public readonly static CTL_CODE FSCTL_CSV_GET_VOLUME_PATH_NAME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 148U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_GET_VOLUME_NAME_FOR_VOLUME_MOUNT_POINT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 149U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_GET_VOLUME_PATH_NAMES_FOR_VOLUME_NAME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 150U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_IS_FILE_ON_CSV_VOLUME = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 151U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static CTL_CODE FSCTL_CORRUPTION_HANDLING = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 152U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_OFFLOAD_READ = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 153U, IO.METHOD_BUFFERED, IO.FILE_READ_ACCESS);
        public readonly static CTL_CODE FSCTL_OFFLOAD_WRITE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 154U, IO.METHOD_BUFFERED, IO.FILE_WRITE_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN7)
        public readonly static CTL_CODE FSCTL_CSV_INTERNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 155U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /* _WIN32_WINNT >= _WIN32_WINNT_WIN7 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static CTL_CODE FSCTL_SET_PURGE_FAILURE_MODE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 156U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_QUERY_FILE_LAYOUT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 157U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_IS_VOLUME_OWNED_BYCSVFS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 158U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_GET_INTEGRITY_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 159U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);                  // FSCTL_GET_INTEGRITY_INFORMATION_BUFFER
        public readonly static CTL_CODE FSCTL_SET_INTEGRITY_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 160U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA | IO.FILE_WRITE_DATA); // FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
        public readonly static CTL_CODE FSCTL_QUERY_FILE_REGIONS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 161U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        //
        // Dedup FSCTLs
        // Values 162 - 170 are reserved for Dedup.
        //

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static CTL_CODE FSCTL_DEDUP_FILE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 165U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_DEDUP_QUERY_FILE_HASHES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 166U, IO.METHOD_NEITHER, IO.FILE_READ_DATA);
        public readonly static CTL_CODE FSCTL_DEDUP_QUERY_RANGE_STATE = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 167U, IO.METHOD_NEITHER, IO.FILE_READ_DATA);
        public readonly static CTL_CODE FSCTL_DEDUP_QUERY_REPARSE_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 168U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
        public readonly static CTL_CODE FSCTL_RKF_INTERNAL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 171U, IO.METHOD_NEITHER, IO.FILE_ANY_ACCESS); // Resume Key Filter
        public readonly static CTL_CODE FSCTL_SCRUB_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 172U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_REPAIR_COPIES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 173U, IO.METHOD_BUFFERED, IO.FILE_READ_DATA | IO.FILE_WRITE_DATA);
        public readonly static CTL_CODE FSCTL_DISABLE_LOCAL_BUFFERING = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 174U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_MGMT_LOCK = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 175U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_QUERY_DOWN_LEVEL_FILE_SYSTEM_CHARACTERISTICS = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 176U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_ADVANCE_FILE_ID = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 177U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_SYNC_TUNNEL_REQUEST = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 178U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_QUERY_VETO_FILE_DIRECT_IO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 179U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_WRITE_USN_REASON = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 180U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_CONTROL = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 181U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_GET_REFS_VOLUME_DATA = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 182U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_CSV_H_BREAKING_SYNC_TUNNEL_REQUEST = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 185U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /*_WIN32_WINNT >= _WIN32_WINNT_WIN8 */

        // if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        public readonly static CTL_CODE FSCTL_QUERY_STORAGE_CLASSES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 187U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_QUERY_REGION_INFO = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 188U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_USN_TRACK_MODIFIED_RANGES = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 189U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS); // USN_TRACK_MODIFIED_RANGES

        // endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        // if  (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE)
        public readonly static CTL_CODE FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 192U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_SVHDX_SYNC_TUNNEL_REQUEST = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 193U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);
        public readonly static CTL_CODE FSCTL_SVHDX_SET_INITIATOR_INFORMATION = new CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 194U, IO.METHOD_BUFFERED, IO.FILE_ANY_ACCESS);

        // endif  /* (_WIN32_WINNT >= _WIN32_WINNT_WINBLUE) */
        //
        // AVIO IOCTLS.
        //

        public readonly static CTL_CODE IOCTL_AVIO_ALLOCATE_STREAM = new CTL_CODE(FILE_DEVICE_AVIO, 1U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE IOCTL_AVIO_FREE_STREAM = new CTL_CODE(FILE_DEVICE_AVIO, 2U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);
        public readonly static CTL_CODE IOCTL_AVIO_MODIFY_STREAM = new CTL_CODE(FILE_DEVICE_AVIO, 3U, IO.METHOD_BUFFERED, IO.FILE_SPECIAL_ACCESS);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum MOVE_FILE_FLAGS : uint
        {

            /// <summary>
            /// If the file is to be moved to a different volume, the function simulates the move by using the CopyFile and DeleteFile functions.
            /// If the file is successfully copied to a different volume and the original file is unable to be deleted, the function succeeds leaving the source file intact.
            /// This value cannot be used with MOVEFILE_DELAY_UNTIL_REBOOT.
            /// </summary>
            MOVEFILE_COPY_ALLOWED = 2U,

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MOVEFILE_CREATE_HARDLINK = 16U,


            /// <summary>
            /// The system does not move the file until the operating system is restarted. The system moves the file immediately after AUTOCHK is executed, but before creating any paging files. Consequently, this parameter enables the function to delete paging files from previous startups.
            /// This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.
            /// This value cannot be used with MOVEFILE_COPY_ALLOWED.
            /// </summary>
            MOVEFILE_DELAY_UNTIL_REBOOT = 4U,


            /// <summary>
            /// The function fails if the source file is a link source, but the file cannot be tracked after the move. This situation can occur if the destination is a volume formatted with the FAT file system.
            /// </summary>
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 32U,


            /// <summary>
            /// If a file named lpNewFileName exists, the function replaces its contents with the contents of the lpExistingFileName file.
            /// This value cannot be used if lpNewFileName or lpExistingFileName names a directory.
            /// </summary>
            MOVEFILE_REPLACE_EXISTING = 1U,


            /// <summary>
            /// The function does not return until the file has actually been moved on the disk.
            /// Setting this value guarantees that a move performed as a copy and delete operation is flushed to disk before the function returns. The flush occurs at the end of the copy operation.
            /// This value has no effect if MOVEFILE_DELAY_UNTIL_REBOOT is set.
            /// </summary>
            MOVEFILE_WRITE_THROUGH = 8U
        }


        // DWORD CALLBACK CopyProgressRoutine(
        // _In_      LARGE_INTEGER TotalFileSize,
        // _In_      LARGE_INTEGER TotalBytesTransferred,
        // _In_      LARGE_INTEGER StreamSize,
        // _In_      LARGE_INTEGER StreamBytesTransferred,
        // _In_      DWORD dwStreamNumber,
        // _In_      DWORD dwCallbackReason,
        // _In_      HANDLE hSourceFile,
        // _In_      HANDLE hDestinationFile,
        // _In_opt_  LPVOID lpData
        // );

        /// <summary>
        /// Copy Progress Callback Reason
        /// </summary>
        public enum CALLBACK_REASON : uint
        {
            /// <summary>
            /// Another part of the data file was copied.
            /// </summary>
            CALLBACK_CHUNK_FINISHED = 0U,

            /// <summary>
            /// Another stream was created and is about to be copied.
            /// This is the callback reason given when the callback routine is first invoked.
            /// </summary>
            CALLBACK_STREAM_SWITCH = 1U
        }

        public delegate uint CopyProgressRoutine(LARGE_INTEGER TotalFileSize, LARGE_INTEGER TotalBytesTrasnferred, LARGE_INTEGER StreamSize, LARGE_INTEGER StreambytesTransferred, uint dwStreamNumber, CALLBACK_REASON dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);





        // BOOL WINAPI MoveFileWithProgress(
        // _In_     LPCTSTR            lpExistingFileName,
        // _In_opt_ LPCTSTR            lpNewFileName,
        // _In_opt_ LPPROGRESS_ROUTINE lpProgressRoutine,
        // _In_opt_ LPVOID             lpData,
        // _In_     DWORD              dwFlags
        // );


        [DllImport("kernel32", EntryPoint = "MoveFileWithProgressW", CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern bool MoveFileWithProgress(string lpExistingFilename, string lpNewFilename, [MarshalAs(UnmanagedType.FunctionPtr)] CopyProgressRoutine lpPRogressRoutine, IntPtr lpData, MOVE_FILE_FLAGS dwFlag);

        // BOOL WINAPI CopyFileEx(
        // _In_      LPCTSTR lpExistingFileName,
        // _In_      LPCTSTR lpNewFileName,
        // _In_opt_  LPPROGRESS_ROUTINE lpProgressRoutine,
        // _In_opt_  LPVOID lpData,
        // _In_opt_  LPBOOL pbCancel,
        // _In_      DWORD dwCopyFlags
        // );


        [DllImport("kernel32", EntryPoint = "CopyFilExW", CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern bool CopyFileEx(string lpExistingFilename, string lpNewFilename, [MarshalAs(UnmanagedType.FunctionPtr)] CopyProgressRoutine lpProgressRoutine, IntPtr lpDAta, [MarshalAs(UnmanagedType.Bool)] ref bool pbCancel, uint dwCopyFlags);



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}
