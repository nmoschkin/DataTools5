'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: SystemInfo
''         Provides basic information about the
''         current computer.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports DataTools.Memory
Imports DataTools.Strings

Namespace SystemInformation

#Region "Enumerations"

    ''' <summary>
    ''' Computer firmware types
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FirmwareType
        Unknown = 0
        Bios = 1
        Uefi = 2
        Max = 3
    End Enum

    ''' <summary>
    ''' Processor architecture type.
    ''' </summary>
    Public Enum ArchitectureType As Short

        ''' <summary>
        ''' 32-bit system.
        ''' </summary>
        ''' <remarks></remarks>
        x86 = 0

        ''' <summary>
        ''' Iatium-based system.
        ''' </summary>
        ''' <remarks></remarks>
        IA64 = 6

        ''' <summary>
        ''' 64-bit system.
        ''' </summary>
        ''' <remarks></remarks>
        x64 = 9

    End Enum

    ''' <summary>
    ''' Windows Version Constants - (Maj &lt;&lt; 8) | Min method.
    ''' Minimum value is Vista.  Older versions of Windows are not supported.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum WindowsVersionConstants

        ''' <summary>
        ''' Windows Vista
        ''' </summary>
        ''' <remarks></remarks>
        WindowsVista = &H600

        ''' <summary>
        ''' Windows 7
        ''' </summary>
        ''' <remarks></remarks>
        Windows7 = &H601

        ''' <summary>
        ''' Windows 8
        ''' </summary>
        ''' <remarks></remarks>
        Windows8 = &H602

        ''' <summary>
        ''' Windows 8.1
        ''' </summary>
        ''' <remarks></remarks>
        Windows81 = &H603

        ''' <summary>
        ''' Windows 10
        ''' </summary>
        ''' <remarks></remarks>
        Windows10 = &HA00

    End Enum

    ''' <summary>
    ''' Windows operating system suite masks.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum OSSuiteMask As UShort

        ''' <summary>
        ''' Microsoft BackOffice components are installed.
        ''' </summary>
        <Description("Microsoft BackOffice components are installed.")>
        BackOffice = &H4

        ''' <summary>
        ''' Windows Server 2003, Web Edition is installed.
        ''' </summary>
        <Description("Windows Server 2003, Web Edition is installed.")>
        Blade = &H400

        ''' <summary>
        ''' Windows Server 2003, Compute Cluster Edition is installed.
        ''' </summary>
        <Description("Windows Server 2003, Compute Cluster Edition is installed.")>
        ComputeServer = &H4000

        ''' <summary>
        ''' Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.
        ''' </summary>
        <Description("Windows Server 2008 Datacenter, Windows Server 2003, Datacenter Edition, or Windows 2000 Datacenter Server is installed.")>
        DataCenter = &H80

        ''' <summary>
        ''' Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.
        ''' </summary>
        <Description("Windows Server 2008 Enterprise, Windows Server 2003, Enterprise Edition, or Windows 2000 Advanced Server is installed. Refer to the Remarks section for more information about this bit flag.")>
        Enterprise = &H2

        ''' <summary>
        ''' Windows XP Embedded is installed.
        ''' </summary>
        <Description("Windows XP Embedded is installed.")>
        EmbeddedNT = &H40

        ''' <summary>
        ''' Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.
        ''' </summary>
        <Description("Windows Vista Home Premium, Windows Vista Home Basic, or Windows XP Home Edition is installed.")>
        Personal = &H200

        ''' <summary>
        ''' Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.
        ''' </summary>
        <Description("Remote Desktop is supported, but only one interactive session is supported. This value is set unless the system is running in application server mode.")>
        SingleUser = &H100

        ''' <summary>
        ''' Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.
        ''' </summary>
        <Description("Microsoft Small Business Server was once installed on the system, but may have been upgraded to another version of Windows. Refer to the Remarks section for more information about this bit flag.")>
        SmallBusiness = &H1

        ''' <summary>
        ''' Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.
        ''' </summary>
        <Description("Microsoft Small Business Server is installed with the restrictive client license in force. Refer to the Remarks section for more information about this bit flag.")>
        SmallBusinessRestricted = &H20

        ''' <summary>
        ''' Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.
        ''' </summary>
        <Description("Windows Storage Server 2003 R2 or Windows Storage Server 2003is installed.")>
        StorageServer = &H2000

        ''' <summary>
        ''' Terminal Services is installed. This value is always set.
        ''' If TERMINAL is set but SINGLEUSERTS is not set, the system is running in application server mode.
        ''' </summary>
        <Description("Terminal Services is installed. This value is always set.")>
        Terminal = &H10

        ''' <summary>
        ''' Windows Home Server is installed.
        ''' </summary>
        <Description("Windows Home Server is installed.")>
        HomeServer = &H8000

    End Enum

    ''' <summary>
    ''' Windows product type information.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum OSProductType As Byte

        ''' <summary>
        ''' The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        ''' </summary>
        <Description("The system is a domain controller and the operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")>
        NTDomainController = &H2

        ''' <summary>
        ''' The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
        ''' </summary>
        <Description("The operating system is Windows Server 2012 R2, Windows Server 2012, Windows Server 2008 R2, Windows Server 2008, Windows Server 2003, or Windows 2000 Server.")>
        NTServer = &H3

        ''' <summary>
        ''' The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.
        ''' </summary>
        <Description("The operating system is Windows 8.1, Windows 8, Windows 7, Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.")>
        NTWorkstation = &H1

    End Enum

#End Region

#Region "Structures"

    ''' <summary>
    ''' OSVERSIONINFOEX structure with additional utility and functionality.
    ''' </summary>
    ''' <remarks></remarks>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure OSVERSIONINFOEX

        ''' <summary>
        ''' The size of this structure, in bytes.
        ''' </summary>
        ''' <remarks></remarks>
        Public dwOSVersionInfoSize As Integer

        ''' <summary>
        ''' The major version of the current operating system.
        ''' </summary>
        ''' <remarks></remarks>
        Public dwMajorVersion As Integer

        ''' <summary>
        ''' The minor verison of the current operating system.
        ''' </summary>
        ''' <remarks></remarks>
        Public dwMinorVersion As Integer

        ''' <summary>
        ''' The build number of the current operating system.
        ''' </summary>
        ''' <remarks></remarks>
        Public dwBuildNumber As Integer

        ''' <summary>
        ''' The platform Id of the current operating system.
        ''' Currently, this value should always be 2.
        ''' </summary>
        ''' <remarks></remarks>
        Public dwPlatformId As Integer

        ''' <summary>
        ''' Private character buffer.
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.U2, SizeConst:=128)>
        Private szCSDVersionChar As Char()

        ''' <summary>
        ''' The Service Pack name (if applicable)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property szCSDVersion As String
            Get
                Return (CStr(szCSDVersionChar).Trim(ChrW(0)))
            End Get
        End Property

        ''' <summary>
        ''' Service pack major verison number.
        ''' </summary>
        ''' <remarks></remarks>
        Public wServicePackMajor As Short

        ''' <summary>
        ''' Server pack minor verison number.
        ''' </summary>
        ''' <remarks></remarks>
        Public wServicePackMinor As Short

        ''' <summary>
        ''' The Windows Suite mask.
        ''' </summary>
        ''' <remarks></remarks>
        Public wSuiteMask As OSSuiteMask

        ''' <summary>
        ''' The product type flags.
        ''' </summary>
        ''' <remarks></remarks>
        Public wProductType As OSProductType

        ''' <summary>
        ''' Reserved
        ''' </summary>
        ''' <remarks></remarks>
        Public wReserved As Byte

        ''' <summary>
        ''' Returns a verbose description of the operating system, including version and build number.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VerboseDescription As String
            Get
                Return String.Format("{0} {4} ({1}.{2} Build {3})", Description, dwMajorVersion, dwMinorVersion, dwBuildNumber, Architecture.ToString)
            End Get
        End Property

        ''' <summary>
        ''' Returns the processor architecture of the current system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Architecture As ArchitectureType
            Get
                Return _sysInfo.wProcessorArchitecture
            End Get
        End Property

        ''' <summary>
        ''' Returns the number of logical processors on the current system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Processors As Integer
            Get
                Return _sysInfo.dwNumberOfProcessors
            End Get
        End Property

        ''' <summary>
        ''' Returns the amount of total physical memory on the system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TotalPhysicalMemory As FriendlySizeLong
            Get
                Return _memInfo.ullTotalPhys
            End Get
        End Property

        ''' <summary>
        ''' Returns the amount of available physical memory currently on the system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AvailPhysicalMemory As FriendlySizeLong
            Get
                '' obviously this property is not very useful unless it is live, so let's
                '' refresh our data before sending out the result...
                GlobalMemoryStatusEx(_memInfo)
                Return _memInfo.ullAvailPhys
            End Get
        End Property

        ''' <summary>
        ''' Returns a description of the operating system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Description As String
            Get
                If szCSDVersion = "" Then
                    Return Name
                Else
                    Return Name & " " & szCSDVersion
                End If
            End Get
        End Property

        ''' <summary>
        ''' Returns the name of the operating system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                If key IsNot Nothing Then
                    Dim s As String = CStr(key.GetValue("ProductName"))
                    key.Close()
                    Return s
                End If

                Return Nothing

                '' see the OSVersionNames class, above, for more info.
                'Dim o As Object = OSVersionNames.FindVersion(Me)

                'If o Is Nothing Then
                '    Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                '    If key IsNot Nothing Then
                '        o = key.GetValue("ProductName")
                '        key.Close()
                '    End If
                'Else
                '    Return o.Name
                'End If

                'Return o
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is greater than or equal to Windows 10
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWindows10 As Boolean
            Get
                Return If(dwMajorVersion >= 10, True, False)
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is greater than or equal to Windows 8.1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWindows81 As Boolean
            Get
                Return If(dwMajorVersion > 6, True, (dwMajorVersion = 6) AndAlso (dwMinorVersion >= 3))
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is greater than or equal to Windows 8
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWindows8 As Boolean
            Get
                Return If(dwMajorVersion > 6, True, (dwMajorVersion = 6) AndAlso (dwMinorVersion >= 2))
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is greater than or equal to Windows 7
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsWindows7 As Boolean
            Get
                Return If(dwMajorVersion > 6, True, (dwMajorVersion = 6) AndAlso (dwMinorVersion >= 1))
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is greater than or equal to Windows Vista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsVista As Boolean
            Get
                Return If(dwMajorVersion > 6, True, (dwMajorVersion = 6) AndAlso (dwMinorVersion >= 0))
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is a server greater than or equal to Windows Server 2008
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsServer2008 As Boolean
            Get
                Return IsVista AndAlso IsServer
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is a server greater than or equal to Windows Server 2008 R2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsServer2008R2 As Boolean
            Get
                Return IsWindows7 AndAlso IsServer
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is a server greater than or equal to Windows Server 2012
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsServer2012 As Boolean
            Get
                Return IsWindows8 AndAlso IsServer
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this version is a server greater than or equal to Windows Server 2012 R2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsServer2012R2 As Boolean
            Get
                Return IsWindows81 AndAlso IsServer
            End Get
        End Property

        ''' <summary>
        ''' Returns true if the current operating system is a Windows Server OS.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsServer As Boolean
            Get
                Return wProductType <> OSProductType.NTWorkstation
            End Get
        End Property

        ''' <summary>
        ''' Returns true on a multi-user system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsMultiUser As Boolean
            Get
                Return (wSuiteMask And OSSuiteMask.SingleUser) = 0
            End Get
        End Property

        ''' <summary>
        ''' Returns the Windows version constant.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property WindowsVersion As WindowsVersionConstants
            Get
                Return CType(Me, WindowsVersionConstants)
            End Get
        End Property

        ''' <summary>
        ''' Converts this object into its string representation.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return Name
        End Function

        ''' <summary>
        ''' Returns the current computer's firmware type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FirmwareType As FirmwareType
            Get
                GetFirmwareType(FirmwareType)
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this system was booted from a virtual hard drive.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsVhdBoot As Boolean
            Get
                IsNativeVhdBoot(IsVhdBoot)
            End Get
        End Property

        Public Shared Narrowing Operator CType(operand As OSVERSIONINFOEX) As String
            Return operand.ToString
        End Operator

        Public Shared Narrowing Operator CType(operand As OSVERSIONINFOEX) As WindowsVersionConstants
            Return CType((operand.dwMajorVersion << 8) Or operand.dwMinorVersion, WindowsVersionConstants)
        End Operator

    End Structure


    ''' <summary>
    ''' Operating system version information
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure OSVERSIONINFO

        ''' <summary>
        ''' Size of this structure
        ''' </summary>
        Public OSVersionInfoSize As Integer

        ''' <summary>
        ''' Major version
        ''' </summary>
        Public MajorVersion As Integer

        ''' <summary>
        ''' Minor version
        ''' </summary>
        Public MinorVersion As Integer

        ''' <summary>
        ''' Build number
        ''' </summary>
        Public BuildNumber As Integer

        ''' <summary>
        ''' Platform ID
        ''' </summary>
        Public PlatformId As Integer

        ''' <summary>
        ''' CSD Version
        ''' </summary>
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)>
        Public CSDVersion As String

    End Structure


    ''' <summary>
    ''' Logical processor relationship 
    ''' </summary>
    Public Enum LOGICAL_PROCESSOR_RELATIONSHIP

        ''' <summary>
        ''' Processor core
        ''' </summary>
        RelationProcessorCore


        ''' <summary>
        ''' Numa Node
        ''' </summary>
        RelationNumaNode

        ''' <summary>
        ''' Cache
        ''' </summary>
        RelationCache

        ''' <summary>
        ''' Processor Package
        ''' </summary>
        RelationProcessorPackage

        ''' <summary>
        ''' Processor Group
        ''' </summary>
        RelationGroup

        ''' <summary>
        ''' All
        ''' </summary>
        RelationAll = &HFFFF
    End Enum

    ''' <summary>
    ''' Processor cache type
    ''' </summary>
    Public Enum PROCESSOR_CACHE_TYPE

        ''' <summary>
        ''' Unified
        ''' </summary>
        CacheUnified

        ''' <summary>
        ''' Instruction
        ''' </summary>
        CacheInstruction

        ''' <summary>
        ''' Data
        ''' </summary>
        CacheData


        ''' <summary>
        ''' Trace
        ''' </summary>
        CacheTrace
    End Enum

    ''' <summary>
    ''' Cache descriptor
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure CACHE_DESCRIPTOR
        ''' <summary>
        ''' Level
        ''' </summary>
        Public Level As Byte

        ''' <summary>
        ''' Associativity
        ''' </summary>
        Public Associativity As Byte

        ''' <summary>
        ''' Line size
        ''' </summary>
        Public LineSize As Short

        ''' <summary>
        ''' Size
        ''' </summary>
        Public Size As Integer

        ''' <summary>
        ''' Type
        ''' </summary>
        Public Type As PROCESSOR_CACHE_TYPE
    End Structure


    ''' <summary>
    ''' Logical processor information stucture.  Contains information about a logical processor on the local machine.
    ''' </summary>
    <StructLayout(LayoutKind.Explicit)>
    Public Structure SYSTEM_LOGICAL_PROCESSOR_INFORMATION

        ''' <summary>
        ''' Processor mask
        ''' </summary>
        <FieldOffset(0)>
        Public ProcessorMask As Long

        ''' <summary>
        ''' Processor relationship (entity kind)
        ''' </summary>

        <FieldOffset(8)>
        Public Relationship As LOGICAL_PROCESSOR_RELATIONSHIP

        ''' <summary>
        ''' Flags
        ''' </summary>
        <FieldOffset(12)>
        Public Flags As Byte

        ''' <summary>
        ''' Node number
        ''' </summary>
        <FieldOffset(12)>
        Public NodeNumber As Integer


        ''' <summary>
        ''' Cache descriptor
        ''' </summary>
        <FieldOffset(12)>
        Public CacheDescriptor As CACHE_DESCRIPTOR

        ''' <summary>
        ''' Reserved 1
        ''' </summary>
        <FieldOffset(12)>
        Public Reserved1 As Long

        ''' <summary>
        ''' Reserved 2
        ''' </summary>
        <FieldOffset(24)>
        Public Reserved2 As Long

    End Structure


    ''' <summary>
    ''' System information.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure SYSTEM_INFO

        ''' <summary>
        ''' Processor Architecture Type enumeration value.
        ''' </summary>
        Public wProcessorArchitecture As ArchitectureType

        ''' <summary>
        ''' Reserved
        ''' </summary>
        Public wReserved As Short

        ''' <summary>
        ''' System memory page size
        ''' </summary>
        Public dwPageSize As Integer

        ''' <summary>
        ''' Minimum allowed application memory address
        ''' </summary>
        Public lpMinimumApplicationAddress As UIntPtr

        ''' <summary>
        ''' Maximum allowed application memory address
        ''' </summary>
        Public lpMaximumApplicationAddress As UIntPtr

        ''' <summary>
        ''' Active processor mask
        ''' </summary>
        Public dwActiveProcessorMask As Integer

        ''' <summary>
        ''' Number of processors on the local machine
        ''' </summary>
        Public dwNumberOfProcessors As Integer

        ''' <summary>
        ''' Processor type
        ''' </summary>
        Public dwProcessorType As Integer

        ''' <summary>
        ''' Allocation granularity
        ''' </summary>
        Public dwAllocationGranularity As Integer

        ''' <summary>
        ''' Processor level
        ''' </summary>
        Public wProcessorLevel As Short

        ''' <summary>
        ''' Processor revision
        ''' </summary>
        Public wProcessorRevision As Short
    End Structure

    ''' <summary>
    ''' MEMORYSTATUSEX structure
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MEMORYSTATUSEX

        ''' <summary>
        ''' Length of this structure
        ''' </summary>
        Public dwLength As Integer

        ''' <summary>
        ''' Memory load
        ''' </summary>
        Public dwMemoryLoad As Integer

        ''' <summary>
        ''' Total physical memory on the machine
        ''' </summary>
        Public ullTotalPhys As ULong

        ''' <summary>
        ''' Total available memroy on the machine
        ''' </summary>
        Public ullAvailPhys As ULong

        ''' <summary>
        ''' Total paging file capacity
        ''' </summary>
        Public ullTotalPageFile As ULong

        ''' <summary>
        ''' Available paging file capacity
        ''' </summary>
        Public ullAvailPageFile As ULong

        ''' <summary>
        ''' Total virtual memory 
        ''' </summary>
        Public ullTotalVirtual As ULong

        ''' <summary>
        ''' Available virtual memory
        ''' </summary>
        Public ullAvailVirtual As ULong


        ''' <summary>
        ''' Available extended virtual memory
        ''' </summary>
        Public ullAvailExtendedVirtual As ULong
    End Structure

#End Region

#Region "OSVersionNames Class"

    ''' <summary>
    ''' Contains a static list of Windows versions and their characteristics.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class OSVersionNames
        Private _Name As String
        Private _VerMaj As Integer
        Private _VerMin As Integer
        Private _Server As Boolean

        Private Shared _col As New List(Of OSVersionNames)

        ''' <summary>
        ''' Returns a list of all OS versions recognized by this assembly.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Versions As List(Of OSVersionNames)
            Get
                Return _col
            End Get
        End Property

        ''' <summary>
        ''' Finds the OSVersionNames object for the operating system data specified in the osx parameter.
        ''' </summary>
        ''' <param name="osx">Version information of the operating system for which to retrieve the object.</param>
        ''' <returns>An OSVersionNames object.</returns>
        ''' <remarks></remarks>
        Public Shared Function FindVersion(osx As OSVERSIONINFOEX) As OSVersionNames
            Return FindVersion(osx.dwMajorVersion, osx.dwMinorVersion, (osx.wProductType <> OSProductType.NTWorkstation))
        End Function

        ''' <summary>
        ''' Finds the OSVersionNames object for the operating system data specified.
        ''' </summary>
        ''' <param name="verMaj">The major version of the operating system to find.</param>
        ''' <param name="verMin">The minor version of the operating system to find.</param>
        ''' <param name="server">Indicates that the querent is looking for a server OS.</param>
        ''' <returns>An OSVersionNames object.</returns>
        ''' <remarks></remarks>
        Public Shared Function FindVersion(verMaj As Integer, verMin As Integer, Optional server As Boolean = False) As OSVersionNames
            For Each v In _col
                If v.VersionMajor = verMaj AndAlso v.VersionMinor = verMin AndAlso v.Server = server Then Return v
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Returns the name of the operating system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        ''' <summary>
        ''' Returns the major version
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VersionMajor As Integer
            Get
                Return _VerMaj
            End Get
        End Property

        ''' <summary>
        ''' Returns the minor version
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VersionMinor As Integer
            Get
                Return _VerMin
            End Get
        End Property

        ''' <summary>
        ''' Indicates that this version is a server.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Server As Boolean
            Get
                Return _Server
            End Get
        End Property

        Protected Sub New(name As String, vermaj As Integer, vermin As Integer, Optional serv As Boolean = False)
            _Name = name
            _VerMaj = vermaj
            _VerMin = vermin
            _Server = serv
        End Sub

        Shared Sub New()

            _col.Add(New OSVersionNames("Windows 10", 10, 0, False))
            _col.Add(New OSVersionNames("Windows Server 10", 10, 0, True))

            _col.Add(New OSVersionNames("Windows 8.1", 6, 3, False))
            _col.Add(New OSVersionNames("Windows Server 2012 R2", 6, 3, True))

            _col.Add(New OSVersionNames("Windows 8", 6, 2, False))
            _col.Add(New OSVersionNames("Windows Server 2012", 6, 2, True))

            _col.Add(New OSVersionNames("Windows 7", 6, 1, False))
            _col.Add(New OSVersionNames("Windows Server 2008 R2", 6, 1, True))

            _col.Add(New OSVersionNames("Windows Vista", 6, 0, False))
            _col.Add(New OSVersionNames("Windows Server 2008", 6, 0, True))

            '' Nothing in this project is designed to work on Window XP, so looking for it is pointless.
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function

    End Class

#End Region

#Region "System Info Initialization"

    ''' <summary>
    ''' System information.
    ''' </summary>
    Public Module SysInfo

        Friend Declare Unicode Function IsWindowsXPOrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsXPSP1OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsXPSP2OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsXPSP3OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsVistaOrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsVistaSP1OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsVistaSP2OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindows7OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindows7SP1OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindows8OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindows8Point10OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindows10OrGreater Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsServer Lib "kernel32.dll" () As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function IsWindowsVersionOrGreater Lib "kernel32.dll" (maj As UShort, min As UShort, servicePack As UShort) As <MarshalAs(UnmanagedType.Bool)> Boolean
        Friend Declare Unicode Function GetVersionExW Lib "kernel32" (<MarshalAs(UnmanagedType.Struct)> ByRef pData As OSVERSIONINFO) As Integer
        Friend Declare Unicode Function GetVersionExW Lib "kernel32" (<MarshalAs(UnmanagedType.Struct)> ByRef pData As OSVERSIONINFOEX) As Integer
        Friend Declare Unicode Function GetFirmwareType Lib "kernel32" (ByRef firmwareType As FirmwareType) As Boolean
        Friend Declare Unicode Function IsNativeVhdBoot Lib "kernel32" (ByRef nativeVhd As Boolean) As Boolean
        Friend Declare Unicode Sub GetNativeSystemInfo Lib "kernel32" (ByRef lpSysInfo As SYSTEM_INFO)
        Friend Declare Unicode Function GetLogicalProcessorInformation Lib "kernel32" (buffer As IntPtr, ByRef ReturnLength As Integer) As Boolean
        Friend Declare Unicode Function GlobalMemoryStatusEx Lib "kernel32" (ByRef lpStatusEx As MEMORYSTATUSEX) As Boolean

        ''' <summary>
        ''' Contains details about the current operating system and computing environment.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property OSInfo As OSVERSIONINFOEX = GetOSInfo()

        Friend _sysInfo As SYSTEM_INFO

        ''' <summary>
        ''' Contains specific configuration details about the current native system mode.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SystemInfo As SYSTEM_INFO
            Get
                Return _sysInfo
            End Get
        End Property

        Friend _memInfo As MEMORYSTATUSEX

        ''' <summary>
        ''' Contains detailed memory information.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MemoryInfo As MEMORYSTATUSEX
            Get
                Return _memInfo
            End Get
        End Property


        Friend _procRaw() As SYSTEM_LOGICAL_PROCESSOR_INFORMATION

        ''' <summary>
        ''' Contains information about the logical processors on the system.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property LogicalProcessors As SYSTEM_LOGICAL_PROCESSOR_INFORMATION()
            Get
                Return _procRaw
            End Get
        End Property

        Sub New()

            '' let's get some version information!

            _memInfo.dwLength = Marshal.SizeOf(_memInfo)

            GetNativeSystemInfo(_sysInfo)
            GlobalMemoryStatusEx(_memInfo)

            '' now let's figure out how many processors we have on this system
            Dim mm As New MemPtr
            Dim org As MemPtr

            Dim lp As New SYSTEM_LOGICAL_PROCESSOR_INFORMATION
            Dim lRet As Integer = 0

            Dim rets() As SYSTEM_LOGICAL_PROCESSOR_INFORMATION
            Dim i As Integer
            Dim c As Integer

            '' The maximum number of processors for any version of Windows is 128, we'll allocate more for extra information.
            mm.Alloc(Marshal.SizeOf(lp) * 1024)

            '' record the original memory pointer
            org = mm

            lRet = CInt(mm.Length)

            GetLogicalProcessorInformation(mm, lRet)
            c = CInt(lRet / Marshal.SizeOf(lp))

            ReDim rets(c - 1)

            For i = 0 To c - 1
                rets(i) = mm.ToStruct(Of SYSTEM_LOGICAL_PROCESSOR_INFORMATION)()
                mm += Marshal.SizeOf(lp)

                '' what we're really after are the number of cores.
                If (rets(i).Relationship = LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore) Then
                    _sysInfo.dwNumberOfProcessors += 1
                End If
            Next

            '' store that data in case we need it for later.
            _procRaw = rets

            '' free our unmanaged resources.
            org.Free()

        End Sub

        ''' <summary>
        ''' Retrieves the current Operating System information.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function GetOSInfo() As OSVERSIONINFOEX
            Dim lpOS As New OSVERSIONINFOEX

            lpOS.dwOSVersionInfoSize = Marshal.SizeOf(lpOS)

            GetVersionExW(lpOS)

            Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
            If key IsNot Nothing Then

                Dim maj As Integer = CInt(key.GetValue("CurrentMajorVersionNumber"))
                Dim min As Integer = CInt(key.GetValue("CurrentMinorVersionNumber"))

                Dim build As Integer = CInt(key.GetValue("CurrentBuildNumber"))
                Dim name As String = CStr(key.GetValue("ProductName"))

                lpOS.dwMajorVersion = maj
                lpOS.dwMinorVersion = min
                lpOS.dwBuildNumber = build

                key.Close()

            End If

            'Try
            '    If IsWindowsVersionOrGreater(6, 4, 0) Then
            '        lpOS.dwMinorVersion = 4
            '    End If
            'Catch ex As Exception
            '    lpOS.dwMinorVersion = 4

            'End Try

            Return lpOS
        End Function

    End Module

#End Region

End Namespace