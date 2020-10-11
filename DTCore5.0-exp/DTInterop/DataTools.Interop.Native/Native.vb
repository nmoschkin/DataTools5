'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Native
''         Myriad Windows API Declares
''
'' Started in 2000 on Windows 98/ME (and then later 2000).
''
'' Still kicking in 2014 on Windows 8.1!
'' A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

'' Some enum documentation copied from the MSDN (and in some cases, updated).
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''



Option Explicit On

Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing

Imports CoreCT.Memory
Imports CoreCT.Memory.NativeLib

Namespace Native

    ''' <summary>
    ''' A large cross-section of pInvoke-related items
    ''' </summary>
    ''' <remarks>This class is not well documented.</remarks>
    Public Module PInvoke

#Region "Window Messages"

        Public Const WM_NULL = &H0
        Public Const WM_CREATE = &H1
        Public Const WM_DESTROY = &H2
        Public Const WM_MOVE = &H3
        Public Const WM_SIZE = &H5

        Public Const WM_ACTIVATE = &H6

        Public Const WA_INACTIVE = 0
        Public Const WA_ACTIVE = 1
        Public Const WA_CLICKACTIVE = 2

        Public Const WM_SETFOCUS = &H7
        Public Const WM_KILLFOCUS = &H8
        Public Const WM_ENABLE = &HA
        Public Const WM_SETREDRAW = &HB
        Public Const WM_SETTEXT = &HC
        Public Const WM_GETTEXT = &HD
        Public Const WM_GETTEXTLENGTH = &HE
        Public Const WM_PAINT = &HF
        Public Const WM_CLOSE = &H10
        Public Const WM_QUERYENDSESSION = &H11
        Public Const WM_QUERYOPEN = &H13
        Public Const WM_ENDSESSION = &H16
        Public Const WM_QUIT = &H12
        Public Const WM_ERASEBKGND = &H14
        Public Const WM_SYSCOLORCHANGE = &H15
        Public Const WM_SHOWWINDOW = &H18
        Public Const WM_WININICHANGE = &H1A
        Public Const WM_SETTINGCHANGE = WM_WININICHANGE
        Public Const WM_DEVMODECHANGE = &H1B
        Public Const WM_ACTIVATEAPP = &H1C
        Public Const WM_FONTCHANGE = &H1D
        Public Const WM_TIMECHANGE = &H1E
        Public Const WM_CANCELMODE = &H1F
        Public Const WM_SETCURSOR = &H20
        Public Const WM_MOUSEACTIVATE = &H21
        Public Const WM_CHILDACTIVATE = &H22
        Public Const WM_QUEUESYNC = &H23
        Public Const WM_GETMINMAXINFO = &H24

        Public Const WM_PAINTICON = &H26
        Public Const WM_ICONERASEBKGND = &H27
        Public Const WM_NEXTDLGCTL = &H28
        Public Const WM_SPOOLERSTATUS = &H2A
        Public Const WM_DRAWITEM = &H2B
        Public Const WM_MEASUREITEM = &H2C
        Public Const WM_DELETEITEM = &H2D
        Public Const WM_VKEYTOITEM = &H2E
        Public Const WM_CHARTOITEM = &H2F
        Public Const WM_SETFONT = &H30
        Public Const WM_GETFONT = &H31
        Public Const WM_SETHOTKEY = &H32
        Public Const WM_GETHOTKEY = &H33
        Public Const WM_QUERYDRAGICON = &H37
        Public Const WM_COMPAREITEM = &H39
        Public Const WM_GETOBJECT = &H3D
        Public Const WM_COMPACTING = &H41
        Public Const WM_COMMNOTIFY = &H44
        Public Const WM_WINDOWPOSCHANGING = &H46
        Public Const WM_WINDOWPOSCHANGED = &H47

        Public Const WM_POWER = &H48

        Public Const PWR_OK = 1
        Public Const PWR_FAIL = (-1)
        Public Const PWR_SUSPENDREQUEST = 1
        Public Const PWR_SUSPENDRESUME = 2
        Public Const PWR_CRITICALRESUME = 3

        Public Const WM_COPYDATA = &H4A
        Public Const WM_CANCELJOURNAL = &H4B

        Public Const WM_NOTIFY = &H4E
        Public Const WM_INPUTLANGCHANGEREQUEST = &H50
        Public Const WM_INPUTLANGCHANGE = &H51
        Public Const WM_TCARD = &H52
        Public Const WM_HELP = &H53
        Public Const WM_USERCHANGED = &H54
        Public Const WM_NOTIFYFORMAT = &H55

        Public Const NFR_ANSI = 1
        Public Const NFR_UNICODE = 2
        Public Const NF_QUERY = 3
        Public Const NF_REQUERY = 4

        Public Const WM_CONTEXTMENU = &H7B
        Public Const WM_STYLECHANGING = &H7C
        Public Const WM_STYLECHANGED = &H7D
        Public Const WM_DISPLAYCHANGE = &H7E
        Public Const WM_GETICON = &H7F
        Public Const WM_SETICON = &H80
        Public Const WM_NCCREATE = &H81
        Public Const WM_NCDESTROY = &H82
        Public Const WM_NCCALCSIZE = &H83
        Public Const WM_NCHITTEST = &H84
        Public Const WM_NCPAINT = &H85
        Public Const WM_NCACTIVATE = &H86
        Public Const WM_GETDLGCODE = &H87
        Public Const WM_SYNCPAINT = &H88

        Public Const WM_NCMOUSEMOVE = &HA0
        Public Const WM_NCLBUTTONDOWN = &HA1
        Public Const WM_NCLBUTTONUP = &HA2
        Public Const WM_NCLBUTTONDBLCLK = &HA3
        Public Const WM_NCRBUTTONDOWN = &HA4
        Public Const WM_NCRBUTTONUP = &HA5
        Public Const WM_NCRBUTTONDBLCLK = &HA6
        Public Const WM_NCMBUTTONDOWN = &HA7
        Public Const WM_NCMBUTTONUP = &HA8
        Public Const WM_NCMBUTTONDBLCLK = &HA9

        Public Const WM_NCXBUTTONDOWN = &HAB
        Public Const WM_NCXBUTTONUP = &HAC
        Public Const WM_NCXBUTTONDBLCLK = &HAD

        Public Const WM_INPUT_DEVICE_CHANGE = &HFE
        Public Const WM_INPUT = &HFF
        Public Const WM_KEYFIRST = &H100
        Public Const WM_KEYDOWN = &H100
        Public Const WM_KEYUP = &H101
        Public Const WM_CHAR = &H102
        Public Const WM_DEADCHAR = &H103
        Public Const WM_SYSKEYDOWN = &H104
        Public Const WM_SYSKEYUP = &H105
        Public Const WM_SYSCHAR = &H106
        Public Const WM_SYSDEADCHAR = &H107
        Public Const WM_UNICHAR = &H109
        Public Const WM_KEYLAST = &H109
        Public Const UNICODE_NOCHAR = &HFFFF
        Public Const WM_IME_STARTCOMPOSITION = &H10D
        Public Const WM_IME_ENDCOMPOSITION = &H10E
        Public Const WM_IME_COMPOSITION = &H10F
        Public Const WM_IME_KEYLAST = &H10F
        Public Const WM_INITDIALOG = &H110
        Public Const WM_COMMAND = &H111
        Public Const WM_SYSCOMMAND = &H112
        Public Const WM_TIMER = &H113
        Public Const WM_HSCROLL = &H114
        Public Const WM_VSCROLL = &H115
        Public Const WM_INITMENU = &H116
        Public Const WM_INITMENUPOPUP = &H117
        Public Const WM_GESTURE = &H119
        Public Const WM_GESTURENOTIFY = &H11A
        Public Const WM_MENUSELECT = &H11F
        Public Const WM_MENUCHAR = &H120
        Public Const WM_ENTERIDLE = &H121
        Public Const WM_MENURBUTTONUP = &H122
        Public Const WM_MENUDRAG = &H123
        Public Const WM_MENUGETOBJECT = &H124
        Public Const WM_UNINITMENUPOPUP = &H125
        Public Const WM_MENUCOMMAND = &H126

        Public Const WM_CHANGEUISTATE = &H127
        Public Const WM_UPDATEUISTATE = &H128
        Public Const WM_QUERYUISTATE = &H129

        Public Const UIS_SET = 1
        Public Const UIS_CLEAR = 2
        Public Const UIS_INITIALIZE = 3

        Public Const UISF_HIDEFOCUS = &H1
        Public Const UISF_HIDEACCEL = &H2

        Public Const UISF_ACTIVE = &H4

        Public Const WM_CTLCOLORMSGBOX = &H132
        Public Const WM_CTLCOLOREDIT = &H133
        Public Const WM_CTLCOLORLISTBOX = &H134
        Public Const WM_CTLCOLORBTN = &H135
        Public Const WM_CTLCOLORDLG = &H136
        Public Const WM_CTLCOLORSCROLLBAR = &H137
        Public Const WM_CTLCOLORSTATIC = &H138
        Public Const MN_GETHMENU = &H1E1

        Public Const WM_MOUSEFIRST = &H200
        Public Const WM_MOUSEMOVE = &H200
        Public Const WM_LBUTTONDOWN = &H201
        Public Const WM_LBUTTONUP = &H202
        Public Const WM_LBUTTONDBLCLK = &H203
        Public Const WM_RBUTTONDOWN = &H204
        Public Const WM_RBUTTONUP = &H205
        Public Const WM_RBUTTONDBLCLK = &H206
        Public Const WM_MBUTTONDOWN = &H207
        Public Const WM_MBUTTONUP = &H208
        Public Const WM_MBUTTONDBLCLK = &H209

        Public Const WM_MOUSEWHEEL = &H20A

        Public Const WM_XBUTTONDOWN = &H20B
        Public Const WM_XBUTTONUP = &H20C
        Public Const WM_XBUTTONDBLCLK = &H20D

        Public Const WM_MOUSEHWHEEL = &H20E

        Public Const WM_MOUSELAST = &H20E

        Public Const WHEEL_DELTA = 120

        Public Function GET_WHEEL_DELTA_WPARAM(wParam As IntPtr) As Short
            Return CType(wParam.ToInt32 >> 16, Short)
        End Function

        Public Const WHEEL_PAGESCROLL As UInteger = UInt32.MaxValue

        Public Function GET_KEYSTATE_WPARAM(wParam As IntPtr) As Short
            Return CType(wParam.ToInt32 And &HFFFFI, Short)
        End Function

        Public Function GET_NCHITTEST_WPARAM(wParam As IntPtr) As Short
            Return CType(wParam.ToInt32 And &HFFFFI, Short)
        End Function

        Public Function GET_XBUTTON_WPARAM(wParam As IntPtr) As Short
            Return CType(wParam.ToInt32 >> 16, Short)
        End Function

        Public Const XBUTTON1 = &H1
        Public Const XBUTTON2 = &H2

        Public Const WM_PARENTNOTIFY = &H210
        Public Const WM_ENTERMENULOOP = &H211
        Public Const WM_EXITMENULOOP = &H212

        Public Const WM_NEXTMENU = &H213
        Public Const WM_SIZING = &H214
        Public Const WM_CAPTURECHANGED = &H215
        Public Const WM_MOVING = &H216

        Public Const WM_POWERBROADCAST = &H218

        Public Const PBT_APMQUERYSUSPEND = &H0
        Public Const PBT_APMQUERYSTANDBY = &H1

        Public Const PBT_APMQUERYSUSPENDFAILED = &H2
        Public Const PBT_APMQUERYSTANDBYFAILED = &H3

        Public Const PBT_APMSUSPEND = &H4
        Public Const PBT_APMSTANDBY = &H5

        Public Const PBT_APMRESUMECRITICAL = &H6
        Public Const PBT_APMRESUMESUSPEND = &H7
        Public Const PBT_APMRESUMESTANDBY = &H8

        Public Const PBTF_APMRESUMEFROMFAILURE = &H1

        Public Const PBT_APMBATTERYLOW = &H9
        Public Const PBT_APMPOWERSTATUSCHANGE = &HA

        Public Const PBT_APMOEMEVENT = &HB

        Public Const PBT_APMRESUMEAUTOMATIC = &H12

        Public Const PBT_POWERSETTINGCHANGE = &H8013

        <StructLayout(LayoutKind.Sequential)>
        Public Structure POWERBROADCAST_SETTING
            Public PowerSetting As Guid
            Public DataLength As UInteger
            Public Data As Byte
        End Structure

        Public Const WM_MDICREATE = &H220
        Public Const WM_MDIDESTROY = &H221
        Public Const WM_MDIACTIVATE = &H222
        Public Const WM_MDIRESTORE = &H223
        Public Const WM_MDINEXT = &H224
        Public Const WM_MDIMAXIMIZE = &H225
        Public Const WM_MDITILE = &H226
        Public Const WM_MDICASCADE = &H227
        Public Const WM_MDIICONARRANGE = &H228
        Public Const WM_MDIGETACTIVE = &H229

        Public Const WM_MDISETMENU = &H230
        Public Const WM_ENTERSIZEMOVE = &H231
        Public Const WM_EXITSIZEMOVE = &H232
        Public Const WM_DROPFILES = &H233
        Public Const WM_MDIREFRESHMENU = &H234

        Public Const WM_POINTERDEVICECHANGE = &H238
        Public Const WM_POINTERDEVICEINRANGE = &H239
        Public Const WM_POINTERDEVICEOUTOFRANGE = &H23A

        Public Const WM_TOUCH = &H240

        Public Const WM_NCPOINTERUPDATE = &H241
        Public Const WM_NCPOINTERDOWN = &H242
        Public Const WM_NCPOINTERUP = &H243
        Public Const WM_POINTERUPDATE = &H245
        Public Const WM_POINTERDOWN = &H246
        Public Const WM_POINTERUP = &H247
        Public Const WM_POINTERENTER = &H249
        Public Const WM_POINTERLEAVE = &H24A
        Public Const WM_POINTERACTIVATE = &H24B
        Public Const WM_POINTERCAPTURECHANGED = &H24C
        Public Const WM_TOUCHHITTESTING = &H24D
        Public Const WM_POINTERWHEEL = &H24E
        Public Const WM_POINTERHWHEEL = &H24F
        Public Const DM_POINTERHITTEST = &H250

        Public Const WM_IME_SETCONTEXT = &H281
        Public Const WM_IME_NOTIFY = &H282
        Public Const WM_IME_CONTROL = &H283
        Public Const WM_IME_COMPOSITIONFULL = &H284
        Public Const WM_IME_SELECT = &H285
        Public Const WM_IME_CHAR = &H286

        Public Const WM_IME_REQUEST = &H288

        Public Const WM_IME_KEYDOWN = &H290
        Public Const WM_IME_KEYUP = &H291

        Public Const WM_MOUSEHOVER = &H2A1
        Public Const WM_MOUSELEAVE = &H2A3

        Public Const WM_NCMOUSEHOVER = &H2A0
        Public Const WM_NCMOUSELEAVE = &H2A2

        Public Const WM_WTSSESSION_CHANGE = &H2B1

        Public Const WM_TABLET_FIRST = &H2C0
        Public Const WM_TABLET_LAST = &H2DF

        Public Const WM_DPICHANGED = &H2E0

        Public Const WM_CUT = &H300
        Public Const WM_COPY = &H301
        Public Const WM_PASTE = &H302
        Public Const WM_CLEAR = &H303
        Public Const WM_UNDO = &H304
        Public Const WM_RENDERFORMAT = &H305
        Public Const WM_RENDERALLFORMATS = &H306
        Public Const WM_DESTROYCLIPBOARD = &H307
        Public Const WM_DRAWCLIPBOARD = &H308
        Public Const WM_PAINTCLIPBOARD = &H309
        Public Const WM_VSCROLLCLIPBOARD = &H30A
        Public Const WM_SIZECLIPBOARD = &H30B
        Public Const WM_ASKCBFORMATNAME = &H30C
        Public Const WM_CHANGECBCHAIN = &H30D
        Public Const WM_HSCROLLCLIPBOARD = &H30E
        Public Const WM_QUERYNEWPALETTE = &H30F
        Public Const WM_PALETTEISCHANGING = &H310
        Public Const WM_PALETTECHANGED = &H311
        Public Const WM_HOTKEY = &H312

        Public Const WM_PRINT = &H317
        Public Const WM_PRINTCLIENT = &H318

        Public Const WM_APPCOMMAND = &H319

        Public Const WM_THEMECHANGED = &H31A

        Public Const WM_CLIPBOARDUPDATE = &H31D

        Public Const WM_DWMCOMPOSITIONCHANGED = &H31E
        Public Const WM_DWMNCRENDERINGCHANGED = &H31F
        Public Const WM_DWMCOLORIZATIONCOLORCHANGED = &H320
        Public Const WM_DWMWINDOWMAXIMIZEDCHANGE = &H321

        Public Const WM_DWMSENDICONICTHUMBNAIL = &H323
        Public Const WM_DWMSENDICONICLIVEPREVIEWBITMAP = &H326

        Public Const WM_GETTITLEBARINFOEX = &H33F

        Public Const WM_HANDHELDFIRST = &H358
        Public Const WM_HANDHELDLAST = &H35F

        Public Const WM_AFXFIRST = &H360
        Public Const WM_AFXLAST = &H37F

        Public Const WM_PENWINFIRST = &H380
        Public Const WM_PENWINLAST = &H38F

        Public Const WM_APP = &H8000

        Public Const WM_USER = &H400

        Public Const MK_LBUTTON = &H1
        Public Const MK_RBUTTON = &H2
        Public Const MK_MBUTTON = &H10
        Public Const MK_SHIFT = &H4
        Public Const MK_CONTROL = &H8

#End Region

