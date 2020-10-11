'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: DevProp
''         Native Device Properites.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System
Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Security
Imports DataTools.Interop.Native
Imports CoreCT.Memory
Imports CoreCT.Text

Namespace Native


    ''' <summary>
    ''' Bus types.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BusType

        ''' <summary>
        ''' Internal
        ''' </summary>
        Internal

        ''' <summary>
        ''' PCMCIA
        ''' </summary>
        PCMCIA

        ''' <summary>
        ''' PCI
        ''' </summary>
        PCI

        ''' <summary>
        ''' ISAPnP
        ''' </summary>
        ISAPnP

        ''' <summary>
        ''' EISA
        ''' </summary>
        EISA

        ''' <summary>
        ''' MCA
        ''' </summary>
        MCA

        ''' <summary>
        ''' Serenum
        ''' </summary>
        Serenum

        ''' <summary>
        ''' USB
        ''' </summary>
        USB

        ''' <summary>
        ''' Parallel Port
        ''' </summary>
        ParallelPort

        ''' <summary>
        ''' UsB Printer
        ''' </summary>
        USBPrinter

        ''' <summary>
        ''' DOT4 Dotmatrix Printer
        ''' </summary>
        DOT4Printer

        ''' <summary>
        ''' IEEE 1394 / Firewire
        ''' </summary>
        Bus1394

        ''' <summary>
        ''' Human Interface Device
        ''' </summary>
        HID

        ''' <summary>
        ''' AVC
        ''' </summary>
        AVC

        ''' <summary>
        ''' Infrared (IRDA) device
        ''' </summary>
        IRDA

        ''' <summary>
        ''' MicroSD card
        ''' </summary>
        SD

        ''' <summary>
        ''' ACPI
        ''' </summary>
        ACPI

        ''' <summary>
        ''' Software Device
        ''' </summary>
        SoftwareDevice

    End Enum

    ''' <summary>
    ''' Device properties dialog
    ''' </summary>
    Public Module DevPropDialog

        Declare Unicode Function DeviceProperties_RunDLL _
        Lib "devmgr.dll" _
        Alias "DeviceProperties_RunDLLW" (hwnd As IntPtr,
                                             hAppInstance As IntPtr,
                                             <MarshalAs(UnmanagedType.LPWStr)> cmdLine As String,
                                             nCmdShow As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        ''' <summary>
        ''' Opens the hardware properties dialog box for the specified instance id.
        ''' </summary>
        ''' <param name="InstanceId"></param>
        ''' <param name="hwnd"></param>
        ''' <remarks></remarks>
        Public Sub OpenDeviceProperties(InstanceId As String, Optional hwnd As IntPtr = Nothing)

            If InstanceId Is Nothing Then Return
            DeviceProperties_RunDLL(hwnd, Process.GetCurrentProcess.Handle, "/DeviceId """ & InstanceId & """", SW_SHOWNORMAL)

        End Sub

        Friend Function GuidToBusType(g As Guid) As BusType

            If g = GUID_BUS_TYPE_INTERNAL Then Return BusType.Internal
            If g = GUID_BUS_TYPE_PCMCIA Then Return BusType.PCMCIA
            If g = GUID_BUS_TYPE_PCI Then Return BusType.PCI
            If g = GUID_BUS_TYPE_ISAPNP Then Return BusType.ISAPnP
            If g = GUID_BUS_TYPE_EISA Then Return BusType.EISA
            If g = GUID_BUS_TYPE_MCA Then Return BusType.MCA
            If g = GUID_BUS_TYPE_SERENUM Then Return BusType.Serenum
            If g = GUID_BUS_TYPE_USB Then Return BusType.USB

            If g = GUID_BUS_TYPE_LPTENUM Then Return BusType.ParallelPort
            If g = GUID_BUS_TYPE_USBPRINT Then Return BusType.USBPrinter
            If g = GUID_BUS_TYPE_DOT4PRT Then Return BusType.DOT4Printer
            If g = GUID_BUS_TYPE_1394 Then Return BusType.Bus1394
            If g = GUID_BUS_TYPE_HID Then Return BusType.HID
            If g = GUID_BUS_TYPE_AVC Then Return BusType.AVC
            If g = GUID_BUS_TYPE_IRDA Then Return BusType.IRDA
            If g = GUID_BUS_TYPE_SD Then Return BusType.SD

            If g = GUID_BUS_TYPE_ACPI Then Return BusType.ACPI
            If g = GUID_BUS_TYPE_SW_DEVICE Then Return BusType.SoftwareDevice

            Return 0

        End Function

    End Module

    Friend Module DevProp

        Public Const DIGCF_DEFAULT = &H1
        Public Const DIGCF_PRESENT = &H2
        Public Const DIGCF_ALLCLASSES = &H4
        Public Const DIGCF_PROFILE = &H8
        Public Const DIGCF_DEVICEINTERFACE = &H10


        Public ReadOnly INVALID_HANDLE_VALUE As New IntPtr(-1)

#Region "SetupApi"
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SP_DEVICE_INTERFACE_DATA
            Public cbSize As UInteger
            Public InterfaceClassGuid As Guid
            Public Flags As UInteger
            Public Reserved As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SP_DEVICE_INTERFACE_DETAIL_DATA
            Public cbSize As UInteger

            '' this is a array of unknown size.
            Public DevicePath As Char
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SP_DEVINFO_DATA
            Public cbSize As UInteger
            Public ClassGuid As Guid
            Public DevInst As UInteger
            Public Reserved As IntPtr
        End Structure

        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True, CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiGetClassDevs _
        (ByRef ClassGuid As Guid, Enumerator As IntPtr, hwndParent As IntPtr, Flags As ClassDevFlags) As IntPtr
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupDiGetClassDevs", CharSet:=CharSet.Unicode, SetLastError:=True, CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiGetClassDevsNoRef _
        (ClassGuid As Guid, Enumerator As IntPtr, hwndParent As IntPtr, Flags As ClassDevFlags) As IntPtr
        End Function


        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True, CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiGetDevicePropertyKeys(hDev As IntPtr,
                    DeviceInfoData As SP_DEVINFO_DATA,
                    PropertyKeyArray As IntPtr,
                    PropertyKeyCount As UInteger,
                    ByRef RequiredPropertyKeyCount As UInteger,
                    Flags As UInteger) As Boolean

        End Function

        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True,
        CallingConvention:=CallingConvention.StdCall, PreserveSig:=True,
        EntryPoint:="SetupDiGetDevicePropertyW")>
        Public Function SetupDiGetDeviceProperty _
        (DeviceInfoSet As IntPtr,
         ByRef DeviceInfoData As SP_DEVINFO_DATA,
         ByRef PropertyKey As DEVPROPKEY,
         <Out> ByRef PropertyType As UInteger,
         PropertyBuffer As IntPtr,
         PropertyBufferSize As UInteger,
         <Out> ByRef RequiredSize As UInteger,
         Flags As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function




        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True,
        CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiLoadClassIcon _
        (ByRef ClassGuid As Guid,
         ByRef hIcon As IntPtr,
         ByRef MiniIconIndex As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True,
        CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiLoadDeviceIcon(hdev As IntPtr,
                                          ByRef DeviceInfoData As SP_DEVINFO_DATA,
                                          cxIcon As UInteger,
                                          cyIcon As UInteger,
                                          Flags As UInteger,
                                          ByRef hIcon As IntPtr) As Boolean
        End Function


        Public Declare Unicode Function SetupDiGetClassProperty _
        Lib "setupapi.dll" _
        Alias "SetupDiGetClassPropertyW" _
        (ClassGuid As Guid,
         PropertyKey As DEVPROPKEY,
         ByRef propertyType As Integer,
         propertyBuffer As IntPtr,
         propertyBufferSize As Integer,
         ByRef RequiredSize As Integer,
         Flags As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function SetupDiDestroyDeviceInfoList Lib "setupapi.dll" _
        (DeviceInfoSet As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function SetupDiEnumDeviceInterfaces Lib "setupapi.dll" _
        (DeviceInfoSet As IntPtr,
         ByRef DeviceInfoData As SP_DEVINFO_DATA,
         ByRef InterfaceClassGuid As Guid,
         MemberIndex As UInteger,
         ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function SetupDiEnumDeviceInterfaces Lib "setupapi.dll" _
        (DeviceInfoSet As IntPtr,
         DeviceInfoData As IntPtr,
         ByRef InterfaceClassGuid As Guid,
         MemberIndex As UInteger,
         ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function SetupDiEnumDeviceInterfaces Lib "setupapi.dll" _
        (DeviceInfoSet As IntPtr,
         DeviceInfoData As IntPtr,
         ByRef InterfaceClassGuid As Guid,
         MemberIndex As UInteger,
         DeviceInterfaceData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


        <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True,
        CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        Public Function SetupDiEnumDeviceInfo _
        (DeviceInfoSet As IntPtr,
         MemberIndex As UInteger,
         <Out> ByRef DeviceInfoData As SP_DEVINFO_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        '<DllImport("setupapi.dll", EntryPoint:="SetupDiGetDeviceInterfaceDetailW", CharSet:=CharSet.Unicode, SetLastError:=True, _
        '    CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        'Public Function SetupDiGetDeviceInterfaceDetail _
        '    (DeviceInfoSet As IntPtr, _
        '     <MarshalAs(UnmanagedType.Struct)> DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        '     DeviceInterfaceDetailData As SafeHandle, _
        '     DeviceInterfaceDetailDataSize As UInteger, _
        '     ByRef RequiredSize As UInteger, _
        '     DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        'End Function

        '<DllImport("setupapi.dll", EntryPoint:="SetupDiGetDeviceInterfaceDetailW", CharSet:=CharSet.Unicode, SetLastError:=True, _
        '    CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        'Public Function SetupDiGetDeviceInterfaceDetail _
        '    (DeviceInfoSet As IntPtr, _
        '     <MarshalAs(UnmanagedType.Struct)> DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        '     DeviceInterfaceDetailData As SafeHandle, _
        '     DeviceInterfaceDetailDataSize As UInteger, _
        '     ByRef RequiredSize As UInteger, _
        '     <MarshalAs(UnmanagedType.Struct)> DeviceInfoData As SP_DEVINFO_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean
        'End Function

        'Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        'Alias "SetupDiGetDeviceInterfaceDetailW" _
        '(DeviceInfoSet As IntPtr, _
        ' ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        ' ByRef DeviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA, _
        ' DeviceInterfaceDetailDataSize As UInteger, _
        ' ByRef RequiredSize As UInteger, _
        ' ByRef DeviceInfoData As SP_DEVINFO_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
    Alias "SetupDiGetDeviceInterfaceDetailW" _
        (<[In]> DeviceInfoSet As IntPtr,
         <[In], MarshalAs(UnmanagedType.Struct)>
         ByRef beviceInterfaceData As SP_DEVICE_INTERFACE_DATA,
         <Out, [In]> DeviceInterfaceDetailData As IntPtr,
         <[In]> DeviceInterfaceDetailDataSize As UInteger,
         <Out> ByRef RequiredSize As UInteger,
         <Out> DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        'Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        'Alias "SetupDiGetDeviceInterfaceDetailW" _
        '    (DeviceInfoSet As IntPtr, _
        '     DeviceInterfaceData As IntPtr, _
        '     DeviceInterfaceDetailData As IntPtr, _
        '     DeviceInterfaceDetailDataSize As UInteger, _
        '     ByRef RequiredSize As UInteger, _
        '     DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        'Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        'Alias "SetupDiGetDeviceInterfaceDetailW" _
        '    (DeviceInfoSet As IntPtr, _
        '     ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        '     ByRef DeviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA, _
        '     DeviceInterfaceDetailDataSize As UInteger, _
        '     ByRef RequiredSize As UInteger, _
        '     DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


#End Region

#Region "DevClass Definitions"
        Public ReadOnly GUID_DEVCLASS_1394 As New Guid(&H6BDD1FC1L, &H810F, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_1394DEBUG As New Guid(&H66F250D6L, &H7801, &H4A64, &HB1, &H39, &HEE, &HA8, &HA, &H45, &HB, &H24)
        Public ReadOnly GUID_DEVCLASS_61883 As New Guid(&H7EBEFBC0L, &H3200, &H11D2, &HB4, &HC2, &H0, &HA0, &HC9, &H69, &H7D, &H7)
        Public ReadOnly GUID_DEVCLASS_ADAPTER As New Guid(&H4D36E964L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_APMSUPPORT As New Guid(&HD45B1C18L, &HC8FA, &H11D1, &H9F, &H77, &H0, &H0, &HF8, &H5, &HF5, &H30)
        Public ReadOnly GUID_DEVCLASS_AVC As New Guid(&HC06FF265L, &HAE09, &H48F0, &H81, &H2C, &H16, &H75, &H3D, &H7C, &HBA, &H83)
        Public ReadOnly GUID_DEVCLASS_BATTERY As New Guid(&H72631E54L, &H78A4, &H11D0, &HBC, &HF7, &H0, &HAA, &H0, &HB7, &HB3, &H2A)
        Public ReadOnly GUID_DEVCLASS_BIOMETRIC As New Guid(&H53D29EF7L, &H377C, &H4D14, &H86, &H4B, &HEB, &H3A, &H85, &H76, &H93, &H59)
        Public ReadOnly GUID_DEVCLASS_BLUETOOTH As New Guid(&HE0CBF06CL, &HCD8B, &H4647, &HBB, &H8A, &H26, &H3B, &H43, &HF0, &HF9, &H74)
        Public ReadOnly GUID_DEVCLASS_CDROM As New Guid(&H4D36E965L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_COMPUTER As New Guid(&H4D36E966L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_DECODER As New Guid(&H6BDD1FC2L, &H810F, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_DISKDRIVE As New Guid(&H4D36E967L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_DISPLAY As New Guid(&H4D36E968L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_DOT4 As New Guid(&H48721B56L, &H6795, &H11D2, &HB1, &HA8, &H0, &H80, &HC7, &H2E, &H74, &HA2)
        Public ReadOnly GUID_DEVCLASS_DOT4PRINT As New Guid(&H49CE6AC8L, &H6F86, &H11D2, &HB1, &HE5, &H0, &H80, &HC7, &H2E, &H74, &HA2)
        Public ReadOnly GUID_DEVCLASS_ENUM1394 As New Guid(&HC459DF55L, &HDB08, &H11D1, &HB0, &H9, &H0, &HA0, &HC9, &H8, &H1F, &HF6)
        Public ReadOnly GUID_DEVCLASS_FDC As New Guid(&H4D36E969L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_FLOPPYDISK As New Guid(&H4D36E980L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_GPS As New Guid(&H6BDD1FC3L, &H810F, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_HDC As New Guid(&H4D36E96AL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_HIDCLASS As New Guid(&H745A17A0L, &H74D3, &H11D0, &HB6, &HFE, &H0, &HA0, &HC9, &HF, &H57, &HDA)
        Public ReadOnly GUID_DEVCLASS_IMAGE As New Guid(&H6BDD1FC6L, &H810F, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_INFINIBAND As New Guid(&H30EF7132L, &HD858, &H4A0C, &HAC, &H24, &HB9, &H2, &H8A, &H5C, &HCA, &H3F)
        Public ReadOnly GUID_DEVCLASS_INFRARED As New Guid(&H6BDD1FC5L, &H810F, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_KEYBOARD As New Guid(&H4D36E96BL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_LEGACYDRIVER As New Guid(&H8ECC055DL, &H47F, &H11D1, &HA5, &H37, &H0, &H0, &HF8, &H75, &H3E, &HD1)
        Public ReadOnly GUID_DEVCLASS_MEDIA As New Guid(&H4D36E96CL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MEDIUM_CHANGER As New Guid(&HCE5939AEL, &HEBDE, &H11D0, &HB1, &H81, &H0, &H0, &HF8, &H75, &H3E, &HC4)
        Public ReadOnly GUID_DEVCLASS_MEMORY As New Guid(&H5099944AL, &HF6B9, &H4057, &HA0, &H56, &H8C, &H55, &H2, &H28, &H54, &H4C)
        Public ReadOnly GUID_DEVCLASS_MODEM As New Guid(&H4D36E96DL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MONITOR As New Guid(&H4D36E96EL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MOUSE As New Guid(&H4D36E96FL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MTD As New Guid(&H4D36E970L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MULTIFUNCTION As New Guid(&H4D36E971L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_MULTIPORTSERIAL As New Guid(&H50906CB8L, &HBA12, &H11D1, &HBF, &H5D, &H0, &H0, &HF8, &H5, &HF5, &H30)
        Public ReadOnly GUID_DEVCLASS_NET As New Guid(&H4D36E972L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_NETCLIENT As New Guid(&H4D36E973L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_NETSERVICE As New Guid(&H4D36E974L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_NETTRANS As New Guid(&H4D36E975L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_NODRIVER As New Guid(&H4D36E976L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_PCMCIA As New Guid(&H4D36E977L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_PNPPRINTERS As New Guid(&H4658EE7EL, &HF050, &H11D1, &HB6, &HBD, &H0, &HC0, &H4F, &HA3, &H72, &HA7)
        Public ReadOnly GUID_DEVCLASS_PORTS As New Guid(&H4D36E978L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_PRINTER As New Guid(&H4D36E979L, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)

        '' Proud of myself for this one.
        Public ReadOnly GUID_DEVCLASS_PRINTER_QUEUE As New Guid(&H1ED2BBF9, &H11F0, &H4084, &HB2, &H1F, &HAD, &H83, &HA8, &HE6, &HDC, &HDC)

        Public ReadOnly GUID_DEVCLASS_PRINTERUPGRADE As New Guid(&H4D36E97AL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_PROCESSOR As New Guid(&H50127DC3L, &HF36, &H415E, &HA6, &HCC, &H4C, &HB3, &HBE, &H91, &HB, &H65)
        Public ReadOnly GUID_DEVCLASS_SBP2 As New Guid(&HD48179BEL, &HEC20, &H11D1, &HB6, &HB8, &H0, &HC0, &H4F, &HA3, &H72, &HA7)
        Public ReadOnly GUID_DEVCLASS_SCSIADAPTER As New Guid(&H4D36E97BL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_SECURITYACCELERATOR As New Guid(&H268C95A1L, &HEDFE, &H11D3, &H95, &HC3, &H0, &H10, &HDC, &H40, &H50, &HA5)
        Public ReadOnly GUID_DEVCLASS_SENSOR As New Guid(&H5175D334L, &HC371, &H4806, &HB3, &HBA, &H71, &HFD, &H53, &HC9, &H25, &H8D)
        Public ReadOnly GUID_DEVCLASS_SIDESHOW As New Guid(&H997B5D8DL, &HC442, &H4F2E, &HBA, &HF3, &H9C, &H8E, &H67, &H1E, &H9E, &H21)
        Public ReadOnly GUID_DEVCLASS_SMARTCARDREADER As New Guid(&H50DD5230L, &HBA8A, &H11D1, &HBF, &H5D, &H0, &H0, &HF8, &H5, &HF5, &H30)
        Public ReadOnly GUID_DEVCLASS_SOUND As New Guid(&H4D36E97CL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_SYSTEM As New Guid(&H4D36E97DL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_TAPEDRIVE As New Guid(&H6D807884L, &H7D21, &H11CF, &H80, &H1C, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_UNKNOWN As New Guid(&H4D36E97EL, &HE325, &H11CE, &HBF, &HC1, &H8, &H0, &H2B, &HE1, &H3, &H18)
        Public ReadOnly GUID_DEVCLASS_USB As New Guid(&H36FC9E60L, &HC465, &H11CF, &H80, &H56, &H44, &H45, &H53, &H54, &H0, &H0)
        Public ReadOnly GUID_DEVCLASS_VOLUME As New Guid(&H71A27CDDL, &H812A, &H11D0, &HBE, &HC7, &H8, &H0, &H2B, &HE2, &H9, &H2F)
        Public ReadOnly GUID_DEVCLASS_VOLUMESNAPSHOT As New Guid(&H533C5B84L, &HEC70, &H11D2, &H95, &H5, &H0, &HC0, &H4F, &H79, &HDE, &HAF)
        Public ReadOnly GUID_DEVCLASS_WCEUSBS As New Guid(&H25DBCE51L, &H6C8F, &H4A72, &H8A, &H6D, &HB5, &H4C, &H2B, &H4F, &HC8, &H35)
        Public ReadOnly GUID_DEVCLASS_WPD As New Guid(&HEEC5AD98L, &H8080, &H425F, &H92, &H2A, &HDA, &HBF, &H3D, &HE3, &HF6, &H9A)
        Public ReadOnly GUID_DEVCLASS_EHSTORAGESILO As New Guid(&H9DA2B80FL, &HF89F, &H4A49, &HA5, &HC2, &H51, &H1B, &H8, &H5B, &H9E, &H8A)
        Public ReadOnly GUID_DEVCLASS_FIRMWARE As New Guid(&HF2E7DD72L, &H6468, &H4E36, &HB6, &HF1, &H64, &H88, &HF4, &H2C, &H1B, &H52)
        Public ReadOnly GUID_DEVCLASS_EXTENSION As New Guid(&HE2F84CE7L, &H8EFA, &H411C, &HAA, &H69, &H97, &H45, &H4C, &HA4, &HCB, &H57)

        ''
        '' Define filesystem filter classes used for classification and load ordering.
        '' Classes are listed below in order from "highest" (i.e., farthest from the
        '' filesystem) to "lowest" (i.e., closest to the filesystem).
        ''
        Public ReadOnly GUID_DEVCLASS_FSFILTER_TOP As New Guid(&HB369BAF4L, &H5568, &H4E82, &HA8, &H7E, &HA9, &H3E, &HB1, &H6B, &HCA, &H87)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_ACTIVITYMONITOR As New Guid(&HB86DFF51L, &HA31E, &H4BAC, &HB3, &HCF, &HE8, &HCF, &HE7, &H5C, &H9F, &HC2)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_UNDELETE As New Guid(&HFE8F1572L, &HC67A, &H48C0, &HBB, &HAC, &HB, &H5C, &H6D, &H66, &HCA, &HFB)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_ANTIVIRUS As New Guid(&HB1D1A169L, &HC54F, &H4379, &H81, &HDB, &HBE, &HE7, &HD8, &H8D, &H74, &H54)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_REPLICATION As New Guid(&H48D3EBC4L, &H4CF8, &H48FF, &HB8, &H69, &H9C, &H68, &HAD, &H42, &HEB, &H9F)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_CONTINUOUSBACKUP As New Guid(&H71AA14F8L, &H6FAD, &H4622, &HAD, &H77, &H92, &HBB, &H9D, &H7E, &H69, &H47)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_CONTENTSCREENER As New Guid(&H3E3F0674L, &HC83C, &H4558, &HBB, &H26, &H98, &H20, &HE1, &HEB, &HA5, &HC5)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_QUOTAMANAGEMENT As New Guid(&H8503C911L, &HA6C7, &H4919, &H8F, &H79, &H50, &H28, &HF5, &H86, &H6B, &HC)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_SYSTEMRECOVERY As New Guid(&H2DB15374L, &H706E, &H4131, &HA0, &HC7, &HD7, &HC7, &H8E, &HB0, &H28, &H9A)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_CFSMETADATASERVER As New Guid(&HCDCF0939L, &HB75B, &H4630, &HBF, &H76, &H80, &HF7, &HBA, &H65, &H58, &H84)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_HSM As New Guid(&HD546500AL, &H2AEB, &H45F6, &H94, &H82, &HF4, &HB1, &H79, &H9C, &H31, &H77)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_COMPRESSION As New Guid(&HF3586BAFL, &HB5AA, &H49B5, &H8D, &H6C, &H5, &H69, &H28, &H4C, &H63, &H9F)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_ENCRYPTION As New Guid(&HA0A701C0L, &HA511, &H42FF, &HAA, &H6C, &H6, &HDC, &H3, &H95, &H57, &H6F)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_VIRTUALIZATION As New Guid(&HF75A86C0L, &H10D8, &H4C3A, &HB2, &H33, &HED, &H60, &HE4, &HCD, &HFA, &HAC)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_PHYSICALQUOTAMANAGEMENT As New Guid(&H6A0A8E78L, &HBBA6, &H4FC4, &HA7, &H9, &H1E, &H33, &HCD, &H9, &HD6, &H7E)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_OPENFILEBACKUP As New Guid(&HF8ECAFA6L, &H66D1, &H41A5, &H89, &H9B, &H66, &H58, &H5D, &H72, &H16, &HB7)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_SECURITYENHANCER As New Guid(&HD02BC3DAL, &HC8E, &H4945, &H9B, &HD5, &HF1, &H88, &H3C, &H22, &H6C, &H8C)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_COPYPROTECTION As New Guid(&H89786FF1L, &H9C12, &H402F, &H9C, &H9E, &H17, &H75, &H3C, &H7F, &H43, &H75)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_BOTTOM As New Guid(&H37765EA0L, &H5958, &H4FC9, &HB0, &H4B, &H2F, &HDF, &HEF, &H97, &HE5, &H9E)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_SYSTEM As New Guid(&H5D1B9AAAL, &H1E2, &H46AF, &H84, &H9F, &H27, &H2B, &H3F, &H32, &H4C, &H46)
        Public ReadOnly GUID_DEVCLASS_FSFILTER_INFRASTRUCTURE As New Guid(&HE55FA6F9L, &H128C, &H4D04, &HAB, &HAB, &H63, &HC, &H74, &HB1, &H45, &H3A)

        Public ReadOnly BUS1394_CLASS_GUID As New Guid("6BDD1FC1-810F-11d0-BEC7-08002BE2092F")
        Public ReadOnly GUID_61883_CLASS As New Guid("7EBEFBC0-3200-11d2-B4C2-00A0C9697D07")
        Public ReadOnly GUID_DEVICE_APPLICATIONLAUNCH_BUTTON As New Guid("629758EE-986E-4D9E-8E47-DE27F8AB054D")
        Public ReadOnly GUID_DEVICE_BATTERY As New Guid("72631E54-78A4-11D0-BCF7-00AA00B7B32A")
        Public ReadOnly GUID_DEVICE_LID As New Guid("4AFA3D52-74A7-11d0-be5e-00A0C9062857")
        Public ReadOnly GUID_DEVICE_MEMORY As New Guid("3FD0F03D-92E0-45FB-B75C-5ED8FFB01021")
        Public ReadOnly GUID_DEVICE_MESSAGE_INDICATOR As New Guid("CD48A365-FA94-4CE2-A232-A1B764E5D8B4")
        Public ReadOnly GUID_DEVICE_PROCESSOR As New Guid("97FADB10-4E33-40AE-359C-8BEF029DBDD0")
        Public ReadOnly GUID_DEVICE_SYS_BUTTON As New Guid("4AFA3D53-74A7-11d0-be5e-00A0C9062857")
        Public ReadOnly GUID_DEVICE_THERMAL_ZONE As New Guid("4AFA3D51-74A7-11d0-be5e-00A0C9062857")
        Public ReadOnly GUID_BTHPORT_DEVICE_INTERFACE As New Guid("0850302A-B344-4fda-9BE9-90576B8D46F0")
        Public ReadOnly GUID_DEVINTERFACE_BRIGHTNESS As New Guid("FDE5BBA4-B3F9-46FB-BDAA-0728CE3100B4")
        Public ReadOnly GUID_DEVINTERFACE_DISPLAY_ADAPTER As New Guid("5B45201D-F2F2-4F3B-85BB-30FF1F953599")
        Public ReadOnly GUID_DEVINTERFACE_I2C As New Guid("2564AA4F-DDDB-4495-B497-6AD4A84163D7")
        Public ReadOnly GUID_DEVINTERFACE_IMAGE As New Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F")
        Public ReadOnly GUID_DEVINTERFACE_MONITOR As New Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7")
        Public ReadOnly GUID_DEVINTERFACE_OPM As New Guid("BF4672DE-6B4E-4BE4-A325-68A91EA49C09")
        Public ReadOnly GUID_DEVINTERFACE_VIDEO_OUTPUT_ARRIVAL As New Guid("1AD9E4F0-F88D-4360-BAB9-4C2D55E564CD")
        Public ReadOnly GUID_DISPLAY_DEVICE_ARRIVAL As New Guid("1CA05180-A699-450A-9A0C-DE4FBE3DDD89")
        Public ReadOnly GUID_DEVINTERFACE_HID As New Guid("4D1E55B2-F16F-11CF-88CB-001111000030")
        Public ReadOnly GUID_DEVINTERFACE_KEYBOARD As New Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd")
        Public ReadOnly GUID_DEVINTERFACE_MOUSE As New Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD")
        Public ReadOnly GUID_DEVINTERFACE_PRINTER As New Guid("{0ECEF634-6EF0-472A-8085-5AD023ECBCCD}")
        Public ReadOnly GUID_DEVINTERFACE_MODEM As New Guid("2C7089AA-2E0E-11D1-B114-00C04FC2AAE4")
        Public ReadOnly GUID_DEVINTERFACE_NET As New Guid("CAC88484-7515-4C03-82E6-71A87ABAC361")
        Public ReadOnly GUID_DEVINTERFACE_SENSOR As New Guid(&HBA1BB692L, &H9B7A, &H4833, &H9A, &H1E, &H52, &H5E, &HD1, &H34, &HE7, &HE2)
        Public ReadOnly GUID_DEVINTERFACE_COMPORT As New Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73")
        Public ReadOnly GUID_DEVINTERFACE_PARALLEL As New Guid("97F76EF0-F883-11D0-AF1F-0000F800845C")
        Public ReadOnly GUID_DEVINTERFACE_PARCLASS As New Guid("811FC6A5-F728-11D0-A537-0000F8753ED1")
        Public ReadOnly GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR As New Guid("4D36E978-E325-11CE-BFC1-08002BE10318")
        Public ReadOnly GUID_DEVINTERFACE_CDCHANGER As New Guid("53F56312-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_CDROM As New Guid("53F56308-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_DISK As New Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_FLOPPY As New Guid("53F56311-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_MEDIUMCHANGER As New Guid("53F56310-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_PARTITION As New Guid("53F5630A-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_STORAGEPORT As New Guid("2ACCFE60-C130-11D2-B082-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_TAPE As New Guid("53F5630B-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_VOLUME As New Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_DEVINTERFACE_WRITEONCEDISK As New Guid("53F5630C-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_IO_VOLUME_DEVICE_INTERFACE As New Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly MOUNTDEV_MOUNTED_DEVICE_GUID As New Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B")
        Public ReadOnly GUID_AVC_CLASS As New Guid("095780C3-48A1-4570-BD95-46707F78C2DC")
        Public ReadOnly GUID_VIRTUAL_AVC_CLASS As New Guid("616EF4D0-23CE-446D-A568-C31EB01913D0")
        Public ReadOnly KSCATEGORY_ACOUSTIC_ECHO_CANCEL As New Guid("BF963D80-C559-11D0-8A2B-00A0C9255AC1")
        Public ReadOnly KSCATEGORY_AUDIO As New Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196")
        Public ReadOnly KSCATEGORY_AUDIO_DEVICE As New Guid("FBF6F530-07B9-11D2-A71E-0000F8004788")
        Public ReadOnly KSCATEGORY_AUDIO_GFX As New Guid("9BAF9572-340C-11D3-ABDC-00A0C90AB16F")
        Public ReadOnly KSCATEGORY_AUDIO_SPLITTER As New Guid("9EA331FA-B91B-45F8-9285-BD2BC77AFCDE")
        Public ReadOnly KSCATEGORY_BDA_IP_SINK As New Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0")
        Public ReadOnly KSCATEGORY_BDA_NETWORK_EPG As New Guid("71985F49-1CA1-11d3-9CC8-00C04F7971E0")
        Public ReadOnly KSCATEGORY_BDA_NETWORK_PROVIDER As New Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0")
        Public ReadOnly KSCATEGORY_BDA_NETWORK_TUNER As New Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0")
        Public ReadOnly KSCATEGORY_BDA_RECEIVER_COMPONENT As New Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0")
        Public ReadOnly KSCATEGORY_BDA_TRANSPORT_INFORMATION As New Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E")
        Public ReadOnly KSCATEGORY_BRIDGE As New Guid("085AFF00-62CE-11CF-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_CAPTURE As New Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196")
        Public ReadOnly KSCATEGORY_CLOCK As New Guid("53172480-4791-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_COMMUNICATIONSTRANSFORM As New Guid("CF1DDA2C-9743-11D0-A3EE-00A0C9223196")
        Public ReadOnly KSCATEGORY_CROSSBAR As New Guid("A799A801-A46D-11D0-A18C-00A02401DCD4")
        Public ReadOnly KSCATEGORY_DATACOMPRESSOR As New Guid("1E84C900-7E70-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_DATADECOMPRESSOR As New Guid("2721AE20-7E70-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_DATATRANSFORM As New Guid("2EB07EA0-7E70-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_DRM_DESCRAMBLE As New Guid("FFBB6E3F-CCFE-4D84-90D9-421418B03A8E")
        Public ReadOnly KSCATEGORY_ENCODER As New Guid("19689BF6-C384-48fd-AD51-90E58C79F70B")
        Public ReadOnly KSCATEGORY_ESCALANTE_PLATFORM_DRIVER As New Guid("74F3AEA8-9768-11D1-8E07-00A0C95EC22E")
        Public ReadOnly KSCATEGORY_FILESYSTEM As New Guid("760FED5E-9357-11D0-A3CC-00A0C9223196")
        Public ReadOnly KSCATEGORY_INTERFACETRANSFORM As New Guid("CF1DDA2D-9743-11D0-A3EE-00A0C9223196")
        Public ReadOnly KSCATEGORY_MEDIUMTRANSFORM As New Guid("CF1DDA2E-9743-11D0-A3EE-00A0C9223196")
        Public ReadOnly KSCATEGORY_MICROPHONE_ARRAY_PROCESSOR As New Guid("830A44F2-A32D-476B-BE97-42845673B35A")
        Public ReadOnly KSCATEGORY_MIXER As New Guid("AD809C00-7B88-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_MULTIPLEXER As New Guid("7A5DE1D3-01A1-452c-B481-4FA2B96271E8")
        Public ReadOnly KSCATEGORY_NETWORK As New Guid("67C9CC3C-69C4-11D2-8759-00A0C9223196")
        Public ReadOnly KSCATEGORY_PREFERRED_MIDIOUT_DEVICE As New Guid("D6C50674-72C1-11D2-9755-0000F8004788")
        Public ReadOnly KSCATEGORY_PREFERRED_WAVEIN_DEVICE As New Guid("D6C50671-72C1-11D2-9755-0000F8004788")
        Public ReadOnly KSCATEGORY_PREFERRED_WAVEOUT_DEVICE As New Guid("D6C5066E-72C1-11D2-9755-0000F8004788")
        Public ReadOnly KSCATEGORY_PROXY As New Guid("97EBAACA-95BD-11D0-A3EA-00A0C9223196")
        Public ReadOnly KSCATEGORY_QUALITY As New Guid("97EBAACB-95BD-11D0-A3EA-00A0C9223196")
        Public ReadOnly KSCATEGORY_REALTIME As New Guid("EB115FFC-10C8-4964-831D-6DCB02E6F23F")
        Public ReadOnly KSCATEGORY_RENDER As New Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196")
        Public ReadOnly KSCATEGORY_SPLITTER As New Guid("0A4252A0-7E70-11D0-A5D6-28DB04C10000")
        Public ReadOnly KSCATEGORY_SYNTHESIZER As New Guid("DFF220F3-F70F-11D0-B917-00A0C9223196")
        Public ReadOnly KSCATEGORY_SYSAUDIO As New Guid("A7C7A5B1-5AF3-11D1-9CED-00A024BF0407")
        Public ReadOnly KSCATEGORY_TEXT As New Guid("6994AD06-93EF-11D0-A3CC-00A0C9223196")
        Public ReadOnly KSCATEGORY_TOPOLOGY As New Guid("DDA54A40-1E4C-11D1-A050-405705C10000")
        Public ReadOnly KSCATEGORY_TVAUDIO As New Guid("A799A802-A46D-11D0-A18C-00A02401DCD4")
        Public ReadOnly KSCATEGORY_TVTUNER As New Guid("A799A800-A46D-11D0-A18C-00A02401DCD4")
        Public ReadOnly KSCATEGORY_VBICODEC As New Guid("07DAD660-22F1-11D1-A9F4-00C04FBBDE8F")
        Public ReadOnly KSCATEGORY_VIDEO As New Guid("6994AD05-93EF-11D0-A3CC-00A0C9223196")
        Public ReadOnly KSCATEGORY_VIRTUAL As New Guid("3503EAC4-1F26-11D1-8AB0-00A0C9223196")
        Public ReadOnly KSCATEGORY_VPMUX As New Guid("A799A803-A46D-11D0-A18C-00A02401DCD4")
        Public ReadOnly KSCATEGORY_WDMAUD As New Guid("3E227E76-690D-11D2-8161-0000F8775BF1")
        Public ReadOnly KSMFT_CATEGORY_AUDIO_DECODER As New Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7")
        Public ReadOnly KSMFT_CATEGORY_AUDIO_EFFECT As New Guid("11064c48-3648-4ed0-932e-05ce8ac811b7")
        Public ReadOnly KSMFT_CATEGORY_AUDIO_ENCODER As New Guid("91c64bd0-f91e-4d8c-9276-db248279d975")
        Public ReadOnly KSMFT_CATEGORY_DEMULTIPLEXER As New Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1")
        Public ReadOnly KSMFT_CATEGORY_MULTIPLEXER As New Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b")
        Public ReadOnly KSMFT_CATEGORY_OTHER As New Guid("90175d57-b7ea-4901-aeb3-933a8747756f")
        Public ReadOnly KSMFT_CATEGORY_VIDEO_DECODER As New Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91")
        Public ReadOnly KSMFT_CATEGORY_VIDEO_EFFECT As New Guid("12e17c21-532c-4a6e-8a1c-40825a736397")
        Public ReadOnly KSMFT_CATEGORY_VIDEO_ENCODER As New Guid("f79eac7d-e545-4387-bdee-d647d7bde42a")
        Public ReadOnly KSMFT_CATEGORY_VIDEO_PROCESSOR As New Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302")
        Public ReadOnly GUID_DEVINTERFACE_USB_DEVICE As New Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED")
        Public ReadOnly GUID_DEVINTERFACE_USB_HOST_CONTROLLER As New Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27")
        Public ReadOnly GUID_DEVINTERFACE_USB_HUB As New Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8")
        Public ReadOnly GUID_DEVINTERFACE_WPD As New Guid("6AC27878-A6FA-4155-BA85-F98F491D4F33")
        Public ReadOnly GUID_DEVINTERFACE_WPD_PRIVATE As New Guid("BA0C718F-4DED-49B7-BDD3-FABE28661211")
        Public ReadOnly GUID_DEVINTERFACE_SIDESHOW As New Guid("152E5811-FEB9-4B00-90F4-D32947AE1681")
        Public ReadOnly GUID_DEVINTERFACE_USBPRINT As New Guid(&H28D78FAD, &H5A12, &H11D1, &HAE, &H5B, &H0, &H0, &HF8, &H3, &HA8, &HC2)

#End Region

#Region "Bus Type Guids"

        Public ReadOnly GUID_BUS_TYPE_INTERNAL As New Guid(&H1530EA73L, &H86B, &H11D1, &HA0, &H9F, &H0, &HC0, &H4F, &HC3, &H40, &HB1)
        Public ReadOnly GUID_BUS_TYPE_PCMCIA As New Guid(&H9343630L, &HAF9F, &H11D0, &H92, &HE9, &H0, &H0, &HF8, &H1E, &H1B, &H30)
        Public ReadOnly GUID_BUS_TYPE_PCI As New Guid(&HC8EBDFB0L, &HB510, &H11D0, &H80, &HE5, &H0, &HA0, &HC9, &H25, &H42, &HE3)
        Public ReadOnly GUID_BUS_TYPE_ISAPNP As New Guid(&HE676F854L, &HD87D, &H11D0, &H92, &HB2, &H0, &HA0, &HC9, &H5, &H5F, &HC5)
        Public ReadOnly GUID_BUS_TYPE_EISA As New Guid(&HDDC35509L, &HF3FC, &H11D0, &HA5, &H37, &H0, &H0, &HF8, &H75, &H3E, &HD1)
        Public ReadOnly GUID_BUS_TYPE_MCA As New Guid(&H1C75997AL, &HDC33, &H11D0, &H92, &HB2, &H0, &HA0, &HC9, &H5, &H5F, &HC5)
        Public ReadOnly GUID_BUS_TYPE_SERENUM As New Guid(&H77114A87L, &H8944, &H11D1, &HBD, &H90, &H0, &HA0, &HC9, &H6, &HBE, &H2D)
        Public ReadOnly GUID_BUS_TYPE_USB As New Guid(&H9D7DEBBCL, &HC85D, &H11D1, &H9E, &HB4, &H0, &H60, &H8, &HC3, &HA1, &H9A)

        Public ReadOnly GUID_BUS_TYPE_LPTENUM As New Guid(&HC4CA1000L, &H2DDC, &H11D5, &HA1, &H7A, &H0, &HC0, &H4F, &H60, &H52, &H4D)
        Public ReadOnly GUID_BUS_TYPE_USBPRINT As New Guid(&H441EE000L, &H4342, &H11D5, &HA1, &H84, &H0, &HC0, &H4F, &H60, &H52, &H4D)
        Public ReadOnly GUID_BUS_TYPE_DOT4PRT As New Guid(&H441EE001L, &H4342, &H11D5, &HA1, &H84, &H0, &HC0, &H4F, &H60, &H52, &H4D)
        Public ReadOnly GUID_BUS_TYPE_1394 As New Guid(&HF74E73EBL, &H9AC5, &H45EB, &HBE, &H4D, &H77, &H2C, &HC7, &H1D, &HDF, &HB3)
        Public ReadOnly GUID_BUS_TYPE_HID As New Guid(&HEEAF37D0L, &H1963, &H47C4, &HAA, &H48, &H72, &H47, &H6D, &HB7, &HCF, &H49)
        Public ReadOnly GUID_BUS_TYPE_AVC As New Guid(&HC06FF265L, &HAE09, &H48F0, &H81, &H2C, &H16, &H75, &H3D, &H7C, &HBA, &H83)
        Public ReadOnly GUID_BUS_TYPE_IRDA As New Guid(&H7AE17DC1L, &HC944, &H44D6, &H88, &H1F, &H4C, &H2E, &H61, &H5, &H3B, &HC1)
        Public ReadOnly GUID_BUS_TYPE_SD As New Guid(&HE700CC04L, &H4036, &H4E89, &H95, &H79, &H89, &HEB, &HF4, &H5F, &H0, &HCD)

        Public ReadOnly GUID_BUS_TYPE_ACPI As New Guid(&HD7B46895L, &H1A, &H4942, &H89, &H1F, &HA7, &HD4, &H66, &H10, &HA8, &H43)
        Public ReadOnly GUID_BUS_TYPE_SW_DEVICE As New Guid(&H6D10322L, &H7DE0, &H4CEF, &H8E, &H25, &H19, &H7D, &HE, &H74, &H42, &HE2)

#End Region

        ''
        '' Property type modifiers.  Used to modify base DEVPROP_TYPE_ values, as
        '' appropriate.  Not valid as standalone DEVPROPTYPE values.
        ''
        Public Const DEVPROP_TYPEMOD_ARRAY = &H1000  '' array of fixed-sized data elements
        Public Const DEVPROP_TYPEMOD_LIST = &H2000  '' list of variable-sized data elements

        ''
        '' Property data types.
        ''
        Public Const DEVPROP_TYPE_EMPTY = &H0  '' nothing, no property data
        Public Const DEVPROP_TYPE_NULL = &H1  '' null property data
        Public Const DEVPROP_TYPE_SBYTE = &H2  '' 8-bit signed int (SBYTE)
        Public Const DEVPROP_TYPE_BYTE = &H3  '' 8-bit unsigned int (BYTE)
        Public Const DEVPROP_TYPE_INT16 = &H4  '' 16-bit signed int (SHORT)
        Public Const DEVPROP_TYPE_UINT16 = &H5  '' 16-bit unsigned int (USHORT)
        Public Const DEVPROP_TYPE_INT32 = &H6  '' 32-bit signed int (LONG)
        Public Const DEVPROP_TYPE_UINT32 = &H7  '' 32-bit unsigned int (ULONG)
        Public Const DEVPROP_TYPE_INT64 = &H8  '' 64-bit signed int (LONG64)
        Public Const DEVPROP_TYPE_UINT64 = &H9  '' 64-bit unsigned int (ULONG64)
        Public Const DEVPROP_TYPE_FLOAT = &HA  '' 32-bit floating-point (FLOAT)
        Public Const DEVPROP_TYPE_DOUBLE = &HB  '' 64-bit floating-point (DOUBLE)
        Public Const DEVPROP_TYPE_DECIMAL = &HC  '' 128-bit data (DECIMAL)
        Public Const DEVPROP_TYPE_GUID = &HD  '' 128-bit unique identifier (GUID)
        Public Const DEVPROP_TYPE_CURRENCY = &HE  '' 64 bit signed int currency value (CURRENCY)
        Public Const DEVPROP_TYPE_DATE = &HF  '' date (DATE)
        Public Const DEVPROP_TYPE_FILETIME = &H10  '' file time (FILETIME)
        Public Const DEVPROP_TYPE_BOOLEAN = &H11  '' 8-bit boolean = (DEVPROP_BOOLEAN)
        Public Const DEVPROP_TYPE_STRING = &H12  '' null-terminated string
        Public Const DEVPROP_TYPE_STRING_LIST = (DEVPROP_TYPE_STRING Or DEVPROP_TYPEMOD_LIST) '' multi-sz string list
        Public Const DEVPROP_TYPE_SECURITY_DESCRIPTOR = &H13  '' self-relative binary SECURITY_DESCRIPTOR
        Public Const DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING = &H14  '' security descriptor string (SDDL format)
        Public Const DEVPROP_TYPE_DEVPROPKEY = &H15  '' device property key = (DEVPROPKEY)
        Public Const DEVPROP_TYPE_DEVPROPTYPE = &H16  '' device property type = (DEVPROPTYPE)
        Public Const DEVPROP_TYPE_BINARY = (DEVPROP_TYPE_BYTE Or DEVPROP_TYPEMOD_ARRAY)  '' custom binary data
        Public Const DEVPROP_TYPE_ERROR = &H17  '' 32-bit Win32 system error code
        Public Const DEVPROP_TYPE_NTSTATUS = &H18  '' 32-bit NTSTATUS code
        Public Const DEVPROP_TYPE_STRING_INDIRECT = &H19  '' string resource (@[path\]<dllname>,-<strId>)

        ''
        '' Max base DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        ''
        Public Const MAX_DEVPROP_TYPE = &H19  '' max valid DEVPROP_TYPE_ value
        Public Const MAX_DEVPROP_TYPEMOD = &H2000  '' max valid DEVPROP_TYPEMOD_ value

        ''
        '' Bitmasks for extracting DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        ''
        Public Const DEVPROP_MASK_TYPE = &HFFF  '' range for base DEVPROP_TYPE_ values
        Public Const DEVPROP_MASK_TYPEMOD = &HF000  '' mask for DEVPROP_TYPEMOD_ type modifiers

        ''
        '' Property type specific data types.
        ''

        '' 8-bit boolean type definition for DEVPROP_TYPE_BOOLEAN (True=-1, False=0)

        Public Enum DEVPROP_BOOLEAN As Byte
            DEVPROP_FALSE = 0
            DEVPROP_TRUE = &HFF
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure DEVPROPKEY
            <MarshalAs(UnmanagedType.Struct)>
            Public fmtid As Guid
            Public pid As UInteger

            Public Sub New(i1 As Integer, s1 As Short, s2 As Short, b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte, b5 As Byte, b6 As Byte, b7 As Byte, b8 As Byte, pid As UInteger)
                fmtid = New Guid(i1, s1, s2, b1, b2, b3, b4, b5, b6, b7, b8)
                Me.pid = pid
            End Sub

            Public Overrides Function ToString() As String
                Return GetKeyName(Me)
            End Function

            Public Overrides Function Equals(obj As Object) As Boolean
                If TypeOf obj Is DEVPROPKEY Then
                    If CType(obj, DEVPROPKEY).fmtid.Equals(fmtid) AndAlso
                    CType(obj, DEVPROPKEY).pid = pid Then
                        Return True
                    End If
                End If
                Return False
            End Function

            Public Shared Operator =(operand1 As DEVPROPKEY, operand2 As DEVPROPKEY) As Boolean
                Return operand1.Equals(operand2)
            End Operator

            Public Shared Operator <>(operand1 As DEVPROPKEY, operand2 As DEVPROPKEY) As Boolean
                Return Not operand1.Equals(operand2)
            End Operator

        End Structure

        ''
        '' DEVPKEY_NAME
        '' Common DEVPKEY used to retrieve the display name for an object.
        ''
        Public ReadOnly DEVPKEY_NAME As New DEVPROPKEY(&HB725F130, &H47EFS, &H101AS, &HA5, &HF1, &H2, &H60, &H8C, &H9E, &HEB, &HAC, 10)    '' DEVPROP_TYPE_STRING

        ''
        '' Device properties
        '' These DEVPKEYs correspond to the SetupAPI SPDRP_XXX device properties.
        ''
        Public ReadOnly DEVPKEY_Device_DeviceDesc As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 2)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_HardwareIds As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 3)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_CompatibleIds As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 4)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_Service As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 6)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_Class As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 9)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_ClassGuid As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 10)    '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_Device_Driver As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 11)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_ConfigFlags As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 12)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_Manufacturer As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 13)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_FriendlyName As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 14)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_LocationInfo As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 15)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_PDOName As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 16)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_Capabilities As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 17)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_UINumber As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 18)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_UpperFilters As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 19)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_LowerFilters As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 20)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_BusTypeGuid As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 21)    '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_Device_LegacyBusType As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 22)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_BusNumber As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 23)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_EnumeratorName As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 24)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_Security As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 25)    '' DEVPROP_TYPE_SECURITY_DESCRIPTOR
        Public ReadOnly DEVPKEY_Device_SecuritySDS As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 26)    '' DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        Public ReadOnly DEVPKEY_Device_DevType As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 27)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_Exclusive As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 28)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_Characteristics As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 29)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_Address As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 30)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_UINumberDescFormat As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 31)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_PowerData As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 32)    '' DEVPROP_TYPE_BINARY
        Public ReadOnly DEVPKEY_Device_RemovalPolicy As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 33)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_RemovalPolicyDefault As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 34)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_RemovalPolicyOverride As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 35)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_InstallState As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 36)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_LocationPaths As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 37)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_BaseContainerId As New DEVPROPKEY(&HA45C254E, &HDF1CS, &H4EFDS, &H80, &H20, &H67, &HD1, &H46, &HA8, &H50, &HE0, 38)    '' DEVPROP_TYPE_GUID

        ''
        '' Device and Device Interface property
        '' Common DEVPKEY used to retrieve the device instance id associated with devices and device interfaces.
        ''
        Public ReadOnly DEVPKEY_Device_InstanceId As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 256)   '' DEVPROP_TYPE_STRING

        ''
        '' Device properties
        '' These DEVPKEYs correspond to a device's status and problem code.
        ''
        Public ReadOnly DEVPKEY_Device_DevNodeStatus As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 2)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_ProblemCode As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 3)     '' DEVPROP_TYPE_UINT32

        ''
        '' Device properties
        '' These DEVPKEYs correspond to a device's relations.
        ''
        Public ReadOnly DEVPKEY_Device_EjectionRelations As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 4)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_RemovalRelations As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 5)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_PowerRelations As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 6)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_BusRelations As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 7)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_Parent As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 8)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_Children As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 9)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_Siblings As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 10)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_TransportRelations As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 11)    '' DEVPROP_TYPE_STRING_LIST

        ''
        '' Device property
        '' This DEVPKEY corresponds to a the status code that resulted in a device to be in a problem state.
        ''
        Public ReadOnly DEVPKEY_Device_ProblemStatus As New DEVPROPKEY(&H4340A6C5, &H93FAS, &H4706S, &H97, &H2C, &H7B, &H64, &H80, &H8, &HA5, &HA7, 12)     '' DEVPROP_TYPE_NTSTATUS

        ''
        '' Device properties
        '' These DEVPKEYs are set for the corresponding types of root-enumerated devices.
        ''
        Public ReadOnly DEVPKEY_Device_Reported As New DEVPROPKEY(&H80497100, &H8C73S, &H48B9S, &HAA, &HD9, &HCE, &H38, &H7E, &H19, &HC5, &H6E, 2)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_Legacy As New DEVPROPKEY(&H80497100, &H8C73S, &H48B9S, &HAA, &HD9, &HCE, &H38, &H7E, &H19, &HC5, &H6E, 3)     '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Device Container Id
        ''
        Public ReadOnly DEVPKEY_Device_ContainerId As New DEVPROPKEY(&H8C7ED206, &H3F8AS, &H4827S, &HB3, &HAB, &HAE, &H9E, &H1F, &HAE, &HFC, &H6C, 2)     '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_Device_InLocalMachineContainer As New DEVPROPKEY(&H8C7ED206, &H3F8AS, &H4827S, &HB3, &HAB, &HAE, &H9E, &H1F, &HAE, &HFC, &H6C, 4)    '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Device property
        '' This DEVPKEY correspond to a device's model.
        ''
        Public ReadOnly DEVPKEY_Device_Model As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 39)    '' DEVPROP_TYPE_STRING

        ''
        '' Device Experience related Keys
        ''
        Public ReadOnly DEVPKEY_Device_ModelId As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 2)     '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_Device_FriendlyNameAttributes As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 3)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_ManufacturerAttributes As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 4)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_PresenceNotForDevice As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 5)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_SignalStrength As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 6)     '' DEVPROP_TYPE_INT32
        Public ReadOnly DEVPKEY_Device_IsAssociateableByUserAction As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 7) '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_ShowInUninstallUI As New DEVPROPKEY(&H80D81EA6, &H7473S, &H4B0CS, &H82, &H16, &HEF, &HC1, &H1A, &H2C, &H4C, &H8B, 8)     '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Other Device properties
        ''
        Public DEVPKEY_Numa_Proximity_Domain As DEVPROPKEY = DEVPKEY_Device_Numa_Proximity_Domain
        Public ReadOnly DEVPKEY_Device_Numa_Proximity_Domain As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 1)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_DHP_Rebalance_Policy As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 2)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_Numa_Node As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 3)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_BusReportedDeviceDesc As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 4)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_IsPresent As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 5)     '' DEVPROP_TYPE_BOOL
        Public ReadOnly DEVPKEY_Device_HasProblem As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 6)     '' DEVPROP_TYPE_BOOL
        Public ReadOnly DEVPKEY_Device_ConfigurationId As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 7)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_ReportedDeviceIdsHash As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 8)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_PhysicalDeviceLocation As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 9)     '' DEVPROP_TYPE_BINARY
        Public ReadOnly DEVPKEY_Device_BiosDeviceName As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 10)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverProblemDesc As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 11)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DebuggerSafe As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 12)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_PostInstallInProgress As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 13)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_Stack As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 14)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_ExtendedConfigurationIds As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 15)  '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_IsRebootRequired As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 16)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_FirmwareDate As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 17)    '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_Device_FirmwareVersion As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 18)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_FirmwareRevision As New DEVPROPKEY(&H540B947E, &H8B40S, &H45BCS, &HA8, &HA2, &H6A, &HB, &H89, &H4C, &HBD, &HA2, 19)    '' DEVPROP_TYPE_STRING

        ''
        '' Device Session Id
        ''
        Public ReadOnly DEVPKEY_Device_SessionId As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 6)     '' DEVPROP_TYPE_UINT32

        ''
        '' Device activity timestamp properties
        ''
        Public ReadOnly DEVPKEY_Device_InstallDate As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 100)   '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_Device_FirstInstallDate As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 101)   '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_Device_LastArrivalDate As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 102)   '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_Device_LastRemovalDate As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 103)   '' DEVPROP_TYPE_FILETIME

        ''
        '' Device driver properties
        ''
        Public ReadOnly DEVPKEY_Device_DriverDate As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 2)      '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_Device_DriverVersion As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 3)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverDesc As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 4)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverInfPath As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 5)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverInfSection As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 6)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverInfSectionExt As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 7)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_MatchingDeviceId As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 8)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverProvider As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 9)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverPropPageProvider As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 10)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverCoInstallers As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 11)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_Device_ResourcePickerTags As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 12)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_ResourcePickerExceptions As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 13)   '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_Device_DriverRank As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 14)     '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_Device_DriverLogoLevel As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 15)     '' DEVPROP_TYPE_UINT32

        ''
        '' Device properties
        '' These DEVPKEYs may be set by the driver package installed for a device.
        ''
        Public ReadOnly DEVPKEY_Device_NoConnectSound As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 17)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_GenericDriverInstalled As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 18)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_AdditionalSoftwareRequested As New DEVPROPKEY(&HA8B865DD, &H2E3DS, &H4094S, &HAD, &H97, &HE5, &H93, &HA7, &HC, &H75, &HD6, 19) ''DEVPROP_TYPE_BOOLEAN

        ''
        '' Device safe-removal properties
        ''
        Public ReadOnly DEVPKEY_Device_SafeRemovalRequired As New DEVPROPKEY(&HAFD97640, &H86A3S, &H4210S, &HB6, &H7C, &H28, &H9C, &H41, &HAA, &HBE, &H55, 2)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_Device_SafeRemovalRequiredOverride As New DEVPROPKEY(&HAFD97640, &H86A3S, &H4210S, &HB6, &H7C, &H28, &H9C, &H41, &HAA, &HBE, &H55, 3) '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Device properties
        '' These DEVPKEYs may be set by the driver package installed for a device.
        ''
        Public ReadOnly DEVPKEY_DrvPkg_Model As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 2)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DrvPkg_VendorWebSite As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 3)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DrvPkg_DetailedDescription As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 4)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DrvPkg_DocumentationLink As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 5)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DrvPkg_Icon As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 6)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DrvPkg_BrandingIcon As New DEVPROPKEY(&HCF73BB51, &H3ABFS, &H44A2S, &H85, &HE0, &H9A, &H3D, &HC7, &HA1, &H21, &H32, 7)     '' DEVPROP_TYPE_STRING_LIST

        ''
        '' Device setup class properties
        '' These DEVPKEYs correspond to the SetupAPI SPCRP_XXX setup class properties.
        ''
        Public ReadOnly DEVPKEY_DeviceClass_UpperFilters As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 19)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceClass_LowerFilters As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 20)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceClass_Security As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 25)    '' DEVPROP_TYPE_SECURITY_DESCRIPTOR
        Public ReadOnly DEVPKEY_DeviceClass_SecuritySDS As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 26)    '' DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        Public ReadOnly DEVPKEY_DeviceClass_DevType As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 27)    '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_DeviceClass_Exclusive As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 28)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceClass_Characteristics As New DEVPROPKEY(&H4321918B, &HF69ES, &H470DS, &HA5, &HDE, &H4D, &H88, &HC7, &H5A, &HD2, &H4B, 29)    '' DEVPROP_TYPE_UINT32

        ''
        '' Device setup class properties
        ''
        Public ReadOnly DEVPKEY_DeviceClass_Name As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 2)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_ClassName As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 3)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_Icon As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 4)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_ClassInstaller As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 5)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_PropPageProvider As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 6)      '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_NoInstallClass As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 7)      '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceClass_NoDisplayClass As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 8)      '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceClass_SilentInstall As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 9)      '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceClass_NoUseClass As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 10)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceClass_DefaultService As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 11)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceClass_IconPath As New DEVPROPKEY(&H259ABFFC, &H50A7S, &H47CES, &HAF, &H8, &H68, &HC9, &HA7, &HD7, &H33, &H66, 12)     '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceClass_DHPRebalanceOptOut As New DEVPROPKEY(&HD14D3EF3, &H66CFS, &H4BA2S, &H9D, &H38, &HD, &HDB, &H37, &HAB, &H47, &H1, 2)    '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Other Device setup class properties
        ''
        Public ReadOnly DEVPKEY_DeviceClass_ClassCoInstallers As New DEVPROPKEY(&H713D1703, &HA2E2S, &H49F5S, &H92, &H14, &H56, &H47, &H2E, &HF3, &HDA, &H5C, 2)     '' DEVPROP_TYPE_STRING_LIST

        ''
        '' Device interface properties
        ''
        Public ReadOnly DEVPKEY_DeviceInterface_FriendlyName As New DEVPROPKEY(&H26E516E, &HB814S, &H414BS, &H83, &HCD, &H85, &H6D, &H6F, &HEF, &H48, &H22, 2)     '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceInterface_Enabled As New DEVPROPKEY(&H26E516E, &HB814S, &H414BS, &H83, &HCD, &H85, &H6D, &H6F, &HEF, &H48, &H22, 3)     '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceInterface_ClassGuid As New DEVPROPKEY(&H26E516E, &HB814S, &H414BS, &H83, &HCD, &H85, &H6D, &H6F, &HEF, &H48, &H22, 4)     '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_DeviceInterface_ReferenceString As New DEVPROPKEY(&H26E516E, &HB814S, &H414BS, &H83, &HCD, &H85, &H6D, &H6F, &HEF, &H48, &H22, 5)   '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceInterface_Restricted As New DEVPROPKEY(&H26E516E, &HB814S, &H414BS, &H83, &HCD, &H85, &H6D, &H6F, &HEF, &H48, &H22, 6)   '' DEVPROP_TYPE_BOOLEAN

        ''
        '' Device interface class properties
        ''
        Public ReadOnly DEVPKEY_DeviceInterfaceClass_DefaultInterface As New DEVPROPKEY(&H14C83A99, &HB3F, &H44B7S, &HBE, &H4C, &HA1, &H78, &HD3, &H99, &H5, &H64, 2) '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceInterfaceClass_Name As New DEVPROPKEY(&H14C83A99, &HB3F, &H44B7S, &HBE, &H4C, &HA1, &H78, &HD3, &H99, &H5, &H64, 3) '' DEVPROP_TYPE_STRING

        ''
        '' Device Container Properties
        ''
        Public ReadOnly DEVPKEY_DeviceContainer_Address As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 51)    '' DEVPROP_TYPE_STRING  Or  DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_DiscoveryMethod As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 52)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_IsEncrypted As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 53)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsAuthenticated As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 54)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsConnected As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 55)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsPaired As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 56)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_Icon As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 57)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_Version As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 65)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_Last_Seen As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 66)    '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_DeviceContainer_Last_Connected As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 67)    '' DEVPROP_TYPE_FILETIME
        Public ReadOnly DEVPKEY_DeviceContainer_IsShowInDisconnectedState As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 68)   '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsLocalMachine As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 70)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_MetadataPath As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 71)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_IsMetadataSearchInProgress As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 72)          '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_MetadataChecksum As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 73)            '' DEVPROP_TYPE_BINARY
        Public ReadOnly DEVPKEY_DeviceContainer_IsNotInterestingForDisplay As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 74)          '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_LaunchDeviceStageOnDeviceConnect As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 76)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_LaunchDeviceStageFromExplorer As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 77)       '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_BaselineExperienceId As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 78)    '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_DeviceContainer_IsDeviceUniquelyIdentifiable As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 79)        '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_AssociationArray As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 80)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_DeviceDescription1 As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 81)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_DeviceDescription2 As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 82)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_HasProblem As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 83)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsSharedDevice As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 84)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsNetworkDevice As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 85)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_IsDefaultDevice As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 86)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_MetadataCabinet As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 87)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_RequiresPairingElevation As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 88)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_ExperienceId As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 89)    '' DEVPROP_TYPE_GUID
        Public ReadOnly DEVPKEY_DeviceContainer_Category As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 90)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_Category_Desc_Singular As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 91)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_Category_Desc_Plural As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 92)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_Category_Icon As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 93)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_CategoryGroup_Desc As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 94)    '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_CategoryGroup_Icon As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 95)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_PrimaryCategory As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 97)    '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_UnpairUninstall As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 98)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_RequiresUninstallElevation As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 99)  '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_DeviceFunctionSubRank As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 100)   '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_DeviceContainer_AlwaysShowDeviceAsConnected As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 101)    '' DEVPROP_TYPE_BOOLEAN
        Public ReadOnly DEVPKEY_DeviceContainer_ConfigFlags As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 105)   '' DEVPROP_TYPE_UINT32
        Public ReadOnly DEVPKEY_DeviceContainer_PrivilegedPackageFamilyNames As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 106)   '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_CustomPrivilegedPackageFamilyNames As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 107)   '' DEVPROP_TYPE_STRING_LIST
        Public ReadOnly DEVPKEY_DeviceContainer_IsRebootRequired As New DEVPROPKEY(&H78C34FC8, &H104AS, &H4ACAS, &H9E, &HA4, &H52, &H4D, &H52, &H99, &H6E, &H57, 108)   '' DEVPROP_TYPE_BOOLEAN

        Public ReadOnly DEVPKEY_DeviceContainer_FriendlyName As New DEVPROPKEY(&H656A3BB3, &HECC0S, &H43FDS, &H84, &H77, &H4A, &HE0, &H40, &H4A, &H96, &HCD, 12288) '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_Manufacturer As New DEVPROPKEY(&H656A3BB3, &HECC0S, &H43FDS, &H84, &H77, &H4A, &HE0, &H40, &H4A, &H96, &HCD, 8192)  '' DEVPROP_TYPE_STRING
        Public ReadOnly DEVPKEY_DeviceContainer_ModelName As New DEVPROPKEY(&H656A3BB3, &HECC0S, &H43FDS, &H84, &H77, &H4A, &HE0, &H40, &H4A, &H96, &HCD, 8194)  '' DEVPROP_TYPE_STRING (localizable)
        Public ReadOnly DEVPKEY_DeviceContainer_ModelNumber As New DEVPROPKEY(&H656A3BB3, &HECC0S, &H43FDS, &H84, &H77, &H4A, &HE0, &H40, &H4A, &H96, &HCD, 8195)  '' DEVPROP_TYPE_STRING

        Public ReadOnly DEVPKEY_DeviceContainer_InstallInProgress As New DEVPROPKEY(&H83DA6326, &H97A6S, &H4088S, &H94, &H53, &HA1, &H92, &H3F, &H57, &H3B, &H29, 9)     '' DEVPROP_TYPE_BOOLEAN

        Private Function GetKeyName(dpk As DEVPROPKEY) As String

            Dim fi() As FieldInfo = GetType(DevProp).GetFields(BindingFlags.Static Or BindingFlags.Public)

            Dim g As DEVPROPKEY

            For Each fe In fi
                If fe.FieldType <> GetType(DEVPROPKEY) Then Continue For
                g = CType(fe.GetValue(Nothing), DEVPROPKEY)

                If g = dpk Then
                    '' we found it!
                    Dim strs() As String = TextTools.Split(fe.Name, "_")

                    Dim s As String = TextTools.SeparateCamel(strs(1)) & " Property: " & vbCrLf & TextTools.SeparateCamel(strs(2))
                    Return s
                End If

            Next
            Return dpk.fmtid.ToString & ": " & dpk.pid
        End Function


        Public Enum DEVICE_POWER_STATE
            PowerDeviceUnspecified = 0
            PowerDeviceD0 = 1
            PowerDeviceD1 = 2
            PowerDeviceD2 = 3
            PowerDeviceD3 = 4
            PowerDeviceMaximum = 5
        End Enum

        Public Enum SYSTEM_POWER_STATE
            PowerSystemUnspecified = 0
            PowerSystemWorking = 1
            PowerSystemSleeping1 = 2
            PowerSystemSleeping2 = 3
            PowerSystemSleeping3 = 4
            PowerSystemHibernate = 5
            PowerSystemShutdown = 6
            PowerSystemMaximum = 7
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure DEVICE_CAPABALITIES
            Public Size As UShort
            Public Version As UShort
            Public Capabilities As DeviceCapabilities
            Public Address As Integer
            Public UINumber As Integer

            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.I4, SizeConst:=7)>
            Public DeviceState() As DEVICE_POWER_STATE

            Public SystemWake As SYSTEM_POWER_STATE
            Public DeviceWake As DEVICE_POWER_STATE
            Public D1Latency As Integer
            Public D2Latency As Integer
            Public D3Latency As Integer
        End Structure


        ''' <summary>
        ''' Parses a comma-separated list of string values into an array of enumeration values.
        ''' </summary>
        ''' <typeparam name="T">The enum type to parse.</typeparam>
        ''' <param name="values">The comma-separated list of strings to parse.</param>
        ''' <returns>An array of T</returns>
        ''' <remarks></remarks>
        Public Function EnumListParse(Of T)(values As String) As T()

            Dim x As T
            Dim l As Object = Nothing

            If x.GetType.IsEnum = False Then
                Throw New ArgumentException("T must be an enumeration type.")
                Return Nothing
            End If

            Dim vs() As String = TextTools.Split(values, ",")
            Dim vOut() As T = Nothing

            Dim enames() As String = [Enum].GetNames(GetType(T))

            Dim i As Integer = 0
            Dim c As Integer = 0
            Dim e As Integer = 0

            If vs Is Nothing Then Return Nothing

            c = vs.Length - 1

            For i = 0 To c
                vs(i) = vs(i).Trim

                If enames.Contains(vs(i)) Then
                    x = CType([Enum].Parse(GetType(T), vs(i)), T)

                    ReDim Preserve vOut(e)
                    vOut(e) = x
                    e += 1

                End If

            Next

            Return vOut

        End Function


        ''' <summary>
        ''' Parses a comma-separated list of string values into an a flag enum result.
        ''' </summary>
        ''' <typeparam name="T">The enum type to parse.</typeparam>
        ''' <param name="values">The comma-separated list of strings to parse.</param>
        ''' <returns>An array of T</returns>
        ''' <remarks></remarks>
        Public Function FlagsParse(Of T)(values As String) As Integer

            Dim x As Integer
            Dim l As Object = Nothing

            If GetType(T).IsEnum = False Then
                Throw New ArgumentException("T must be an enumeration type.")
                Return Nothing
            End If

            Dim vs() As String = TextTools.Split(values, ",")
            Dim vOut As Integer = 0

            Dim enames() As String = [Enum].GetNames(GetType(T))

            Dim i As Integer = 0
            Dim c As Integer = 0
            Dim e As Integer = 0

            If vs Is Nothing Then Return Nothing

            c = vs.Length - 1

            For i = 0 To c
                vs(i) = vs(i).Trim

                If enames.Contains(vs(i)) Then
                    x = CInt([Enum].Parse(GetType(T), vs(i)))
                    vOut = vOut Or x
                    e += 1

                End If

            Next

            Return vOut

        End Function

    End Module



    ''' <summary>
    ''' Device property types.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DevPropTypes
        ''' <summary>
        ''' nothing, no property data
        ''' </summary>
        <Description("nothing, no property data")>
        Empty = DEVPROP_TYPE_EMPTY

        ''' <summary>
        ''' null property data
        ''' </summary>
        <Description("null property data")>
        Null = DEVPROP_TYPE_NULL

        ''' <summary>
        ''' 8-bit signed int (SBYTE)
        ''' </summary>
        <Description("8-bit signed int (SBYTE)")>
        [SByte] = DEVPROP_TYPE_SBYTE

        ''' <summary>
        ''' 8-bit unsigned int (BYTE)
        ''' </summary>
        <Description("8-bit unsigned int (BYTE)")>
        [Byte] = DEVPROP_TYPE_BYTE

        ''' <summary>
        ''' 16-bit signed int (SHORT)
        ''' </summary>
        <Description("16-bit signed int (SHORT)")>
        [Int16] = DEVPROP_TYPE_INT16

        ''' <summary>
        ''' 16-bit unsigned int (USHORT)
        ''' </summary>
        <Description("16-bit unsigned int (USHORT)")>
        [UInt16] = DEVPROP_TYPE_UINT16

        ''' <summary>
        ''' 32-bit signed int (LONG)
        ''' </summary>
        <Description("32-bit signed int (LONG)")>
        [Int32] = DEVPROP_TYPE_INT32

        ''' <summary>
        ''' 32-bit unsigned int (ULONG)
        ''' </summary>
        <Description("32-bit unsigned int (ULONG)")>
        [UInt32] = DEVPROP_TYPE_UINT32

        ''' <summary>
        ''' 64-bit signed int (LONG64)
        ''' </summary>
        <Description("64-bit signed int (LONG64)")>
        [Int64] = DEVPROP_TYPE_INT64

        ''' <summary>
        ''' 64-bit unsigned int (ULONG64)
        ''' </summary>
        <Description("64-bit unsigned int (ULONG64)")>
        [UInt64] = DEVPROP_TYPE_UINT64

        ''' <summary>
        ''' 32-bit floating-point (FLOAT)
        ''' </summary>
        <Description("32-bit floating-point (FLOAT)")>
        [Float] = DEVPROP_TYPE_FLOAT

        ''' <summary>
        ''' 64-bit floating-point (DOUBLE)
        ''' </summary>
        <Description("64-bit floating-point (DOUBLE)")>
        [Double] = DEVPROP_TYPE_DOUBLE

        ''' <summary>
        ''' 128-bit data (DECIMAL)
        ''' </summary>
        <Description("128-bit data (DECIMAL)")>
        [Decimal] = DEVPROP_TYPE_DECIMAL

        ''' <summary>
        ''' 128-bit unique identifier (GUID)
        ''' </summary>
        <Description("128-bit unique identifier (GUID)")>
        [Guid] = DEVPROP_TYPE_GUID

        ''' <summary>
        ''' 64 bit signed int currency value (CURRENCY)
        ''' </summary>
        <Description("64 bit signed int currency value (CURRENCY)")>
        [Currency] = DEVPROP_TYPE_CURRENCY

        ''' <summary>
        ''' date (DATE)
        ''' </summary>
        <Description("date (DATE)")>
        [Date] = DEVPROP_TYPE_DATE

        ''' <summary>
        ''' file time (FILETIME)
        ''' </summary>
        <Description("file time (FILETIME)")>
        [FileTime] = DEVPROP_TYPE_FILETIME

        ''' <summary>
        ''' 8-bit boolean = (DEVPROP_BOOLEAN)
        ''' </summary>
        <Description("8-bit boolean = (DEVPROP_BOOLEAN)")>
        [Boolean] = DEVPROP_TYPE_BOOLEAN

        ''' <summary>
        ''' null-terminated string
        ''' </summary>
        <Description("null-terminated string")>
        [String] = DEVPROP_TYPE_STRING

        ''' <summary>
        ''' multi-sz string list
        ''' </summary>
        <Description("multi-sz string list")>
        StringList = (DEVPROP_TYPE_STRING Or DEVPROP_TYPEMOD_LIST)

        ''' <summary>
        ''' self-relative binary SECURITY_DESCRIPTOR
        ''' </summary>
        <Description("self-relative binary SECURITY_DESCRIPTOR")>
        SecurityDescriptor = DEVPROP_TYPE_SECURITY_DESCRIPTOR

        ''' <summary>
        ''' security descriptor string (SDDL format)
        ''' </summary>
        <Description("security descriptor string (SDDL format)")>
        SecurityDescriptorString = DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING

        ''' <summary>
        ''' device property key = (DEVPROPKEY)
        ''' </summary>
        <Description("device property key = (DEVPROPKEY)")>
        DevPropKey = DEVPROP_TYPE_DEVPROPKEY

        ''' <summary>
        ''' device property type = (DEVPROPTYPE)
        ''' </summary>
        <Description("device property type = (DEVPROPTYPE)")>
        DevPropType = DEVPROP_TYPE_DEVPROPTYPE

        ''' <summary>
        ''' custom binary data
        ''' </summary>
        <Description("custom binary data")>
        Binary = (DEVPROP_TYPE_BYTE Or DEVPROP_TYPEMOD_ARRAY)

        ''' <summary>
        ''' 32-bit Win32 system error code
        ''' </summary>
        <Description("32-bit Win32 system error code")>
        [Error] = DEVPROP_TYPE_ERROR

        ''' <summary>
        ''' 32-bit NTSTATUS code
        ''' </summary>
        <Description("32-bit NTSTATUS code")>
        NTStatus = DEVPROP_TYPE_NTSTATUS

        ''' <summary>
        ''' string resource (@[path\]&lt;dllname&gt;,-&lt;strId&gt;)
        ''' </summary>
        <Description("string resource (@[path\]<dllname>,-<strId>)")>
        StringIndirect = DEVPROP_TYPE_STRING_INDIRECT

        ''' <summary>
        ''' Array property key type modifier.
        ''' </summary>
        ''' <remarks></remarks>
        [Array] = DEVPROP_TYPEMOD_ARRAY

        ''' <summary>
        ''' List property key type modifier.
        ''' </summary>
        ''' <remarks></remarks>
        List = DEVPROP_TYPEMOD_LIST

    End Enum

    ''' <summary>
    ''' Enumeration flags for the SetupDiGetClassDevs
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum ClassDevFlags As Integer

        ''' <summary>
        ''' Return only the device that is associated with the system default device interface, if one is set, for the specified device interface classes.
        ''' </summary>
        ''' <remarks></remarks>
        [Default] = DIGCF_DEFAULT

        ''' <summary>
        ''' Return only devices that are currently present in a system.
        ''' </summary>
        ''' <remarks></remarks>
        Present = DIGCF_PRESENT

        ''' <summary>
        ''' Return a list of installed devices for all device setup classes or all device interface classes.
        ''' </summary>
        ''' <remarks></remarks>
        AllClasses = DIGCF_ALLCLASSES

        ''' <summary>
        ''' Return only devices that are a part of the current hardware profile.
        ''' </summary>
        ''' <remarks></remarks>
        Profile = DIGCF_PROFILE

        ''' <summary>
        ''' Return devices that support device interfaces for the specified device interface classes. 
        ''' This flag must be set in the Flags parameter if the Enumerator parameter specifies a device instance ID.
        ''' </summary>
        ''' <remarks></remarks>
        DeviceInterface = DIGCF_DEVICEINTERFACE

    End Enum

End Namespace