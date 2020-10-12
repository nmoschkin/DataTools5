// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: DevProp
// '         Native Device Properites.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Hardware.Native
{


    /// <summary>
    /// Bus types.
    /// </summary>
    /// <remarks></remarks>
    public enum BusType
    {

        /// <summary>
        /// Internal
        /// </summary>
        Internal,

        /// <summary>
        /// PCMCIA
        /// </summary>
        PCMCIA,

        /// <summary>
        /// PCI
        /// </summary>
        PCI,

        /// <summary>
        /// ISAPnP
        /// </summary>
        ISAPnP,

        /// <summary>
        /// EISA
        /// </summary>
        EISA,

        /// <summary>
        /// MCA
        /// </summary>
        MCA,

        /// <summary>
        /// Serenum
        /// </summary>
        Serenum,

        /// <summary>
        /// USB
        /// </summary>
        USB,

        /// <summary>
        /// Parallel Port
        /// </summary>
        ParallelPort,

        /// <summary>
        /// UsB Printer
        /// </summary>
        USBPrinter,

        /// <summary>
        /// DOT4 Dotmatrix Printer
        /// </summary>
        DOT4Printer,

        /// <summary>
        /// IEEE 1394 / Firewire
        /// </summary>
        Bus1394,

        /// <summary>
        /// Human Interface Device
        /// </summary>
        HID,

        /// <summary>
        /// AVC
        /// </summary>
        AVC,

        /// <summary>
        /// Infrared (IRDA) device
        /// </summary>
        IRDA,

        /// <summary>
        /// MicroSD card
        /// </summary>
        SD,

        /// <summary>
        /// ACPI
        /// </summary>
        ACPI,

        /// <summary>
        /// Software Device
        /// </summary>
        SoftwareDevice
    }

    /// <summary>
    /// Device properties dialog
    /// </summary>
    public static class DevPropDialog
    {
        [DllImport("devmgr.dll", EntryPoint = "DeviceProperties_RunDLLW", CharSet = CharSet.Unicode)]
        static extern bool DeviceProperties_RunDLL(IntPtr hwnd, IntPtr hAppInstance, [MarshalAs(UnmanagedType.LPWStr)] string cmdLine, int nCmdShow);

        /// <summary>
        /// Opens the hardware properties dialog box for the specified instance id.
        /// </summary>
        /// <param name="InstanceId"></param>
        /// <param name="hwnd"></param>
        /// <remarks></remarks>
        public static void OpenDeviceProperties(string InstanceId, IntPtr hwnd = default)
        {
            if (InstanceId is null)
                return;
            
            string argcmdLine = "/DeviceId \"" + InstanceId + "\"";
            
            DeviceProperties_RunDLL(hwnd, Process.GetCurrentProcess().Handle, argcmdLine, PInvoke.SW_SHOWNORMAL);
        }

        internal static BusType GuidToBusType(Guid g)
        {
            if (g == DevProp.GUID_BUS_TYPE_INTERNAL)
                return BusType.Internal;
            if (g == DevProp.GUID_BUS_TYPE_PCMCIA)
                return BusType.PCMCIA;
            if (g == DevProp.GUID_BUS_TYPE_PCI)
                return BusType.PCI;
            if (g == DevProp.GUID_BUS_TYPE_ISAPNP)
                return BusType.ISAPnP;
            if (g == DevProp.GUID_BUS_TYPE_EISA)
                return BusType.EISA;
            if (g == DevProp.GUID_BUS_TYPE_MCA)
                return BusType.MCA;
            if (g == DevProp.GUID_BUS_TYPE_SERENUM)
                return BusType.Serenum;
            if (g == DevProp.GUID_BUS_TYPE_USB)
                return BusType.USB;
            if (g == DevProp.GUID_BUS_TYPE_LPTENUM)
                return BusType.ParallelPort;
            if (g == DevProp.GUID_BUS_TYPE_USBPRINT)
                return BusType.USBPrinter;
            if (g == DevProp.GUID_BUS_TYPE_DOT4PRT)
                return BusType.DOT4Printer;
            if (g == DevProp.GUID_BUS_TYPE_1394)
                return BusType.Bus1394;
            if (g == DevProp.GUID_BUS_TYPE_HID)
                return BusType.HID;
            if (g == DevProp.GUID_BUS_TYPE_AVC)
                return BusType.AVC;
            if (g == DevProp.GUID_BUS_TYPE_IRDA)
                return BusType.IRDA;
            if (g == DevProp.GUID_BUS_TYPE_SD)
                return BusType.SD;
            if (g == DevProp.GUID_BUS_TYPE_ACPI)
                return BusType.ACPI;
            if (g == DevProp.GUID_BUS_TYPE_SW_DEVICE)
                return BusType.SoftwareDevice;
            return 0;
        }
    }

    internal static class DevProp
    {
        public const int DIGCF_DEFAULT = 0x1;
        public const int DIGCF_PRESENT = 0x2;
        public const int DIGCF_ALLCLASSES = 0x4;
        public const int DIGCF_PROFILE = 0x8;
        public const int DIGCF_DEVICEINTERFACE = 0x10;
        public readonly static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;

            // ' this is a array of unknown size.
            public char DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern IntPtr SetupDiGetClassDevs([MarshalAs(UnmanagedType.LPStruct)] Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, ClassDevFlags Flags);

        [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassDevs", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern IntPtr SetupDiGetClassDevsNoRef([MarshalAs(UnmanagedType.LPStruct)] Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, ClassDevFlags Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern bool SetupDiGetDevicePropertyKeys(IntPtr hDev, [MarshalAs(UnmanagedType.Struct)] ref SP_DEVINFO_DATA DeviceInfoData, IntPtr PropertyKeyArray, uint PropertyKeyCount, ref uint RequiredPropertyKeyCount, uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true, EntryPoint = "SetupDiGetDevicePropertyW")]
        public static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, [MarshalAs(UnmanagedType.Struct)] ref SP_DEVINFO_DATA DeviceInfoData, [MarshalAs(UnmanagedType.Struct)] ref DEVPROPKEY PropertyKey, out uint PropertyType, IntPtr PropertyBuffer, uint PropertyBufferSize, out uint RequiredSize, uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern bool SetupDiLoadClassIcon([MarshalAs(UnmanagedType.LPStruct)] Guid ClassGuid, ref IntPtr hIcon, ref int MiniIconIndex);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern bool SetupDiLoadDeviceIcon(IntPtr hdev, ref SP_DEVINFO_DATA DeviceInfoData, uint cxIcon, uint cyIcon, uint Flags, ref IntPtr hIcon);

        [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassPropertyW", CharSet = CharSet.Unicode)]
        public static extern bool SetupDiGetClassProperty([MarshalAs(UnmanagedType.LPStruct)] Guid ClassGuid, [MarshalAs(UnmanagedType.Struct)] ref DEVPROPKEY PropertyKey, ref int propertyType, IntPtr propertyBuffer, int propertyBufferSize, ref int RequiredSize, int Flags);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, [MarshalAs(UnmanagedType.LPStruct)] SP_DEVINFO_DATA DeviceInfoData, Guid InterfaceClassGuid, uint MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, [MarshalAs(UnmanagedType.LPStruct)] Guid InterfaceClassGuid, uint MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, [MarshalAs(UnmanagedType.LPStruct)] Guid InterfaceClassGuid, uint MemberIndex, IntPtr DeviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, out SP_DEVINFO_DATA DeviceInfoData);

        // <DllImport("setupapi.dll", EntryPoint:="SetupDiGetDeviceInterfaceDetailW", CharSet:=CharSet.Unicode, SetLastError:=True, _
        // CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        // Public Function SetupDiGetDeviceInterfaceDetail _
        // (DeviceInfoSet As IntPtr, _
        // <MarshalAs(UnmanagedType.Struct)> DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        // DeviceInterfaceDetailData As SafeHandle, _
        // DeviceInterfaceDetailDataSize As UInteger, _
        // ByRef RequiredSize As UInteger, _
        // DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        // End Function

        // <DllImport("setupapi.dll", EntryPoint:="SetupDiGetDeviceInterfaceDetailW", CharSet:=CharSet.Unicode, SetLastError:=True, _
        // CallingConvention:=CallingConvention.StdCall, PreserveSig:=True)>
        // Public Function SetupDiGetDeviceInterfaceDetail _
        // (DeviceInfoSet As IntPtr, _
        // <MarshalAs(UnmanagedType.Struct)> DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        // DeviceInterfaceDetailData As SafeHandle, _
        // DeviceInterfaceDetailDataSize As UInteger, _
        // ByRef RequiredSize As UInteger, _
        // <MarshalAs(UnmanagedType.Struct)> DeviceInfoData As SP_DEVINFO_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean
        // End Function

        // Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        // Alias "SetupDiGetDeviceInterfaceDetailW" _
        // (DeviceInfoSet As IntPtr, _
        // ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        // ByRef DeviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA, _
        // DeviceInterfaceDetailDataSize As UInteger, _
        // ByRef RequiredSize As UInteger, _
        // ByRef DeviceInfoData As SP_DEVINFO_DATA) As <MarshalAs(UnmanagedType.Bool)> Boolean

        [DllImport("setupapi.dll", EntryPoint = "SetupDiGetDeviceInterfaceDetailW")]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr DeviceInfoSet,
            [MarshalAs(UnmanagedType.Struct)] ref SP_DEVICE_INTERFACE_DATA beviceInterfaceData, 
            IntPtr DeviceInterfaceDetailData, 
            uint DeviceInterfaceDetailDataSize, 
            out uint RequiredSize, 
            IntPtr DeviceInfoData);

        // Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        // Alias "SetupDiGetDeviceInterfaceDetailW" _
        // (DeviceInfoSet As IntPtr, _
        // DeviceInterfaceData As IntPtr, _
        // DeviceInterfaceDetailData As IntPtr, _
        // DeviceInterfaceDetailDataSize As UInteger, _
        // ByRef RequiredSize As UInteger, _
        // DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        // Public Declare Function SetupDiGetDeviceInterfaceDetail Lib "setupapi.dll" _
        // Alias "SetupDiGetDeviceInterfaceDetailW" _
        // (DeviceInfoSet As IntPtr, _
        // ByRef DeviceInterfaceData As SP_DEVICE_INTERFACE_DATA, _
        // ByRef DeviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA, _
        // DeviceInterfaceDetailDataSize As UInteger, _
        // ByRef RequiredSize As UInteger, _
        // DeviceInfoData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public readonly static Guid GUID_DEVCLASS_1394 = new Guid(0x6BDD1FC1, 0x810F, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_1394DEBUG = new Guid(0x66F250D6, 0x7801, 0x4A64, 0xB1, 0x39, 0xEE, 0xA8, 0xA, 0x45, 0xB, 0x24);
        public readonly static Guid GUID_DEVCLASS_61883 = new Guid(0x7EBEFBC0, 0x3200, 0x11D2, 0xB4, 0xC2, 0x0, 0xA0, 0xC9, 0x69, 0x7D, 0x7);
        public readonly static Guid GUID_DEVCLASS_ADAPTER = new Guid(0x4D36E964, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_APMSUPPORT = new Guid(0xD45B1C18, 0xC8FA, 0x11D1, 0x9F, 0x77, 0x0, 0x0, 0xF8, 0x5, 0xF5, 0x30);
        public readonly static Guid GUID_DEVCLASS_AVC = new Guid(0xC06FF265, 0xAE09, 0x48F0, 0x81, 0x2C, 0x16, 0x75, 0x3D, 0x7C, 0xBA, 0x83);
        public readonly static Guid GUID_DEVCLASS_BATTERY = new Guid(0x72631E54, 0x78A4, 0x11D0, 0xBC, 0xF7, 0x0, 0xAA, 0x0, 0xB7, 0xB3, 0x2A);
        public readonly static Guid GUID_DEVCLASS_BIOMETRIC = new Guid(0x53D29EF7, 0x377C, 0x4D14, 0x86, 0x4B, 0xEB, 0x3A, 0x85, 0x76, 0x93, 0x59);
        public readonly static Guid GUID_DEVCLASS_BLUETOOTH = new Guid(0xE0CBF06C, 0xCD8B, 0x4647, 0xBB, 0x8A, 0x26, 0x3B, 0x43, 0xF0, 0xF9, 0x74);
        public readonly static Guid GUID_DEVCLASS_CDROM = new Guid(0x4D36E965, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_COMPUTER = new Guid(0x4D36E966, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_DECODER = new Guid(0x6BDD1FC2, 0x810F, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_DISKDRIVE = new Guid(0x4D36E967, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_DISPLAY = new Guid(0x4D36E968, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_DOT4 = new Guid(0x48721B56, 0x6795, 0x11D2, 0xB1, 0xA8, 0x0, 0x80, 0xC7, 0x2E, 0x74, 0xA2);
        public readonly static Guid GUID_DEVCLASS_DOT4PRINT = new Guid(0x49CE6AC8, 0x6F86, 0x11D2, 0xB1, 0xE5, 0x0, 0x80, 0xC7, 0x2E, 0x74, 0xA2);
        public readonly static Guid GUID_DEVCLASS_ENUM1394 = new Guid(0xC459DF55, 0xDB08, 0x11D1, 0xB0, 0x9, 0x0, 0xA0, 0xC9, 0x8, 0x1F, 0xF6);
        public readonly static Guid GUID_DEVCLASS_FDC = new Guid(0x4D36E969, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_FLOPPYDISK = new Guid(0x4D36E980, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_GPS = new Guid(0x6BDD1FC3, 0x810F, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_HDC = new Guid(0x4D36E96A, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_HIDCLASS = new Guid(0x745A17A0, 0x74D3, 0x11D0, 0xB6, 0xFE, 0x0, 0xA0, 0xC9, 0xF, 0x57, 0xDA);
        public readonly static Guid GUID_DEVCLASS_IMAGE = new Guid(0x6BDD1FC6, 0x810F, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_INFINIBAND = new Guid(0x30EF7132, 0xD858, 0x4A0C, 0xAC, 0x24, 0xB9, 0x2, 0x8A, 0x5C, 0xCA, 0x3F);
        public readonly static Guid GUID_DEVCLASS_INFRARED = new Guid(0x6BDD1FC5, 0x810F, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_KEYBOARD = new Guid(0x4D36E96B, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_LEGACYDRIVER = new Guid(0x8ECC055D, 0x47F, 0x11D1, 0xA5, 0x37, 0x0, 0x0, 0xF8, 0x75, 0x3E, 0xD1);
        public readonly static Guid GUID_DEVCLASS_MEDIA = new Guid(0x4D36E96C, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MEDIUM_CHANGER = new Guid(0xCE5939AE, 0xEBDE, 0x11D0, 0xB1, 0x81, 0x0, 0x0, 0xF8, 0x75, 0x3E, 0xC4);
        public readonly static Guid GUID_DEVCLASS_MEMORY = new Guid(0x5099944A, 0xF6B9, 0x4057, 0xA0, 0x56, 0x8C, 0x55, 0x2, 0x28, 0x54, 0x4C);
        public readonly static Guid GUID_DEVCLASS_MODEM = new Guid(0x4D36E96D, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MONITOR = new Guid(0x4D36E96E, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MOUSE = new Guid(0x4D36E96F, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MTD = new Guid(0x4D36E970, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MULTIFUNCTION = new Guid(0x4D36E971, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_MULTIPORTSERIAL = new Guid(0x50906CB8, 0xBA12, 0x11D1, 0xBF, 0x5D, 0x0, 0x0, 0xF8, 0x5, 0xF5, 0x30);
        public readonly static Guid GUID_DEVCLASS_NET = new Guid(0x4D36E972, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_NETCLIENT = new Guid(0x4D36E973, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_NETSERVICE = new Guid(0x4D36E974, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_NETTRANS = new Guid(0x4D36E975, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_NODRIVER = new Guid(0x4D36E976, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_PCMCIA = new Guid(0x4D36E977, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_PNPPRINTERS = new Guid(0x4658EE7E, 0xF050, 0x11D1, 0xB6, 0xBD, 0x0, 0xC0, 0x4F, 0xA3, 0x72, 0xA7);
        public readonly static Guid GUID_DEVCLASS_PORTS = new Guid(0x4D36E978, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_PRINTER = new Guid(0x4D36E979, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);

        // ' Proud of myself for this one.
        public readonly static Guid GUID_DEVCLASS_PRINTER_QUEUE = new Guid(0x1ED2BBF9, 0x11F0, 0x4084, 0xB2, 0x1F, 0xAD, 0x83, 0xA8, 0xE6, 0xDC, 0xDC);
        public readonly static Guid GUID_DEVCLASS_PRINTERUPGRADE = new Guid(0x4D36E97A, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_PROCESSOR = new Guid(0x50127DC3, 0xF36, 0x415E, 0xA6, 0xCC, 0x4C, 0xB3, 0xBE, 0x91, 0xB, 0x65);
        public readonly static Guid GUID_DEVCLASS_SBP2 = new Guid(0xD48179BE, 0xEC20, 0x11D1, 0xB6, 0xB8, 0x0, 0xC0, 0x4F, 0xA3, 0x72, 0xA7);
        public readonly static Guid GUID_DEVCLASS_SCSIADAPTER = new Guid(0x4D36E97B, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_SECURITYACCELERATOR = new Guid(0x268C95A1, 0xEDFE, 0x11D3, 0x95, 0xC3, 0x0, 0x10, 0xDC, 0x40, 0x50, 0xA5);
        public readonly static Guid GUID_DEVCLASS_SENSOR = new Guid(0x5175D334, 0xC371, 0x4806, 0xB3, 0xBA, 0x71, 0xFD, 0x53, 0xC9, 0x25, 0x8D);
        public readonly static Guid GUID_DEVCLASS_SIDESHOW = new Guid(0x997B5D8D, 0xC442, 0x4F2E, 0xBA, 0xF3, 0x9C, 0x8E, 0x67, 0x1E, 0x9E, 0x21);
        public readonly static Guid GUID_DEVCLASS_SMARTCARDREADER = new Guid(0x50DD5230, 0xBA8A, 0x11D1, 0xBF, 0x5D, 0x0, 0x0, 0xF8, 0x5, 0xF5, 0x30);
        public readonly static Guid GUID_DEVCLASS_SOUND = new Guid(0x4D36E97C, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_SYSTEM = new Guid(0x4D36E97D, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_TAPEDRIVE = new Guid(0x6D807884, 0x7D21, 0x11CF, 0x80, 0x1C, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_UNKNOWN = new Guid(0x4D36E97E, 0xE325, 0x11CE, 0xBF, 0xC1, 0x8, 0x0, 0x2B, 0xE1, 0x3, 0x18);
        public readonly static Guid GUID_DEVCLASS_USB = new Guid(0x36FC9E60, 0xC465, 0x11CF, 0x80, 0x56, 0x44, 0x45, 0x53, 0x54, 0x0, 0x0);
        public readonly static Guid GUID_DEVCLASS_VOLUME = new Guid(0x71A27CDD, 0x812A, 0x11D0, 0xBE, 0xC7, 0x8, 0x0, 0x2B, 0xE2, 0x9, 0x2F);
        public readonly static Guid GUID_DEVCLASS_VOLUMESNAPSHOT = new Guid(0x533C5B84, 0xEC70, 0x11D2, 0x95, 0x5, 0x0, 0xC0, 0x4F, 0x79, 0xDE, 0xAF);
        public readonly static Guid GUID_DEVCLASS_WCEUSBS = new Guid(0x25DBCE51, 0x6C8F, 0x4A72, 0x8A, 0x6D, 0xB5, 0x4C, 0x2B, 0x4F, 0xC8, 0x35);
        public readonly static Guid GUID_DEVCLASS_WPD = new Guid(0xEEC5AD98, 0x8080, 0x425F, 0x92, 0x2A, 0xDA, 0xBF, 0x3D, 0xE3, 0xF6, 0x9A);
        public readonly static Guid GUID_DEVCLASS_EHSTORAGESILO = new Guid(0x9DA2B80F, 0xF89F, 0x4A49, 0xA5, 0xC2, 0x51, 0x1B, 0x8, 0x5B, 0x9E, 0x8A);
        public readonly static Guid GUID_DEVCLASS_FIRMWARE = new Guid(0xF2E7DD72, 0x6468, 0x4E36, 0xB6, 0xF1, 0x64, 0x88, 0xF4, 0x2C, 0x1B, 0x52);
        public readonly static Guid GUID_DEVCLASS_EXTENSION = new Guid(0xE2F84CE7, 0x8EFA, 0x411C, 0xAA, 0x69, 0x97, 0x45, 0x4C, 0xA4, 0xCB, 0x57);

        // '
        // ' Define filesystem filter classes used for classification and load ordering.
        // ' Classes are listed below in order from "highest" (i.e., farthest from the
        // ' filesystem) to "lowest" (i.e., closest to the filesystem).
        // '
        public readonly static Guid GUID_DEVCLASS_FSFILTER_TOP = new Guid(0xB369BAF4, 0x5568, 0x4E82, 0xA8, 0x7E, 0xA9, 0x3E, 0xB1, 0x6B, 0xCA, 0x87);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_ACTIVITYMONITOR = new Guid(0xB86DFF51, 0xA31E, 0x4BAC, 0xB3, 0xCF, 0xE8, 0xCF, 0xE7, 0x5C, 0x9F, 0xC2);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_UNDELETE = new Guid(0xFE8F1572, 0xC67A, 0x48C0, 0xBB, 0xAC, 0xB, 0x5C, 0x6D, 0x66, 0xCA, 0xFB);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_ANTIVIRUS = new Guid(0xB1D1A169, 0xC54F, 0x4379, 0x81, 0xDB, 0xBE, 0xE7, 0xD8, 0x8D, 0x74, 0x54);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_REPLICATION = new Guid(0x48D3EBC4, 0x4CF8, 0x48FF, 0xB8, 0x69, 0x9C, 0x68, 0xAD, 0x42, 0xEB, 0x9F);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_CONTINUOUSBACKUP = new Guid(0x71AA14F8, 0x6FAD, 0x4622, 0xAD, 0x77, 0x92, 0xBB, 0x9D, 0x7E, 0x69, 0x47);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_CONTENTSCREENER = new Guid(0x3E3F0674, 0xC83C, 0x4558, 0xBB, 0x26, 0x98, 0x20, 0xE1, 0xEB, 0xA5, 0xC5);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_QUOTAMANAGEMENT = new Guid(0x8503C911, 0xA6C7, 0x4919, 0x8F, 0x79, 0x50, 0x28, 0xF5, 0x86, 0x6B, 0xC);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_SYSTEMRECOVERY = new Guid(0x2DB15374, 0x706E, 0x4131, 0xA0, 0xC7, 0xD7, 0xC7, 0x8E, 0xB0, 0x28, 0x9A);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_CFSMETADATASERVER = new Guid(0xCDCF0939, 0xB75B, 0x4630, 0xBF, 0x76, 0x80, 0xF7, 0xBA, 0x65, 0x58, 0x84);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_HSM = new Guid(0xD546500A, 0x2AEB, 0x45F6, 0x94, 0x82, 0xF4, 0xB1, 0x79, 0x9C, 0x31, 0x77);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_COMPRESSION = new Guid(0xF3586BAF, 0xB5AA, 0x49B5, 0x8D, 0x6C, 0x5, 0x69, 0x28, 0x4C, 0x63, 0x9F);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_ENCRYPTION = new Guid(0xA0A701C0, 0xA511, 0x42FF, 0xAA, 0x6C, 0x6, 0xDC, 0x3, 0x95, 0x57, 0x6F);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_VIRTUALIZATION = new Guid(0xF75A86C0, 0x10D8, 0x4C3A, 0xB2, 0x33, 0xED, 0x60, 0xE4, 0xCD, 0xFA, 0xAC);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_PHYSICALQUOTAMANAGEMENT = new Guid(0x6A0A8E78, 0xBBA6, 0x4FC4, 0xA7, 0x9, 0x1E, 0x33, 0xCD, 0x9, 0xD6, 0x7E);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_OPENFILEBACKUP = new Guid(0xF8ECAFA6, 0x66D1, 0x41A5, 0x89, 0x9B, 0x66, 0x58, 0x5D, 0x72, 0x16, 0xB7);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_SECURITYENHANCER = new Guid(0xD02BC3DA, 0xC8E, 0x4945, 0x9B, 0xD5, 0xF1, 0x88, 0x3C, 0x22, 0x6C, 0x8C);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_COPYPROTECTION = new Guid(0x89786FF1, 0x9C12, 0x402F, 0x9C, 0x9E, 0x17, 0x75, 0x3C, 0x7F, 0x43, 0x75);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_BOTTOM = new Guid(0x37765EA0, 0x5958, 0x4FC9, 0xB0, 0x4B, 0x2F, 0xDF, 0xEF, 0x97, 0xE5, 0x9E);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_SYSTEM = new Guid(0x5D1B9AAA, 0x1E2, 0x46AF, 0x84, 0x9F, 0x27, 0x2B, 0x3F, 0x32, 0x4C, 0x46);
        public readonly static Guid GUID_DEVCLASS_FSFILTER_INFRASTRUCTURE = new Guid(0xE55FA6F9, 0x128C, 0x4D04, 0xAB, 0xAB, 0x63, 0xC, 0x74, 0xB1, 0x45, 0x3A);
        public readonly static Guid BUS1394_CLASS_GUID = new Guid("6BDD1FC1-810F-11d0-BEC7-08002BE2092F");
        public readonly static Guid GUID_61883_CLASS = new Guid("7EBEFBC0-3200-11d2-B4C2-00A0C9697D07");
        public readonly static Guid GUID_DEVICE_APPLICATIONLAUNCH_BUTTON = new Guid("629758EE-986E-4D9E-8E47-DE27F8AB054D");
        public readonly static Guid GUID_DEVICE_BATTERY = new Guid("72631E54-78A4-11D0-BCF7-00AA00B7B32A");
        public readonly static Guid GUID_DEVICE_LID = new Guid("4AFA3D52-74A7-11d0-be5e-00A0C9062857");
        public readonly static Guid GUID_DEVICE_MEMORY = new Guid("3FD0F03D-92E0-45FB-B75C-5ED8FFB01021");
        public readonly static Guid GUID_DEVICE_MESSAGE_INDICATOR = new Guid("CD48A365-FA94-4CE2-A232-A1B764E5D8B4");
        public readonly static Guid GUID_DEVICE_PROCESSOR = new Guid("97FADB10-4E33-40AE-359C-8BEF029DBDD0");
        public readonly static Guid GUID_DEVICE_SYS_BUTTON = new Guid("4AFA3D53-74A7-11d0-be5e-00A0C9062857");
        public readonly static Guid GUID_DEVICE_THERMAL_ZONE = new Guid("4AFA3D51-74A7-11d0-be5e-00A0C9062857");
        public readonly static Guid GUID_BTHPORT_DEVICE_INTERFACE = new Guid("0850302A-B344-4fda-9BE9-90576B8D46F0");
        public readonly static Guid GUID_DEVINTERFACE_BRIGHTNESS = new Guid("FDE5BBA4-B3F9-46FB-BDAA-0728CE3100B4");
        public readonly static Guid GUID_DEVINTERFACE_DISPLAY_ADAPTER = new Guid("5B45201D-F2F2-4F3B-85BB-30FF1F953599");
        public readonly static Guid GUID_DEVINTERFACE_I2C = new Guid("2564AA4F-DDDB-4495-B497-6AD4A84163D7");
        public readonly static Guid GUID_DEVINTERFACE_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");
        public readonly static Guid GUID_DEVINTERFACE_MONITOR = new Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7");
        public readonly static Guid GUID_DEVINTERFACE_OPM = new Guid("BF4672DE-6B4E-4BE4-A325-68A91EA49C09");
        public readonly static Guid GUID_DEVINTERFACE_VIDEO_OUTPUT_ARRIVAL = new Guid("1AD9E4F0-F88D-4360-BAB9-4C2D55E564CD");
        public readonly static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new Guid("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");
        public readonly static Guid GUID_DEVINTERFACE_HID = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
        public readonly static Guid GUID_DEVINTERFACE_KEYBOARD = new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd");
        public readonly static Guid GUID_DEVINTERFACE_MOUSE = new Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD");
        public readonly static Guid GUID_DEVINTERFACE_PRINTER = new Guid("{0ECEF634-6EF0-472A-8085-5AD023ECBCCD}");
        public readonly static Guid GUID_DEVINTERFACE_MODEM = new Guid("2C7089AA-2E0E-11D1-B114-00C04FC2AAE4");
        public readonly static Guid GUID_DEVINTERFACE_NET = new Guid("CAC88484-7515-4C03-82E6-71A87ABAC361");
        public readonly static Guid GUID_DEVINTERFACE_SENSOR = new Guid(0xBA1BB692, 0x9B7A, 0x4833, 0x9A, 0x1E, 0x52, 0x5E, 0xD1, 0x34, 0xE7, 0xE2);
        public readonly static Guid GUID_DEVINTERFACE_COMPORT = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");
        public readonly static Guid GUID_DEVINTERFACE_PARALLEL = new Guid("97F76EF0-F883-11D0-AF1F-0000F800845C");
        public readonly static Guid GUID_DEVINTERFACE_PARCLASS = new Guid("811FC6A5-F728-11D0-A537-0000F8753ED1");
        public readonly static Guid GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR = new Guid("4D36E978-E325-11CE-BFC1-08002BE10318");
        public readonly static Guid GUID_DEVINTERFACE_CDCHANGER = new Guid("53F56312-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_CDROM = new Guid("53F56308-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_DISK = new Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_FLOPPY = new Guid("53F56311-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_MEDIUMCHANGER = new Guid("53F56310-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_PARTITION = new Guid("53F5630A-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_STORAGEPORT = new Guid("2ACCFE60-C130-11D2-B082-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_TAPE = new Guid("53F5630B-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_VOLUME = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_DEVINTERFACE_WRITEONCEDISK = new Guid("53F5630C-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_IO_VOLUME_DEVICE_INTERFACE = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid MOUNTDEV_MOUNTED_DEVICE_GUID = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public readonly static Guid GUID_AVC_CLASS = new Guid("095780C3-48A1-4570-BD95-46707F78C2DC");
        public readonly static Guid GUID_VIRTUAL_AVC_CLASS = new Guid("616EF4D0-23CE-446D-A568-C31EB01913D0");
        public readonly static Guid KSCATEGORY_ACOUSTIC_ECHO_CANCEL = new Guid("BF963D80-C559-11D0-8A2B-00A0C9255AC1");
        public readonly static Guid KSCATEGORY_AUDIO = new Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196");
        public readonly static Guid KSCATEGORY_AUDIO_DEVICE = new Guid("FBF6F530-07B9-11D2-A71E-0000F8004788");
        public readonly static Guid KSCATEGORY_AUDIO_GFX = new Guid("9BAF9572-340C-11D3-ABDC-00A0C90AB16F");
        public readonly static Guid KSCATEGORY_AUDIO_SPLITTER = new Guid("9EA331FA-B91B-45F8-9285-BD2BC77AFCDE");
        public readonly static Guid KSCATEGORY_BDA_IP_SINK = new Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0");
        public readonly static Guid KSCATEGORY_BDA_NETWORK_EPG = new Guid("71985F49-1CA1-11d3-9CC8-00C04F7971E0");
        public readonly static Guid KSCATEGORY_BDA_NETWORK_PROVIDER = new Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0");
        public readonly static Guid KSCATEGORY_BDA_NETWORK_TUNER = new Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0");
        public readonly static Guid KSCATEGORY_BDA_RECEIVER_COMPONENT = new Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0");
        public readonly static Guid KSCATEGORY_BDA_TRANSPORT_INFORMATION = new Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E");
        public readonly static Guid KSCATEGORY_BRIDGE = new Guid("085AFF00-62CE-11CF-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_CAPTURE = new Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196");
        public readonly static Guid KSCATEGORY_CLOCK = new Guid("53172480-4791-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_COMMUNICATIONSTRANSFORM = new Guid("CF1DDA2C-9743-11D0-A3EE-00A0C9223196");
        public readonly static Guid KSCATEGORY_CROSSBAR = new Guid("A799A801-A46D-11D0-A18C-00A02401DCD4");
        public readonly static Guid KSCATEGORY_DATACOMPRESSOR = new Guid("1E84C900-7E70-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_DATADECOMPRESSOR = new Guid("2721AE20-7E70-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_DATATRANSFORM = new Guid("2EB07EA0-7E70-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_DRM_DESCRAMBLE = new Guid("FFBB6E3F-CCFE-4D84-90D9-421418B03A8E");
        public readonly static Guid KSCATEGORY_ENCODER = new Guid("19689BF6-C384-48fd-AD51-90E58C79F70B");
        public readonly static Guid KSCATEGORY_ESCALANTE_PLATFORM_DRIVER = new Guid("74F3AEA8-9768-11D1-8E07-00A0C95EC22E");
        public readonly static Guid KSCATEGORY_FILESYSTEM = new Guid("760FED5E-9357-11D0-A3CC-00A0C9223196");
        public readonly static Guid KSCATEGORY_INTERFACETRANSFORM = new Guid("CF1DDA2D-9743-11D0-A3EE-00A0C9223196");
        public readonly static Guid KSCATEGORY_MEDIUMTRANSFORM = new Guid("CF1DDA2E-9743-11D0-A3EE-00A0C9223196");
        public readonly static Guid KSCATEGORY_MICROPHONE_ARRAY_PROCESSOR = new Guid("830A44F2-A32D-476B-BE97-42845673B35A");
        public readonly static Guid KSCATEGORY_MIXER = new Guid("AD809C00-7B88-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_MULTIPLEXER = new Guid("7A5DE1D3-01A1-452c-B481-4FA2B96271E8");
        public readonly static Guid KSCATEGORY_NETWORK = new Guid("67C9CC3C-69C4-11D2-8759-00A0C9223196");
        public readonly static Guid KSCATEGORY_PREFERRED_MIDIOUT_DEVICE = new Guid("D6C50674-72C1-11D2-9755-0000F8004788");
        public readonly static Guid KSCATEGORY_PREFERRED_WAVEIN_DEVICE = new Guid("D6C50671-72C1-11D2-9755-0000F8004788");
        public readonly static Guid KSCATEGORY_PREFERRED_WAVEOUT_DEVICE = new Guid("D6C5066E-72C1-11D2-9755-0000F8004788");
        public readonly static Guid KSCATEGORY_PROXY = new Guid("97EBAACA-95BD-11D0-A3EA-00A0C9223196");
        public readonly static Guid KSCATEGORY_QUALITY = new Guid("97EBAACB-95BD-11D0-A3EA-00A0C9223196");
        public readonly static Guid KSCATEGORY_REALTIME = new Guid("EB115FFC-10C8-4964-831D-6DCB02E6F23F");
        public readonly static Guid KSCATEGORY_RENDER = new Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196");
        public readonly static Guid KSCATEGORY_SPLITTER = new Guid("0A4252A0-7E70-11D0-A5D6-28DB04C10000");
        public readonly static Guid KSCATEGORY_SYNTHESIZER = new Guid("DFF220F3-F70F-11D0-B917-00A0C9223196");
        public readonly static Guid KSCATEGORY_SYSAUDIO = new Guid("A7C7A5B1-5AF3-11D1-9CED-00A024BF0407");
        public readonly static Guid KSCATEGORY_TEXT = new Guid("6994AD06-93EF-11D0-A3CC-00A0C9223196");
        public readonly static Guid KSCATEGORY_TOPOLOGY = new Guid("DDA54A40-1E4C-11D1-A050-405705C10000");
        public readonly static Guid KSCATEGORY_TVAUDIO = new Guid("A799A802-A46D-11D0-A18C-00A02401DCD4");
        public readonly static Guid KSCATEGORY_TVTUNER = new Guid("A799A800-A46D-11D0-A18C-00A02401DCD4");
        public readonly static Guid KSCATEGORY_VBICODEC = new Guid("07DAD660-22F1-11D1-A9F4-00C04FBBDE8F");
        public readonly static Guid KSCATEGORY_VIDEO = new Guid("6994AD05-93EF-11D0-A3CC-00A0C9223196");
        public readonly static Guid KSCATEGORY_VIRTUAL = new Guid("3503EAC4-1F26-11D1-8AB0-00A0C9223196");
        public readonly static Guid KSCATEGORY_VPMUX = new Guid("A799A803-A46D-11D0-A18C-00A02401DCD4");
        public readonly static Guid KSCATEGORY_WDMAUD = new Guid("3E227E76-690D-11D2-8161-0000F8775BF1");
        public readonly static Guid KSMFT_CATEGORY_AUDIO_DECODER = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");
        public readonly static Guid KSMFT_CATEGORY_AUDIO_EFFECT = new Guid("11064c48-3648-4ed0-932e-05ce8ac811b7");
        public readonly static Guid KSMFT_CATEGORY_AUDIO_ENCODER = new Guid("91c64bd0-f91e-4d8c-9276-db248279d975");
        public readonly static Guid KSMFT_CATEGORY_DEMULTIPLEXER = new Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1");
        public readonly static Guid KSMFT_CATEGORY_MULTIPLEXER = new Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b");
        public readonly static Guid KSMFT_CATEGORY_OTHER = new Guid("90175d57-b7ea-4901-aeb3-933a8747756f");
        public readonly static Guid KSMFT_CATEGORY_VIDEO_DECODER = new Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91");
        public readonly static Guid KSMFT_CATEGORY_VIDEO_EFFECT = new Guid("12e17c21-532c-4a6e-8a1c-40825a736397");
        public readonly static Guid KSMFT_CATEGORY_VIDEO_ENCODER = new Guid("f79eac7d-e545-4387-bdee-d647d7bde42a");
        public readonly static Guid KSMFT_CATEGORY_VIDEO_PROCESSOR = new Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302");
        public readonly static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
        public readonly static Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER = new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");
        public readonly static Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");
        public readonly static Guid GUID_DEVINTERFACE_WPD = new Guid("6AC27878-A6FA-4155-BA85-F98F491D4F33");
        public readonly static Guid GUID_DEVINTERFACE_WPD_PRIVATE = new Guid("BA0C718F-4DED-49B7-BDD3-FABE28661211");
        public readonly static Guid GUID_DEVINTERFACE_SIDESHOW = new Guid("152E5811-FEB9-4B00-90F4-D32947AE1681");
        public readonly static Guid GUID_DEVINTERFACE_USBPRINT = new Guid(0x28D78FAD, 0x5A12, 0x11D1, 0xAE, 0x5B, 0x0, 0x0, 0xF8, 0x3, 0xA8, 0xC2);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public readonly static Guid GUID_BUS_TYPE_INTERNAL = new Guid(0x1530EA73, 0x86B, 0x11D1, 0xA0, 0x9F, 0x0, 0xC0, 0x4F, 0xC3, 0x40, 0xB1);
        public readonly static Guid GUID_BUS_TYPE_PCMCIA = new Guid(0x9343630, 0xAF9F, 0x11D0, 0x92, 0xE9, 0x0, 0x0, 0xF8, 0x1E, 0x1B, 0x30);
        public readonly static Guid GUID_BUS_TYPE_PCI = new Guid(0xC8EBDFB0, 0xB510, 0x11D0, 0x80, 0xE5, 0x0, 0xA0, 0xC9, 0x25, 0x42, 0xE3);
        public readonly static Guid GUID_BUS_TYPE_ISAPNP = new Guid(0xE676F854, 0xD87D, 0x11D0, 0x92, 0xB2, 0x0, 0xA0, 0xC9, 0x5, 0x5F, 0xC5);
        public readonly static Guid GUID_BUS_TYPE_EISA = new Guid(0xDDC35509, 0xF3FC, 0x11D0, 0xA5, 0x37, 0x0, 0x0, 0xF8, 0x75, 0x3E, 0xD1);
        public readonly static Guid GUID_BUS_TYPE_MCA = new Guid(0x1C75997A, 0xDC33, 0x11D0, 0x92, 0xB2, 0x0, 0xA0, 0xC9, 0x5, 0x5F, 0xC5);
        public readonly static Guid GUID_BUS_TYPE_SERENUM = new Guid(0x77114A87, 0x8944, 0x11D1, 0xBD, 0x90, 0x0, 0xA0, 0xC9, 0x6, 0xBE, 0x2D);
        public readonly static Guid GUID_BUS_TYPE_USB = new Guid(0x9D7DEBBC, 0xC85D, 0x11D1, 0x9E, 0xB4, 0x0, 0x60, 0x8, 0xC3, 0xA1, 0x9A);
        public readonly static Guid GUID_BUS_TYPE_LPTENUM = new Guid(0xC4CA1000, 0x2DDC, 0x11D5, 0xA1, 0x7A, 0x0, 0xC0, 0x4F, 0x60, 0x52, 0x4D);
        public readonly static Guid GUID_BUS_TYPE_USBPRINT = new Guid(0x441EE000, 0x4342, 0x11D5, 0xA1, 0x84, 0x0, 0xC0, 0x4F, 0x60, 0x52, 0x4D);
        public readonly static Guid GUID_BUS_TYPE_DOT4PRT = new Guid(0x441EE001, 0x4342, 0x11D5, 0xA1, 0x84, 0x0, 0xC0, 0x4F, 0x60, 0x52, 0x4D);
        public readonly static Guid GUID_BUS_TYPE_1394 = new Guid(0xF74E73EB, 0x9AC5, 0x45EB, 0xBE, 0x4D, 0x77, 0x2C, 0xC7, 0x1D, 0xDF, 0xB3);
        public readonly static Guid GUID_BUS_TYPE_HID = new Guid(0xEEAF37D0, 0x1963, 0x47C4, 0xAA, 0x48, 0x72, 0x47, 0x6D, 0xB7, 0xCF, 0x49);
        public readonly static Guid GUID_BUS_TYPE_AVC = new Guid(0xC06FF265, 0xAE09, 0x48F0, 0x81, 0x2C, 0x16, 0x75, 0x3D, 0x7C, 0xBA, 0x83);
        public readonly static Guid GUID_BUS_TYPE_IRDA = new Guid(0x7AE17DC1, 0xC944, 0x44D6, 0x88, 0x1F, 0x4C, 0x2E, 0x61, 0x5, 0x3B, 0xC1);
        public readonly static Guid GUID_BUS_TYPE_SD = new Guid(0xE700CC04, 0x4036, 0x4E89, 0x95, 0x79, 0x89, 0xEB, 0xF4, 0x5F, 0x0, 0xCD);
        public readonly static Guid GUID_BUS_TYPE_ACPI = new Guid(0xD7B46895, 0x1A, 0x4942, 0x89, 0x1F, 0xA7, 0xD4, 0x66, 0x10, 0xA8, 0x43);
        public readonly static Guid GUID_BUS_TYPE_SW_DEVICE = new Guid(0x6D10322, 0x7DE0, 0x4CEF, 0x8E, 0x25, 0x19, 0x7D, 0xE, 0x74, 0x42, 0xE2);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // '
        // ' Property type modifiers.  Used to modify base DEVPROP_TYPE_ values, as
        // ' appropriate.  Not valid as standalone DEVPROPTYPE values.
        // '
        public const int DEVPROP_TYPEMOD_ARRAY = 0x1000;  // ' array of fixed-sized data elements
        public const int DEVPROP_TYPEMOD_LIST = 0x2000;  // ' list of variable-sized data elements

        // '
        // ' Property data types.
        // '
        public const int DEVPROP_TYPE_EMPTY = 0x0;  // ' nothing, no property data
        public const int DEVPROP_TYPE_NULL = 0x1;  // ' null property data
        public const int DEVPROP_TYPE_SBYTE = 0x2;  // ' 8-bit signed int (SBYTE)
        public const int DEVPROP_TYPE_BYTE = 0x3;  // ' 8-bit unsigned int (BYTE)
        public const int DEVPROP_TYPE_INT16 = 0x4;  // ' 16-bit signed int (SHORT)
        public const int DEVPROP_TYPE_UINT16 = 0x5;  // ' 16-bit unsigned int (USHORT)
        public const int DEVPROP_TYPE_INT32 = 0x6;  // ' 32-bit signed int (LONG)
        public const int DEVPROP_TYPE_UINT32 = 0x7;  // ' 32-bit unsigned int (ULONG)
        public const int DEVPROP_TYPE_INT64 = 0x8;  // ' 64-bit signed int (LONG64)
        public const int DEVPROP_TYPE_UINT64 = 0x9;  // ' 64-bit unsigned int (ULONG64)
        public const int DEVPROP_TYPE_FLOAT = 0xA;  // ' 32-bit floating-point (FLOAT)
        public const int DEVPROP_TYPE_DOUBLE = 0xB;  // ' 64-bit floating-point (DOUBLE)
        public const int DEVPROP_TYPE_DECIMAL = 0xC;  // ' 128-bit data (DECIMAL)
        public const int DEVPROP_TYPE_GUID = 0xD;  // ' 128-bit unique identifier (GUID)
        public const int DEVPROP_TYPE_CURRENCY = 0xE;  // ' 64 bit signed int currency value (CURRENCY)
        public const int DEVPROP_TYPE_DATE = 0xF;  // ' date (DATE)
        public const int DEVPROP_TYPE_FILETIME = 0x10;  // ' file time (FILETIME)
        public const int DEVPROP_TYPE_BOOLEAN = 0x11;  // ' 8-bit boolean = (DEVPROP_BOOLEAN)
        public const int DEVPROP_TYPE_STRING = 0x12;  // ' null-terminated string
        public const int DEVPROP_TYPE_STRING_LIST = DEVPROP_TYPE_STRING | DEVPROP_TYPEMOD_LIST; // ' multi-sz string list
        public const int DEVPROP_TYPE_SECURITY_DESCRIPTOR = 0x13;  // ' self-relative binary SECURITY_DESCRIPTOR
        public const int DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING = 0x14;  // ' security descriptor string (SDDL format)
        public const int DEVPROP_TYPE_DEVPROPKEY = 0x15;  // ' device property key = (DEVPROPKEY)
        public const int DEVPROP_TYPE_DEVPROPTYPE = 0x16;  // ' device property type = (DEVPROPTYPE)
        public const int DEVPROP_TYPE_BINARY = DEVPROP_TYPE_BYTE | DEVPROP_TYPEMOD_ARRAY;  // ' custom binary data
        public const int DEVPROP_TYPE_ERROR = 0x17;  // ' 32-bit Win32 system error code
        public const int DEVPROP_TYPE_NTSTATUS = 0x18;  // ' 32-bit NTSTATUS code
        public const int DEVPROP_TYPE_STRING_INDIRECT = 0x19;  // ' string resource (@[path\]<dllname>,-<strId>)

        // '
        // ' Max base DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        // '
        public const int MAX_DEVPROP_TYPE = 0x19;  // ' max valid DEVPROP_TYPE_ value
        public const int MAX_DEVPROP_TYPEMOD = 0x2000;  // ' max valid DEVPROP_TYPEMOD_ value

        // '
        // ' Bitmasks for extracting DEVPROP_TYPE_ and DEVPROP_TYPEMOD_ values.
        // '
        public const int DEVPROP_MASK_TYPE = 0xFFF;  // ' range for base DEVPROP_TYPE_ values
        public const int DEVPROP_MASK_TYPEMOD = 0xF000;  // ' mask for DEVPROP_TYPEMOD_ type modifiers

        // '
        // ' Property type specific data types.
        // '

        // ' 8-bit boolean type definition for DEVPROP_TYPE_BOOLEAN (True=-1, False=0)

        public enum DEVPROP_BOOLEAN : byte
        {
            DEVPROP_FALSE = 0,
            DEVPROP_TRUE = 0xFF
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEVPROPKEY
        {
            [MarshalAs(UnmanagedType.Struct)]
            public Guid fmtid;
            public uint pid;

            public DEVPROPKEY(uint ui1, ushort s1, short s2, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, uint pid)
            {
                fmtid = new Guid((int)ui1, (short)s1, s2, b1, b2, b3, b4, b5, b6, b7, b8);
                this.pid = pid;
            }

            public override string ToString()
            {
                return GetKeyName(this);
            }

            public override bool Equals(object obj)
            {
                if (obj is DEVPROPKEY)
                {
                    if (((DEVPROPKEY)obj).fmtid.Equals(fmtid) && ((DEVPROPKEY)obj).pid == pid)
                    {
                        return true;
                    }
                }

                return false;
            }

            public static bool operator ==(DEVPROPKEY operand1, DEVPROPKEY operand2)
            {
                return operand1.Equals(operand2);
            }

            public static bool operator !=(DEVPROPKEY operand1, DEVPROPKEY operand2)
            {
                return !operand1.Equals(operand2);
            }
        }

        // '
        // ' DEVPKEY_NAME
        // ' Common DEVPKEY used to retrieve the display name for an object.
        // '
        public readonly static DEVPROPKEY DEVPKEY_NAME = new DEVPROPKEY(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x2, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 10U);    // ' DEVPROP_TYPE_STRING

        // '
        // ' Device properties
        // ' These DEVPKEYs correspond to the SetupAPI SPDRP_XXX device properties.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_DeviceDesc = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 2U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_HardwareIds = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 3U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_CompatibleIds = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 4U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_Service = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 6U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_Class = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 9U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_ClassGuid = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 10U);    // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_Device_Driver = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 11U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_ConfigFlags = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 12U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_Manufacturer = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 13U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_FriendlyName = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 14U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_LocationInfo = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 15U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_PDOName = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 16U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_Capabilities = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 17U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_UINumber = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 18U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_UpperFilters = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 19U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_LowerFilters = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 20U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_BusTypeGuid = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 21U);    // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_Device_LegacyBusType = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 22U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_BusNumber = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 23U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_EnumeratorName = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 24U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_Security = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 25U);    // ' DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public readonly static DEVPROPKEY DEVPKEY_Device_SecuritySDS = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 26U);    // ' DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DevType = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 27U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_Exclusive = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 28U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_Characteristics = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 29U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_Address = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 30U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_UINumberDescFormat = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 31U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_PowerData = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 32U);    // ' DEVPROP_TYPE_BINARY
        public readonly static DEVPROPKEY DEVPKEY_Device_RemovalPolicy = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 33U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_RemovalPolicyDefault = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 34U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_RemovalPolicyOverride = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 35U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_InstallState = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 36U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_LocationPaths = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 37U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_BaseContainerId = new DEVPROPKEY(0xA45C254E, 0xDF1C, 0x4EFD, 0x80, 0x20, 0x67, 0xD1, 0x46, 0xA8, 0x50, 0xE0, 38U);    // ' DEVPROP_TYPE_GUID

        // '
        // ' Device and Device Interface property
        // ' Common DEVPKEY used to retrieve the device instance id associated with devices and device interfaces.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_InstanceId = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 256U);   // ' DEVPROP_TYPE_STRING

        // '
        // ' Device properties
        // ' These DEVPKEYs correspond to a device's status and problem code.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_DevNodeStatus = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 2U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_ProblemCode = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 3U);     // ' DEVPROP_TYPE_UINT32

        // '
        // ' Device properties
        // ' These DEVPKEYs correspond to a device's relations.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_EjectionRelations = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 4U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_RemovalRelations = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 5U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_PowerRelations = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 6U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_BusRelations = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 7U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_Parent = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 8U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_Children = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 9U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_Siblings = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 10U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_TransportRelations = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 11U);    // ' DEVPROP_TYPE_STRING_LIST

        // '
        // ' Device property
        // ' This DEVPKEY corresponds to a the status code that resulted in a device to be in a problem state.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_ProblemStatus = new DEVPROPKEY(0x4340A6C5, 0x93FA, 0x4706, 0x97, 0x2C, 0x7B, 0x64, 0x80, 0x8, 0xA5, 0xA7, 12U);     // ' DEVPROP_TYPE_NTSTATUS

        // '
        // ' Device properties
        // ' These DEVPKEYs are set for the corresponding types of root-enumerated devices.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_Reported = new DEVPROPKEY(0x80497100, 0x8C73, 0x48B9, 0xAA, 0xD9, 0xCE, 0x38, 0x7E, 0x19, 0xC5, 0x6E, 2U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_Legacy = new DEVPROPKEY(0x80497100, 0x8C73, 0x48B9, 0xAA, 0xD9, 0xCE, 0x38, 0x7E, 0x19, 0xC5, 0x6E, 3U);     // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Device Container Id
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_ContainerId = new DEVPROPKEY(0x8C7ED206, 0x3F8A, 0x4827, 0xB3, 0xAB, 0xAE, 0x9E, 0x1F, 0xAE, 0xFC, 0x6C, 2U);     // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_Device_InLocalMachineContainer = new DEVPROPKEY(0x8C7ED206, 0x3F8A, 0x4827, 0xB3, 0xAB, 0xAE, 0x9E, 0x1F, 0xAE, 0xFC, 0x6C, 4U);    // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Device property
        // ' This DEVPKEY correspond to a device's model.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_Model = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 39U);    // ' DEVPROP_TYPE_STRING

        // '
        // ' Device Experience related Keys
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_ModelId = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 2U);     // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_Device_FriendlyNameAttributes = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 3U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_ManufacturerAttributes = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 4U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_PresenceNotForDevice = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 5U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_SignalStrength = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 6U);     // ' DEVPROP_TYPE_INT32
        public readonly static DEVPROPKEY DEVPKEY_Device_IsAssociateableByUserAction = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 7U); // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_ShowInUninstallUI = new DEVPROPKEY(0x80D81EA6, 0x7473, 0x4B0C, 0x82, 0x16, 0xEF, 0xC1, 0x1A, 0x2C, 0x4C, 0x8B, 8U);     // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Other Device properties
        // '
        public static DEVPROPKEY DEVPKEY_Numa_Proximity_Domain = DEVPKEY_Device_Numa_Proximity_Domain;
        public readonly static DEVPROPKEY DEVPKEY_Device_Numa_Proximity_Domain = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 1U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_DHP_Rebalance_Policy = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 2U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_Numa_Node = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 3U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 4U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_IsPresent = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 5U);     // ' DEVPROP_TYPE_BOOL
        public readonly static DEVPROPKEY DEVPKEY_Device_HasProblem = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 6U);     // ' DEVPROP_TYPE_BOOL
        public readonly static DEVPROPKEY DEVPKEY_Device_ConfigurationId = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 7U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_ReportedDeviceIdsHash = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 8U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_PhysicalDeviceLocation = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 9U);     // ' DEVPROP_TYPE_BINARY
        public readonly static DEVPROPKEY DEVPKEY_Device_BiosDeviceName = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 10U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverProblemDesc = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 11U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DebuggerSafe = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 12U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_PostInstallInProgress = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 13U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_Stack = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 14U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_ExtendedConfigurationIds = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 15U);  // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_IsRebootRequired = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 16U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_FirmwareDate = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 17U);    // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_Device_FirmwareVersion = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 18U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_FirmwareRevision = new DEVPROPKEY(0x540B947E, 0x8B40, 0x45BC, 0xA8, 0xA2, 0x6A, 0xB, 0x89, 0x4C, 0xBD, 0xA2, 19U);    // ' DEVPROP_TYPE_STRING

        // '
        // ' Device Session Id
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_SessionId = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 6U);     // ' DEVPROP_TYPE_UINT32

        // '
        // ' Device activity timestamp properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_InstallDate = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 100U);   // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_Device_FirstInstallDate = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 101U);   // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_Device_LastArrivalDate = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 102U);   // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_Device_LastRemovalDate = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 103U);   // ' DEVPROP_TYPE_FILETIME

        // '
        // ' Device driver properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverDate = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 2U);      // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverVersion = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 3U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverDesc = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 4U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverInfPath = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 5U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverInfSection = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 6U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverInfSectionExt = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 7U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_MatchingDeviceId = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 8U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverProvider = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 9U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverPropPageProvider = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 10U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverCoInstallers = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 11U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_Device_ResourcePickerTags = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 12U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_ResourcePickerExceptions = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 13U);   // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverRank = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 14U);     // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_Device_DriverLogoLevel = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 15U);     // ' DEVPROP_TYPE_UINT32

        // '
        // ' Device properties
        // ' These DEVPKEYs may be set by the driver package installed for a device.
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_NoConnectSound = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 17U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_GenericDriverInstalled = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 18U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_AdditionalSoftwareRequested = new DEVPROPKEY(0xA8B865DD, 0x2E3D, 0x4094, 0xAD, 0x97, 0xE5, 0x93, 0xA7, 0xC, 0x75, 0xD6, 19U); // 'DEVPROP_TYPE_BOOLEAN

        // '
        // ' Device safe-removal properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_Device_SafeRemovalRequired = new DEVPROPKEY(0xAFD97640, 0x86A3, 0x4210, 0xB6, 0x7C, 0x28, 0x9C, 0x41, 0xAA, 0xBE, 0x55, 2U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_Device_SafeRemovalRequiredOverride = new DEVPROPKEY(0xAFD97640, 0x86A3, 0x4210, 0xB6, 0x7C, 0x28, 0x9C, 0x41, 0xAA, 0xBE, 0x55, 3U); // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Device properties
        // ' These DEVPKEYs may be set by the driver package installed for a device.
        // '
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_Model = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 2U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_VendorWebSite = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 3U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_DetailedDescription = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 4U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_DocumentationLink = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 5U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_Icon = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 6U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DrvPkg_BrandingIcon = new DEVPROPKEY(0xCF73BB51, 0x3ABF, 0x44A2, 0x85, 0xE0, 0x9A, 0x3D, 0xC7, 0xA1, 0x21, 0x32, 7U);     // ' DEVPROP_TYPE_STRING_LIST

        // '
        // ' Device setup class properties
        // ' These DEVPKEYs correspond to the SetupAPI SPCRP_XXX setup class properties.
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_UpperFilters = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 19U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_LowerFilters = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 20U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_Security = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 25U);    // ' DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_SecuritySDS = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 26U);    // ' DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_DevType = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 27U);    // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_Exclusive = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 28U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_Characteristics = new DEVPROPKEY(0x4321918B, 0xF69E, 0x470D, 0xA5, 0xDE, 0x4D, 0x88, 0xC7, 0x5A, 0xD2, 0x4B, 29U);    // ' DEVPROP_TYPE_UINT32

        // '
        // ' Device setup class properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_Name = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 2U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_ClassName = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 3U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_Icon = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 4U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_ClassInstaller = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 5U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_PropPageProvider = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 6U);      // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_NoInstallClass = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 7U);      // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_NoDisplayClass = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 8U);      // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_SilentInstall = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 9U);      // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_NoUseClass = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 10U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_DefaultService = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 11U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_IconPath = new DEVPROPKEY(0x259ABFFC, 0x50A7, 0x47CE, 0xAF, 0x8, 0x68, 0xC9, 0xA7, 0xD7, 0x33, 0x66, 12U);     // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_DHPRebalanceOptOut = new DEVPROPKEY(0xD14D3EF3, 0x66CF, 0x4BA2, 0x9D, 0x38, 0xD, 0xDB, 0x37, 0xAB, 0x47, 0x1, 2U);    // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Other Device setup class properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceClass_ClassCoInstallers = new DEVPROPKEY(0x713D1703, 0xA2E2, 0x49F5, 0x92, 0x14, 0x56, 0x47, 0x2E, 0xF3, 0xDA, 0x5C, 2U);     // ' DEVPROP_TYPE_STRING_LIST

        // '
        // ' Device interface properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterface_FriendlyName = new DEVPROPKEY(0x26E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22, 2U);     // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterface_Enabled = new DEVPROPKEY(0x26E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22, 3U);     // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterface_ClassGuid = new DEVPROPKEY(0x26E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22, 4U);     // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterface_ReferenceString = new DEVPROPKEY(0x26E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22, 5U);   // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterface_Restricted = new DEVPROPKEY(0x26E516E, 0xB814, 0x414B, 0x83, 0xCD, 0x85, 0x6D, 0x6F, 0xEF, 0x48, 0x22, 6U);   // ' DEVPROP_TYPE_BOOLEAN

        // '
        // ' Device interface class properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterfaceClass_DefaultInterface = new DEVPROPKEY(0x14C83A99, 0xB3F, 0x44B7, 0xBE, 0x4C, 0xA1, 0x78, 0xD3, 0x99, 0x5, 0x64, 2U); // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceInterfaceClass_Name = new DEVPROPKEY(0x14C83A99, 0xB3F, 0x44B7, 0xBE, 0x4C, 0xA1, 0x78, 0xD3, 0x99, 0x5, 0x64, 3U); // ' DEVPROP_TYPE_STRING

        // '
        // ' Device Container Properties
        // '
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Address = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 51U);    // ' DEVPROP_TYPE_STRING  Or  DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_DiscoveryMethod = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 52U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsEncrypted = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 53U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsAuthenticated = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 54U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsConnected = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 55U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsPaired = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 56U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Icon = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 57U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Version = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 65U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Last_Seen = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 66U);    // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Last_Connected = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 67U);    // ' DEVPROP_TYPE_FILETIME
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsShowInDisconnectedState = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 68U);   // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsLocalMachine = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 70U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_MetadataPath = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 71U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsMetadataSearchInProgress = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 72U);          // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_MetadataChecksum = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 73U);            // ' DEVPROP_TYPE_BINARY
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsNotInterestingForDisplay = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 74U);          // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_LaunchDeviceStageOnDeviceConnect = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 76U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_LaunchDeviceStageFromExplorer = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 77U);       // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_BaselineExperienceId = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 78U);    // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsDeviceUniquelyIdentifiable = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 79U);        // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_AssociationArray = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 80U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_DeviceDescription1 = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 81U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_DeviceDescription2 = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 82U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_HasProblem = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 83U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsSharedDevice = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 84U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsNetworkDevice = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 85U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsDefaultDevice = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 86U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_MetadataCabinet = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 87U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_RequiresPairingElevation = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 88U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_ExperienceId = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 89U);    // ' DEVPROP_TYPE_GUID
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Category = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 90U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Category_Desc_Singular = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 91U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Category_Desc_Plural = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 92U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Category_Icon = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 93U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_CategoryGroup_Desc = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 94U);    // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_CategoryGroup_Icon = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 95U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_PrimaryCategory = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 97U);    // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_UnpairUninstall = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 98U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_RequiresUninstallElevation = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 99U);  // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_DeviceFunctionSubRank = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 100U);   // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_AlwaysShowDeviceAsConnected = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 101U);    // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_ConfigFlags = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 105U);   // ' DEVPROP_TYPE_UINT32
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_PrivilegedPackageFamilyNames = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 106U);   // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_CustomPrivilegedPackageFamilyNames = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 107U);   // ' DEVPROP_TYPE_STRING_LIST
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_IsRebootRequired = new DEVPROPKEY(0x78C34FC8, 0x104A, 0x4ACA, 0x9E, 0xA4, 0x52, 0x4D, 0x52, 0x99, 0x6E, 0x57, 108U);   // ' DEVPROP_TYPE_BOOLEAN
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_FriendlyName = new DEVPROPKEY(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 12288U); // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_Manufacturer = new DEVPROPKEY(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 8192U);  // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_ModelName = new DEVPROPKEY(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 8194U);  // ' DEVPROP_TYPE_STRING (localizable)
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_ModelNumber = new DEVPROPKEY(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 8195U);  // ' DEVPROP_TYPE_STRING
        public readonly static DEVPROPKEY DEVPKEY_DeviceContainer_InstallInProgress = new DEVPROPKEY(0x83DA6326, 0x97A6, 0x4088, 0x94, 0x53, 0xA1, 0x92, 0x3F, 0x57, 0x3B, 0x29, 9U);     // ' DEVPROP_TYPE_BOOLEAN

        private static string GetKeyName(DEVPROPKEY dpk)
        {
            var fi = typeof(DevProp).GetFields(BindingFlags.Static | BindingFlags.Public);
            DEVPROPKEY g;
            foreach (var fe in fi)
            {
                if (fe.FieldType != typeof(DEVPROPKEY))
                    continue;
                g = (DEVPROPKEY)fe.GetValue(null);
                if (g == dpk)
                {
                    // ' we found it!
                    var strs = TextTools.Split(fe.Name, "_");
                    string s = TextTools.SeparateCamel(strs[1]) + " Property: " + "\r\n" + TextTools.SeparateCamel(strs[2]);
                    return s;
                }
            }

            return dpk.fmtid.ToString() + ": " + dpk.pid;
        }

        public enum DEVICE_POWER_STATE
        {
            PowerDeviceUnspecified = 0,
            PowerDeviceD0 = 1,
            PowerDeviceD1 = 2,
            PowerDeviceD2 = 3,
            PowerDeviceD3 = 4,
            PowerDeviceMaximum = 5
        }

        public enum SYSTEM_POWER_STATE
        {
            PowerSystemUnspecified = 0,
            PowerSystemWorking = 1,
            PowerSystemSleeping1 = 2,
            PowerSystemSleeping2 = 3,
            PowerSystemSleeping3 = 4,
            PowerSystemHibernate = 5,
            PowerSystemShutdown = 6,
            PowerSystemMaximum = 7
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEVICE_CAPABALITIES
        {
            public ushort Size;
            public ushort Version;
            public DevClassPresenting.DeviceCapabilities Capabilities;
            public int Address;
            public int UINumber;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 7)]
            public DEVICE_POWER_STATE[] DeviceState;
            public SYSTEM_POWER_STATE SystemWake;
            public DEVICE_POWER_STATE DeviceWake;
            public int D1Latency;
            public int D2Latency;
            public int D3Latency;
        }


        /// <summary>
        /// Parses a comma-separated list of string values into an array of enumeration values.
        /// </summary>
        /// <typeparam name="T">The enum type to parse.</typeparam>
        /// <param name="values">The comma-separated list of strings to parse.</param>
        /// <returns>An array of T</returns>
        /// <remarks></remarks>
        public static T[] EnumListParse<T>(string values)
        {
            var x = default(T);
            object l = null;
            if (x.GetType().IsEnum == false)
            {
                throw new ArgumentException("T must be an enumeration type.");
            }

            var vs = TextTools.Split(values, ",");
            T[] vOut = null;
            var enames = Enum.GetNames(typeof(T));
            int i = 0;
            int c = 0;
            int e = 0;
            if (vs is null)
                return null;
            c = vs.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                vs[i] = vs[i].Trim();
                if (enames.Contains(vs[i]))
                {
                    x = (T)Enum.Parse(typeof(T), vs[i]);
                    Array.Resize(ref vOut, e + 1);
                    vOut[e] = x;
                    e += 1;
                }
            }

            return vOut;
        }


        /// <summary>
        /// Parses a comma-separated list of string values into an a flag enum result.
        /// </summary>
        /// <typeparam name="T">The enum type to parse.</typeparam>
        /// <param name="values">The comma-separated list of strings to parse.</param>
        /// <returns>An array of T</returns>
        /// <remarks></remarks>
        public static int FlagsParse<T>(string values)
        {
            int x;
            object l = null;
            if (typeof(T).IsEnum == false)
            {
                throw new ArgumentException("T must be an enumeration type.");
                return default;
            }

            var vs = TextTools.Split(values, ",");
            int vOut = 0;
            var enames = Enum.GetNames(typeof(T));
            int i = 0;
            int c = 0;
            int e = 0;
            if (vs is null)
                return default;
            c = vs.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                vs[i] = vs[i].Trim();
                if (enames.Contains(vs[i]))
                {
                    x = (int)(Enum.Parse(typeof(T), vs[i]));
                    vOut = vOut | x;
                    e += 1;
                }
            }

            return vOut;
        }
    }



    /// <summary>
    /// Device property types.
    /// </summary>
    /// <remarks></remarks>
    public enum DevPropTypes
    {
        /// <summary>
        /// nothing, no property data
        /// </summary>
        [Description("nothing, no property data")]
        Empty = DevProp.DEVPROP_TYPE_EMPTY,

        /// <summary>
        /// null property data
        /// </summary>
        [Description("null property data")]
        Null = DevProp.DEVPROP_TYPE_NULL,

        /// <summary>
        /// 8-bit signed int (SBYTE)
        /// </summary>
        [Description("8-bit signed int (SBYTE)")]
        SByte = DevProp.DEVPROP_TYPE_SBYTE,

        /// <summary>
        /// 8-bit unsigned int (BYTE)
        /// </summary>
        [Description("8-bit unsigned int (BYTE)")]
        Byte = DevProp.DEVPROP_TYPE_BYTE,

        /// <summary>
        /// 16-bit signed int (SHORT)
        /// </summary>
        [Description("16-bit signed int (SHORT)")]
        Int16 = DevProp.DEVPROP_TYPE_INT16,

        /// <summary>
        /// 16-bit unsigned int (USHORT)
        /// </summary>
        [Description("16-bit unsigned int (USHORT)")]
        UInt16 = DevProp.DEVPROP_TYPE_UINT16,

        /// <summary>
        /// 32-bit signed int (LONG)
        /// </summary>
        [Description("32-bit signed int (LONG)")]
        Int32 = DevProp.DEVPROP_TYPE_INT32,

        /// <summary>
        /// 32-bit unsigned int (ULONG)
        /// </summary>
        [Description("32-bit unsigned int (ULONG)")]
        UInt32 = DevProp.DEVPROP_TYPE_UINT32,

        /// <summary>
        /// 64-bit signed int (LONG64)
        /// </summary>
        [Description("64-bit signed int (LONG64)")]
        Int64 = DevProp.DEVPROP_TYPE_INT64,

        /// <summary>
        /// 64-bit unsigned int (ULONG64)
        /// </summary>
        [Description("64-bit unsigned int (ULONG64)")]
        UInt64 = DevProp.DEVPROP_TYPE_UINT64,

        /// <summary>
        /// 32-bit floating-point (FLOAT)
        /// </summary>
        [Description("32-bit floating-point (FLOAT)")]
        Float = DevProp.DEVPROP_TYPE_FLOAT,

        /// <summary>
        /// 64-bit floating-point (DOUBLE)
        /// </summary>
        [Description("64-bit floating-point (DOUBLE)")]
        Double = DevProp.DEVPROP_TYPE_DOUBLE,

        /// <summary>
        /// 128-bit data (DECIMAL)
        /// </summary>
        [Description("128-bit data (DECIMAL)")]
        Decimal = DevProp.DEVPROP_TYPE_DECIMAL,

        /// <summary>
        /// 128-bit unique identifier (GUID)
        /// </summary>
        [Description("128-bit unique identifier (GUID)")]
        Guid = DevProp.DEVPROP_TYPE_GUID,

        /// <summary>
        /// 64 bit signed int currency value (CURRENCY)
        /// </summary>
        [Description("64 bit signed int currency value (CURRENCY)")]
        Currency = DevProp.DEVPROP_TYPE_CURRENCY,

        /// <summary>
        /// date (DATE)
        /// </summary>
        [Description("date (DATE)")]
        Date = DevProp.DEVPROP_TYPE_DATE,

        /// <summary>
        /// file time (FILETIME)
        /// </summary>
        [Description("file time (FILETIME)")]
        FileTime = DevProp.DEVPROP_TYPE_FILETIME,

        /// <summary>
        /// 8-bit boolean = (DEVPROP_BOOLEAN)
        /// </summary>
        [Description("8-bit boolean = (DEVPROP_BOOLEAN)")]
        Boolean = DevProp.DEVPROP_TYPE_BOOLEAN,

        /// <summary>
        /// null-terminated string
        /// </summary>
        [Description("null-terminated string")]
        String = DevProp.DEVPROP_TYPE_STRING,

        /// <summary>
        /// multi-sz string list
        /// </summary>
        [Description("multi-sz string list")]
        StringList = DevProp.DEVPROP_TYPE_STRING | DevProp.DEVPROP_TYPEMOD_LIST,

        /// <summary>
        /// self-relative binary SECURITY_DESCRIPTOR
        /// </summary>
        [Description("self-relative binary SECURITY_DESCRIPTOR")]
        SecurityDescriptor = DevProp.DEVPROP_TYPE_SECURITY_DESCRIPTOR,

        /// <summary>
        /// security descriptor string (SDDL format)
        /// </summary>
        [Description("security descriptor string (SDDL format)")]
        SecurityDescriptorString = DevProp.DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING,

        /// <summary>
        /// device property key = (DEVPROPKEY)
        /// </summary>
        [Description("device property key = (DEVPROPKEY)")]
        DevPropKey = DevProp.DEVPROP_TYPE_DEVPROPKEY,

        /// <summary>
        /// device property type = (DEVPROPTYPE)
        /// </summary>
        [Description("device property type = (DEVPROPTYPE)")]
        DevPropType = DevProp.DEVPROP_TYPE_DEVPROPTYPE,

        /// <summary>
        /// custom binary data
        /// </summary>
        [Description("custom binary data")]
        Binary = DevProp.DEVPROP_TYPE_BYTE | DevProp.DEVPROP_TYPEMOD_ARRAY,

        /// <summary>
        /// 32-bit Win32 system error code
        /// </summary>
        [Description("32-bit Win32 system error code")]
        Error = DevProp.DEVPROP_TYPE_ERROR,

        /// <summary>
        /// 32-bit NTSTATUS code
        /// </summary>
        [Description("32-bit NTSTATUS code")]
        NTStatus = DevProp.DEVPROP_TYPE_NTSTATUS,

        /// <summary>
        /// string resource (@[path\]&lt;dllname&gt;,-&lt;strId&gt;)
        /// </summary>
        [Description(@"string resource (@[path\]<dllname>,-<strId>)")]
        StringIndirect = DevProp.DEVPROP_TYPE_STRING_INDIRECT,

        /// <summary>
        /// Array property key type modifier.
        /// </summary>
        /// <remarks></remarks>
        Array = DevProp.DEVPROP_TYPEMOD_ARRAY,

        /// <summary>
        /// List property key type modifier.
        /// </summary>
        /// <remarks></remarks>
        List = DevProp.DEVPROP_TYPEMOD_LIST
    }

    /// <summary>
    /// Enumeration flags for the SetupDiGetClassDevs
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum ClassDevFlags : int
    {

        /// <summary>
        /// Return only the device that is associated with the system default device interface, if one is set, for the specified device interface classes.
        /// </summary>
        /// <remarks></remarks>
        Default = DevProp.DIGCF_DEFAULT,

        /// <summary>
        /// Return only devices that are currently present in a system.
        /// </summary>
        /// <remarks></remarks>
        Present = DevProp.DIGCF_PRESENT,

        /// <summary>
        /// Return a list of installed devices for all device setup classes or all device interface classes.
        /// </summary>
        /// <remarks></remarks>
        AllClasses = DevProp.DIGCF_ALLCLASSES,

        /// <summary>
        /// Return only devices that are a part of the current hardware profile.
        /// </summary>
        /// <remarks></remarks>
        Profile = DevProp.DIGCF_PROFILE,

        /// <summary>
        /// Return devices that support device interfaces for the specified device interface classes.
        /// This flag must be set in the Flags parameter if the Enumerator parameter specifies a device instance ID.
        /// </summary>
        /// <remarks></remarks>
        DeviceInterface = DevProp.DIGCF_DEVICEINTERFACE
    }
}