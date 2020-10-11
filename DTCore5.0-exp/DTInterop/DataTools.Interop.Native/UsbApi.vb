'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: UsbApi
''         USB-related structures, enums and functions.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports CoreCT.Memory

Namespace Native

#Region "USB Subsystem"

    Friend Module libusb

        ''' <summary>
        ''' Pack 1 structures for USB.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const gPack As Integer = 1

#Region "WDK Errors"

        Public Enum USBRESULT As Integer
            ''' <summary>
            ''' Operation succeeded
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Operation succeeded")>
            OK = 0

            ''' <summary>
            ''' Operation not permitted
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Operation not permitted")>
            EPERM = 1

            ''' <summary>
            ''' No entry, ENOFILE, no such file or directory
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No entry, ENOFILE, no such file or directory")>
            ENOENT = 2

            ''' <summary>
            ''' No such process
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No such process")>
            ESRCH = 3

            ''' <summary>
            ''' Interrupted function call
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Interrupted function call")>
            EINTR = 4

            ''' <summary>
            ''' Input/output error
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Input/output error")>
            EIO = 5

            ''' <summary>
            ''' No such device or address
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No such device or address")>
            ENXIO = 6

            ''' <summary>
            ''' Arg list too long
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Arg list too long")>
            E2BIG = 7

            ''' <summary>
            ''' Exec format error
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Exec format error")>
            ENOEXEC = 8

            ''' <summary>
            ''' Bad file descriptor
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Bad file descriptor")>
            EBADF = 9

            ''' <summary>
            ''' No child processes
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No child processes")>
            ECHILD = 10

            ''' <summary>
            ''' Resource temporarily unavailable
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Resource temporarily unavailable")>
            EAGAIN = 11

            ''' <summary>
            ''' Not enough space
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Not enough space")>
            ENOMEM = 12

            ''' <summary>
            ''' Permission denied
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Permission denied")>
            EACCES = 13

            ''' <summary>
            ''' Bad address
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Bad address")>
            EFAULT = 14

            ''' <summary>
            ''' strerror reports ""Resource device""
            ''' </summary>
            ''' <remarks></remarks>
            <Description("strerror reports ""Resource device""")>
            EBUSY = 16

            ''' <summary>
            ''' File exists
            ''' </summary>
            ''' <remarks></remarks>
            <Description("File exists")>
            EEXIST = 17

            ''' <summary>
            ''' Improper link (cross-device link?)
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Improper link (cross-device link?)")>
            EXDEV = 18

            ''' <summary>
            ''' No such device
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No such device")>
            ENODEV = 19

            ''' <summary>
            ''' Not a directory
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Not a directory")>
            ENOTDIR = 20

            ''' <summary>
            ''' Is a directory
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Is a directory")>
            EISDIR = 21

            ''' <summary>
            ''' Invalid argument
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Invalid argument")>
            EINVAL = 22

            ''' <summary>
            ''' Too many open files in system
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Too many open files in system")>
            ENFILE = 23

            ''' <summary>
            ''' Too many open files
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Too many open files")>
            EMFILE = 24

            ''' <summary>
            ''' Inappropriate I/O control operation
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Inappropriate I/O control operation")>
            ENOTTY = 25

            ''' <summary>
            ''' File too large
            ''' </summary>
            ''' <remarks></remarks>
            <Description("File too large")>
            EFBIG = 27

            ''' <summary>
            ''' No space left on device
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No space left on device")>
            ENOSPC = 28

            ''' <summary>
            ''' Invalid seek (seek on a pipe?)
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Invalid seek (seek on a pipe?)")>
            ESPIPE = 29

            ''' <summary>
            ''' Read-only file system
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Read-only file system")>
            EROFS = 30

            ''' <summary>
            ''' Too many links
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Too many links")>
            EMLINK = 31

            ''' <summary>
            ''' Broken pipe
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Broken pipe")>
            EPIPE = 32

            ''' <summary>
            ''' Domain error (math functions)
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Domain error (math functions)")>
            EDOM = 33

            ''' <summary>
            ''' Result too large (possibly too small)
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Result too large (possibly too small)")>
            ERANGE = 34

            ''' <summary>
            ''' Resource deadlock avoided
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Resource deadlock avoided")>
            EDEADLK = 36

            ''' <summary>
            ''' Filename too long
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Filename too long")>
            ENAMETOOLONG = 38

            ''' <summary>
            ''' No locks available
            ''' </summary>
            ''' <remarks></remarks>
            <Description("No locks available")>
            ENOLCK = 39

            ''' <summary>
            ''' Function not implemented
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Function not implemented")>
            ENOSYS = 40

            ''' <summary>
            ''' Directory not empty
            ''' </summary>
            ''' <remarks></remarks>
            <Description("Directory not empty")>
            ENOTEMPTY = 41

        End Enum

        Public Function FormatUsbResult(r As USBRESULT) As String

            Dim fi() As FieldInfo = r.GetType.GetFields(BindingFlags.Public Or BindingFlags.Static)

            For Each fe In fi
                If CInt(fe.GetValue(r)) = r Then
                    Dim attr As DescriptionAttribute
                    attr = CType(fe.GetCustomAttribute(GetType(DescriptionAttribute)), DescriptionAttribute)
                    Return attr.Description
                End If
            Next

            Return Nothing
        End Function