#Region "Device Broadcast Messages"

        Public Const WM_DEVICECHANGE = &H219

        Public Enum DeviceBroadcastType

            ''' <summary>
            ''' A request to change the current configuration (dock or undock) has been canceled.
            ''' </summary>
            <Description("A request to change the current configuration (dock or undock) has been canceled.")>
            DBT_CONFIGCHANGECANCELED = &H19

            ''' <summary>
            ''' The current configuration has changed, due to a dock or undock.
            ''' </summary>
            <Description("The current configuration has changed, due to a dock or undock.")>
            DBT_CONFIGCHANGED = &H18

            ''' <summary>
            ''' A custom event has occurred.
            ''' </summary>
            <Description("A custom event has occurred.")>
            DBT_CUSTOMEVENT = &H8006

            ''' <summary>
            ''' A device or piece of media has been inserted and is now available.
            ''' </summary>
            <Description("A device or piece of media has been inserted and is now available.")>
            DBT_DEVICEARRIVAL = &H8000

            ''' <summary>
            ''' Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.
            ''' </summary>
            <Description("Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.")>
            DBT_DEVICEQUERYREMOVE = &H8001

            ''' <summary>
            ''' A request to remove a device or piece of media has been canceled.
            ''' </summary>
            <Description("A request to remove a device or piece of media has been canceled.")>
            DBT_DEVICEQUERYREMOVEFAILED = &H8002

            ''' <summary>
            ''' A device or piece of media has been removed.
            ''' </summary>
            <Description("A device or piece of media has been removed.")>
            DBT_DEVICEREMOVECOMPLETE = &H8004

            ''' <summary>
            ''' A device or piece of media is about to be removed. Cannot be denied.
            ''' </summary>
            <Description("A device or piece of media is about to be removed. Cannot be denied.")>
            DBT_DEVICEREMOVEPENDING = &H8003

            ''' <summary>
            ''' A device-specific event has occurred.
            ''' </summary>
            <Description("A device-specific event has occurred.")>
            DBT_DEVICETYPESPECIFIC = &H8005

            ''' <summary>
            ''' A device has been added to or removed from the system.
            ''' </summary>
            <Description("A device has been added to or removed from the system.")>
            DBT_DEVNODES_CHANGED = &H7

            ''' <summary>
            ''' Permission is requested to change the current configuration (dock or undock).
            ''' </summary>
            <Description("Permission is requested to change the current configuration (dock or undock).")>
            DBT_QUERYCHANGECONFIG = &H17

            ''' <summary>
            ''' The meaning of this message is user-defined.
            ''' </summary>
            <Description("The meaning of this message is user-defined.")>
            DBT_USERDEFINED = &HFFFF

        End Enum

        Public Enum DeviceBroadcastDeviceType
            ''' <summary>
            ''' Class of devices. This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.
            ''' </summary>
            <Description("Class of devices. This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.")>
            DBT_DEVTYP_DEVICEINTERFACE = &H5

            ''' <summary>
            ''' File system handle. This structure is a DEV_BROADCAST_HANDLE structure.
            ''' </summary>
            <Description("File system handle. This structure is a DEV_BROADCAST_HANDLE structure.")>
            DBT_DEVTYP_HANDLE = &H6

            ''' <summary>
            ''' OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.
            ''' </summary>
            <Description("OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.")>
            DBT_DEVTYP_OEM = &H0

            ''' <summary>
            ''' Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.
            ''' </summary>
            <Description("Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.")>
            DBT_DEVTYP_PORT = &H3

            ''' <summary>
            ''' Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.
            ''' </summary>
            <Description("Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.")>
            DBT_DEVTYP_VOLUME = &H2

        End Enum

#End Region

#Region "SysCommands"

        Public Const SC_SIZE = &HF000
        Public Const SC_MOVE = &HF010
        Public Const SC_MINIMIZE = &HF020
        Public Const SC_MAXIMIZE = &HF030
        Public Const SC_NEXTWINDOW = &HF040
        Public Const SC_PREVWINDOW = &HF050
        Public Const SC_CLOSE = &HF060
        Public Const SC_VSCROLL = &HF070
        Public Const SC_HSCROLL = &HF080
        Public Const SC_MOUSEMENU = &HF090
        Public Const SC_KEYMENU = &HF100
        Public Const SC_ARRANGE = &HF110
        Public Const SC_RESTORE = &HF120
        Public Const SC_TASKLIST = &HF130
        Public Const SC_SCREENSAVE = &HF140
        Public Const SC_HOTKEY = &HF150
        Public Const SC_DEFAULT = &HF160
        Public Const SC_MONITORPOWER = &HF170
        Public Const SC_CONTEXTHELP = &HF180
        Public Const SC_SEPARATOR = &HF00F
        Public Const SCF_ISSECURE = &H1

        Public Const SC_ICON = SC_MINIMIZE
        Public Const SC_ZOOM = SC_MAXIMIZE

        Public Function GET_SC_WPARAM(wParam As IntPtr) As Integer
            Return (wParam.ToInt32 And &HFFF0)
        End Function

#End Region

#Region "Windows"

        Public Const GWL_WNDPROC = (-4)
        Public Const GWL_STYLE = (-16)
        Public Const GWL_EXSTYLE = (-20)

        '' Window Creation

        '' Window Styles 1

        Public Const WS_OVERLAPPED = &H0&
        Public Const WS_POPUP = &H80000000
        Public Const WS_CHILD = &H40000000
        Public Const WS_MINIMIZE = &H20000000
        Public Const WS_VISIBLE = &H10000000
        Public Const WS_DISABLED = &H8000000
        Public Const WS_CLIPSIBLINGS = &H4000000
        Public Const WS_CLIPCHILDREN = &H2000000
        Public Const WS_MAXIMIZE = &H1000000
        Public Const WS_CAPTION = &HC00000
        Public Const WS_BORDER = &H800000
        Public Const WS_DLGFRAME = &H400000
        Public Const WS_VSCROLL = &H200000
        Public Const WS_HSCROLL = &H100000
        Public Const WS_SYSMENU = &H80000
        Public Const WS_THICKFRAME = &H40000
        Public Const WS_GROUP = &H20000
        Public Const WS_TABSTOP = &H10000

        Public Const WS_MINIMIZEBOX = &H20000
        Public Const WS_MAXIMIZEBOX = &H10000

        Public Const WS_TILED = WS_OVERLAPPED
        Public Const WS_ICONIC = WS_MINIMIZE
        Public Const WS_SIZEBOX = WS_THICKFRAME

        '' Window Styles 2

        Public Const WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED +
                    WS_CAPTION +
                    WS_SYSMENU +
                    WS_THICKFRAME +
                    WS_MINIMIZEBOX +
                    WS_MAXIMIZEBOX)

        Public Const WS_POPUPWINDOW = (WS_POPUP +
                    WS_BORDER +
                    WS_SYSMENU)

        Public Const WS_CHILDWINDOW = (WS_CHILD)

        Public Const WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW
        '
        ' Extended Window Styles
        '
        Public Const WS_EX_DLGMODALFRAME = &H1&
        Public Const WS_EX_NOPARENTNOTIFY = &H4&
        Public Const WS_EX_TOPMOST = &H8&
        Public Const WS_EX_ACCEPTFILES = &H10&
        Public Const WS_EX_TRANSPARENT = &H20&

        Public Const WS_EX_MDICHILD = &H40&
        Public Const WS_EX_TOOLWINDOW = &H80&
        Public Const WS_EX_WINDOWEDGE = &H100&
        Public Const WS_EX_CLIENTEDGE = &H200&
        Public Const WS_EX_CONTEXTHELP = &H400&

        Public Const WS_EX_RIGHT = &H1000&
        Public Const WS_EX_LEFT = &H0&
        Public Const WS_EX_RTLREADING = &H2000&
        Public Const WS_EX_LTRREADING = &H0&
        Public Const WS_EX_LEFTSCROLLBAR = &H4000&
        Public Const WS_EX_RIGHTSCROLLBAR = &H0&

        Public Const WS_EX_CONTROLPARENT = &H10000
        Public Const WS_EX_STATICEDGE = &H20000
        Public Const WS_EX_APPWINDOW = &H40000

        Public Const WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE + WS_EX_CLIENTEDGE)
        Public Const WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE + WS_EX_TOOLWINDOW + WS_EX_TOPMOST)

        ' Windows 5.0& (2000/Millenium)

        Public Const WS_EX_LAYERED = &H80000

        Public Const WS_EX_NOINHERITLAYOUT = &H100000     ' Disable inheritence of mirroring by children
        Public Const WS_EX_LAYOUTRTL = &H400000           ' Right to left mirroring

        ' Windows NT 5.0& (Windows 2000) only

        Public Const WS_EX_NOACTIVATE = &H8000000

        Public Const SW_HIDE = 0
        Public Const SW_SHOWNORMAL = 1
        Public Const SW_NORMAL = 1
        Public Const SW_SHOWMINIMIZED = 2
        Public Const SW_SHOWMAXIMIZED = 3
        Public Const SW_MAXIMIZE = 3
        Public Const SW_SHOWNOACTIVATE = 4
        Public Const SW_SHOW = 5
        Public Const SW_MINIMIZE = 6
        Public Const SW_SHOWMINNOACTIVE = 7
        Public Const SW_SHOWNA = 8
        Public Const SW_RESTORE = 9
        Public Const SW_SHOWDEFAULT = 10
        Public Const SW_FORCEMINIMIZE = 11
        Public Const SW_MAX = 11

#End Region

#Region "Common Control Messages"

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure COLORSCHEME
            Public dwSize As Integer
            Public clrBtnHighlight As Integer
            Public clrBtnShadow As Integer
        End Structure

        Public Const CCM_FIRST = &H2000      '' Common control shared messages
        Public Const CCM_LAST = (CCM_FIRST + &H200)

        Public Const CCM_SETBKCOLOR = (CCM_FIRST + 1) '' lParam is bkColor

        Public Const CCM_SETCOLORSCHEME = (CCM_FIRST + 2) '' lParam is color scheme
        Public Const CCM_GETCOLORSCHEME = (CCM_FIRST + 3) '' fills in COLORSCHEME pointed to by lParam
        Public Const CCM_GETDROPTARGET = (CCM_FIRST + 4)
        Public Const CCM_SETUNICODEFORMAT = (CCM_FIRST + 5)
        Public Const CCM_GETUNICODEFORMAT = (CCM_FIRST + 6)

#End Region

#Region "ListView"

        Public Const LVS_ICON = &H0
        Public Const LVS_REPORT = &H1
        Public Const LVS_SMALLICON = &H2
        Public Const LVS_LIST = &H3
        Public Const LVS_TYPEMASK = &H3
        Public Const LVS_SINGLESEL = &H4
        Public Const LVS_SHOWSELALWAYS = &H8
        Public Const LVS_SORTASCENDING = &H10
        Public Const LVS_SORTDESCENDING = &H20
        Public Const LVS_SHAREIMAGELISTS = &H40
        Public Const LVS_NOLABELWRAP = &H80
        Public Const LVS_AUTOARRANGE = &H100
        Public Const LVS_EDITLABELS = &H200
        Public Const LVS_OWNERDATA = &H1000
        Public Const LVS_NOSCROLL = &H2000

        Public Const LVS_TYPESTYLEMASK = &HFC00

        Public Const LVS_ALIGNTOP = &H0
        Public Const LVS_ALIGNLEFT = &H800
        Public Const LVS_ALIGNMASK = &HC00

        Public Const LVS_OWNERDRAWFIXED = &H400
        Public Const LVS_NOCOLUMNHEADER = &H4000
        Public Const LVS_NOSORTHEADER = &H8000

        '' end_r_commctrl
        Public Const LVM_FIRST = &H1000

        Public Const LVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT
        Public Const LVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT

        Public Function ListView_SetUnicodeFormat(hwnd As IntPtr, fUnicode As Integer) As IntPtr
            Return SendMessage(hwnd, LVM_SETUNICODEFORMAT, fUnicode, IntPtr.Zero)
        End Function

        Public Function ListView_GetUnicodeFormat(hwnd As IntPtr) As IntPtr
            Return SendMessage(hwnd, LVM_GETUNICODEFORMAT, IntPtr.Zero, IntPtr.Zero)
        End Function

        Public Const LVM_GETBKCOLOR = (LVM_FIRST + 0)

        Public Const LVM_SETBKCOLOR = (LVM_FIRST + 1)
        Public Const LVM_GETIMAGELIST = (LVM_FIRST + 2)

        Public Function ListView_SetBkColor(hwnd As IntPtr) As System.Drawing.Color
            Return Color.FromArgb(CInt(SendMessage(hwnd, LVM_GETBKCOLOR, 0, 0)))
        End Function

        Public Function ListView_SetBkColor(hwnd As IntPtr, clrBk As Color) As IntPtr
            Return SendMessage(hwnd, LVM_SETBKCOLOR, 0, clrBk.ToArgb)
        End Function

        Public Function ListView_GetImageList(hwnd As IntPtr, iImageList As Integer) As IntPtr
            Return SendMessage(hwnd, LVM_GETIMAGELIST, iImageList, 0)
        End Function

        Public Const LVSIL_NORMAL = 0
        Public Const LVSIL_SMALL = 1
        Public Const LVSIL_STATE = 2
        Public Const LVSIL_GROUPHEADER = 3

        Public Const LVM_SETIMAGELIST = (LVM_FIRST + 3)

        Public Function ListView_SetImageList(hwnd As IntPtr, iImageList As Integer, hImageList As IntPtr) As IntPtr
            Return SendMessage(hwnd, LVM_SETIMAGELIST, iImageList, hImageList)
        End Function

        Public Const LVM_GETITEMCOUNT = (LVM_FIRST + 4)

        Public Function ListView_GetItemCount(hwnd As IntPtr) As IntPtr
            Return SendMessage(hwnd, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero)
        End Function

        Public Const LVIF_TEXT = &H1
        Public Const LVIF_IMAGE = &H2
        Public Const LVIF_PARAM = &H4
        Public Const LVIF_STATE = &H8
        Public Const LVIF_INDENT = &H10
        Public Const LVIF_NORECOMPUTE = &H800
#If (NTDDI_VERSION >= NTDDI_WINXP) Then
        Public Const LVIF_GROUPID = &H100
        Public Const LVIF_COLUMNS = &H200
#End If

#If (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const LVIF_COLFMT = &H10000 '' The piColFmt member is valid in addition to puColumns
#End If

        Public Const LVIS_FOCUSED = &H1
        Public Const LVIS_SELECTED = &H2
        Public Const LVIS_CUT = &H4
        Public Const LVIS_DROPHILITED = &H8
        Public Const LVIS_GLOW = &H10
        Public Const LVIS_ACTIVATING = &H20

        Public Const LVIS_OVERLAYMASK = &HF00
        Public Const LVIS_STATEIMAGEMASK = &HF000

#End Region

#Region "System Parameters Info"
        ' System Parameters info constants

        Public Const SPI_GETACCESSTIMEOUT = 60
        Public Const SPI_GETANIMATION = 72
        Public Const SPI_GETBEEP = 1
        Public Const SPI_GETBORDER = 5
        Public Const SPI_GETDEFAULTINPUTLANG = 89
        Public Const SPI_GETDRAGFULLWINDOWS = 38
        Public Const SPI_GETFASTTASKSWITCH = 35
        Public Const SPI_GETFILTERKEYS = 50
        Public Const SPI_GETFONTSMOOTHING = 74
        Public Const SPI_GETGRIDGRANULARITY = 18
        Public Const SPI_GETHIGHCONTRAST = 66
        Public Const SPI_GETICONMETRICS = 45
        Public Const SPI_GETICONTITLELOGFONT = 31
        Public Const SPI_GETICONTITLEWRAP = 25
        Public Const SPI_GETKEYBOARDDELAY = 22
        Public Const SPI_GETKEYBOARDPREF = 68
        Public Const SPI_GETKEYBOARDSPEED = 10
        Public Const SPI_GETLOWPOWERACTIVE = 83
        Public Const SPI_GETLOWPOWERTIMEOUT = 79
        Public Const SPI_GETMENUDROPALIGNMENT = 27
        Public Const SPI_GETMINIMIZEDMETRICS = 43
        Public Const SPI_GETMOUSE = 3
        Public Const SPI_GETMOUSEKEYS = 54
        Public Const SPI_GETMOUSETRAILS = 94
        Public Const SPI_GETNONCLIENTMETRICS = 41
        Public Const SPI_GETPOWEROFFACTIVE = 84
        Public Const SPI_GETPOWEROFFTIMEOUT = 80
        Public Const SPI_GETSCREENREADER = 70
        Public Const SPI_GETSCREENSAVEACTIVE = 16
        Public Const SPI_GETSCREENSAVETIMEOUT = 14
        Public Const SPI_GETSERIALKEYS = 62
        Public Const SPI_GETSHOWSOUNDS = 56
        Public Const SPI_GETSOUNDSENTRY = 64
        Public Const SPI_GETSTICKYKEYS = 58
        Public Const SPI_GETTOGGLEKEYS = 52
        Public Const SPI_GETWINDOWSEXTENSION = 92
        Public Const SPI_GETWORKAREA = 48

        Public Const SPI_ICONHORIZONTALSPACING = 13
        Public Const SPI_ICONVERTICALSPACING = 24

        Public Const SPI_LANGDRIVER = 12

        Public Const SPI_SCREENSAVERRUNNING = 97

        Public Const SPI_SETACCESSTIMEOUT = 61
        Public Const SPI_SETANIMATION = 73
        Public Const SPI_SETBEEP = 2
        Public Const SPI_SETBORDER = 6
        Public Const SPI_SETCURSORS = 87
        Public Const SPI_SETDEFAULTINPUTLANG = 90
        Public Const SPI_SETDESKPATTERN = 21
        Public Const SPI_SETDESKWALLPAPER = 20
        Public Const SPI_SETDOUBLECLICKTIME = 32
        Public Const SPI_SETDOUBLECLKHEIGHT = 30
        Public Const SPI_SETDOUBLECLKWIDTH = 29
        Public Const SPI_SETDRAGFULLWINDOWS = 37
        Public Const SPI_SETDRAGHEIGHT = 77
        Public Const SPI_SETDRAGWIDTH = 76
        Public Const SPI_SETFASTTASKSWITCH = 36
        Public Const SPI_SETFILTERKEYS = 51
        Public Const SPI_SETFONTSMOOTHING = 75
        Public Const SPI_SETGRIDGRANULARITY = 19
        Public Const SPI_SETHANDHELD = 78
        Public Const SPI_SETHIGHCONTRAST = 67
        Public Const SPI_SETICONMETRICS = 46
        Public Const SPI_SETICONS = 88
        Public Const SPI_SETICONTITLELOGFONT = 34
        Public Const SPI_SETICONTITLEWRAP = 26
        Public Const SPI_SETKEYBOARDDELAY = 23
        Public Const SPI_SETKEYBOARDPREF = 69
        Public Const SPI_SETKEYBOARDSPEED = 11
        Public Const SPI_SETLANGTOGGLE = 91
        Public Const SPI_SETLOWPOWERACTIVE = 85
        Public Const SPI_SETLOWPOWERTIMEOUT = 81
        Public Const SPI_SETMENUDROPALIGNMENT = 28
        Public Const SPI_SETMINIMIZEDMETRICS = 44
        Public Const SPI_SETMOUSE = 4
        Public Const SPI_SETMOUSEBUTTONSWAP = 33
        Public Const SPI_SETMOUSEKEYS = 55
        Public Const SPI_SETMOUSETRAILS = 93
        Public Const SPI_SETNONCLIENTMETRICS = 42
        Public Const SPI_SETPENWINDOWS = 49
        Public Const SPI_SETPOWEROFFACTIVE = 86
        Public Const SPI_SETPOWEROFFTIMEOUT = 82
        Public Const SPI_SETSCREENREADER = 71
        Public Const SPI_SETSCREENSAVEACTIVE = 17
        Public Const SPI_SETSCREENSAVETIMEOUT = 15
        Public Const SPI_SETSERIALKEYS = 63
        Public Const SPI_SETSHOWSOUNDS = 57
        Public Const SPI_SETSOUNDSENTRY = 65
        Public Const SPI_SETSTICKYKEYS = 59
        Public Const SPI_SETTOGGLEKEYS = 53
        Public Const SPI_SETWORKAREA = 47

#End Region

#Region "DrawFrameControl"
        '' Frame Control

        Public Const DFC_BUTTON = 4
        Public Const DFC_CAPTION = 1
        Public Const DFC_MENU = 2
        Public Const DFC_SCROLL = 3

        Public Const DFCS_ADJUSTRECT = &H2000
        Public Const DFCS_BUTTON3STATE = &H8
        Public Const DFCS_BUTTONCHECK = &H0
        Public Const DFCS_BUTTONPUSH = &H10
        Public Const DFCS_BUTTONRADIO = &H4
        Public Const DFCS_BUTTONRADIOIMAGE = &H1
        Public Const DFCS_BUTTONRADIOMASK = &H2
        Public Const DFCS_CAPTIONCLOSE = &H0
        Public Const DFCS_CAPTIONHELP = &H4
        Public Const DFCS_CAPTIONMAX = &H2
        Public Const DFCS_CAPTIONMIN = &H1
        Public Const DFCS_CAPTIONRESTORE = &H3
        Public Const DFCS_CHECKED = &H400
        Public Const DFCS_FLAT = &H4000
        Public Const DFCS_INACTIVE = &H100
        Public Const DFCS_MENUARROW = &H0
        Public Const DFCS_MENUARROWRIGHT = &H4
        Public Const DFCS_MENUBULLET = &H2
        Public Const DFCS_MENUCHECK = &H1
        Public Const DFCS_MONO = &H8000
        Public Const DFCS_PUSHED = &H200
        Public Const DFCS_SCROLLCOMBOBOX = &H5
        Public Const DFCS_SCROLLDOWN = &H1
        Public Const DFCS_SCROLLLEFT = &H2
        Public Const DFCS_SCROLLRIGHT = &H3
        Public Const DFCS_SCROLLSIZEGRIP = &H8
        Public Const DFCS_SCROLLSIZEGRIPRIGHT = &H10
        Public Const DFCS_SCROLLUP = &H0
        Public Const DC_ACTIVE = &H1
        Public Const DC_SMALLCAP = &H2
        Public Const DC_ICON = &H4
        Public Const DC_TEXT = &H8
        Public Const DC_INBUTTON = &H10
        Public Const DC_GRADIENT = &H20

#End Region

#Region "System Colors"

        '' System Colors

        Public Const COLOR_ACTIVEBORDER = 10
        Public Const COLOR_ACTIVECAPTION = 2
        Public Const COLOR_GRADIENTACTIVECAPTION = 27
        Public Const COLOR_ADJ_MAX = 100
        Public Const COLOR_ADJ_MIN = -100
        Public Const COLOR_APPWORKSPACE = 12
        Public Const COLOR_BACKGROUND = 1
        Public Const COLOR_BTNFACE = 15
        Public Const COLOR_BTNHIGHLIGHT = 20
        Public Const COLOR_BTNSHADOW = 16
        Public Const COLOR_BTNTEXT = 18
        Public Const COLOR_CAPTIONTEXT = 9
        Public Const COLOR_GRAYTEXT = 17
        Public Const COLOR_HIGHLIGHT = 13
        Public Const COLOR_HIGHLIGHTTEXT = 14
        Public Const COLOR_INACTIVEBORDER = 11
        Public Const COLOR_INACTIVECAPTION = 3
        Public Const COLOR_GRADIENTINACTIVECAPTION = 28
        Public Const COLOR_INACTIVECAPTIONTEXT = 19
        Public Const COLOR_MENU = 4
        Public Const COLOR_MENUTEXT = 7
        Public Const COLOR_SCROLLBAR = 0
        Public Const COLOR_WINDOW = 5
        Public Const COLOR_WINDOWFRAME = 6
        Public Const COLOR_WINDOWTEXT = 8

        Public Const COLOR_3DDKSHADOW = 21
        Public Const COLOR_3DLIGHT = 22
        Public Const COLOR_INFOTEXT = 23
        Public Const COLOR_INFOBK = 24

        ' Windows 98/2000

        Public Const COLOR_HOTLIGHT = 26

#End Region

#Region "Scrollbars"

        '' Scroll Bar Types

        Public Const SB_BOTH = 3
        Public Const SB_BOTTOM = 7
        Public Const SB_CTL = 2
        Public Const SB_ENDSCROLL = 8
        Public Const SB_HORZ = 0
        Public Const SB_LEFT = 6
        Public Const SB_LINEDOWN = 1
        Public Const SB_LINELEFT = 0
        Public Const SB_LINERIGHT = 1
        Public Const SB_LINEUP = 0
        Public Const SB_PAGEDOWN = 3
        Public Const SB_PAGELEFT = 2
        Public Const SB_PAGERIGHT = 3
        Public Const SB_PAGEUP = 2
        Public Const SB_RIGHT = 7
        Public Const SB_THUMBPOSITION = 4
        Public Const SB_THUMBTRACK = 5
        Public Const SB_TOP = 6
        Public Const SB_VERT = 1

        '' Scroll Bar Messages

        Public Const SBM_ENABLE_ARROWS = &HE4
        Public Const SBM_GETPOS = &HE1
        Public Const SBM_GETRANGE = &HE3
        Public Const SBM_SETPOS = &HE0
        Public Const SBM_SETRANGE = &HE2
        Public Const SBM_SETRANGEREDRAW = &HE6

        '' Scroll Bar Window Styles

        Public Const SBS_BOTTOMALIGN = &H4
        Public Const SBS_HORZ = &H0
        Public Const SBS_LEFTALIGN = &H2
        Public Const SBS_RIGHTALIGN = &H4
        Public Const SBS_SIZEBOX = &H8
        Public Const SBS_SIZEBOXBOTTOMRIGHTALIGN = &H4
        Public Const SBS_SIZEBOXTOPLEFTALIGN = &H2
        Public Const SBS_TOPALIGN = &H2
        Public Const SBS_VERT = &H1

        '' EnableScrollBar() flags

        Public Const ESB_DISABLE_BOTH = &H3
        Public Const ESB_DISABLE_DOWN = &H2
        Public Const ESB_DISABLE_LEFT = &H1
        Public Const ESB_DISABLE_RIGHT = &H2
        Public Const ESB_DISABLE_UP = &H1

        Public Const ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT
        Public Const ESB_DISABLE_LTUP = ESB_DISABLE_LEFT

        Public Const ESB_ENABLE_BOTH = &H0

        Public Const SIF_RANGE = &H1
        Public Const SIF_PAGE = &H2
        Public Const SIF_POS = &H4
        Public Const SIF_DISABLENOSCROLL = &H8
        Public Const SIF_TRACKPOS = &H10
        Public Const SIF_ALL = (SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS)

#End Region

#Region "Menus"

        Public Const TPM_LEFTBUTTON = &H0&
        Public Const TPM_RIGHTBUTTON = &H2&
        Public Const TPM_LEFTALIGN = &H0&
        Public Const TPM_CENTERALIGN = &H4&
        Public Const TPM_RIGHTALIGN = &H8&

        Public Const TPM_TOPALIGN = &H0&
        Public Const TPM_VCENTERALIGN = &H10&
        Public Const TPM_BOTTOMALIGN = &H20&

        Public Const TPM_HORIZONTAL = &H0&            '' Horz alignment matters more ''
        Public Const TPM_VERTICAL = &H40&             '' Vert alignment matters more ''
        Public Const TPM_NONOTIFY = &H80&             '' Don't send any notification msgs ''
        Public Const TPM_RETURNCMD = &H100&

        Public Const TPM_RECURSE = &H1&

        ' fMask flags

        Public Const MIIM_STATE = &H1
        Public Const MIIM_ID = &H2
        Public Const MIIM_SUBMENU = &H4
        Public Const MIIM_CHECKMARKS = &H8
        Public Const MIIM_TYPE = &H10
        Public Const MIIM_DATA = &H20

        '' New for Windows 98/2000

        Public Const MIIM_STRING = &H40
        Public Const MIIM_BITMAP = &H80
        Public Const MIIM_FTYPE = &H100

        '' End fMask flags

        ' Menu Flags

        Public Const MF_INSERT = &H0&
        Public Const MF_CHANGE = &H80&
        Public Const MF_APPEND = &H100&
        Public Const MF_DELETE = &H200&
        Public Const MF_REMOVE = &H1000&

        Public Const MF_BYCOMMAND = &H0&
        Public Const MF_BYPOSITION = &H400&

        Public Const MF_SEPARATOR = &H800&

        Public Const MF_ENABLED = &H0&
        Public Const MF_GRAYED = &H1&
        Public Const MF_DISABLED = &H2&

        Public Const MF_UNCHECKED = &H0&
        Public Const MF_CHECKED = &H8&
        Public Const MF_USECHECKBITMAPS = &H200&

        Public Const MF_STRING = &H0&
        Public Const MF_BITMAP = &H4&
        Public Const MF_OWNERDRAW = &H100&

        Public Const MF_POPUP = &H10&
        Public Const MF_MENUBARBREAK = &H20&
        Public Const MF_MENUBREAK = &H40&

        Public Const MF_UNHILITE = &H0&
        Public Const MF_HILITE = &H80&

        Public Const MF_DEFAULT = &H1000&
        Public Const MF_SYSMENU = &H2000&
        Public Const MF_HELP = &H4000&
        Public Const MF_RIGHTJUSTIFY = &H4000&

        Public Const MF_MOUSESELECT = &H8000&
        Public Const MF_END = &H80&                    ' Obsolete -- only used by old RES files

        Public Const MFT_STRING = MF_STRING
        Public Const MFT_BITMAP = MF_BITMAP
        Public Const MFT_MENUBARBREAK = MF_MENUBARBREAK
        Public Const MFT_MENUBREAK = MF_MENUBREAK
        Public Const MFT_OWNERDRAW = MF_OWNERDRAW
        Public Const MFT_RADIOGROUP = &H200&
        Public Const MFT_SEPARATOR = MF_SEPARATOR
        Public Const MFT_RIGHTORDER = &H2000&
        Public Const MFT_RIGHTJUSTIFY = MF_RIGHTJUSTIFY

        Public Const MFS_GRAYED = &H3&
        Public Const MFS_DISABLED = MFS_GRAYED
        Public Const MFS_CHECKED = MF_CHECKED
        Public Const MFS_HILITE = MF_HILITE
        Public Const MFS_ENABLED = MF_ENABLED
        Public Const MFS_UNCHECKED = MF_UNCHECKED
        Public Const MFS_UNHILITE = MF_UNHILITE
        Public Const MFS_DEFAULT = MF_DEFAULT

        ' New for Windows 2000/98

        Public Const MFS_MASK = &H108B&
        Public Const MFS_HOTTRACKDRAWN = &H10000000
        Public Const MFS_CACHEDBMP = &H20000000
        Public Const MFS_BOTTOMGAPDROP = &H40000000
        Public Const MFS_TOPGAPDROP = &H80000000
        Public Const MFS_GAPDROP = &HC0000000

        ' for the SetMenuInfo API function

        Public Const MNS_NOCHECK = &H80000000
        Public Const MNS_MODELESS = &H40000000
        Public Const MNS_DRAGDROP = &H20000000
        Public Const MNS_AUTODISMISS = &H10000000
        Public Const MNS_NOTIFYBYPOS = &H8000000
        Public Const MNS_CHECKORBMP = &H4000000

        Public Const MIM_MAXHEIGHT = &H1
        Public Const MIM_BACKGROUND = &H2
        Public Const MIM_HELPID = &H4
        Public Const MIM_MENUDATA = &H8
        Public Const MIM_STYLE = &H10
        Public Const MIM_APPLYTOSUBMENUS = &H80000000

#End Region

#Region "Network Shares"

        Public Const PARMNUM_BASE_INFOLEVEL = 1000

        Public Const SHARE_NETNAME_PARMNUM = 1
        Public Const SHARE_TYPE_PARMNUM = 3
        Public Const SHARE_REMARK_PARMNUM = 4
        Public Const SHARE_PERMISSIONS_PARMNUM = 5
        Public Const SHARE_MAX_USES_PARMNUM = 6
        Public Const SHARE_CURRENT_USES_PARMNUM = 7
        Public Const SHARE_PATH_PARMNUM = 8
        Public Const SHARE_PASSWD_PARMNUM = 9
        Public Const SHARE_FILE_SD_PARMNUM = 501
        Public Const SHARE_SERVER_PARMNUM = 503

        ''
        '' Single-field infolevels for NetShareSetInfo.
        ''

        Public Const SHARE_REMARK_INFOLEVEL =
    (PARMNUM_BASE_INFOLEVEL + SHARE_REMARK_PARMNUM)

        Public Const SHARE_MAX_USES_INFOLEVEL =
    (PARMNUM_BASE_INFOLEVEL + SHARE_MAX_USES_PARMNUM)

        Public Const SHARE_FILE_SD_INFOLEVEL =
    (PARMNUM_BASE_INFOLEVEL + SHARE_FILE_SD_PARMNUM)

        Public Const SHI1_NUM_ELEMENTS = 4
        Public Const SHI2_NUM_ELEMENTS = 10

        ''
        '' Share types (shi1_type and shi2_type fields).
        ''

        Public Const STYPE_DISKTREE = 0
        Public Const STYPE_PRINTQ = 1
        Public Const STYPE_DEVICE = 2
        Public Const STYPE_IPC = 3

        Public Const STYPE_MASK = &HFF  '' AND with shi_type to

        Public Const STYPE_RESERVED1 = &H1000000  '' Reserved for internal processing
        Public Const STYPE_RESERVED2 = &H2000000
        Public Const STYPE_RESERVED3 = &H4000000
        Public Const STYPE_RESERVED4 = &H8000000
        Public Const STYPE_RESERVED_ALL = &H3FFFFF00

        Public Const STYPE_TEMPORARY = &H40000000
        Public Const STYPE_SPECIAL = &H80000000

        Public Const SHI_USES_UNLIMITED As Integer = &HFFFFFFFFI

        Public Const ACCESS_READ = &H1
        Public Const ACCESS_WRITE = &H2
        Public Const ACCESS_CREATE = &H4
        Public Const ACCESS_EXEC = &H8
        Public Const ACCESS_DELETE = &H10
        Public Const ACCESS_ATTRIB = &H20
        Public Const ACCESS_PERM = &H40

        Public Const ACCESS_GROUP = &H8000

#End Region

#Region "Shell API"

        ' IContextMenu and shell

        ''
        ''  Begin ShellExecuteEx and family
        ''

        '/* ShellExecute() and ShellExecuteEx() error codes */

        '/* regular WinExec() codes */
        Public Const SE_ERR_FNF = 2       ' file not found
        Public Const SE_ERR_PNF = 3       ' path not found
        Public Const SE_ERR_ACCESSDENIED = 5       ' access denied
        Public Const SE_ERR_OOM = 8       ' out of memory
        Public Const SE_ERR_DLLNOTFOUND = 32

        '/* error values for ShellExecute() beyond the regular WinExec() codes */
        Public Const SE_ERR_SHARE = 26
        Public Const SE_ERR_ASSOCINCOMPLETE = 27
        Public Const SE_ERR_DDETIMEOUT = 28
        Public Const SE_ERR_DDEFAIL = 29
        Public Const SE_ERR_DDEBUSY = 30
        Public Const SE_ERR_NOASSOC = 31

        ' Note CLASSKEY overrides CLASSNAME
        Public Const SEE_MASK_DEFAULT = &H0
        Public Const SEE_MASK_CLASSNAME = &H1   ' SHELLEXECUTEINFO.lpClass is valid
        Public Const SEE_MASK_CLASSKEY = &H3   ' SHELLEXECUTEINFO.hkeyClass is valid
        ' Note SEE_MASK_INVOKEIDLIST(=&HC) implies SEE_MASK_IDLIST(=&H04)
        Public Const SEE_MASK_IDLIST = &H4   ' SHELLEXECUTEINFO.lpIDList is valid
        Public Const SEE_MASK_INVOKEIDLIST = &HC   ' enable IContextMenu based verbs
#If (NTDDI_VERSION < NTDDI_VISTA) Then
Public Const SEE_MASK_ICON              = &H00000010   ' not used
#End If ' (NTDDI_VERSION < NTDDI_VISTA)
        Public Const SEE_MASK_HOTKEY = &H20   ' SHELLEXECUTEINFO.dwHotKey is valid
        Public Const SEE_MASK_NOCLOSEPROCESS = &H40   ' SHELLEXECUTEINFO.hProcess
        Public Const SEE_MASK_CONNECTNETDRV = &H80   ' enables re-connecting disconnected network drives
        Public Const SEE_MASK_NOASYNC = &H100   ' block on the call until the invoke has completed, use for callers that exit after calling ShellExecuteEx()
        Public Const SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC ' Use SEE_MASK_NOASYNC instead of SEE_MASK_FLAG_DDEWAIT as it more accuratly describes the behavior
        Public Const SEE_MASK_DOENVSUBST = &H200   ' indicates that SHELLEXECUTEINFO.lpFile contains env vars that should be expanded
        Public Const SEE_MASK_FLAG_NO_UI = &H400   ' disable UI including error messages
        Public Const SEE_MASK_UNICODE = &H4000
        Public Const SEE_MASK_NO_CONSOLE = &H8000
        Public Const SEE_MASK_ASYNCOK = &H100000
#If (NTDDI_VERSION >= NTDDI_WIN2K) Then
        Public Const SEE_MASK_HMONITOR = &H200000   ' SHELLEXECUTEINFO.hMonitor
#End If ' (NTDDI_VERSION >= NTDDI_WIN2K)
#If (NTDDI_VERSION >= NTDDI_WINXPSP1) Then
        Public Const SEE_MASK_NOZONECHECKS = &H800000
#End If ' (NTDDI_VERSION >= NTDDI_WINXPSP1)
#If (NTDDI_VERSION >= NTDDI_WIN2K) Then
        Public Const SEE_MASK_NOQUERYCLASSSTORE = &H1000000
        Public Const SEE_MASK_WAITFORINPUTIDLE = &H2000000
#End If ' (NTDDI_VERSION >= NTDDI_WIN2K)
#If (NTDDI_VERSION >= NTDDI_WINXP) Then
        Public Const SEE_MASK_FLAG_LOG_USAGE = &H4000000
#End If ' (NTDDI_VERSION >= NTDDI_WINXP)

#If (NTDDI_VERSION >= NTDDI_WIN8) Then
        ' When SEE_MASK_FLAG_HINST_IS_SITE is specified SHELLEXECUTEINFO.hInstApp is used as an
        ' _In_ parameter and specifies a IUnknown* to be used as a site pointer. The site pointer
        ' is used to provide services to shell execute, the handler binding process and the verb handlers
        ' once they are invoked.
        Public Const SEE_MASK_FLAG_HINST_IS_SITE = &H8000000
#End If ' (NTDDI_VERSION >= NTDDI_WIN8)

        Public Const CMF_NORMAL = &H0
        Public Const CMF_DEFAULTONLY = &H1
        Public Const CMF_VERBSONLY = &H2
        Public Const CMF_EXPLORE = &H4
        Public Const CMF_NOVERBS = &H8
        Public Const CMF_CANRENAME = &H10
        Public Const CMF_NODEFAULT = &H20
        Public Const CMF_INCLUDESTATIC = &H40

#If (NTDDI_VERSION < NTDDI_VISTA) Then
Public Const CMF_INCLUDESTATIC       =&H00000040
#End If
#If (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const CMF_ITEMMENU = &H80
#End If
        Public Const CMF_EXTENDEDVERBS = &H100
#If (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const CMF_DISABLEDVERBS = &H200
#End If

        Public Const CMF_ASYNCVERBSTATE = &H400
        Public Const CMF_OPTIMIZEFORINVOKE = &H800
        Public Const CMF_SYNCCASCADEMENU = &H1000
        Public Const CMF_DONOTPICKDEFAULT = &H2000
        Public Const CMF_RESERVED = &HFFFF0000

        ' GetCommandString uFlags
        Public Const GCS_VERBA = &H0     ' canonical verb
        Public Const GCS_HELPTEXTA = &H1     ' help text (for status bar)
        Public Const GCS_VALIDATEA = &H2     ' validate command exists
        Public Const GCS_VERBW = &H4     ' canonical verb (unicode)
        Public Const GCS_HELPTEXTW = &H5     ' help text (unicode version)
        Public Const GCS_VALIDATEW = &H6     ' validate command exists (unicode)
        Public Const GCS_VERBICONW = &H14     ' icon string (unicode)
        Public Const GCS_UNICODE = &H4     ' for bit testing - Unicode string

        Public Const GCS_VERB = GCS_VERBW
        Public Const GCS_HELPTEXT = GCS_HELPTEXTW
        Public Const GCS_VALIDATE = GCS_VALIDATEW

        Public Const CMDSTR_NEWFOLDERA = "NewFolder"
        Public Const CMDSTR_VIEWLISTA = "ViewList"
        Public Const CMDSTR_VIEWDETAILSA = "ViewDetails"
        Public Const CMDSTR_NEWFOLDERW = "NewFolder"
        Public Const CMDSTR_VIEWLISTW = "ViewList"
        Public Const CMDSTR_VIEWDETAILSW = "ViewDetails"

        Public Const CMDSTR_NEWFOLDER = CMDSTR_NEWFOLDERW
        Public Const CMDSTR_VIEWLIST = CMDSTR_VIEWLISTW
        Public Const CMDSTR_VIEWDETAILS = CMDSTR_VIEWDETAILSW

        Public Const CMIC_MASK_HOTKEY = SEE_MASK_HOTKEY
        Public Const CMIC_MASK_ICON = &H10
        Public Const CMIC_MASK_FLAG_NO_UI = SEE_MASK_FLAG_NO_UI
        Public Const CMIC_MASK_UNICODE = SEE_MASK_UNICODE
        Public Const CMIC_MASK_NO_CONSOLE = SEE_MASK_NO_CONSOLE
#If (NTDDI_VERSION < NTDDI_VISTA) Then
Public Const CMIC_MASK_HASLINKNAME   = SEE_MASK_HASLINKNAME
Public Const CMIC_MASK_HASTITLE      = SEE_MASK_HASTITLE
#End If  ' NTDDI_VISTA
        Public Const CMIC_MASK_ASYNCOK = SEE_MASK_ASYNCOK
#If (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const CMIC_MASK_NOASYNC = SEE_MASK_NOASYNC
#End If
        Public Const CMIC_MASK_SHIFT_DOWN = &H10000000
        Public Const CMIC_MASK_CONTROL_DOWN = &H40000000
        Public Const CMIC_MASK_FLAG_LOG_USAGE = SEE_MASK_FLAG_LOG_USAGE
        Public Const CMIC_MASK_NOZONECHECKS = SEE_MASK_NOZONECHECKS
        Public Const CMIC_MASK_PTINVOKE = &H20000000

        Public Const SHCIDS_ALLFIELDS = &H80000000L
        Public Const SHCIDS_CANONICALONLY = &H10000000L
        Public Const SHCIDS_BITMASK = &HFFFF0000L
        Public Const SHCIDS_COLUMNMASK = &HFFFFL
        Public Const SFGAO_CANCOPY = &H1        '' Objects can be copied    (&H1)
        Public Const SFGAO_CANMOVE = &H2        '' Objects can be moved     (&H2)
        Public Const SFGAO_CANLINK = &H4        '' Objects can be linked    (&H4)
        Public Const SFGAO_STORAGE = &H8L     '' supports BindToObject(IID_IStorage)
        Public Const SFGAO_CANRENAME = &H10L     '' Objects can be renamed
        Public Const SFGAO_CANDELETE = &H20L     '' Objects can be deleted
        Public Const SFGAO_HASPROPSHEET = &H40L     '' Objects have property sheets
        Public Const SFGAO_DROPTARGET = &H100L     '' Objects are drop target
        Public Const SFGAO_CAPABILITYMASK = &H177L
        Public Const SFGAO_SYSTEM = &H1000L     '' System object
        Public Const SFGAO_ENCRYPTED = &H2000L     '' Object is encrypted (use alt color)
        Public Const SFGAO_ISSLOW = &H4000L     '' 'Slow' object
        Public Const SFGAO_GHOSTED = &H8000L     '' Ghosted icon
        Public Const SFGAO_LINK = &H10000L     '' Shortcut (link)
        Public Const SFGAO_SHARE = &H20000L     '' Shared
        Public Const SFGAO_READONLY = &H40000L     '' Read-only
        Public Const SFGAO_HIDDEN = &H80000L     '' Hidden object
        Public Const SFGAO_DISPLAYATTRMASK = &HFC000L
        Public Const SFGAO_FILESYSANCESTOR = &H10000000L     '' May contain children with SFGAO_FILESYSTEM
        Public Const SFGAO_FOLDER = &H20000000L     '' Support BindToObject(IID_IShellFolder)
        Public Const SFGAO_FILESYSTEM = &H40000000L     '' Is a win32 file system object (file/folder/root)
        Public Const SFGAO_HASSUBFOLDER = &H80000000L     '' May contain children with SFGAO_FOLDER (may be slow)
        Public Const SFGAO_CONTENTSMASK = &H80000000L
        Public Const SFGAO_VALIDATE = &H1000000L     '' Invalidate cached information (may be slow)
        Public Const SFGAO_REMOVABLE = &H2000000L     '' Is this removeable media?
        Public Const SFGAO_COMPRESSED = &H4000000L     '' Object is compressed (use alt color)
        Public Const SFGAO_BROWSABLE = &H8000000L     '' Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
        Public Const SFGAO_NONENUMERATED = &H100000L     '' Is a non-enumerated object (should be hidden)
        Public Const SFGAO_NEWCONTENT = &H200000L     '' Should show bold in explorer tree
        Public Const SFGAO_CANMONIKER = &H400000L     '' Obsolete
        Public Const SFGAO_HASSTORAGE = &H400000L     '' Obsolete
        Public Const SFGAO_STREAM = &H400000L     '' Supports BindToObject(IID_IStream)
        Public Const SFGAO_STORAGEANCESTOR = &H800000L     '' May contain children with SFGAO_STORAGE or SFGAO_STREAM
        Public Const SFGAO_STORAGECAPMASK = &H70C50008L     '' For determining storage capabilities, ie for open/save semantics
        Public Const SFGAO_PKEYSFGAOMASK = &H81044000L     '' Attributes that are masked out for PKEY_SFGAOFlags because they are considered to cause slow calculations or lack context (SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER and others)

        Public Const SHGFI_ICON = &H100     '' get icon
        Public Const SHGFI_DISPLAYNAME = &H200     '' get display name
        Public Const SHGFI_TYPENAME = &H400     '' get type name
        Public Const SHGFI_ATTRIBUTES = &H800     '' get attributes
        Public Const SHGFI_ICONLOCATION = &H1000     '' get icon location
        Public Const SHGFI_EXETYPE = &H2000     '' return exe type
        Public Const SHGFI_SYSICONINDEX = &H4000     '' get system icon index
        Public Const SHGFI_LINKOVERLAY = &H8000     '' put a link overlay on icon
        Public Const SHGFI_SELECTED = &H10000     '' show icon in selected state
#If (NTDDI_VERSION >= NTDDI_WIN2K) Then
        Public Const SHGFI_ATTR_SPECIFIED = &H20000     '' get only specified attributes
#End If '' (NTDDI_VERSION >= NTDDI_WIN2K)
        Public Const SHGFI_LARGEICON = &H0     '' get large icon
        Public Const SHGFI_SMALLICON = &H1     '' get small icon
        Public Const SHGFI_OPENICON = &H2     '' get open icon
        Public Const SHGFI_SHELLICONSIZE = &H4     '' get shell size icon
        Public Const SHGFI_PIDL = &H8     '' pszPath is a pidl
        Public Const SHGFI_USEFILEATTRIBUTES = &H10     '' use passed dwFileAttribute

        Public Const SHGFI_ADDOVERLAYS = &H20     '' apply the appropriate overlays
        Public Const SHGFI_OVERLAYINDEX = &H40     '' Get the index of the overlay
        '' in the upper 8 bits of the iIcon

        Public Const BIF_RETURNONLYFSDIRS = &H1   ' For finding a folder to start document searching
        Public Const BIF_DONTGOBELOWDOMAIN = &H2 ' For starting the Find Computer
        Public Const BIF_STATUSTEXT = &H4
        Public Const BIF_RETURNFSANCESTORS = &H8
        Public Const BIF_EDITBOX = &H10
        Public Const BIF_VALIDATE = &H20 ' insist on valid result (or CANCEL)

        Public Const BIF_BROWSEFORCOMPUTER = &H1000   ' Browsing for Computers.
        Public Const BIF_BROWSEFORPRINTER = &H2000 ' Browsing for Printers
        Public Const BIF_BROWSEINCLUDEFILES = &H4000  ' Browsing for Everything

        ' message from browser

        Public Const BFFM_INITIALIZED = 1
        Public Const BFFM_SELCHANGED = 2
        Public Const BFFM_VALIDATEFAILED = 3  ' lParam:szPath ret:1(cont),0(EndDialog)
        ' Public Const BFFM_VALIDATEFAILEDW = 4  ' lParam:wzPath ret:1(cont),0(EndDialog)

        ' messages to browser

        Public Const BFFM_SETSTATUSTEXTA = (WM_USER + 100)
        Public Const BFFM_ENABLEOK = (WM_USER + 101)
        Public Const BFFM_SETSELECTIONA = (WM_USER + 102)
        Public Const BFFM_SETSELECTIONW = (WM_USER + 103)
        Public Const BFFM_SETSTATUSTEXTW = (WM_USER + 104)

        ' Get Data From ID List
        Public Const SHGDFIL_FINDDATA = 1&
        Public Const SHGDFIL_NETRESOURCE = 2&
        Public Const SHGDFIL_DESCRIPTIONID = 3&

        '' Shell Description ID

        Public Const SHDID_ROOT_REGITEM = 1&
        Public Const SHDID_FS_FILE = 2&
        Public Const SHDID_FS_DIRECTORY = 3&
        Public Const SHDID_FS_OTHER = 4&
        Public Const SHDID_COMPUTER_DRIVE35 = 5&
        Public Const SHDID_COMPUTER_DRIVE525 = 6&
        Public Const SHDID_COMPUTER_REMOVABLE = 7&
        Public Const SHDID_COMPUTER_FIXED = 8&
        Public Const SHDID_COMPUTER_NETDRIVE = 9&
        Public Const SHDID_COMPUTER_CDROM = 10&
        Public Const SHDID_COMPUTER_RAMDISK = 11&
        Public Const SHDID_COMPUTER_OTHER = 12&
        Public Const SHDID_NET_DOMAIN = 13&
        Public Const SHDID_NET_SERVER = 14&
        Public Const SHDID_NET_SHARE = 15&
        Public Const SHDID_NET_RESTOFNET = 16&
        Public Const SHDID_NET_OTHER = 17&

        '' FO_MOVE ''these need to be kept in sync with the ones in shlobj.h

        Public Const FO_MOVE = &H1
        Public Const FO_COPY = &H2
        Public Const FO_DELETE = &H3
        Public Const FO_RENAME = &H4

        Public Const FOF_MULTIDESTFILES = &H1
        Public Const FOF_CONFIRMMOUSE = &H2
        Public Const FOF_SILENT = &H4                    '' don't create progress/report
        Public Const FOF_RENAMEONCOLLISION = &H8
        Public Const FOF_NOCONFIRMATION = &H10           '' Don't prompt the user.
        Public Const FOF_WANTMAPPINGHANDLE = &H20        '' Fill in SHFILEOPSTRUCT.hNameMappings
        '' Must be freed using SHFreeNameMappings
        Public Const FOF_ALLOWUNDO = &H40
        Public Const FOF_FILESONLY = &H80                '' on *.*, do only files
        Public Const FOF_SIMPLEPROGRESS = &H100          '' means don't show names of files
        Public Const FOF_NOCONFIRMMKDIR = &H200          '' don't confirm making any needed dirs
        Public Const FOF_NOERRORUI = &H400               '' don't put up error UI
        Public Const FOF_NOCOPYSECURITYATTRIBS = &H800   '' dont copy NT file Security Attributes

        Public Const PO_DELETE = &H13         '' printer is being deleted
        Public Const PO_RENAME = &H14         '' printer is being renamed
        Public Const PO_PORTCHANGE = &H20     '' port this printer connected to is being changed
        '' if this id is set, the strings received by
        '' the copyhook are a doubly-null terminated
        '' list of strings.  The first is the printer
        '' name and the second is the printer port.
        Public Const PO_REN_PORT = &H34       '' PO_RENAME and PO_PORTCHANGE at same time.

        Public Const CSIDL_DESKTOP = &H0
        Public Const CSIDL_INTERNET = &H1
        Public Const CSIDL_PROGRAMS = &H2
        Public Const CSIDL_CONTROLS = &H3
        Public Const CSIDL_PRINTERS = &H4
        Public Const CSIDL_PERSONAL = &H5
        Public Const CSIDL_FAVORITES = &H6
        Public Const CSIDL_STARTUP = &H7
        Public Const CSIDL_RECENT = &H8
        Public Const CSIDL_SENDTO = &H9
        Public Const CSIDL_BITBUCKET = &HA
        Public Const CSIDL_STARTMENU = &HB
        Public Const CSIDL_DESKTOPDIRECTORY = &H10
        Public Const CSIDL_DRIVES = &H11
        Public Const CSIDL_NETWORK = &H12
        Public Const CSIDL_NETHOOD = &H13
        Public Const CSIDL_FONTS = &H14
        Public Const CSIDL_TEMPLATES = &H15
        Public Const CSIDL_COMMON_STARTMENU = &H16
        Public Const CSIDL_COMMON_PROGRAMS = &H17
        Public Const CSIDL_COMMON_STARTUP = &H18
        Public Const CSIDL_COMMON_DESKTOPDIRECTORY = &H19
        Public Const CSIDL_APPDATA = &H1A
        Public Const CSIDL_PRINTHOOD = &H1B
        Public Const CSIDL_ALTSTARTUP = &H1D                          '' DBCS
        Public Const CSIDL_COMMON_ALTSTARTUP = &H1E                   '' DBCS
        Public Const CSIDL_COMMON_FAVORITES = &H1F
        Public Const CSIDL_INTERNET_CACHE = &H20
        Public Const CSIDL_COOKIES = &H21
        Public Const CSIDL_HISTORY = &H22

#If (NTDDI_VERSION >= NTDDI_WINXP) Then
        Public Const SHIL_LARGE = 0   '' normally 32x32
        Public Const SHIL_SMALL = 1   '' normally 16x16
        Public Const SHIL_EXTRALARGE = 2
        Public Const SHIL_SYSSMALL = 3   '' like SHIL_SMALL, but tracks system small icon metric correctly
#If (NTDDI_VERSION >= NTDDI_VISTA) Then
        Public Const SHIL_JUMBO = 4   '' normally 256x256
        Public Const SHIL_LAST = SHIL_JUMBO
#Else
Public Const SHIL_LAST           SHIL_SYSSMALL
#End If '' (NTDDI_VERSION >= NTDDI_VISTA)
#End If '' (NTDDI_VERSION >= NTDDI_WINXP)

#End Region

#Region "DLL Libraries"

        Public Const IMAGE_BITMAP = 0
        Public Const IMAGE_ICON = 1
        Public Const IMAGE_CURSOR = 2
        Public Const IMAGE_ENHMETAFILE = 3

        Public Const LR_DEFAULTCOLOR = &H0
        Public Const LR_MONOCHROME = &H1
        Public Const LR_COLOR = &H2
        Public Const LR_COPYRETURNORG = &H4
        Public Const LR_COPYDELETEORG = &H8
        Public Const LR_LOADFROMFILE = &H10
        Public Const LR_LOADTRANSPARENT = &H20
        Public Const LR_DEFAULTSIZE = &H40
        Public Const LR_VGACOLOR = &H80
        Public Const LR_LOADMAP3DCOLORS = &H1000
        Public Const LR_CREATEDIBSECTION = &H2000
        Public Const LR_COPYFROMRESOURCE = &H4000
        Public Const LR_SHARED = &H8000

#End Region

#Region "GDI Objects"

#Region "Device Capabilities"

        '' Public Const Device = Parameters for GetDeviceCaps() ''
        Public Const DRIVERVERSION = 0     '' Device driver version                    ''
        Public Const TECHNOLOGY = 2     '' Device classification                    ''
        Public Const HORZSIZE = 4     '' Horizontal size in millimeters           ''
        Public Const VERTSIZE = 6     '' Vertical size in millimeters             ''
        Public Const HORZRES = 8     '' Horizontal width in pixels               ''
        Public Const VERTRES = 10    '' Vertical height in pixels                ''
        Public Const BITSPIXEL = 12    '' Number of bits per pixel                 ''
        Public Const PLANES = 14    '' Number of planes                         ''
        Public Const NUMBRUSHES = 16    '' Number of brushes the device has         ''
        Public Const NUMPENS = 18    '' Number of pens the device has            ''
        Public Const NUMMARKERS = 20    '' Number of markers the device has         ''
        Public Const NUMFONTS = 22    '' Number of fonts the device has           ''
        Public Const NUMCOLORS = 24    '' Number of colors the device supports     ''
        Public Const PDEVICESIZE = 26    '' Size required for device descriptor      ''
        Public Const CURVECAPS = 28    '' Curve capabilities                       ''
        Public Const LINECAPS = 30    '' Line capabilities                        ''
        Public Const POLYGONALCAPS = 32    '' Polygonal capabilities                   ''
        Public Const TEXTCAPS = 34    '' Text capabilities                        ''
        Public Const CLIPCAPS = 36    '' Clipping capabilities                    ''
        Public Const RASTERCAPS = 38    '' Bitblt capabilities                      ''
        Public Const ASPECTX = 40    '' Length of the X leg                      ''
        Public Const ASPECTY = 42    '' Length of the Y leg                      ''
        Public Const ASPECTXY = 44    '' Length of the hypotenuse                 ''

        Public Const LOGPIXELSX = 88    '' Logical pixels/inch in X                 ''
        Public Const LOGPIXELSY = 90    '' Logical pixels/inch in Y                 ''

        Public Const SIZEPALETTE = 104    '' Number of entries in physical palette    ''
        Public Const NUMRESERVED = 106    '' Number of reserved entries in palette    ''
        Public Const COLORRES = 108    '' Actual color resolution                  ''

        '' Public Const Printing = related DeviceCaps. These replace the appropriate Escapes

        Public Const PHYSICALWIDTH = 110 '' Physical Width in device units           ''
        Public Const PHYSICALHEIGHT = 111 '' Physical Height in device units          ''
        Public Const PHYSICALOFFSETX = 112 '' Physical Printable Area x margin         ''
        Public Const PHYSICALOFFSETY = 113 '' Physical Printable Area y margin         ''
        Public Const SCALINGFACTORX = 114 '' Scaling factor x                         ''
        Public Const SCALINGFACTORY = 115 '' Scaling factor y                         ''

        '' Public Const Display = driver specific

        Public Const VREFRESH = 116  '' Current vertical refresh rate of the    ''
        '' Public Const display = device (for displays only) in Hz ''
        Public Const DESKTOPVERTRES = 117  '' Horizontal width of entire desktop in   ''
        '' pixels                                  ''
        Public Const DESKTOPHORZRES = 118  '' Vertical height of entire desktop in    ''
        '' pixels                                  ''
        Public Const BLTALIGNMENT = 119  '' Preferred blt alignment                 ''


        Public Const SHADEBLENDCAPS = 120  '' Shading and blending caps               ''
        Public Const COLORMGMTCAPS = 121  '' Color Management caps                   ''
        '' WINVER >= &H0500 ''

        '' Public Const Device = Capability Masks: ''

        '' Public Const Device = Technologies ''
        Public Const DT_PLOTTER = 0   '' Vector plotter                   ''
        Public Const DT_RASDISPLAY = 1   '' Raster display                   ''
        Public Const DT_RASPRINTER = 2   '' Raster printer                   ''
        Public Const DT_RASCAMERA = 3   '' Raster camera                    ''
        Public Const DT_CHARSTREAM = 4   '' Character-stream, PLP            ''
        Public Const DT_METAFILE = 5   '' Metafile, VDM                    ''
        Public Const DT_DISPFILE = 6   '' Display-file                     ''

        '' Public Const Curve = Capabilities ''
        Public Const CC_NONE = 0   '' Curves not supported             ''
        Public Const CC_CIRCLES = 1   '' Can do circles                   ''
        Public Const CC_PIE = 2   '' Can do pie wedges                ''
        Public Const CC_CHORD = 4   '' Can do chord arcs                ''
        Public Const CC_ELLIPSES = 8   '' Can do ellipese                  ''
        Public Const CC_WIDE = 16  '' Can do wide lines                ''
        Public Const CC_STYLED = 32  '' Can do styled lines              ''
        Public Const CC_WIDESTYLED = 64  '' Can do wide styled lines         ''
        Public Const CC_INTERIORS = 128 '' Can do interiors                 ''
        Public Const CC_ROUNDRECT = 256 ''                                  ''

        '' Public Const Line = Capabilities ''
        Public Const LC_NONE = 0   '' Lines not supported              ''
        Public Const LC_POLYLINE = 2   '' Can do polylines                 ''
        Public Const LC_MARKER = 4   '' Can do markers                   ''
        Public Const LC_POLYMARKER = 8   '' Can do polymarkers               ''
        Public Const LC_WIDE = 16  '' Can do wide lines                ''
        Public Const LC_STYLED = 32  '' Can do styled lines              ''
        Public Const LC_WIDESTYLED = 64  '' Can do wide styled lines         ''
        Public Const LC_INTERIORS = 128 '' Can do interiors                 ''

        '' Public Const Polygonal = Capabilities ''
        Public Const PC_NONE = 0   '' Polygonals not supported         ''
        Public Const PC_POLYGON = 1   '' Can do polygons                  ''
        Public Const PC_RECTANGLE = 2   '' Can do rectangles                ''
        Public Const PC_WINDPOLYGON = 4   '' Can do winding polygons          ''
        Public Const PC_TRAPEZOID = 4   '' Can do trapezoids                ''
        Public Const PC_SCANLINE = 8   '' Can do scanlines                 ''
        Public Const PC_WIDE = 16  '' Can do wide borders              ''
        Public Const PC_STYLED = 32  '' Can do styled borders            ''
        Public Const PC_WIDESTYLED = 64  '' Can do wide styled borders       ''
        Public Const PC_INTERIORS = 128 '' Can do interiors                 ''
        Public Const PC_POLYPOLYGON = 256 '' Can do polypolygons              ''
        Public Const PC_PATHS = 512 '' Can do paths                     ''

        '' Public Const Clipping = Capabilities ''
        Public Const CP_NONE = 0   '' No clipping of output            ''
        Public Const CP_RECTANGLE = 1   '' Output clipped to rects          ''
        Public Const CP_REGION = 2   '' obsolete                         ''

        '' Public Const Text = Capabilities ''
        Public Const TC_OP_CHARACTER = &H1  '' Can do OutputPrecision   CHARACTER      ''
        Public Const TC_OP_STROKE = &H2  '' Can do OutputPrecision   STROKE         ''
        Public Const TC_CP_STROKE = &H4  '' Can do ClipPrecision     STROKE         ''
        Public Const TC_CR_90 = &H8  '' Can do CharRotAbility    90             ''
        Public Const TC_CR_ANY = &H10  '' Can do CharRotAbility    ANY            ''
        Public Const TC_SF_X_YINDEP = &H20  '' Can do ScaleFreedom      X_YINDEPENDENT ''
        Public Const TC_SA_DOUBLE = &H40  '' Can do ScaleAbility      DOUBLE         ''
        Public Const TC_SA_INTEGER = &H80  '' Can do ScaleAbility      INTEGER        ''
        Public Const TC_SA_CONTIN = &H100  '' Can do ScaleAbility      CONTINUOUS     ''
        Public Const TC_EA_DOUBLE = &H200  '' Can do EmboldenAbility   DOUBLE         ''
        Public Const TC_IA_ABLE = &H400  '' Can do ItalisizeAbility  ABLE           ''
        Public Const TC_UA_ABLE = &H800  '' Can do UnderlineAbility  ABLE           ''
        Public Const TC_SO_ABLE = &H1000  '' Can do StrikeOutAbility  ABLE           ''
        Public Const TC_RA_ABLE = &H2000  '' Can do RasterFontAble    ABLE           ''
        Public Const TC_VA_ABLE = &H4000  '' Can do VectorFontAble    ABLE           ''
        Public Const TC_RESERVED = &H8000
        Public Const TC_SCROLLBLT = &H10000  '' Don't do text scroll with blt           ''

        '' NOGDICAPMASKS ''

        '' Public Const Raster = Capabilities ''
        Public Const RC_NONE = 0
        Public Const RC_BITBLT = 1       '' Can do standard BLT.             ''
        Public Const RC_BANDING = 2       '' Device requires banding support  ''
        Public Const RC_SCALING = 4       '' Device requires scaling support  ''
        Public Const RC_BITMAP64 = 8       '' Device can support >64K bitmap   ''
        Public Const RC_GDI20_OUTPUT = &H10      '' has 2.0 output calls         ''
        Public Const RC_GDI20_STATE = &H20
        Public Const RC_SAVEBITMAP = &H40
        Public Const RC_DI_BITMAP = &H80      '' supports DIB to memory       ''
        Public Const RC_PALETTE = &H100      '' supports a palette           ''
        Public Const RC_DIBTODEV = &H200      '' supports DIBitsToDevice      ''
        Public Const RC_BIGFONT = &H400      '' supports >64K fonts          ''
        Public Const RC_STRETCHBLT = &H800      '' supports StretchBlt          ''
        Public Const RC_FLOODFILL = &H1000      '' supports FloodFill           ''
        Public Const RC_STRETCHDIB = &H2000      '' supports StretchDIBits       ''
        Public Const RC_OP_DX_OUTPUT = &H4000
        Public Const RC_DEVBITS = &H8000



        '' Public Const Shading = and blending caps ''
        Public Const SB_NONE = &H0
        Public Const SB_CONST_ALPHA = &H1
        Public Const SB_PIXEL_ALPHA = &H2
        Public Const SB_PREMULT_ALPHA = &H4

        Public Const SB_GRAD_RECT = &H10
        Public Const SB_GRAD_TRI = &H20

        '' Public Const Color = Management caps ''
        Public Const CM_NONE = &H0
        Public Const CM_DEVICE_ICM = &H1
        Public Const CM_GAMMA_RAMP = &H2
        Public Const CM_CMYK_COLOR = &H4

        '' WINVER >= &H0500 ''


#End Region

#Region "Brushes"

        ' Brush Styles

        Public Const BS_DIBPATTERN = 5
        Public Const BS_DIBPATTERN8X8 = 8
        Public Const BS_DIBPATTERNPT = 6
        Public Const BS_HATCHED = 2
        Public Const BS_NULL = 1
        Public Const BS_HOLLOW = BS_NULL
        Public Const BS_PATTERN = 3
        Public Const BS_PATTERN8X8 = 7
        Public Const BS_SOLID = 0

        '' Hatch brush constants

        Public Const HS_BDIAGONAL = 3
        Public Const HS_BDIAGONAL1 = 7
        Public Const HS_CROSS = 4
        Public Const HS_DIAGCROSS = 5
        Public Const HS_DITHEREDBKCLR = 24
        Public Const HS_DITHEREDCLR = 20
        Public Const HS_DITHEREDTEXTCLR = 22
        Public Const HS_FDIAGONAL = 2
        Public Const HS_FDIAGONAL1 = 6
        Public Const HS_HALFTONE = 18
        Public Const HS_HORIZONTAL = 0
        Public Const HS_NOSHADE = 17
        Public Const HS_SOLID = 8
        Public Const HS_SOLIDBKCLR = 23
        Public Const HS_SOLIDCLR = 19
        Public Const HS_SOLIDTEXTCLR = 21
        Public Const HS_VERTICAL = 1

#End Region

#Region "Pens"
        ' Pen Styles

        Public Const PS_ALTERNATE = 8
        Public Const PS_COSMETIC = &H0
        Public Const PS_DASH = 1
        Public Const PS_DASHDOT = 3
        Public Const PS_DASHDOTDOT = 4
        Public Const PS_DOT = 2
        Public Const PS_ENDCAP_FLAT = &H200
        Public Const PS_ENDCAP_MASK = &HF00
        Public Const PS_ENDCAP_ROUND = &H0
        Public Const PS_ENDCAP_SQUARE = &H100
        Public Const PS_GEOMETRIC = &H10000
        Public Const PS_INSIDEFRAME = 6
        Public Const PS_JOIN_BEVEL = &H1000
        Public Const PS_JOIN_MASK = &HF000
        Public Const PS_JOIN_MITER = &H2000
        Public Const PS_JOIN_ROUND = &H0
        Public Const PS_NULL = 5
        Public Const PS_SOLID = 0&
        Public Const PS_STYLE_MASK = &HF
        Public Const PS_PTCMASK = &HF0000
        Public Const PS_USERSTYLE = 7
#End Region

        '' GetObject object constants
        Public Const BI_RGB = 0L
        Public Const BI_RLE8 = 1L
        Public Const BI_RLE4 = 2L
        Public Const BI_BITFIELDS = 3L
        Public Const BI_JPEG = 4L
        Public Const BI_PNG = 5L

        Public Const OBJ_BITMAP = 7
        Public Const OBJ_BRUSH = 2
        Public Const OBJ_DC = 3
        Public Const OBJ_ENHMETADC = 12
        Public Const OBJ_ENHMETAFILE = 13
        Public Const OBJ_EXTPEN = 11
        Public Const OBJ_FONT = 6
        Public Const OBJ_MEMDC = 10
        Public Const OBJ_METADC = 4
        Public Const OBJ_METAFILE = 9
        Public Const OBJ_PAL = 5
        Public Const OBJ_PEN = 1
        Public Const OBJ_REGION = 8



#End Region

#Region "BitBlt"
        ' BitBlt flags

        Public Const MERGEPAINT = &HBB0226
        Public Const SRCERASE = &H440328
        Public Const SRCINVERT = &H660046
        Public Const SRCPAINT = &HEE0086
        Public Const SRCCOPY = &HCC0020
        Public Const MERGECOPY = &HC000CA
        Public Const NOTSRCCOPY = &H330008
        Public Const NOTSRCERASE = &H1100A6

#End Region

#Region "Text"
        '' Draw Modes

        Public Const OPAQUE = 2
        Public Const TRANSPARENT = 1

        '' Text Alignment

        Public Const TA_BASELINE = 2
        Public Const TA_BOTTOM = 8
        Public Const TA_CENTER = 6
        Public Const TA_LEFT = 0
        Public Const TA_NOUPDATECP = 0
        Public Const TA_RIGHT = 2
        Public Const TA_TOP = 0
        Public Const TA_UPDATECP = 1

        Public Const TA_MASK = (TA_BASELINE + TA_CENTER + TA_UPDATECP)

        Public Const ETO_GRAYED = &H1
        Public Const ETO_OPAQUE = &H2
        Public Const ETO_CLIPPED = &H4
        Public Const ETO_GLYPH_INDEX = &H10
        Public Const ETO_RTLREADING = &H80
        Public Const ETO_NUMERICSLOCAL = &H400
        Public Const ETO_NUMERICSLATIN = &H800
        Public Const ETO_IGNORELANGUAGE = &H1000
        Public Const ETO_PDY = &H2000

#End Region

#Region "Draw States"

        '' Check states.

        Public Const DRWCHK_NORMAL = 0
        Public Const DRWCHK_SELECTED = 1
        Public Const DRWCHK_DISABLED = 2

        '' Draw Types

        Public Const DST_COMPLEX = &H0
        Public Const DST_TEXT = &H1
        Public Const DST_PREFIXTEXT = &H2
        Public Const DST_ICON = &H3
        Public Const DST_BITMAP = &H4

        '' Draw states

        Public Const DSS_NORMAL = &H0
        Public Const DSS_UNION = &H10         ' Gray string appearance '
        Public Const DSS_DISABLED = &H20
        Public Const DSS_MONO = &H80
        Public Const DSS_RIGHT = &H8000

#End Region

#Region "Fonts"


        Public Enum FontWeight As Integer

            FW_DONTCARE = 0
            FW_THIN = 100
            FW_EXTRALIGHT = 200
            FW_LIGHT = 300
            FW_NORMAL = 400
            FW_MEDIUM = 500
            FW_SEMIBOLD = 600
            FW_BOLD = 700
            FW_EXTRABOLD = 800
            FW_HEAVY = 900
        End Enum

        Public Enum FontCharSet As Byte

            ANSI_CHARSET = 0
            DEFAULT_CHARSET = 1
            SYMBOL_CHARSET = 2
            SHIFTJIS_CHARSET = 128
            HANGEUL_CHARSET = 129
            HANGUL_CHARSET = 129
            GB2312_CHARSET = 134
            CHINESEBIG5_CHARSET = 136
            OEM_CHARSET = 255
            JOHAB_CHARSET = 130
            HEBREW_CHARSET = 177
            ARABIC_CHARSET = 178
            GREEK_CHARSET = 161
            TURKISH_CHARSET = 162
            VIETNAMESE_CHARSET = 163
            THAI_CHARSET = 222
            EASTEUROPE_CHARSET = 238
            RUSSIAN_CHARSET = 204
            MAC_CHARSET = 77
            BALTIC_CHARSET = 186
        End Enum

        Public Enum FontPrecision As Byte

            OUT_DEFAULT_PRECIS = 0
            OUT_STRING_PRECIS = 1
            OUT_CHARACTER_PRECIS = 2
            OUT_STROKE_PRECIS = 3
            OUT_TT_PRECIS = 4
            OUT_DEVICE_PRECIS = 5
            OUT_RASTER_PRECIS = 6
            OUT_TT_ONLY_PRECIS = 7
            OUT_OUTLINE_PRECIS = 8
            OUT_SCREEN_OUTLINE_PRECIS = 9
            OUT_PS_ONLY_PRECIS = 10
        End Enum

        Public Enum FontClipPrecision As Byte

            CLIP_DEFAULT_PRECIS = 0
            CLIP_CHARACTER_PRECIS = 1
            CLIP_STROKE_PRECIS = 2
            CLIP_MASK = &HF
            CLIP_LH_ANGLES = (1 << 4)
            CLIP_TT_ALWAYS = (2 << 4)
            CLIP_DFA_DISABLE = (4 << 4)
            CLIP_EMBEDDED = (8 << 4)
        End Enum

        Public Enum FontQuality As Byte

            DEFAULT_QUALITY = 0
            DRAFT_QUALITY = 1
            PROOF_QUALITY = 2
            NONANTIALIASED_QUALITY = 3
            ANTIALIASED_QUALITY = 4
            CLEARTYPE_QUALITY = 5
            CLEARTYPE_NATURAL_QUALITY = 6
        End Enum

        <Flags>
        Public Enum FontPitchAndFamily As Byte
            DEFAULT_PITCH = 0
            FIXED_PITCH = 1
            VARIABLE_PITCH = 2
            FF_DONTCARE = (0 << 4)
            FF_ROMAN = (1 << 4)
            FF_SWISS = (2 << 4)
            FF_MODERN = (3 << 4)
            FF_SCRIPT = (4 << 4)
            FF_DECORATIVE = (5 << 4)
        End Enum

#End Region

#Region "Structures"

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure FILEDESCRIPTOR
            Public dwFlags As UInteger
            Public clsid As Guid
            Public sizel As SIZE
            Public pointl As POINT
            Public dwFileAttributes As UInteger
            Public ftCreationTime As FILETIME
            Public ftLastAccessTime As FILETIME
            Public ftLastWriteTime As FILETIME
            Public nFileSizeHigh As UInteger
            Public nFileSizeLow As UInteger

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
            Public cFileName As String

            Public Sub New(name As String, size As ULong, createDate As Date, writeDate As Date)
                cFileName = Path.GetFileName(name)
                nFileSizeLow = (CUInt(size And &HFFFFFFFFUL))
                nFileSizeHigh = (CUInt(size >> 32))

                LocalToFileTime(writeDate, ftLastWriteTime)
                LocalToFileTime(writeDate, ftLastAccessTime)
                LocalToFileTime(createDate, ftCreationTime)
            End Sub

            Public Overrides Function ToString() As String
                Return cFileName
            End Function

            Public Shared Widening Operator CType(operand As FILEDESCRIPTOR) As String
                Return operand.cFileName
            End Operator

            Public Shared Narrowing Operator CType(operand As String) As FILEDESCRIPTOR
                Dim wf As New WIN32_FIND_DATA
                Dim i As IntPtr

                i = FindFirstFile(operand, wf)
                If CLng(i) > -1 Then
                    FindClose(i)

                    Dim fd As New FILEDESCRIPTOR

                    fd.cFileName = wf.cFilename
                    fd.dwFileAttributes = CUInt(wf.dwFileAttributes)
                    fd.ftCreationTime = wf.ftCreationTime
                    fd.ftLastAccessTime = wf.ftLastAccessTime
                    fd.ftLastWriteTime = wf.ftLastWriteTime
                    fd.nFileSizeHigh = CUInt(wf.nFileSizeHigh)
                    fd.nFileSizeLow = wf.nFileSizeLow

                    Return fd
                End If

                Return Nothing
            End Operator

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure RGBQUAD
            Public rgbBlue As Byte
            Public rgbGreen As Byte
            Public rgbRed As Byte
            Public rgbReserved As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure RGBTRIPLE
            Public rgbtBlue As Byte
            Public rgbtGreen As Byte
            Public rgbtRed As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Public Structure BITMAPINFOHEADER
            Public biSize As Integer
            Public biWidth As Integer
            Public biHeight As Integer
            Public biPlanes As Short
            Public biBitCount As Short
            Public biCompression As Integer
            Public biSizeImage As Integer
            Public biXPelsPerMeter As Integer
            Public biYPelsPerMeter As Integer
            Public biClrUsed As Integer
            Public biClrImportant As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BITMAPINFO
            Public bmiHeader As BITMAPINFOHEADER
            Public bmiColors As RGBQUAD
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BITMAPFILEHEADER
            Public bfType As Short
            Public bfSize As Integer
            Public bfReserved1 As Short
            Public bfReserved2 As Short
            Public bfOffBits As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BITMAPCOREHEADER
            Public bcSize As Integer
            Public bcWidth As Short
            Public bcHeight As Short
            Public bcPlanes As Short
            Public bcBitCount As Short
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BITMAPCOREINFO
            Public bmciHeader As BITMAPCOREHEADER
            Public bmciColors As RGBTRIPLE
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BITMAPSTRUCT
            Public bmStructure As Integer
            Public bmWidth As Integer
            Public bmHeight As Integer
            Public bmWidthBytes As Integer
            Public bmPlanes As Short
            Public bmBitsPixel As Short
            Public bmBits As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure ICONINFO
            Public fIcon As Integer
            Public xHotspot As Integer
            Public yHotspot As Integer
            Public hbmMask As IntPtr
            Public hbmColor As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure ICONMETRICS
            Public cbSize As Integer
            Public iHorzSpacing As Integer
            Public iVertSpacing As Integer
            Public iTitleWrap As Integer
            Public lfFont As LOGFONT
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BITDATA
            Public Blue As Byte
            Public Green As Byte
            Public Red As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BITDATAREV
            Public Red As Byte
            Public Green As Byte
            Public Blue As Byte
        End Structure

        Public Enum SpecialFolderConstants
            Desktop = &H0
            Internet = &H1
            Programs = &H2
            Controls = &H3
            Printers = &H4
            Personal = &H5
            Favorites = &H6
            StartUp = &H7
            Recent = &H8
            SendTo = &H9
            BitBucket = &HA
            StartMenu = &HB
            DesktopDirectory = &H10
            Drives = &H11
            Network = &H12
            Nethood = &H13
            Fonts = &H14
            Templates = &H15
            CommonStartMenu = &H16
            CommonPrograms = &H17
            CommonStartup = &H18
            CommonDesktopDirectory = &H19
            AppData = &H1A
            PrintHood = &H1B
            AltStartup = &H1D                          '' DBCS
            CommonAltStartup = &H1E                   '' DBCS
            CommonFavorites = &H1F
            InternetCache = &H20
            Cookies = &H21
            History = &H22
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure IMAGEINFO
            Public hbmImage As IntPtr
            Public hbmMask As IntPtr
            Public Unused1 As Integer
            Public Unused2 As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public rcImage As RECT

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHFILEOPSTRUCT
            Public hWnd As IntPtr
            Public wFunc As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public pFrom As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public pTo As String

            Public fFlags As Integer
            Public fAnyOperationsAborted As Boolean
            Public hNameMappings As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpszProgressTitle As String
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHELLEXECUTEINFO
            Public cbSize As Integer
            Public fMask As UInteger
            Public hWnd As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpVerb As String

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpFile As String

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpParamaters As String

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpDirectory As String

            Public nShow As Integer
            Public hInstApp As IntPtr

            Public lpIDList As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpClass As String
            Public hkeyClass As IntPtr
            Public dwHotKey As Integer
            Public hIcon As IntPtr
            Public hProcess As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHELLEXECUTEINFOPTR
            Public cbSize As Integer
            Public fMask As UInteger
            Public hWnd As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpVerb As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpFile As String

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpParamaters As String

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpDirectory As String

            Public nShow As Integer
            Public hInstApp As IntPtr

            Public lpIDList As IntPtr

            <MarshalAs(UnmanagedType.LPTStr)>
            Public lpClass As String
            Public hkeyClass As IntPtr
            Public dwHotKey As Integer
            Public hIcon As IntPtr
            Public hProcess As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHFILEINFO
            Public hIcon As IntPtr           '  out: icon
            Public iIcon As Integer          '  out: icon index
            Public dwAttributes As Integer               '  out: SFGAO_ flags

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
            Public szDisplayName As String

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
            Public szStructureName As String
        End Structure
        '' Share folder information structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHARE_INFO_2

            <MarshalAs(UnmanagedType.LPWStr)>
            Public shi2_netname As String
            Public shi2_type As UInteger

            <MarshalAs(UnmanagedType.LPWStr)>
            Public shi2_remark As String
            Public shi2_permissions As UInteger
            Public shi2_max_users As UInteger
            Public shi2_current_uses As UInteger

            <MarshalAs(UnmanagedType.LPWStr)>
            Public shi2_path As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public shi2_passwd As String
        End Structure

        '' system browser-for-folder info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BROWSEINFO
            Public hWndOwner As Integer
            Public pidlRoot As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public DisplayName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpszTitle As String

            Public ulFlags As Short
            Public lpfn As Integer
            Public lParam As Integer
            Public iImage As Short
        End Structure

        ''' <summary>
        ''' SYSTEMTIME native date-time structure.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SYSTEMTIME
            Public wYear As UShort
            Public wMonth As UShort
            Public wDayOfWeek As UShort
            Public wDay As UShort
            Public wHour As UShort
            Public wMinute As UShort
            Public wSecond As UShort
            Public wMilliseconds As UShort

            Public Shared Narrowing Operator CType(operand As SYSTEMTIME) As Date
                Return New Date(operand.wYear, operand.wMonth, operand.wDay, operand.wHour, operand.wMinute, operand.wSecond, operand.wMilliseconds)
            End Operator

            Public Shared Narrowing Operator CType(operand As Date) As SYSTEMTIME
                Return New SYSTEMTIME(operand)
            End Operator

            ''' <summary>
            ''' Initialize a new SYSTEMTIME structure with the specified DateTime object.
            ''' </summary>
            ''' <param name="t">A DateTime object.</param>
            ''' <remarks></remarks>
            Public Sub New(t As Date)
                wYear = CUShort(t.Year)
                wMonth = CUShort(t.Month)
                wDayOfWeek = CUShort(t.DayOfWeek)
                wDay = CUShort(t.Day)
                wHour = CUShort(t.Hour)
                wMinute = CUShort(t.Minute)
                wSecond = CUShort(t.Second)
                wMilliseconds = CUShort(t.Millisecond)
            End Sub

        End Structure

        ''' <summary>
        ''' System FILETIME structure with automated conversion to CLR DateTime objects and Long integers.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure FILETIME
            Public lo As UInteger
            Public hi As Integer

            ''' <summary>
            ''' Converts a system DateTime object into a FILETIME structure.
            ''' </summary>
            ''' <param name="t">System DateTime.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function FromDateTime(t As Date) As FILETIME
                Return New FILETIME(t)
            End Function

            ''' <summary>
            ''' Converts the current FILETIME structure into a DateTime object.
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function ToDateTime() As Date
                ToDateTime = FileToLocal(Me)
            End Function

            Public Shared Widening Operator CType(operand As FILETIME) As Date
                Return operand.ToDateTime
            End Operator

            Public Shared Narrowing Operator CType(operand As Date) As FILETIME
                Return FILETIME.FromDateTime(operand)
            End Operator

            Public Shared Widening Operator CType(operand As Long) As FILETIME
                Dim ft As New FILETIME
                ft.lo = CUInt(operand And &HFFFFFFFFUI)
                ft.hi = CInt((operand And &HFFFFFFFF00000000L) >> 32)

                Return ft
            End Operator

            Public Shared Widening Operator CType(operand As FILETIME) As Long
                Return MakeLong(operand.lo, operand.hi)
            End Operator

            Public Overrides Function ToString() As String
                Return ToDateTime.ToString
            End Function

            ''' <summary>
            ''' Initialize a new FILETIME structure with the specified DateTime object.
            ''' </summary>
            ''' <param name="t">System DateTime.</param>
            ''' <remarks></remarks>
            Public Sub New(t As Date)
                LocalToFileTime(t, Me)
            End Sub

            ''' <summary>
            ''' Initialize a new FILETIME structure with the specified value.
            ''' </summary>
            ''' <param name="t">64 bit integer time value.</param>
            ''' <remarks></remarks>
            Public Sub New(t As Long)
                Me.lo = CUInt(t And &HFFFFFFFF)
                Me.hi = CInt(t And (&HFFFFFFFF00000000 >> 32))
            End Sub

            ''' <summary>
            ''' Initialize a new FILETIME structure with the specified low and high order values.
            ''' </summary>
            ''' <param name="l">Value of the low-order DWORD</param>
            ''' <param name="h">Value of the high-order DWORD</param>
            ''' <remarks></remarks>
            Public Sub New(l As Integer, h As Integer)
                lo = CUInt(l)
                hi = h
            End Sub

        End Structure

        ''' <summary>
        ''' Native WIN32_FIND_DATA structure used in FindFirstFile/FindFirstFileEx and FindNextFile
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure WIN32_FIND_DATA

            Public dwFileAttributes As FileAttributes

            <MarshalAs(UnmanagedType.Struct)>
            Public ftCreationTime As FILETIME

            <MarshalAs(UnmanagedType.Struct)>
            Public ftLastAccessTime As FILETIME

            <MarshalAs(UnmanagedType.Struct)>
            Public ftLastWriteTime As FILETIME

            Public nFileSizeHigh As Integer

            Public nFileSizeLow As UInteger

            Public dwReserved1 As Integer

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=260)>
            Private _cFilename As Char()

            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=14)>
            Private _cAlternate As Char()

            Public Property cFilename As String
                Get
                    Return CStr(_cFilename).Trim(ChrW(0))
                End Get
                Set(value As String)
                    If value.Length > 260 Then Throw New ArgumentException()
                    Dim mm As New SafePtr

                    mm.Alloc(260 * 2)
                    mm.SetString(0, value, False)
                    Marshal.Copy(mm.Handle, _cFilename, 0, CInt(mm.Size))
                    mm.Free()
                End Set
            End Property

            Public Property cAlternate As String
                Get
                    Return CStr(_cAlternate).Trim(ChrW(0))
                End Get
                Set(value As String)
                    Dim mm As MemPtr
                    If value.Length > 14 Then Throw New argumentexception()

                    mm.Alloc(14 * 2)
                    mm.SetString(0, value, False)
                    Marshal.Copy(mm.Handle, _cAlternate, 0, CInt(mm.Size))
                    mm.Free()
                End Set
            End Property

            Public Property nFileSize As Long
                Get
                    nFileSize = (CLng(nFileSizeHigh) << 32) Or CLng(nFileSizeLow)
                End Get
                Set(value As Long)
                    nFileSizeHigh = CInt(((value >> 32) And &HFFFFFFFFL))
                    nFileSizeLow = CUInt((value And &HFFFFFFFFL))
                End Set
            End Property
        End Structure

        ''' <summary>
        ''' System NETRESOURCE structure.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure NETRESOURCE
            Public dwScope As Integer
            Public dwStructure As Integer
            Public dwDisplayStructure As Integer
            Public dwUsage As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpLocalName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpRemoteName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpComment As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public lpProvider As String

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SHDESCRIPTIONID
            Public dwDescrptionId As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public clsid As System.Guid
        End Structure

        Public Enum EnumExAttributeConstants
            faArchive = &H20
            faCompressed = &H800
            faDirectory = &H10
            faHidden = &H2
            faNormal = &H80
            faReadOnly = &H1
            faSystem = &H4
            faTemporary = &H100
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MODFSFINDDATA
            Public Attributes As EnumExAttributeConstants
            Public CreationTime As Date
            Public LastAccessTime As Date
            Public LastWriteTime As Date
            Public SizeHigh As Integer
            Public SizeLow As Integer

            <MarshalAs(UnmanagedType.LPWStr)>
            Public FileName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public ShortName As String
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
        Public Structure LOGFONT
            Public lfHeight As Int32
            Public lfWidth As Int32
            Public lfEscapement As Int32
            Public lfOrientation As Int32
            Public lfWeight As Int32
            Public lfItalic As Byte
            Public lfUnderline As Byte
            Public lfStrikeOut As Byte
            Public lfCharSet As Byte
            Public lfOutPrecision As Byte
            Public lfClipPrecision As Byte
            Public lfQuality As Byte
            Public lfPitchAndFamily As Byte
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
            Public lfFaceName As String
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure TEXTMETRIC
            Public tmHeight As Integer
            Public tmAscent As Integer
            Public tmDescent As Integer
            Public tmInternalLeading As Integer
            Public tmExternalLeading As Integer
            Public tmAveCharWidth As Integer
            Public tmMaxCharWidth As Integer
            Public tmWeight As Integer
            Public tmOverhang As Integer
            Public tmDigitizedAspectX As Integer
            Public tmDigitizedAspectY As Integer
            Public tmFirstChar As Byte
            Public tmLastChar As Byte
            Public tmDefaultChar As Byte
            Public tmBreakChar As Byte
            Public tmItalic As Byte
            Public tmUnderlined As Byte
            Public tmStruckOut As Byte
            Public tmPitchAndFamily As Byte
            Public tmCharSet As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure ENUMLOGFONTEX

            <MarshalAs(UnmanagedType.Struct)>
            Public elfLogFont As LOGFONT

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=64)>
            Public elfFullName As String

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
            Public elfStyle As String

            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
            Public elfScript As String

            Public Overrides Function ToString() As String
                Return elfFullName
            End Function

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure NONCLIENTMETRICS
            Public cbSize As Integer
            Public iBorderWidth As Integer
            Public iScrollWidth As Integer
            Public iScrollHeight As Integer
            Public iCaptionWidth As Integer
            Public iCaptionHeight As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public lfCaptionFont As LOGFONT
            Public iSMCaptionWidth As Integer
            Public iSMCaptionHeight As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public lfSMCaptionFont As LOGFONT

            Public iMenuWidth As Integer
            Public iMenuHeight As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public lfMenuFont As LOGFONT

            <MarshalAs(UnmanagedType.Struct)>
            Public lfStatusFont As LOGFONT

            <MarshalAs(UnmanagedType.Struct)>
            Public lfMessageFont As LOGFONT

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SCROLLINFO
            Public cbSize As Integer
            Public fMask As Integer
            Public nMin As Integer
            Public nMax As Integer
            Public nPage As Integer
            Public nPos As Integer
            Public nTrackPos As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure LOGBRUSH
            Public lbStyle As Integer,
            lbColor As Integer,
            lbHatch As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure LOGPEN
            Public lopnStyle As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public lopnWidth As POINT

            Public lopnColor As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure PAINTSTRUCT
            Public hDC As System.IntPtr,
            fErase As Integer,
            rcpaint As RECT,
            fRestore As Integer,
            fIncUpdate As Integer

            Dim rgbReserved1 As Integer,
            rgbReserved2 As Integer,
            rgbReserved3 As Integer,
            rgbReserved4 As Integer

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure ACCEL
            Public fVirt As Byte
            Public Key As Short
            Public cmd As Short
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MSG
            Public hWnd As IntPtr
            Public Message As Integer
            Public wParam As Integer
            Public lParam As Integer
            Public time As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public pt As POINT
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MENUINFO
            Public cbSize As Integer
            Public fMask As Integer
            Public dwStyle As Integer
            Public cyMax As Integer
            Public Back As Integer
            Public ContextHelpID As Integer
            Public MenuData As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure MENUITEMINFO
            Public cbSize As Integer
            Public fMask As Integer
            Public fType As Integer
            Public fState As Integer
            Public wID As Integer
            Public hSubMenu As IntPtr
            Public hbmpChecked As IntPtr
            Public hbmpUnchecked As IntPtr
            Public dwItemData As IntPtr

            Public dwTypeData As IntPtr

            Public cch As Integer

            ' Declared for Windows 2000/98.  Still backward compatible

            Public hbmpItem As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure TPMPARAMS
            Public cbSize As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public rcExclude As RECT
        End Structure

        Public Structure MENUBARINFO
            Public cbSize As Integer

            <MarshalAs(UnmanagedType.Struct)>
            Public rcBar As RECT

            Public hMenu As Integer
            Public hWndMenu As Integer
            Public fBarFocused As Boolean
            Public fFocused As Boolean
        End Structure

