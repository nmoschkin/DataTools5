// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: SystemInfo
// '         Provides basic information about the
// '         current computer.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.SystemInformation
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Computer firmware types
    /// </summary>
    /// <remarks></remarks>
    public enum FirmwareType
    {
        Unknown = 0,
        Bios = 1,
        Uefi = 2,
        Max = 3
    }

    /// <summary>
    /// Processor architecture type.
    /// </summary>
    public enum ArchitectureType : short
    {

        /// <summary>
        /// 32-bit system.
        /// </summary>
        /// <remarks></remarks>
        x86 = 0,

        /// <summary>
        /// Iatium-based system.
        /// </summary>
        /// <remarks></remarks>
        IA64 = 6,

        /// <summary>
        /// 64-bit system.
        /// </summary>
        /// <remarks></remarks>
        x64 = 9
    }

    /// <summary>
    /// Windows Version Constants - (Maj &lt;&lt; 8) | Min method.
    /// Minimum value is Vista.  Older versions of Windows are not supported.
    /// </summary>
    /// <remarks></remarks>
    public enum WindowsVersionConstants
    {

        /// <summary>
        /// Windows Vista
        /// </summary>
        /// <remarks></remarks>
        WindowsVista = 0x600,

        /// <summary>
        /// Windows 7
        /// </summary>
        /// <remarks></remarks>
        Windows7 = 0x601,

        /// <summary>
        /// Windows 8
        /// </summary>
        /// <remarks></remarks>
        Windows8 = 0x602,

        /// <summary>
        /// Windows 8.1
        /// </summary>
        /// <remarks></remarks>
        Windows81 = 0x603,

        /// <summary>
        /// Windows 10
        /// </summary>
        /// <remarks></remarks>
        Windows10 = 0xA00
    }

    /// <summary>
    /// Windows operating system suite masks.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum OSSuiteMask : ushort
    {

        /// <summary>
        /// Microsoft BackOffice components are installed.
        /// </summary>
        [Description("Microsoft BackOffice components are installed.")]
        BackOffice = 0x4,

        /// <summary>
        /// Windows Server 2003, Web Edition is installed.
        /// </summary>
        [Description("Windows Server 2003, Web Edition is installed.")]
        Blade = 0x400,

        /// <summary>
        /// Windows Server 2003, Compute Cluster Edition is installed.
        /// </summary>
        [Description("Windows Server 2003, Compute Cluster Edition is installed.")]
        ComputeServer = 0x4000,

        /// <summary>
        /// Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.
        /// </summary>
        [Description("Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.")]
        DataCenter = 0x80,

        /// <summary>
        /// Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.")]
        Enterprise = 0x2,

        /// <summary>
        /// Windows XP Embedded is installed.
        /// </summary>
        [Description("Windows XP Embedded is installed.")]
        EmbeddedNT = 0x40,

        /// <summary>
        /// Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.
        /// </summary>
        [Description("Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.")]
        Personal = 0x200,

        /// <summary>
        /// Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.
        /// </summary>
        [Description("Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.")]
        SingleUser = 0x100,

        /// <summary>
        /// Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.")]
        SmallBusiness = 0x1,

        /// <summary>
        /// Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.
        /// </summary>
        [Description("Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.")]
        SmallBusinessRestricted = 0x20,

        /// <summary>
        /// Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.
        /// </summary>
        [Description("Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.")]
        StorageServer = 0x2000,

        /// <summary>
        /// Terminal Services is installed. This value is always set.
        /// If TERMINAL is set but SINGLEUSERTS is not set, the system is running in application server mode.
        /// </summary>
        [Description("Terminal Services is installed. This value is always set.")]
        Terminal = 0x10,

        /// <summary>
        /// Windows Home Server is installed.
        /// </summary>
        [Description("Windows Home Server is installed.")]
        HomeServer = 0x8000
    }

    /// <summary>
    /// Windows product type information.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum OSProductType : byte
    {

        /// <summary>
        /// The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        /// </summary>
        [Description("The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")]
        NTDomainController = 0x2,

        /// <summary>
        /// The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        /// </summary>
        [Description("The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")]
        NTServer = 0x3,

        /// <summary>
        /// The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.
        /// </summary>
        [Description("The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.")]
        NTWorkstation = 0x1
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// OSVERSIONINFOEX structure with additional utility and functionality.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OSVERSIONINFOEX
    {

        /// <summary>
        /// The size of this structure, in bytes.
        /// </summary>
        /// <remarks></remarks>
        public int dwOSVersionInfoSize;

        /// <summary>
        /// The major version of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwMajorVersion;

        /// <summary>
        /// The minor verison of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwMinorVersion;

        /// <summary>
        /// The build number of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int dwBuildNumber;

        /// <summary>
        /// The platform Id of the current operating system.
        /// Currently, this value should always be 2.
        /// </summary>
        /// <remarks></remarks>
        public int dwPlatformId;

        /// <summary>
        /// Private character buffer.
        /// </summary>
        /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 128)]
        private char[] szCSDVersionChar;

        /// <summary>
        /// The Service Pack name (if applicable)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string szCSDVersion
        {
            get
            {
                GCHandle gch = GCHandle.Alloc(szCSDVersionChar, GCHandleType.Pinned);
                MemPtr mm = gch.AddrOfPinnedObject();

                string vs = (string)mm;
                gch.Free();

                return vs;
            }
        }

        /// <summary>
        /// Service pack major verison number.
        /// </summary>
        /// <remarks></remarks>
        public short wServicePackMajor;

        /// <summary>
        /// Server pack minor verison number.
        /// </summary>
        /// <remarks></remarks>
        public short wServicePackMinor;

        /// <summary>
        /// The Windows Suite mask.
        /// </summary>
        /// <remarks></remarks>
        public OSSuiteMask wSuiteMask;

        /// <summary>
        /// The product type flags.
        /// </summary>
        /// <remarks></remarks>
        public OSProductType wProductType;

        /// <summary>
        /// Reserved
        /// </summary>
        /// <remarks></remarks>
        public byte wReserved;

        /// <summary>
        /// Returns a verbose description of the operating system, including version and build number.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string VerboseDescription
        {
            get
            {
                return string.Format("{0} {4} ({1}.{2} Build {3})", Description, dwMajorVersion, dwMinorVersion, dwBuildNumber, Architecture.ToString());
            }
        }

        /// <summary>
        /// Returns the processor architecture of the current system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ArchitectureType Architecture
        {
            get
            {
                return SysInfo._sysInfo.wProcessorArchitecture;
            }
        }

        /// <summary>
        /// Returns the number of logical processors on the current system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Processors
        {
            get
            {
                return SysInfo._sysInfo.dwNumberOfProcessors;
            }
        }

        /// <summary>
        /// Returns the amount of total physical memory on the system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong TotalPhysicalMemory
        {
            get
            {
                return SysInfo._memInfo.ullTotalPhys;
            }
        }

        /// <summary>
        /// Returns the amount of available physical memory currently on the system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong AvailPhysicalMemory
        {
            get
            {
                // ' obviously this property is not very useful unless it is live, so let's
                // ' refresh our data before sending out the result...
                SysInfo.GlobalMemoryStatusEx(ref SysInfo._memInfo);
                return SysInfo._memInfo.ullAvailPhys;
            }
        }

        /// <summary>
        /// Returns a description of the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(szCSDVersion))
                {
                    return Name;
                }
                else
                {
                    return Name + " " + szCSDVersion;
                }
            }
        }

        /// <summary>
        /// Returns the name of the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                if (key is object)
                {
                    string s = (string)(key.GetValue("ProductName"));
                    key.Close();
                    return s;
                }

                return null;

                // ' see the OSVersionNames class, above, for more info.
                // Dim o As Object = OSVersionNames.FindVersion(Me)

                // If o Is Nothing Then
                // Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                // If key IsNot Nothing Then
                // o = key.GetValue("ProductName")
                // key.Close()
                // End If
                // Else
                // Return o.Name
                // End If

                // Return o
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 10
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows10
        {
            get
            {
                return dwMajorVersion >= 10 ? true : false;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 8.1
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows81
        {
            get
            {
                return dwMajorVersion > 6 ? true : dwMajorVersion == 6 && dwMinorVersion >= 3;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 8
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows8
        {
            get
            {
                return dwMajorVersion > 6 ? true : dwMajorVersion == 6 && dwMinorVersion >= 2;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 7
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows7
        {
            get
            {
                return dwMajorVersion > 6 ? true : dwMajorVersion == 6 && dwMinorVersion >= 1;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows Vista
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsVista
        {
            get
            {
                return dwMajorVersion > 6 ? true : dwMajorVersion == 6 && dwMinorVersion >= 0;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2008
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2008
        {
            get
            {
                return IsVista && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2008 R2
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2008R2
        {
            get
            {
                return IsWindows7 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2012
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2012
        {
            get
            {
                return IsWindows8 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2012 R2
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2012R2
        {
            get
            {
                return IsWindows81 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if the current operating system is a Windows Server OS.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer
        {
            get
            {
                return wProductType != OSProductType.NTWorkstation;
            }
        }

        /// <summary>
        /// Returns true on a multi-user system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsMultiUser
        {
            get
            {
                return (int)(wSuiteMask & OSSuiteMask.SingleUser) == 0;
            }
        }

        /// <summary>
        /// Returns the Windows version constant.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public WindowsVersionConstants WindowsVersion
        {
            get
            {
                return (WindowsVersionConstants)((dwMajorVersion << 8) | dwMinorVersion);
            }
        }

        /// <summary>
        /// Converts this object into its string representation.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns the current computer's firmware type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FirmwareType FirmwareType
        {
            get
            {
                FirmwareType FirmwareTypeRet = default;
                SysInfo.GetFirmwareType(ref FirmwareTypeRet);
                return FirmwareTypeRet;
            }
        }

        /// <summary>
        /// Returns true if this system was booted from a virtual hard drive.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsVhdBoot
        {
            get
            {
                bool IsVhdBootRet = default;
                SysInfo.IsNativeVhdBoot(ref IsVhdBootRet);
                return IsVhdBootRet;
            }
        }

        public static explicit operator string(OSVERSIONINFOEX operand)
        {
            return operand.ToString();
        }

        public static explicit operator WindowsVersionConstants(OSVERSIONINFOEX operand)
        {
            return (WindowsVersionConstants)(operand.dwMajorVersion << 8 | operand.dwMinorVersion);
        }
    }


    /// <summary>
    /// Operating system version information
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OSVERSIONINFO
    {

        /// <summary>
        /// Size of this structure
        /// </summary>
        public int OSVersionInfoSize;

        /// <summary>
        /// Major version
        /// </summary>
        public int MajorVersion;

        /// <summary>
        /// Minor version
        /// </summary>
        public int MinorVersion;

        /// <summary>
        /// Build number
        /// </summary>
        public int BuildNumber;

        /// <summary>
        /// Platform ID
        /// </summary>
        public int PlatformId;

        /// <summary>
        /// CSD Version
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string CSDVersion;
    }


    /// <summary>
    /// Logical processor relationship
    /// </summary>
    public enum LOGICAL_PROCESSOR_RELATIONSHIP
    {

        /// <summary>
        /// Processor core
        /// </summary>
        RelationProcessorCore,


        /// <summary>
        /// Numa Node
        /// </summary>
        RelationNumaNode,

        /// <summary>
        /// Cache
        /// </summary>
        RelationCache,

        /// <summary>
        /// Processor Package
        /// </summary>
        RelationProcessorPackage,

        /// <summary>
        /// Processor Group
        /// </summary>
        RelationGroup,

        /// <summary>
        /// All
        /// </summary>
        RelationAll = 0xFFFF
    }

    /// <summary>
    /// Processor cache type
    /// </summary>
    public enum ProcessorCacheType
    {

        /// <summary>
        /// Unified
        /// </summary>
        CacheUnified,

        /// <summary>
        /// Instruction
        /// </summary>
        CacheInstruction,

        /// <summary>
        /// Data
        /// </summary>
        CacheData,

        /// <summary>
        /// Trace
        /// </summary>
        CacheTrace
    }

    /// <summary>
    /// Cache descriptor
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CACHE_DESCRIPTOR
    {
        /// <summary>
        /// Level
        /// </summary>
        public byte Level;

        /// <summary>
        /// Associativity
        /// </summary>
        public byte Associativity;

        /// <summary>
        /// Line size
        /// </summary>
        public short LineSize;

        /// <summary>
        /// Size
        /// </summary>
        public int Size;

        /// <summary>
        /// Type
        /// </summary>
        public ProcessorCacheType Type;

        public override string ToString()
        {
            string ct = Type.ToString().Replace("Cache", "");

            string s = $"L{Level} {ct} Cache, {TextTools.PrintFriendlySize(Size)}, Line Size {TextTools.PrintFriendlySize(LineSize)}";
            if (Associativity == 0xff) s += ", Fully Associative";

            return s;
        }
    }


    /// <summary>
    /// Logical processor information stucture.  Contains information about a logical processor on the local machine.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION
    {
        public override string ToString()
        {
            string s;

            if (Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationCache)
            {
                s = $"L{CacheDescriptor.Level} Cache, {TextTools.PrintFriendlySize(CacheDescriptor.Size)} ({GetProcMaskInfoString()})";
                return s;
            }
            else if (Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore)
            {
                return $"Processor Core ({GetProcMaskInfoString()})";
            }
            else if (Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorPackage)
            {
                return "Processor Package";
            }
            else if (Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationNumaNode)
            {
                return $"Numa Node {NodeNumber} ({GetProcMaskInfoString()})";
            }

            return base.ToString();
        }

        private string GetProcMaskInfoString()
        {
            long m = ProcessorMask;
            string s = "";

            int c = 0;

            while (m != 0)
            {
                if ((m & 1) == 1)
                {
                    if (s != "") s += ", ";
                    s += (c + 1).ToString();
                }

                m >>= 1;
                c++;
            }

            s = "Processors " + s;
            return s;
        }

#if X64
        /// <summary>
        /// Processor mask
        /// </summary>
        [FieldOffset(0)]
        public long ProcessorMask;

        /// <summary>
        /// Processor relationship (entity kind)
        /// </summary>

        [FieldOffset(8)]
        public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;

        /// <summary>
        /// Flags
        /// </summary>
        [FieldOffset(16)]
        public byte Flags;

        /// <summary>
        /// Node number
        /// </summary>
        [FieldOffset(16)]
        public int NodeNumber;


        /// <summary>
        /// Cache descriptor
        /// </summary>
        [FieldOffset(16)]
        public CACHE_DESCRIPTOR CacheDescriptor;

        /// <summary>
        /// Reserved 1
        /// </summary>
        [FieldOffset(16)]
        public long Reserved1;

        /// <summary>
        /// Reserved 2
        /// </summary>
        [FieldOffset(24)]
        public long Reserved2;

#else
        /// <summary>
        /// Processor mask
        /// </summary>
        [FieldOffset(0)]
        public long ProcessorMask;

        /// <summary>
        /// Processor relationship (entity kind)
        /// </summary>

        [FieldOffset(4)]
        public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;

        /// <summary>
        /// Flags
        /// </summary>
        [FieldOffset(8)]
        public byte Flags;

        /// <summary>
        /// Node number
        /// </summary>
        [FieldOffset(8)]
        public int NodeNumber;


        /// <summary>
        /// Cache descriptor
        /// </summary>
        [FieldOffset(8)]
        public CACHE_DESCRIPTOR CacheDescriptor;

        /// <summary>
        /// Reserved 1
        /// </summary>
        [FieldOffset(8)]
        public long Reserved1;

        /// <summary>
        /// Reserved 2
        /// </summary>
        [FieldOffset(12)]
        public long Reserved2;

#endif

    }


    /// <summary>
    /// System information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SYSTEM_INFO
    {

        /// <summary>
        /// Processor Architecture Type enumeration value.
        /// </summary>
        public ArchitectureType wProcessorArchitecture;

        /// <summary>
        /// Reserved
        /// </summary>
        public short wReserved;

        /// <summary>
        /// System memory page size
        /// </summary>
        public int dwPageSize;

        /// <summary>
        /// Minimum allowed application memory address
        /// </summary>
        public UIntPtr lpMinimumApplicationAddress;

        /// <summary>
        /// Maximum allowed application memory address
        /// </summary>
        public UIntPtr lpMaximumApplicationAddress;

        /// <summary>
        /// Active processor mask
        /// </summary>
        public int dwActiveProcessorMask;

        /// <summary>
        /// Number of processors on the local machine
        /// </summary>
        public int dwNumberOfProcessors;

        /// <summary>
        /// Processor type
        /// </summary>
        public int dwProcessorType;

        /// <summary>
        /// Allocation granularity
        /// </summary>
        public int dwAllocationGranularity;

        /// <summary>
        /// Processor level
        /// </summary>
        public short wProcessorLevel;

        /// <summary>
        /// Processor revision
        /// </summary>
        public short wProcessorRevision;
    }

    /// <summary>
    /// MEMORYSTATUSEX structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORYSTATUSEX
    {

        /// <summary>
        /// Length of this structure
        /// </summary>
        public int dwLength;

        /// <summary>
        /// Memory load
        /// </summary>
        public int dwMemoryLoad;

        /// <summary>
        /// Total physical memory on the machine
        /// </summary>
        public ulong ullTotalPhys;

        /// <summary>
        /// Total available memroy on the machine
        /// </summary>
        public ulong ullAvailPhys;

        /// <summary>
        /// Total paging file capacity
        /// </summary>
        public ulong ullTotalPageFile;

        /// <summary>
        /// Available paging file capacity
        /// </summary>
        public ulong ullAvailPageFile;

        /// <summary>
        /// Total virtual memory
        /// </summary>
        public ulong ullTotalVirtual;

        /// <summary>
        /// Available virtual memory
        /// </summary>
        public ulong ullAvailVirtual;


        /// <summary>
        /// Available extended virtual memory
        /// </summary>
        public ulong ullAvailExtendedVirtual;
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Contains a static list of Windows versions and their characteristics.
    /// </summary>
    /// <remarks></remarks>
    public sealed class OSVersionNames
    {
        private string _Name;
        private int _VerMaj;
        private int _VerMin;
        private bool _Server;
        private static List<OSVersionNames> _col = new List<OSVersionNames>();

        /// <summary>
        /// Returns a list of all OS versions recognized by this assembly.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<OSVersionNames> Versions
        {
            get
            {
                return _col;
            }
        }

        /// <summary>
        /// Finds the OSVersionNames object for the operating system data specified in the osx parameter.
        /// </summary>
        /// <param name="osx">Version information of the operating system for which to retrieve the object.</param>
        /// <returns>An OSVersionNames object.</returns>
        /// <remarks></remarks>
        public static OSVersionNames FindVersion(OSVERSIONINFOEX osx)
        {
            return FindVersion(osx.dwMajorVersion, osx.dwMinorVersion, osx.wProductType != OSProductType.NTWorkstation);
        }

        /// <summary>
        /// Finds the OSVersionNames object for the operating system data specified.
        /// </summary>
        /// <param name="verMaj">The major version of the operating system to find.</param>
        /// <param name="verMin">The minor version of the operating system to find.</param>
        /// <param name="server">Indicates that the querent is looking for a server OS.</param>
        /// <returns>An OSVersionNames object.</returns>
        /// <remarks></remarks>
        public static OSVersionNames FindVersion(int verMaj, int verMin, bool server = false)
        {
            foreach (var v in _col)
            {
                if (v.VersionMajor == verMaj && v.VersionMinor == verMin && v.Server == server)
                    return v;
            }

            return null;
        }

        /// <summary>
        /// Returns the name of the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Returns the major version
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int VersionMajor
        {
            get
            {
                return _VerMaj;
            }
        }

        /// <summary>
        /// Returns the minor version
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int VersionMinor
        {
            get
            {
                return _VerMin;
            }
        }

        /// <summary>
        /// Indicates that this version is a server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Server
        {
            get
            {
                return _Server;
            }
        }

        protected OSVersionNames(string name, int vermaj, int vermin, bool serv = false)
        {
            _Name = name;
            _VerMaj = vermaj;
            _VerMin = vermin;
            _Server = serv;
        }

        static OSVersionNames()
        {
            _col.Add(new OSVersionNames("Windows 10", 10, 0, false));
            _col.Add(new OSVersionNames("Windows Server 10", 10, 0, true));
            _col.Add(new OSVersionNames("Windows 8.1", 6, 3, false));
            _col.Add(new OSVersionNames("Windows Server 2012 R2", 6, 3, true));
            _col.Add(new OSVersionNames("Windows 8", 6, 2, false));
            _col.Add(new OSVersionNames("Windows Server 2012", 6, 2, true));
            _col.Add(new OSVersionNames("Windows 7", 6, 1, false));
            _col.Add(new OSVersionNames("Windows Server 2008 R2", 6, 1, true));
            _col.Add(new OSVersionNames("Windows Vista", 6, 0, false));
            _col.Add(new OSVersionNames("Windows Server 2008", 6, 0, true));

            // ' Nothing in this project is designed to work on Window XP, so looking for it is pointless.
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// System information.
    /// </summary>
    public static class SysInfo
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPOrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP1OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP2OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsXPSP3OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaOrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaSP1OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVistaSP2OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows7OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows7SP1OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows8OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows8Point10OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindows10OrGreater();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsServer();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool IsWindowsVersionOrGreater(ushort maj, ushort min, ushort servicePack);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int GetVersionExW([MarshalAs(UnmanagedType.Struct)] ref OSVERSIONINFO pData);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int GetVersionExW([MarshalAs(UnmanagedType.Struct)] ref OSVERSIONINFOEX pData);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GetFirmwareType(ref FirmwareType firmwareType);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool IsNativeVhdBoot(ref bool nativeVhd);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSysInfo);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GetLogicalProcessorInformation(IntPtr buffer, ref int ReturnLength);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpStatusEx);

        /// <summary>
        /// Contains details about the current operating system and computing environment.
        /// </summary>
        /// <remarks></remarks>
        public static OSVERSIONINFOEX OSInfo { get; private set; } = GetOSInfo();

        internal static SYSTEM_INFO _sysInfo;

        /// <summary>
        /// Contains specific configuration details about the current native system mode.
        /// </summary>
        /// <returns></returns>
        public static SYSTEM_INFO SystemInfo
        {
            get
            {
                return _sysInfo;
            }
        }

        internal static MEMORYSTATUSEX _memInfo;

        /// <summary>
        /// Contains detailed memory information.
        /// </summary>
        /// <returns></returns>
        public static MEMORYSTATUSEX MemoryInfo
        {
            get
            {
                return _memInfo;
            }
        }

        internal static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] _procRaw;

        /// <summary>
        /// Contains information about the logical processors on the system.
        /// </summary>
        /// <returns></returns>
        public static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] LogicalProcessors
        {
            get
            {
                return _procRaw;
            }
        }

        static SysInfo()
        {

            // ' let's get some version information!

            _memInfo.dwLength = Marshal.SizeOf(_memInfo);
            GetNativeSystemInfo(ref _sysInfo);
            GlobalMemoryStatusEx(ref _memInfo);

            // ' now let's figure out how many processors we have on this system
            var mm = new MemPtr();
            MemPtr org;
            var lp = new SYSTEM_LOGICAL_PROCESSOR_INFORMATION();
            int lRet = 0;
            SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] rets;
            int i;
            int c;

            // ' The maximum number of processors for any version of Windows is 128, we'll allocate more for extra information.
            mm.Alloc(Marshal.SizeOf(lp) * 1024);

            // ' record the original memory pointer
            org = mm;
            lRet = (int)mm.Length;
            GetLogicalProcessorInformation(mm, ref lRet);
            c = (int)(lRet / (double)Marshal.SizeOf(lp));
            rets = new SYSTEM_LOGICAL_PROCESSOR_INFORMATION[c];
            var loopTo = c - 1;
            for (i = 0; i <= loopTo; i++)
            {
                rets[i] = mm.ToStruct<SYSTEM_LOGICAL_PROCESSOR_INFORMATION>();
                mm += Marshal.SizeOf(lp);

                // ' what we're really after are the number of cores.
                if (rets[i].Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore)
                {
                    _sysInfo.dwNumberOfProcessors += 1;
                }
            }

            // ' store that data in case we need it for later.
            _procRaw = rets;

            // ' free our unmanaged resources.
            org.Free();
        }

        /// <summary>
        /// Retrieves the current Operating System information.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static OSVERSIONINFOEX GetOSInfo()
        {
            var lpOS = new OSVERSIONINFOEX();
            lpOS.dwOSVersionInfoSize = Marshal.SizeOf(lpOS);
            GetVersionExW(ref lpOS);
            var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (key is object)
            {
                int maj = (int)(key.GetValue("CurrentMajorVersionNumber"));
                int min = (int)(key.GetValue("CurrentMinorVersionNumber"));

                int cb = int.Parse((string)key.GetValue("CurrentBuildNumber"));
                int build = cb;
                //string name = (string)(key.GetValue("ProductName"));
                lpOS.dwMajorVersion = maj;
                lpOS.dwMinorVersion = min;
                lpOS.dwBuildNumber = build;
                key.Close();
            }

            // Try
            // If IsWindowsVersionOrGreater(6, 4, 0) Then
            // lpOS.dwMinorVersion = 4
            // End If
            // Catch ex As Exception
            // lpOS.dwMinorVersion = 4

            // End Try

            return lpOS;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}