#End Region

        Public Const LIBUSB_PATH_MAX = 512

        ''
        ' USB spec information
        '
        ' This is all stuff grabbed from various USB specs and is pretty much
        ' not subject to change
        ''

        ''
        ' Device and/or Interface Class codes
        ''
        Public Const USB_CLASS_PER_INTERFACE = 0  '' for DeviceClass ''
        Public Const USB_CLASS_AUDIO = 1
        Public Const USB_CLASS_COMM = 2
        Public Const USB_CLASS_HID = 3
        Public Const USB_CLASS_PRINTER = 7
        Public Const USB_CLASS_MASS_STORAGE = 8
        Public Const USB_CLASS_HUB = 9
        Public Const USB_CLASS_DATA = 10
        Public Const USB_CLASS_VENDOR_SPEC = &HFF

        ''
        ' Descriptor types
        ''
        Public Const USB_DT_DEVICE = &H1
        Public Const USB_DT_CONFIG = &H2
        Public Const USB_DT_STRING = &H3
        Public Const USB_DT_INTERFACE = &H4
        Public Const USB_DT_ENDPOINT = &H5

        Public Const USB_DT_HID = &H21
        Public Const USB_DT_REPORT = &H22
        Public Const USB_DT_PHYSICAL = &H23
        Public Const USB_DT_HUB = &H29

        ''
        ' Descriptor sizes per descriptor type
        ''
        Public Const USB_DT_DEVICE_SIZE = 18
        Public Const USB_DT_CONFIG_SIZE = 9
        Public Const USB_DT_INTERFACE_SIZE = 9
        Public Const USB_DT_ENDPOINT_SIZE = 7
        Public Const USB_DT_ENDPOINT_AUDIO_SIZE = 9  '' Audio extension ''
        Public Const USB_DT_HUB_NONVAR_SIZE = 7

        '' ensure byte-packed structures ''

        '' All standard descriptors have these 2 fields in common ''
        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_descriptor_header
            Public bLength As Byte
            Public bDescriptorType As Byte
        End Structure

        '' String descriptor ''
        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_string_descriptor

            Public bLength As Byte
            Public bDescriptorType As Byte

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)>
            Public wData() As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure lpusb_string_descriptor
            Public ptr As MemPtr

            Public ReadOnly Property bLength As Byte
                Get
                    If ptr = IntPtr.Zero Then Return 0
                    Return ptr.ByteAt(0)
                End Get
            End Property

            Public ReadOnly Property descriptorType As Byte
                Get
                    If ptr = IntPtr.Zero Then Return 0
                    Return ptr.ByteAt(1)
                End Get
            End Property

            Public ReadOnly Property data As String
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Return System.Text.Encoding.Unicode.GetString(ptr.ToByteArray(2, bLength))
                End Get
            End Property

        End Structure

        ' '' HID descriptor ''
        '<StructLayout(LayoutKind.Sequential, Pack:=gPack)> _
        'Public Structure usb_hid_descriptor
        '    Public bLength As Byte
        '    Public bDescriptorType As Byte
        '    Public bcdHID As UShort
        '    Public bCountryCode As Byte
        '    Public bNumDescriptors As Byte
        'End Structure

        '' Endpopublic descriptor as integer ''
        Public Const USB_MAXENDPOINTS = 32

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_endpoint_descriptor

            Public bLength As Byte
            Public bDescriptorType As Byte
            Public bEndpointAddress As Byte
            Public bmAttributes As Byte
            Public wMaxPacketSize As UShort
            Public bInterval As Byte
            Public bRefresh As Byte
            Public bSynchAddress As Byte

            Public extra As IntPtr '' Extra descriptors ''
            Public extralen As Integer

            Public ReadOnly Property extra_desc As Byte()
                Get
                    Dim mm As MemPtr = extra
                    extra_desc = mm.ToByteArray(0, extralen)
                End Get
            End Property

        End Structure

        Public Const USB_ENDPOINT_ADDRESS_MASK = &HF    '' in bEndpointAddress ''
        Public Const USB_ENDPOINT_DIR_MASK = &H80

        Public Const USB_ENDPOINT_TYPE_MASK = &H3    '' in bmAttributes ''
        Public Const USB_ENDPOINT_TYPE_CONTROL = 0
        Public Const USB_ENDPOINT_TYPE_ISOCHRONOUS = 1
        Public Const USB_ENDPOINT_TYPE_BULK = 2
        Public Const USB_ENDPOINT_TYPE_INTERRUPT = 3

        '' Interface descriptor ''
        Public Const USB_MAXINTERFACES = 32

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_interface_descriptor

            Public bLength As Byte
            Public bDescriptorType As Byte
            Public bInterfaceNumber As Byte
            Public bAlternateSetting As Byte
            Public bNumEndpoints As Byte
            Public bInterfaceClass As Byte
            Public bInterfaceSubClass As Byte
            Public bInterfaceProtocol As Byte
            Public iInterface As Byte

            Private ep As IntPtr ' usb_endpoint_descriptor 'endpopublic Public as integer extra As IntPtr

            Public ReadOnly Property endpoint As usb_endpoint_descriptor
                Get
                    Dim mm As MemPtr = ep
                    endpoint = mm.ToStruct(Of usb_endpoint_descriptor)
                End Get
            End Property

            Public extra As IntPtr
            Public extralen As Integer

            Public ReadOnly Property hidDescriptor As usb_hid_descriptor
                Get
                    Dim mm As SafePtr = CType(extra_desc, SafePtr)

                    Return mm.ToStruct(Of usb_hid_descriptor)
                End Get
            End Property

            Public ReadOnly Property extra_desc As Byte()
                Get
                    Dim mm As MemPtr = extra
                    extra_desc = mm.ToByteArray(0, extralen)
                End Get
            End Property

        End Structure

        Public Const USB_MAXALTSETTING = 128    '' Hard limit ''

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_interface
            Public altsetting As IntPtr ' usb_interface_descriptor
            Public num_altsetting As Integer

            Public ReadOnly Property altsettings() As usb_interface_descriptor()
                Get
                    Dim alt() As usb_interface_descriptor
                    ReDim alt(num_altsetting - 1)
                    Dim p As MemPtr = altsetting

                    For i = 0 To num_altsetting - 1

                        alt(i) = p.ToStruct(Of usb_interface_descriptor)
                        p += Marshal.SizeOf(Of usb_interface_descriptor)()
                    Next
                    Return alt
                End Get
            End Property

        End Structure

        '' Configuration descriptor information.. ''
        Public Const USB_MAXCONFIG = 8

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_config_descriptor

            Public bLength As Byte
            Public bDescriptorType As Byte
            Public wTotalLength As UShort
            Public bNumInterfaces As Byte
            Public bConfigurationValue As Byte
            Public iConfiguration As Byte
            Public bmAttributes As Byte
            Public MaxPower As Byte

            Private iface As IntPtr ' usb_interface 'interface

            Public ReadOnly Property [interface] As usb_interface
                Get
                    Dim mm As MemPtr = iface

                    [interface] = mm.ToStruct(Of usb_interface)
                End Get
            End Property

            Public extra As IntPtr  '' Extra descriptors ''
            Public extralen As Integer

            Public ReadOnly Property extra_desc As Byte()
                Get
                    Dim mm As MemPtr = extra
                    extra_desc = mm.ToByteArray(0, extralen)
                End Get
            End Property

        End Structure

        '' Device descriptor ''
        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_device_descriptor
            Public bLength As Byte
            Public bDescriptorType As Byte
            Public bcdUSB As UShort
            Public bDeviceClass As Byte
            Public bDeviceSubClass As Byte
            Public bDeviceProtocol As Byte
            Public bMaxPacketSize0 As Byte
            Public idVendor As UShort
            Public idProduct As UShort
            Public bcdDevice As UShort
            Public iManufacturer As Byte
            Public iProduct As Byte
            Public iSerialNumber As Byte
            Public bNumConfigurations As Byte
        End Structure
        '' Device descriptor ''

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_device_descriptor_strings
            Public iManufacturer As String
            Public iProduct As String
            Public iSerialNumber As String
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_ctrl_setup

            Public bRequestType As Byte
            Public bRequest As Byte
            Public wValue As UShort
            Public wIndex As UShort
            Public wLength As UShort
        End Structure

        ''
        ' Standard requests
        ''
        Public Const USB_REQ_GET_STATUS = &H0
        Public Const USB_REQ_CLEAR_FEATURE = &H1
        '' = &h02 is reserved ''
        Public Const USB_REQ_SET_FEATURE = &H3
        '' = &h04 is reserved ''
        Public Const USB_REQ_SET_ADDRESS = &H5
        Public Const USB_REQ_GET_DESCRIPTOR = &H6
        Public Const USB_REQ_SET_DESCRIPTOR = &H7
        Public Const USB_REQ_GET_CONFIGURATION = &H8
        Public Const USB_REQ_SET_CONFIGURATION = &H9
        Public Const USB_REQ_GET_INTERFACE = &HA
        Public Const USB_REQ_SET_INTERFACE = &HB
        Public Const USB_REQ_SYNCH_FRAME = &HC

        Public Const USB_TYPE_STANDARD = (&H0 << 5)
        Public Const USB_TYPE_CLASS = (&H1 << 5)
        Public Const USB_TYPE_VENDOR = (&H2 << 5)
        Public Const USB_TYPE_RESERVED = (&H3 << 5)

        Public Const USB_RECIP_DEVICE = &H0
        Public Const USB_RECIP_INTERFACE = &H1
        Public Const USB_RECIP_ENDPOINT = &H2
        Public Const USB_RECIP_OTHER = &H3

        ''
        ' Various libusb API related stuff
        ''

        Public Const USB_ENDPOINT_IN = &H80
        Public Const USB_ENDPOINT_OUT = &H0

        '' Error codes ''
        Public Const USB_ERROR_BEGIN = 500000

        ''
        ' This is supposed to look weird. This file is generated from autoconf
        ' and I didn't want to make this too complicated.
        ''
        Public Function USB_LE16_TO_CPU(x As UShort) As UShort
            Return x
        End Function

        ''
        ' Device reset types for usb_reset_ex.
        ' http://msdn.microsoft.com/en-us/library/ff537269%28VS.85%29.aspx
        ' http://msdn.microsoft.com/en-us/library/ff537243%28v=vs.85%29.aspx
        ''
        Public Const USB_RESET_TYPE_RESET_PORT = (1 << 0)
        Public Const USB_RESET_TYPE_CYCLE_PORT = (1 << 1)
        Public Const USB_RESET_TYPE_FULL_RESET = (USB_RESET_TYPE_CYCLE_PORT Or USB_RESET_TYPE_RESET_PORT)

        '' Data types ''
        '' public structure usb_device ''
        '' public structure usb_bus ''

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_device
            Public [next] As IntPtr ' usb_device
            Public [prev] As IntPtr ' usb_device

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=LIBUSB_PATH_MAX)>
            Public filename As String

            Public bus As IntPtr 'usb_bus

            Public descriptor As usb_device_descriptor  ' usb_device_descriptor

            Public config As IntPtr ' usb_config_descriptor 'config

            Public dev As IntPtr 'void 'dev		'' Darwin support ''

            Public devnum As Byte

            Public num_children As Byte

            Public children As IntPtr  ' usb_device ''children
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure lpusb_device
            Public ptr As MemPtr

            Public ReadOnly Property [next] As lpusb_device
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_device

                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(ptr.UIntAt(0))
                    Else
                        lp.ptr = New IntPtr(ptr.LongAt(0))
                    End If

                    Return lp
                End Get
            End Property

            Public ReadOnly Property prev As lpusb_device
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_device

                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(ptr.UIntAt(1))
                    Else
                        lp.ptr = New IntPtr(ptr.LongAt(1))
                    End If

                    Return lp
                End Get
            End Property

            Public ReadOnly Property filename As String
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    filename = ptr.GetUTF8String(IntPtr.Size * 2)
                End Get
            End Property

            Public ReadOnly Property bus As lpusb_bus
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    bus = New lpusb_bus

                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 2)

                    If IntPtr.Size = 4 Then
                        bus.ptr = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Else
                        bus.ptr = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    End If
                End Get
            End Property

            Public ReadOnly Property descriptor As usb_device_descriptor
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 3)

                    descriptor = ptr.ToStructAt(Of usb_device_descriptor)(bc)
                End Get
            End Property

            Public ReadOnly Property config As usb_config_descriptor
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 3) + Marshal.SizeOf(Of usb_device_descriptor)
                    Dim str As New CoreCT.Memory.MemPtr

                    If IntPtr.Size = 4 Then
                        str.Handle = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Else
                        str.Handle = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    End If

                    If str.Handle <> IntPtr.Zero Then
                        config = str.ToStruct(Of usb_config_descriptor)()
                    End If

                End Get
            End Property

            Public ReadOnly Property dev As IntPtr
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 4) + Marshal.SizeOf(Of usb_device_descriptor)
                    If IntPtr.Size = 4 Then
                        dev = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Else
                        dev = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    End If
                End Get
            End Property

            Public ReadOnly Property devnum As Byte
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 5) + Marshal.SizeOf(Of usb_device_descriptor)
                    Return ptr.ByteAt(bc)
                End Get
            End Property

            Public ReadOnly Property num_children As Byte
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 5) + 1 + Marshal.SizeOf(Of usb_device_descriptor)
                    Return ptr.ByteAt(bc)
                End Get
            End Property

            Public ReadOnly Property children As lpusb_device
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_device
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 5) + 2 + Marshal.SizeOf(Of usb_device_descriptor)
                    Dim mm As MemPtr

                    If IntPtr.Size = 4 Then
                        mm = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                        If mm = IntPtr.Zero Then Return Nothing

                        lp.ptr = New IntPtr(mm.UIntAt(0))
                    Else
                        mm = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                        If mm = IntPtr.Zero Then Return Nothing

                        lp.ptr = New IntPtr(mm.LongAt(0))
                    End If

                    Return lp
                End Get
            End Property

            Public Overrides Function ToString() As String
                If ptr = IntPtr.Zero Then Return "null"
                Return filename
            End Function

        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_bus
            Public [next] As IntPtr ' usb_device
            Public [prev] As IntPtr ' usb_device

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=LIBUSB_PATH_MAX)>
            Public filename As String

            Public devices As IntPtr ' usb_device 'devices

            Public location As UInteger

            Public root_dev As IntPtr ' usb_device 'root_dev
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure lpusb_bus
            Public ptr As MemPtr

            Public Overrides Function ToString() As String
                If ptr = IntPtr.Zero Then Return "null"
                Return filename
            End Function

            Public ReadOnly Property [next] As lpusb_bus
                Get
                    If ptr = IntPtr.Zero Then Return Nothing

                    Dim lp As New lpusb_bus

                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(ptr.UIntAt(0))
                    Else
                        lp.ptr = New IntPtr(ptr.LongAt(0))
                    End If

                    Return lp
                End Get
            End Property

            Public ReadOnly Property prev As lpusb_bus
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_bus

                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(ptr.UIntAt(1))
                    Else
                        lp.ptr = New IntPtr(ptr.LongAt(1))
                    End If

                    Return lp
                End Get
            End Property

            Public ReadOnly Property filename As String
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    filename = ptr.GetUTF8String(IntPtr.Size * 2)
                End Get
            End Property

            Public ReadOnly Property devices As lpusb_device
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_device
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 2)

                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Else
                        lp.ptr = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    End If

                    Return lp
                End Get
            End Property

            Public ReadOnly Property location As UInteger
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As UInteger
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 3)
                    lp = (BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Return lp
                End Get
            End Property

            Public ReadOnly Property root_dev As lpusb_device
                Get
                    If ptr = IntPtr.Zero Then Return Nothing
                    Dim lp As New lpusb_device
                    Dim bc As Integer = LIBUSB_PATH_MAX + (IntPtr.Size * 3) + 4
                    If IntPtr.Size = 4 Then
                        lp.ptr = New IntPtr(BitConverter.ToUInt32(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    Else
                        lp.ptr = New IntPtr(BitConverter.ToInt64(ptr.ToByteArray(bc, IntPtr.Size), 0))
                    End If
                    Return lp
                End Get
            End Property

        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure dll
            Public major As Integer
            Public minor As Integer
            Public micro As Integer
            Public nano As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure driver
            Public major As Integer
            Public minor As Integer
            Public micro As Integer
            Public nano As Integer
        End Structure

        '' Version information, Windows specific ''

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_version
            Public dll As dll
            Public driver As driver
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=gPack)>
        Public Structure usb_dev_handle
            Public ptr As IntPtr
        End Structure

        '' Function prototypes ''

        'Public Declare Function usb_open Lib "libusb0.dll" (dev As lpusb_device) As IntPtr
        'Public Declare Function usb_close Lib "libusb0.dll" (dev As IntPtr) As USBRESULT

        'Public Declare Function usb_get_string Lib "libusb0.dll" ( _
        '                                                        dev As IntPtr, _
        '                                                        index As Integer, _
        '                                                        langid As Integer, _
        '                                                        <MarshalAs(UnmanagedType.LPStr)> _
        '                                                        buf As String, _
        '                                                        buflen As Integer) As USBRESULT

        'Public Declare Function usb_get_string_simple Lib "libusb0.dll" ( _
        '                                                        dev As IntPtr, _
        '                                                        index As Integer, _
        '                                                        buf As MemPtr, _
        '                                                        buflen As Integer) As USBRESULT

        'Public Declare Function usb_get_descriptor_by_endpoint Lib "libusb0.dll" ( _
        '                                                                        udev As IntPtr, _
        '                                                                        ep As Integer, _
        '                                                                        type As Byte, _
        '                                                                        index As Byte, _
        '                                                                        buf As IntPtr, _
        '                                                                        size As Integer) As USBRESULT

        'Public Declare Function usb_get_descriptor Lib "libusb0.dll" ( _
        '                                                            udev As IntPtr, _
        '                                                            type As Byte, _
        '                                                            index As Byte, _
        '                                                            buf As IntPtr, _
        '                                                            size As Integer) As USBRESULT

        'Public Declare Function usb_bulk_write Lib "libusb0.dll" ( _
        '                                                            udev As IntPtr, _
        '                                                            ep As Integer, _
        '                                                            bytes As IntPtr, _
        '                                                            size As Integer, _
        '                                                            timeout As Integer) As USBRESULT

        'Public Declare Function usb_bulk_read Lib "libusb0.dll" ( _
        '                                                            udev As IntPtr, _
        '                                                            ep As Integer, _
        '                                                            bytes As IntPtr, _
        '                                                            size As Integer, _
        '                                                            timeout As Integer) As USBRESULT

        'Public Declare Function usb_interrupt_write Lib "libusb0.dll" ( _
        '                                                        udev As IntPtr, _
        '                                                        ep As Integer, _
        '                                                        bytes As IntPtr, _
        '                                                        size As Integer, _
        '                                                        timeout As Integer) As USBRESULT

        'Public Declare Function usb_interrupt_read Lib "libusb0.dll" ( _
        '                                                            udev As IntPtr, _
        '                                                            ep As Integer, _
        '                                                            bytes As IntPtr, _
        '                                                            size As Integer, _
        '                                                            timeout As Integer) As USBRESULT

        'Public Declare Function usb_control_msg Lib "libusb0.dll" ( _
        '                                                         dev As IntPtr, _
        '                                                         requesttype As Integer, _
        '                                                         request As Integer, _
        '                                                         value As Integer, _
        '                                                         index As Integer, _
        '                                                         bytes As IntPtr, _
        '                                                         size As Integer, _
        '                                                         timeout As Integer) As USBRESULT

        'Public Declare Function usb_set_configuration Lib "libusb0.dll" ( _
        '                                                               dev As IntPtr, _
        '                                                               configuration As Integer) As USBRESULT

        'Public Declare Function usb_claim_interface Lib "libusb0.dll" ( _
        '                                                               dev As IntPtr, _
        '                                                               [interface] As Integer) As USBRESULT

        'Public Declare Function usb_release_interface Lib "libusb0.dll" ( _
        '                                                               dev As IntPtr, _
        '                                                               [interface] As Integer) As USBRESULT

        'Public Declare Function usb_set_altinterface Lib "libusb0.dll" ( _
        '                                                               dev As IntPtr, _
        '                                                               alternate As Integer) As USBRESULT

        'Public Declare Function usb_resetep Lib "libusb0.dll" ( _
        '                                                        dev As IntPtr, _
        '                                                        ep As Integer) As USBRESULT

        'Public Declare Function usb_clear_halt Lib "libusb0.dll" ( _
        '                                                        dev As IntPtr, _
        '                                                        ep As Integer) As USBRESULT

        'Public Declare Function usb_reset Lib "libusb0.dll" (dev As IntPtr) As USBRESULT

        'Public Declare Function usb_reset_ex Lib "libusb0.dll" ( _
        '                                                    dev As IntPtr, _
        '                                                    reset_type As UInteger) As USBRESULT

        'Public Declare Function usb_strerror Lib "libusb0.dll" () As <MarshalAs(UnmanagedType.LPStr)> String

        'Public Declare Sub usb_init Lib "libusb0.dll" ()

        'Public Declare Sub usb_set_debug Lib "libusb0.dll" (level As Integer)

        'Public Declare Function usb_find_busses Lib "libusb0.dll" () As Integer

        'Public Declare Function usb_find_devices Lib "libusb0.dll" () As Integer

        ' '' usb_devices
        'Public Declare Function get_usb_device Lib "libusb0.dll" Alias "usb_device" (dev As IntPtr) As IntPtr

        ' '' usb_busses
        'Public Declare Function usb_get_busses Lib "libusb0.dll" () As IntPtr

        ' '' Windows specific functions
        'Public Declare Function usb_install_service_np Lib "libusb0.dll" () As USBRESULT

        '<DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        'Public Sub usb_install_service_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        'End Sub

        'Public Declare Function usb_uninstall_service_np Lib "libusb0.dll" () As USBRESULT

        '<DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        'Public Sub usb_uninstall_service_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        'End Sub

        'Public Declare Function usb_install_driver_np Lib "libusb0.dll" (<MarshalAs(UnmanagedType.LPStr)> inf_file As String) As USBRESULT

        '<DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        'Public Sub usb_install_driver_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        'End Sub

        'Public Declare Function usb_touch_inf_file_np Lib "libusb0.dll" (<MarshalAs(UnmanagedType.LPStr)> inf_file As String) As USBRESULT

        '<DllImport("libusb0.dll", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Ansi)>
        'Public Sub usb_touch_inf_file_np_rundll(hwnd As IntPtr, instance As IntPtr, <MarshalAs(UnmanagedType.LPStr)> cmd_line As String, cmd_show As Integer)
        'End Sub

        'Public Declare Function usb_install_needs_restart_np Lib "libusb0.dll" () As USBRESULT

        'Public Declare Unicode Function usb_install_npW Lib "libusb0.dll" ( _
        '                                                              hwnd As IntPtr, _
        '                                                              instance As IntPtr, _
        '                                                              <MarshalAs(UnmanagedType.LPWStr)> _
        '                                                              cmd_line As String, _
        '                                                              starg_arg As Integer) As USBRESULT

        'Public Declare Ansi Function usb_install_npA Lib "libusb0.dll" ( _
        '                                                              hwnd As IntPtr, _
        '                                                              instance As IntPtr, _
        '                                                              <MarshalAs(UnmanagedType.LPStr)> _
        '                                                              cmd_line As String, _
        '                                                              starg_arg As Integer) As USBRESULT

        'Public Declare Unicode Function usb_install_np Lib "libusb0.dll" _
        '                                                        Alias "usb_install_npW" ( _
        '                                                          hwnd As IntPtr, _
        '                                                          instance As IntPtr, _
        '                                                          <MarshalAs(UnmanagedType.LPWStr)> _
        '                                                          cmd_line As String, _
        '                                                          starg_arg As Integer) As USBRESULT

        ' '' usb_version
        'Public Declare Function usb_get_version Lib "libusb0.dll" () As IntPtr

        'Public Declare Function usb_isochronous_setup_async Lib "libusb0.dll" ( _
        '                                                                    dev As IntPtr, _
        '                                                                    ByRef context As IntPtr, _
        '                                                                    ep As Byte, _
        '                                                                    pktsize As Integer) As USBRESULT

        'Public Declare Function usb_bulk_setup_async Lib "libusb0.dll" ( _
        '                                                                dev As IntPtr, _
        '                                                                ByRef context As IntPtr, _
        '                                                                ep As Byte) As USBRESULT

        'Public Declare Function usb_interrupt_setup_async Lib "libusb0.dll" ( _
        '                                                                dev As IntPtr, _
        '                                                                ByRef context As IntPtr, _
        '                                                                ep As Byte) As USBRESULT

        'Public Declare Function usb_submit_async Lib "libusb0.dll" ( _
        '                                                        context As IntPtr, _
        '                                                        bytes As IntPtr, _
        '                                                        size As Integer) As USBRESULT

        'Public Declare Function usb_reap_async Lib "libusb0.dll" ( _
        '                                                        context As IntPtr, _
        '                                                        timeout As Integer) As USBRESULT
        'Public Declare Function usb_reap_async_nocancel Lib "libusb0.dll" ( _
        '                                                        context As IntPtr, _
        '                                                        timeout As Integer) As USBRESULT

        'Public Declare Function usb_cancel_async Lib "libusb0.dll" (context As IntPtr) As USBRESULT

        'Public Declare Function usb_free_async Lib "libusb0.dll" (context As IntPtr) As USBRESULT

    End Module

#End Region

#Region "Helper Functions"

    Friend Module UsbLibHelpers

        Public busses() As usb_bus
        Public devices() As usb_device

        Public Declare Function HidD_GetProductString Lib "hid.dll" (
                                                                HidDeviceObject As IntPtr,
                                                                Buffer As IntPtr,
                                                                BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetInputReport Lib "hid.dll" (
                                                              HidDeviceObject As IntPtr,
                                                              ReportBuffer As IntPtr,
                                                              ReportBufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetFeature Lib "hid.dll" (
                                                              HidDeviceObject As IntPtr,
                                                              Buffer As IntPtr,
                                                              BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetFeatureL Lib "hid.dll" Alias "HidD_GetFeature" (
                                                              HidDeviceObject As IntPtr,
                                                              ByRef Buffer As Long,
                                                              BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean


        Public Declare Function HidD_SetFeature Lib "hid.dll" (
                                                              HidDeviceObject As IntPtr,
                                                              Buffer As IntPtr,
                                                              BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetManufacturerString Lib "hid.dll" (
                                                          HidDeviceObject As IntPtr,
                                                          Buffer As IntPtr,
                                                          BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetSerialNumberString Lib "hid.dll" (
                                                          HidDeviceObject As IntPtr,
                                                          Buffer As IntPtr,
                                                          BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetPhysicalDescriptor Lib "hid.dll" (
                                                          HidDeviceObject As IntPtr,
                                                          Buffer As IntPtr,
                                                          BufferLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_GetPreparsedData Lib "hid.dll" (
                                                              HidDeviceObject As IntPtr,
                                                              ByRef PreparsedData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function HidD_FreePreparsedData Lib "hid.dll" (
                                                              PreparsedData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


    End Module

End Namespace

#End Region