#End Region

#Region "Declares"

#Region "Windows"

        Public Declare Unicode Function GetDC Lib "user32" (hWnd As System.IntPtr) As System.IntPtr
        Public Declare Unicode Function ReleaseDC Lib "user32" (hWnd As System.IntPtr, hDC As System.IntPtr) As Integer

        Public Declare Unicode Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoW" (uAction As Integer, uParam As Integer, ByRef lpvParam As Object, fuWinIni As Integer) As Integer
        Public Declare Unicode Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoW" (uAction As Integer, uParam As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef lpvParam As NONCLIENTMETRICS, fuWinIni As Integer) As Integer

        Public Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageW" (hWnd As IntPtr, wMsg As UInteger, wParam As Integer, lParam As Integer) As IntPtr
        Public Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageW" (hWnd As IntPtr, wMsg As UInteger, wParam As Integer, lParam As IntPtr) As IntPtr

        Public Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageW" (hWnd As IntPtr, wMsg As UInteger, wParam As IntPtr, lParam As Integer) As IntPtr
        Public Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageW" (hWnd As IntPtr, wMsg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr

        Public Declare Unicode Function PostMessage Lib "user32" Alias "PostMessageW" (hWnd As IntPtr, wMsg As UInteger, wParam As IntPtr, lParam As IntPtr) As Boolean

        Public Declare Unicode Function GetLastError Lib "kernel32" () As Integer
        Public Declare Unicode Function FormatMessage Lib "kernel32" Alias "FormatMessageW" (dwFlags As UInteger, lpSource As IntPtr, dwMessageId As UInteger, dwLanguageId As UInteger, lpBuffer As IntPtr, dwSize As UInteger, va_list As IntPtr) As Integer

        '' Executable assembly version functions for 16, 32 and 64 bit binaries.
        Public Declare Unicode Function GetFileVersionInfo Lib "version.dll" Alias "GetFileVersionInfoW" (<MarshalAs(UnmanagedType.LPTStr)> lptstrFilename As String, dwHandle As IntPtr, dwLen As Integer, lpData As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        Public Declare Unicode Function GetFileVersionInfoSize Lib "Version.dll" Alias "GetFileVersionInfoSizeW" (<MarshalAs(UnmanagedType.LPTStr)> lptstrFilename As String, lpdwHandle As IntPtr) As UInteger
        Public Declare Unicode Function VerQueryValue Lib "version.dll" Alias "VerQueryValueW" (pBlock As IntPtr, <MarshalAs(UnmanagedType.LPTStr)> lpSubBlock As String, ByRef lplpVoid As IntPtr, ByRef puInt As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetCursorPos Lib "user32.dll" (ByRef lpPoint As POINT) As <MarshalAs(UnmanagedType.Bool)> Boolean
        Public Declare Unicode Function CloseHandle Lib "kernel32.dll" (hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

#End Region

#Region "DLL Library Functions"

        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function LoadLibrary(<MarshalAs(UnmanagedType.LPWStr)> lpFileName As String) As IntPtr
        End Function


        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function GetModuleHandle(<MarshalAs(UnmanagedType.LPWStr)> name As String) As IntPtr
        End Function


        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function GetProcAddress(hModule As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpProcName As String) As IntPtr
        End Function

        <DllImport("kernel32.dll", EntryPoint:="GetProcAddress", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Function GetProcAddressDelegate(hModule As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpProcName As String) As [Delegate]
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Function FreeLibrary(hInst As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Function LoadImage(hInst As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpszName As String, uType As Int32,
                                    cxDesired As Integer, cyDesired As Integer, fuLoad As Int32) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Function LoadImage(hInst As IntPtr, lpszName As IntPtr, uType As Int32,
            cxDesired As Integer, cyDesired As Integer, fuLoad As Int32) As IntPtr
        End Function

        <DllImport("coredll.dll")>
        Public Function LoadIcon(hinst As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> iconName As String) As IntPtr
        End Function

#End Region

#Region "Windows Shell"

        Public Declare Unicode Function ExtractIconEx Lib "shell32.dll" Alias "ExtractIconExW" (<MarshalAs(UnmanagedType.LPTStr)> lpszFile As String, nIconIndex As Integer, ByRef phiconLarge As IntPtr, ByRef phiconSmall As IntPtr, nIcons As UInteger) As Integer
        Public Declare Unicode Function ExtractIconEx2 Lib "shell32.dll" Alias "ExtractIconExW" (<MarshalAs(UnmanagedType.LPTStr)> lpszFile As String, nIconIndex As Integer, phiconLarge As IntPtr, phiconSmall As IntPtr, nIcons As UInteger) As Integer

        Public Declare Unicode Function ShowShareFolderUI Lib "ntshrui.dll" Alias "ShowShareFolderUIW" (hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpszFolder As String) As Integer

        'Public Declare Unicode Function GetCurrentDirectory Lib "kernel32" Alias "GetCurrentDirectoryW" (bLen As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpszBuffer As String) As Integer

        'Public Declare Unicode Function GetFullPathName Lib "kernel32" Alias "GetFullPathNameW" (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String, nBufferLength As Integer, lpBuffer As IntPtr, ByRef lpFilepart As IntPtr) As Integer


        Public Enum ReplaceFileFlags As UInteger

            ''' <summary>
            ''' This value Is Not supported.
            ''' </summary>
            REPLACEFILE_WRITE_THROUGH = &H1

            ''' <summary>
            ''' Ignores errors that occur While merging information (such As attributes And ACLs) from the replaced file To the replacement file. Therefore, If you specify this flag And Do Not have WRITE_DAC access, the Function succeeds but the ACLs are Not preserved.
            ''' </summary>
            REPLACEFILE_IGNORE_MERGE_ERRORS = &H2

            ''' <summary>
            ''' Ignores errors that occur while merging ACL information from the replaced file to the replacement file. Therefore, if you specify this flag And do Not have WRITE_DAC access, the function succeeds but the ACLs are Not preserved. To compile an application that uses this value, define the _WIN32_WINNT macro as 0x0600 Or later.
            '''
            ''' Windows Server 2003 And Windows XP:This value Is Not supported.
            ''' </summary>
            REPLACEFILE_IGNORE_ACL_ERRORS = &H4

        End Enum

        Public Declare Unicode Function ReplaceFile Lib "kernel32" Alias "ReplaceFileW" (<MarshalAs(UnmanagedType.LPWStr)> lpReplacedFileName As String, <MarshalAs(UnmanagedType.LPWStr)> lpReplacementName As String, <MarshalAs(UnmanagedType.LPWStr)> lpBackupFileName As String, dwReplaceFlags As ReplaceFileFlags, lpExclude As IntPtr, lpReserved As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean


        Public Declare Unicode Function SHBrowseForFolder Lib "Shell32" (bInfo As BROWSEINFO) As Integer

        Public Declare Unicode Function SHGetSpecialFolderPath Lib "Shell32" Alias "SHGetSpecialFolderPathW" (hWnd As IntPtr, lpPath As String, nFolder As Integer, fCreate As Boolean) As Integer
        Public Declare Unicode Function SHGetPathFromIDList Lib "Shell32" (pidl As Integer, <MarshalAs(UnmanagedType.LPWStr)> pszPath As String) As Integer

        Public Declare Unicode Function ShellExecuteEx Lib "shell32.dll" Alias "ShellExecuteExW" (ByRef lpInfo As SHELLEXECUTEINFO) As Boolean
        Public Declare Unicode Function ShellExecuteEx Lib "shell32.dll" Alias "ShellExecuteExW" (ByRef lpInfo As SHELLEXECUTEINFOPTR) As Boolean
        Public Declare Unicode Function SHGetFileInfo Lib "shell32.dll" Alias "SHGetFileInfoW" (<MarshalAs(UnmanagedType.LPWStr)> pszPath As String, dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, cbFileInfo As Integer, uFlags As Integer) As Integer
        Public Declare Unicode Function SHGetItemInfo Lib "shell32.dll" Alias "SHGetFileInfoW" (pidl As IntPtr, dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, cbFileInfo As Integer, uFlags As Integer) As IntPtr


        Public Declare Unicode Function SHFileOperation Lib "shell32.dll" Alias "SHFileOperationW" (lpFileOp As SHFILEOPSTRUCT) As Integer

        Public Declare Unicode Function SHGetDataFromIDList Lib "shell32.dll" Alias "SHGetDataFromIDListW" (<MarshalAs(UnmanagedType.Interface)> lpFolder As Object, pidl As Integer, nFormat As Integer, pv As IntPtr, cb As Integer) As Integer

        Public Declare Unicode Function FileIconInit Lib "shell32.dll" Alias "#660" (<MarshalAs(UnmanagedType.Bool)> fRestoreCache As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public mBrowseCurrent As String

        Public BrowserLastFolder As Integer
        Public Declare Unicode Function CopyImage Lib "user32" (Handle As IntPtr, un1 As Integer, n1 As Integer, n2 As Integer, un2 As Integer) As Integer

        Public Declare Unicode Function ImageList_GetImageInfo Lib "comctl32.dll" (himl As IntPtr, i As Integer, ByRef pImageInfo As IMAGEINFO) As Integer
        Public Declare Unicode Function ImageList_GetIcon Lib "comctl32.dll" (himl As IntPtr, i As Integer, flags As UInteger) As Integer

        Public Declare Unicode Function SHGetImageList Lib "shell32.dll" (iImageList As Integer, ByRef riid As Guid, ByRef hIml As IntPtr) As Integer

        Public Declare Unicode Function DestroyIcon Lib "user32.dll" (hIcon As IntPtr) As Integer

#End Region

#Region "Windowing"

        Delegate Function WndProcDelegate(hwnd As IntPtr, uMsg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr

        <DllImport("user32.dll", CharSet:=CharSet.Unicode, EntryPoint:="SetWindowLongPtrW")>
        Public Function SetWindowLongPtr(hWnd As System.IntPtr, code As Integer, value As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Unicode, EntryPoint:="SetWindowLongPtrW")>
        Public Function SetWindowLongPtr(hWnd As System.IntPtr, code As Integer, value As WndProcDelegate) As WndProcDelegate
        End Function

        Public Declare Unicode Function GetWindowRect Lib "user32" (hWnd As System.IntPtr, ByRef rc As RECT) As Boolean

        Public Declare Unicode Function GetWindowLong Lib "user32" Alias "GetWindowLongW" (hWnd As System.IntPtr, code As Integer) As IntPtr

        Public Declare Unicode Function GetWindowLongPtr Lib "user32" Alias "GetWindowLongPtrW" (hWnd As System.IntPtr, code As Integer) As IntPtr

        Public Declare Unicode Function CreateWindowEx Lib "user32" Alias "CreateWindowExW" _
        (dwExStyle As UInteger,
         <MarshalAs(UnmanagedType.LPWStr)> lpClassName As String,
         <MarshalAs(UnmanagedType.LPWStr)> lpWindowName As String,
         dwStyle As UInteger,
         x As Integer,
         y As Integer,
         nWidth As Integer,
         nHeight As Integer,
         hWndParent As IntPtr,
         hMenu As IntPtr,
         hInstance As IntPtr,
         lpParam As IntPtr) As IntPtr

        Public Declare Unicode Function DestroyWindow Lib "user32" (hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Unicode Function GetSysColor Lib "user32" (nIndex As Integer) As Integer

        Public Declare Function GetWindowThreadProcessId Lib "user32" (hwnd As IntPtr, Optional ByRef lpdwProcessId As Integer = Nothing) As Integer


        Public Const DF_ALLOWOTHERACCOUNTHOOK As Integer = 1


        Public Enum DESKTOP_ACCESS_MASK

            ''' <summary>
            ''' Required to delete the object.
            ''' </summary>
            <Description("Required to delete the object.")>
            DELETE = &H10000
            ''' <summary>
            ''' Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            ''' </summary>
            <Description("Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.")>
            READ_CONTROL = &H20000
            ''' <summary>
            ''' Not supported for desktop objects.
            ''' </summary>
            <Description("Not supported for desktop objects.")>
            SYNCHRONIZE = &H100000
            ''' <summary>
            ''' Required to modify the DACL in the security descriptor for the object.
            ''' </summary>
            <Description("Required to modify the DACL in the security descriptor for the object.")>
            WRITE_DAC = &H40000

            ''' <summary>
            ''' Required to change the owner in the security descriptor for the object.
            ''' </summary>
            <Description("Required to change the owner in the security descriptor for the object.")>
            WRITE_OWNER = &H80000

            'The following table lists the object-specific access rights.
            'Access right	Description

            ''' <summary>
            ''' Required to create a menu on the desktop.
            ''' </summary>
            <Description("Required to create a menu on the desktop.")>
            DESKTOP_CREATEMENU = &H4

            ''' <summary>
            ''' Required to create a window on the desktop.
            ''' </summary>
            <Description("Required to create a window on the desktop.")>
            DESKTOP_CREATEWINDOW = &H2

            ''' <summary>
            ''' Required for the desktop to be enumerated.
            ''' </summary>
            <Description("Required for the desktop to be enumerated.")>
            DESKTOP_ENUMERATE = &H40

            ''' <summary>
            ''' Required to establish any of the window hooks.
            ''' </summary>
            <Description("Required to establish any of the window hooks.")>
            DESKTOP_HOOKCONTROL = &H8

            ''' <summary>
            ''' Required to perform journal playback on a desktop.
            ''' </summary>
            <Description("Required to perform journal playback on a desktop.")>
            DESKTOP_JOURNALPLAYBACK = &H20

            ''' <summary>
            ''' Required to perform journal recording on a desktop.
            ''' </summary>
            <Description("Required to perform journal recording on a desktop.")>
            DESKTOP_JOURNALRECORD = &H10

            ''' <summary>
            ''' Required to read objects on the desktop.
            ''' </summary>
            <Description("Required to read objects on the desktop.")>
            DESKTOP_READOBJECTS = &H1

            ''' <summary>
            ''' Required to activate the desktop using the SwitchDesktop function.
            ''' </summary>
            <Description("Required to activate the desktop using the SwitchDesktop function.")>
            DESKTOP_SWITCHDESKTOP = &H100

            ''' <summary>
            ''' Required to write objects on the desktop.
            ''' </summary>
            <Description("Required to write objects on the desktop.")>
            DESKTOP_WRITEOBJECTS = &H80

            'The following are the generic access rights for a desktop object contained in the interactive window station of the user's logon session.
            'Access right	Description

            GENERIC_READ = DESKTOP_ACCESS_MASK.DESKTOP_ENUMERATE Or
                       DESKTOP_ACCESS_MASK.DESKTOP_READOBJECTS Or
                       STANDARD_RIGHTS_READ

            GENERIC_WRITE = DESKTOP_ACCESS_MASK.DESKTOP_CREATEMENU Or
                        DESKTOP_ACCESS_MASK.DESKTOP_CREATEWINDOW Or
                        DESKTOP_ACCESS_MASK.DESKTOP_HOOKCONTROL Or
                        DESKTOP_ACCESS_MASK.DESKTOP_JOURNALPLAYBACK Or
                        DESKTOP_ACCESS_MASK.DESKTOP_JOURNALRECORD Or
                        DESKTOP_ACCESS_MASK.DESKTOP_WRITEOBJECTS Or
                        STANDARD_RIGHTS_WRITE

            GENERIC_EXECUTE = DESKTOP_ACCESS_MASK.DESKTOP_SWITCHDESKTOP Or
                          STANDARD_RIGHTS_EXECUTE

            GENERIC_ALL = DESKTOP_ACCESS_MASK.DESKTOP_CREATEMENU Or
                      DESKTOP_ACCESS_MASK.DESKTOP_CREATEWINDOW Or
                      DESKTOP_ACCESS_MASK.DESKTOP_ENUMERATE Or
                      DESKTOP_ACCESS_MASK.DESKTOP_HOOKCONTROL Or
                      DESKTOP_ACCESS_MASK.DESKTOP_JOURNALPLAYBACK Or
                      DESKTOP_ACCESS_MASK.DESKTOP_JOURNALRECORD Or
                      DESKTOP_ACCESS_MASK.DESKTOP_READOBJECTS Or
                      DESKTOP_ACCESS_MASK.DESKTOP_SWITCHDESKTOP Or
                      DESKTOP_ACCESS_MASK.DESKTOP_WRITEOBJECTS Or
                      STANDARD_RIGHTS_REQUIRED

        End Enum

        ''' <summary>
        ''' Opens the Input Desktop.
        ''' </summary>
        ''' <param name="dwFlags">Optional Flags</param>
        ''' <param name="fInherit">If this is set, all child processes will inherit this handle.</param>
        ''' <param name="dwDesiredAccess">Desktop access mask.</param>
        ''' <returns></returns>
        Public Declare Function OpenInputDesktop Lib "user32" (dwFlags As Integer,
                                                           <MarshalAs(UnmanagedType.Bool)> fInherit As Boolean,
                                                           dwDesiredAccess As DESKTOP_ACCESS_MASK) As IntPtr


        Public Declare Function CloseDesktop Lib "user32" (hDesk As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Declare Function GetThreadDesktop Lib "user32" (dwThreadId As Integer) As IntPtr

        Public Delegate Function EnumWindowsProc(hwnd As IntPtr, lParam As IntPtr) As Boolean

        Public Declare Unicode Function EnumDesktopWindows Lib "user32" (hDesk As IntPtr,
                                                                     <MarshalAs(UnmanagedType.FunctionPtr)>
                                                                     lpfn As EnumWindowsProc,
                                                                     lParam As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

        ''' <summary>
        ''' Returns an array of all current top-level windows HWND pointers on the current desktop.
        ''' </summary>
        ''' <returns>Array of HWNDs as IntPtr</returns>
        Public Function GetCurrentDesktopWindows() As IntPtr()
            Return GetDesktopWindows(IntPtr.Zero)
        End Function

        ''' <summary>
        ''' Returns an array of all current top-level windows HWND pointers on the current desktop.
        ''' </summary>
        ''' <returns>Array of HWNDs as IntPtr</returns>
        Public Function GetDesktopWindows(hDesk As IntPtr) As IntPtr()

            Dim l As New List(Of IntPtr)

            EnumDesktopWindows(hDesk, New EnumWindowsProc(Function(hwnd As IntPtr, lParam As IntPtr) As Boolean
                                                              l.Add(hwnd)
                                                              Return True
                                                          End Function), IntPtr.Zero)


            Return l.ToArray
        End Function

        Public Function GetDesktopProcesses(Optional hDesk As IntPtr = Nothing) As IEnumerable(Of Process)

            Dim l As New List(Of Process)
            Dim hwnd() As IntPtr = GetDesktopWindows(hDesk)
            Dim pid As Integer = 0

            For Each h In hwnd
                GetWindowThreadProcessId(h, pid)
                l.Add(Process.GetProcessById(pid))
            Next

            l.Sort(New Comparison(Of Process)(Function(x As Process, y As Process) As Integer
                                                  Try
                                                      Return String.Compare(x.ProcessName, y.ProcessName)
                                                  Catch ex As Exception
                                                      Return 0
                                                  End Try
                                              End Function))

            Dim t As New List(Of Process)
            Dim c As Process = Nothing

            For Each p In l

                If c Is Nothing OrElse c.Id <> p.Id Then
                    c = p
                    t.Add(c)
                End If
            Next


            Return t
        End Function






#End Region

#Region "Menu System"

        Public Declare Unicode Function GetMessage Lib "user32" Alias "GetMessageW" (<MarshalAs(UnmanagedType.LPStruct)> ByRef lpMsg As MSG, hWnd As IntPtr, wMsgFilterMin As Integer, wMsgFilterMax As Integer) As Integer
        Public Declare Unicode Function PeekMessage Lib "user32" Alias "PeekMessageW" (<MarshalAs(UnmanagedType.LPStruct)> ByRef lpMsg As MSG, hWnd As IntPtr, wMsgFilterMin As Integer, wMsgFilterMax As Integer, wRemoveMsg As Integer) As Integer

        Public Declare Unicode Function TranslateMessage Lib "user32" (<MarshalAs(UnmanagedType.LPStruct)> ByRef lpMsg As MSG) As Integer
        Public Declare Unicode Function GetKeyState Lib "user32" (nVirtKey As Integer) As Integer

        Public Declare Unicode Function CopyAcceleratorTable Lib "user32" Alias "CopyAcceleratorTableW" (hAccelSrc As IntPtr, lpAccelDst As IntPtr, cAccelEntries As Integer) As Integer
        Public Declare Unicode Function CreateAcceleratorTable Lib "user32" Alias "CreateAcceleratorTableW" (lpaccl As IntPtr, cEntries As Integer) As Integer
        Public Declare Unicode Function TranslateAccelerator Lib "user32" Alias "TranslateAcceleratorW" (hWnd As IntPtr, hAccTable As IntPtr, <MarshalAs(UnmanagedType.LPStruct)> ByRef lpMsg As MSG) As Integer
        Public Declare Unicode Function DestroyAcceleratorTable Lib "user32" (haccel As IntPtr) As Integer

        Public Declare Unicode Function TrackPopupMenu Lib "user32" (Handle As IntPtr, wFlags As UInteger, x As Short, y As Short, nReserved As Short, hWnd As IntPtr, lprc As IntPtr) As Integer
        Public Declare Unicode Function TrackPopupMenuEx Lib "user32" (Handle As IntPtr, wFlags As UInteger, x As Integer, y As Integer, hWnd As IntPtr, lpTPMParams As IntPtr) As IntPtr

        Public Declare Unicode Function DrawMenuBar Lib "user32" (hWnd As IntPtr) As Integer

        Public Declare Unicode Function CreatePopupMenu Lib "user32" () As IntPtr
        Public Declare Unicode Function CreateMenu Lib "user32" () As IntPtr
        Public Declare Unicode Function DeleteMenu Lib "user32" (Handle As IntPtr, nPosition As Integer, wFlags As Integer) As Integer

        Public Declare Unicode Function InsertMenuItem Lib "user32" Alias "InsertMenuItemW" (Handle As IntPtr, un As Integer, bool As Boolean, <MarshalAs(UnmanagedType.Struct)> ByRef lpcMenuItemInfo As MENUITEMINFO) As Integer

        Public Declare Unicode Function AppendMenu Lib "user32" Alias "AppendMenuW" (Handle As IntPtr, wFlags As Integer, wIDNewItem As Integer, lpNewItem As IntPtr) As Integer

        Public Declare Unicode Function InsertMenu Lib "user32" Alias "InsertMenuW" (Handle As IntPtr, nPosition As Integer, wFlags As Integer, wIDNewItem As Integer, lpNewItem As IntPtr) As Integer

        Public Declare Unicode Function RemoveMenu Lib "user32" (Handle As IntPtr, nPosition As Integer, wFlags As Integer) As Integer
        Public Declare Unicode Function DestroyMenu Lib "user32" (Handle As IntPtr) As Integer

        Public Declare Unicode Function GetSystemMenu Lib "user32" (hWnd As Integer, bRevert As Integer) As Integer

        Public Declare Unicode Function GetMenuItemCount Lib "user32" (Handle As IntPtr) As Integer
        Public Declare Unicode Function GetMenuItemID Lib "user32" (Handle As IntPtr, nPos As Integer) As Integer
        Public Declare Unicode Function GetMenuDefaultItem Lib "user32" (Handle As IntPtr, <MarshalAs(UnmanagedType.Bool)> fByPos As Boolean, gmdiFlags As Integer) As Integer
        Public Declare Unicode Function GetMenuContextHelpId Lib "user32" (Handle As IntPtr) As Integer
        Public Declare Unicode Function GetMenuCheckMarkDimensions Lib "user32" () As Integer

        Public Declare Unicode Function GetMenu Lib "user32" (hWnd As Integer) As Integer

        Public Declare Unicode Function GetMenuItemInfo Lib "user32" Alias "GetMenuItemInfoW" (Handle As IntPtr, un As Integer, <MarshalAs(UnmanagedType.Bool)> fByItem As Boolean, ByRef lpMenuItemInfo As MENUITEMINFO) As Integer

        Public Declare Unicode Function GetMenuInfo Lib "user32" (hMenu As IntPtr, lpcmi As MENUINFO) As Integer
        Public Declare Unicode Function GetMenuBarInfo Lib "user32" (hWnd As IntPtr, idObject As Integer, idItem As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef pmbi As MENUBARINFO) As Integer

        Public Declare Unicode Function GetMenuItemRect Lib "user32" (hWnd As IntPtr, Handle As IntPtr, uItem As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef lprcItem As RECT) As Integer
        Public Declare Unicode Function GetMenuState Lib "user32" (Handle As IntPtr, wID As Integer, wFlags As Integer) As Integer

        Public Declare Unicode Function GetMenuString Lib "user32" Alias "GetMenuStringW" (Handle As IntPtr, wIDItem As Integer, <MarshalAs(UnmanagedType.LPWStr)> lpString As String, nMaxCount As Integer, wFlag As Integer) As Integer

        Public Declare Unicode Function SetMenuInfo Lib "user32" (hMenu As IntPtr, <MarshalAs(UnmanagedType.LPStruct)> lpcmi As MENUINFO) As Integer

        Public Declare Unicode Function SetMenu Lib "user32" (hWnd As IntPtr, Handle As IntPtr) As Integer
        Public Declare Unicode Function SetMenuContextHelpId Lib "user32" (Handle As IntPtr, dw As Integer) As Integer
        Public Declare Unicode Function SetMenuDefaultItem Lib "user32" (Handle As IntPtr, uItem As Integer, fByPos As Integer) As Integer
        Public Declare Unicode Function SetMenuItemBitmaps Lib "user32" (Handle As IntPtr, nPosition As Integer, wFlags As Integer, hBitmapUnchecked As Integer, hBitmapChecked As Integer) As Integer
        Public Declare Unicode Function EnableMenuItem Lib "user32" (hMenu As IntPtr, wIDEnableItem As Integer, wEnable As Integer) As Integer

        Public Declare Unicode Function SetMenuItemInfo Lib "user32" Alias "SetMenuItemInfoW" (Handle As IntPtr, un As Integer, <MarshalAs(UnmanagedType.Bool)> fByItem As Boolean, ByRef lpcMenuItemInfo As MENUITEMINFO) As Integer

#End Region

#Region "Scrollbars"
        Public Declare Unicode Function SetScrollInfo Lib "user32" (hWnd As System.IntPtr, n As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef lpcScrollInfo As SCROLLINFO, bool As Boolean) As Integer
        Public Declare Unicode Function SetScrollPos Lib "user32" (hWnd As System.IntPtr, nBar As Integer, nPos As Integer, bRedraw As Integer) As Integer
        Public Declare Unicode Function SetScrollRange Lib "user32" (hWnd As System.IntPtr, nBar As Integer, nMinPos As Integer, nMaxPos As Integer, bRedraw As Integer) As Integer
        Public Declare Unicode Function GetScrollInfo Lib "user32" (hWnd As System.IntPtr, n As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef lpScrollInfo As SCROLLINFO) As Integer
        Public Declare Unicode Function GetScrollPos Lib "user32" (hWnd As System.IntPtr, nBar As Integer) As Integer
        Public Declare Unicode Function GetScrollRange Lib "user32" (hWnd As System.IntPtr, nBar As Integer, ByRef lpMinPos As Integer, ByRef lpMaxPos As Integer) As Integer
        Public Declare Unicode Function EnableScrollBar Lib "user32" (hWnd As System.IntPtr, wSBflags As Integer, wArrows As Integer) As Integer
        Public Declare Unicode Function ShowScrollBar Lib "user32" (hWnd As System.IntPtr, wBar As Integer, bShow As Integer) As Integer
#End Region

#Region "GDI Objects"
        Public Declare Unicode Function SelectObject Lib "gdi32" (hDC As System.IntPtr, hObject As IntPtr) As Integer
        Public Declare Unicode Function GetCurrentObject Lib "gdi32" (hDC As System.IntPtr, uObjectType As Integer) As Integer
        Public Declare Unicode Function GetStockObject Lib "gdi32" (nIndex As Integer) As Integer

        '' Pen
        Public Declare Unicode Function GetObject Lib "gdi32" (hObject As IntPtr, nCount As Integer, <MarshalAs(UnmanagedType.Struct)> lpObject As LOGPEN) As Integer

        '' Brush
        Public Declare Unicode Function GetObject Lib "gdi32" (hObject As IntPtr, nCount As Integer, <MarshalAs(UnmanagedType.Struct)> lpObject As LOGBRUSH) As Integer

        Public Declare Unicode Function ExtCreatePen Lib "gdi32" (dwPenStyle As Integer, dwWidth As Integer, <MarshalAs(UnmanagedType.Struct)> lplb As LOGBRUSH, dwStyleCount As Integer, lpStyle As Integer) As Integer

        Public Declare Unicode Function CreatePenIndirect Lib "gdi32" (<MarshalAs(UnmanagedType.Struct)> lpLogPen As LOGPEN) As Integer
        Public Declare Unicode Function CreatePen Lib "gdi32" (nPenStyle As Integer, nWidth As Integer, crColor As Integer) As Integer

        Public Declare Unicode Function CreateBrushIndirect Lib "gdi32" (<MarshalAs(UnmanagedType.Struct)> lpLogBrush As LOGBRUSH) As Integer

        Public Declare Unicode Function CreateIconIndirect Lib "user32" (<MarshalAs(UnmanagedType.Struct)> ByRef lpIconInfo As ICONINFO) As IntPtr
        Public Declare Unicode Function CreateBitmapIndirect Lib "gdi32" (<MarshalAs(UnmanagedType.Struct)> ByRef lpBitmap As BITMAPSTRUCT) As IntPtr

        '    HDC CreateDC(
        '  LPCTSTR lpszDriver,
        '  _In_  LPCTSTR lpszDevice,
        '  LPCTSTR lpszOutput,
        '  _In_  const DEVMODE *lpInitData
        ');

        Public Declare Function DeleteDC Lib "gdi32" (hdc As IntPtr) As Boolean
        Public Declare Unicode Function CreateDC Lib "gdi32" Alias "CreateDCW" (<MarshalAs(UnmanagedType.LPWStr)> lpszDriver As String,
                                                                              <MarshalAs(UnmanagedType.LPWStr)> lpszDevice As String,
                                                                              lpszOutput As IntPtr,
                                                                              devMode As IntPtr) As IntPtr

        Public Declare Function GetDeviceCaps Lib "gdi32" (hDc As IntPtr, index As Integer) As Integer

#End Region

#Region "GDI Paint"

        Public Declare Unicode Function MoveToEx Lib "gdi32" (hDC As System.IntPtr, nXOrg As Integer, nYOrg As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lppt As POINT) As Integer
        Public Declare Unicode Function LineTo Lib "gdi32" (hDC As System.IntPtr, x As Integer, y As Integer) As Integer

        Public Declare Unicode Function GdiFlush Lib "gdi32" () As Integer

        Public Declare Unicode Function FillRect Lib "user32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef lpRect As RECT, hBrush As Integer) As Integer

        Public Declare Unicode Function BeginPaint Lib "user32" (hWnd As System.IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef lpPaint As PAINTSTRUCT) As Integer
        Public Declare Unicode Function EndPaint Lib "user32" (hWnd As System.IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef lpPaint As PAINTSTRUCT) As Integer

        Public Declare Unicode Function SetTextAlign Lib "gdi32" (hDC As System.IntPtr, wFlags As Integer) As Integer
        Public Declare Unicode Function GetTextAlign Lib "gdi32" (hDC As System.IntPtr) As Integer

        Public Declare Unicode Function SetBkMode Lib "gdi32" (hDC As System.IntPtr, nMode As Integer) As Integer

        Public Declare Unicode Function DrawTextW Lib "user32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpStr As String, nCount As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpRect As RECT, wFormat As Integer) As Integer
        Public Declare Unicode Function TextOutW Lib "gdi32" (hDC As System.IntPtr, x As Integer, y As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpString As String, nCount As Integer) As Integer

        Public Declare Unicode Function ExtTextOutW Lib "gdi32" (hDC As System.IntPtr, x As Integer, y As Integer, wOptions As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpRect As RECT, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpString As String, nCount As Integer, lpDx As Integer) As Integer
        Public Declare Unicode Function GetTextExtentPoint32W Lib "gdi32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpsz As String, cbString As Integer, <MarshalAs(UnmanagedType.LPStruct)> ByRef lpSize As SIZE) As Integer

        Public Declare Ansi Function DrawTextA Lib "user32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByRef lpStr As String, nCount As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpRect As RECT, wFormat As Integer) As Integer
        Public Declare Ansi Function TextOutA Lib "gdi32" (hDC As System.IntPtr, x As Integer, y As Integer, <MarshalAs(UnmanagedType.LPStr)> ByRef lpString As String, nCount As Integer) As Integer

        Public Declare Ansi Function ExtTextOutA Lib "gdi32" (hDC As System.IntPtr, x As Integer, y As Integer, wOptions As Integer, ByRef lpRect As RECT, <MarshalAs(UnmanagedType.LPStr)> ByRef lpString As String, nCount As Integer, lpDx As Integer) As Integer
        Public Declare Ansi Function GetTextExtentPoint32A Lib "gdi32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByRef lpsz As String, cbString As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpSize As SIZE) As Integer

        Public Declare Unicode Function GetFontLanguageInfo Lib "gdi32" (hDC As System.IntPtr) As Integer
        Public Declare Unicode Function SetTextColor Lib "gdi32" (hDC As System.IntPtr, crColor As Integer) As Integer

        Public Declare Unicode Function DrawFrameControl Lib "user32" (hDC As System.IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef lpRect As RECT, un1 As Integer, un2 As Integer) As Integer
        '' draw caption text

        Public Declare Unicode Function DrawCaption Lib "user32" (hWnd As IntPtr, hDC As IntPtr, <MarshalAs(UnmanagedType.Struct)> ByRef pcRect As RECT, un As Integer) As Integer
        Public Declare Unicode Function BitBlt Lib "gdi32" (dest As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer, src As IntPtr, x As Integer, y As Integer, dwROP As Integer) As Integer

        Public Declare Unicode Function CreateDIBSection Lib "gdi32" (hdc As IntPtr, pbmi As IntPtr, usage As UInteger, ByRef ppvBits As IntPtr, hSection As IntPtr, offset As Integer) As IntPtr


        Public Structure RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer

            Public Sub New(l As Integer, t As Integer, r As Integer, b As Integer)
                left = l
                top = t
                right = r
                bottom = b
            End Sub
        End Structure

        Public Structure POINT
            Public x As Integer
            Public y As Integer
        End Structure

        Public Structure SIZE
            Public cx As Integer
            Public cy As Integer
        End Structure

#End Region

#Region "Fonts"

        '        int CALLBACK EnumFontFamExProc(
        '   Const LOGFONT    *lpelfe,
        '   Const TEXTMETRIC *lpntme,
        '         DWORD      FontType,
        '         LPARAM     lParam
        ');




#End Region

#Region "Zero Memory"

        Declare Sub RtlZeroMemory Lib "kernel32" Alias "RtlZeroMemory" (pDst As IntPtr,
                                                                             ByteLen As Long)

#End Region

#End Region

    End Module



End Namespace