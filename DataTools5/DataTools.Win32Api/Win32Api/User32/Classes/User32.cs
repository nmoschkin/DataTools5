// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Native
//         Myriad Windows API Declares
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    /// <summary>
    /// A large cross-section of pInvoke-related items
    /// </summary>
    /// <remarks>This class is not well documented.</remarks>
    public static class User32
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int WM_NULL = 0x0;
        public const int WM_CREATE = 0x1;
        public const int WM_DESTROY = 0x2;
        public const int WM_MOVE = 0x3;
        public const int WM_SIZE = 0x5;
        public const int WM_ACTIVATE = 0x6;
        public const int WA_INACTIVE = 0;
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;
        public const int WM_SETFOCUS = 0x7;
        public const int WM_KILLFOCUS = 0x8;
        public const int WM_ENABLE = 0xA;
        public const int WM_SETREDRAW = 0xB;
        public const int WM_SETTEXT = 0xC;
        public const int WM_GETTEXT = 0xD;
        public const int WM_GETTEXTLENGTH = 0xE;
        public const int WM_PAINT = 0xF;
        public const int WM_CLOSE = 0x10;
        public const int WM_QUERYENDSESSION = 0x11;
        public const int WM_QUERYOPEN = 0x13;
        public const int WM_ENDSESSION = 0x16;
        public const int WM_QUIT = 0x12;
        public const int WM_ERASEBKGND = 0x14;
        public const int WM_SYSCOLORCHANGE = 0x15;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_WININICHANGE = 0x1A;
        public const int WM_SETTINGCHANGE = WM_WININICHANGE;
        public const int WM_DEVMODECHANGE = 0x1B;
        public const int WM_ACTIVATEAPP = 0x1C;
        public const int WM_FONTCHANGE = 0x1D;
        public const int WM_TIMECHANGE = 0x1E;
        public const int WM_CANCELMODE = 0x1F;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_CHILDACTIVATE = 0x22;
        public const int WM_QUEUESYNC = 0x23;
        public const int WM_GETMINMAXINFO = 0x24;
        public const int WM_PAINTICON = 0x26;
        public const int WM_ICONERASEBKGND = 0x27;
        public const int WM_NEXTDLGCTL = 0x28;
        public const int WM_SPOOLERSTATUS = 0x2A;
        public const int WM_DRAWITEM = 0x2B;
        public const int WM_MEASUREITEM = 0x2C;
        public const int WM_DELETEITEM = 0x2D;
        public const int WM_VKEYTOITEM = 0x2E;
        public const int WM_CHARTOITEM = 0x2F;
        public const int WM_SETFONT = 0x30;
        public const int WM_GETFONT = 0x31;
        public const int WM_SETHOTKEY = 0x32;
        public const int WM_GETHOTKEY = 0x33;
        public const int WM_QUERYDRAGICON = 0x37;
        public const int WM_COMPAREITEM = 0x39;
        public const int WM_GETOBJECT = 0x3D;
        public const int WM_COMPACTING = 0x41;
        public const int WM_COMMNOTIFY = 0x44;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_WINDOWPOSCHANGED = 0x47;
        public const int WM_POWER = 0x48;
        public const int PWR_OK = 1;
        public const int PWR_FAIL = -1;
        public const int PWR_SUSPENDREQUEST = 1;
        public const int PWR_SUSPENDRESUME = 2;
        public const int PWR_CRITICALRESUME = 3;
        public const int WM_COPYDATA = 0x4A;
        public const int WM_CANCELJOURNAL = 0x4B;
        public const int WM_NOTIFY = 0x4E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x50;
        public const int WM_INPUTLANGCHANGE = 0x51;
        public const int WM_TCARD = 0x52;
        public const int WM_HELP = 0x53;
        public const int WM_USERCHANGED = 0x54;
        public const int WM_NOTIFYFORMAT = 0x55;
        public const int NFR_ANSI = 1;
        public const int NFR_UNICODE = 2;
        public const int NF_QUERY = 3;
        public const int NF_REQUERY = 4;
        public const int WM_CONTEXTMENU = 0x7B;
        public const int WM_STYLECHANGING = 0x7C;
        public const int WM_STYLECHANGED = 0x7D;
        public const int WM_DISPLAYCHANGE = 0x7E;
        public const int WM_GETICON = 0x7F;
        public const int WM_SETICON = 0x80;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCDESTROY = 0x82;
        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCPAINT = 0x85;
        public const int WM_NCACTIVATE = 0x86;
        public const int WM_GETDLGCODE = 0x87;
        public const int WM_SYNCPAINT = 0x88;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCLBUTTONDBLCLK = 0xA3;
        public const int WM_NCRBUTTONDOWN = 0xA4;
        public const int WM_NCRBUTTONUP = 0xA5;
        public const int WM_NCRBUTTONDBLCLK = 0xA6;
        public const int WM_NCMBUTTONDOWN = 0xA7;
        public const int WM_NCMBUTTONUP = 0xA8;
        public const int WM_NCMBUTTONDBLCLK = 0xA9;
        public const int WM_NCXBUTTONDOWN = 0xAB;
        public const int WM_NCXBUTTONUP = 0xAC;
        public const int WM_NCXBUTTONDBLCLK = 0xAD;
        public const int WM_INPUT_DEVICE_CHANGE = 0xFE;
        public const int WM_INPUT = 0xFF;
        public const int WM_KEYFIRST = 0x100;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x102;
        public const int WM_DEADCHAR = 0x103;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WM_SYSCHAR = 0x106;
        public const int WM_SYSDEADCHAR = 0x107;
        public const int WM_UNICHAR = 0x109;
        public const int WM_KEYLAST = 0x109;
        public const int UNICODE_NOCHAR = 0xFFFF;
        public const int WM_IME_STARTCOMPOSITION = 0x10D;
        public const int WM_IME_ENDCOMPOSITION = 0x10E;
        public const int WM_IME_COMPOSITION = 0x10F;
        public const int WM_IME_KEYLAST = 0x10F;
        public const int WM_INITDIALOG = 0x110;
        public const int WM_COMMAND = 0x111;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_TIMER = 0x113;
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;
        public const int WM_INITMENU = 0x116;
        public const int WM_INITMENUPOPUP = 0x117;
        public const int WM_GESTURE = 0x119;
        public const int WM_GESTURENOTIFY = 0x11A;
        public const int WM_MENUSELECT = 0x11F;
        public const int WM_MENUCHAR = 0x120;
        public const int WM_ENTERIDLE = 0x121;
        public const int WM_MENURBUTTONUP = 0x122;
        public const int WM_MENUDRAG = 0x123;
        public const int WM_MENUGETOBJECT = 0x124;
        public const int WM_UNINITMENUPOPUP = 0x125;
        public const int WM_MENUCOMMAND = 0x126;
        public const int WM_CHANGEUISTATE = 0x127;
        public const int WM_UPDATEUISTATE = 0x128;
        public const int WM_QUERYUISTATE = 0x129;
        public const int UIS_SET = 1;
        public const int UIS_CLEAR = 2;
        public const int UIS_INITIALIZE = 3;
        public const int UISF_HIDEFOCUS = 0x1;
        public const int UISF_HIDEACCEL = 0x2;
        public const int UISF_ACTIVE = 0x4;
        public const int WM_CTLCOLORMSGBOX = 0x132;
        public const int WM_CTLCOLOREDIT = 0x133;
        public const int WM_CTLCOLORLISTBOX = 0x134;
        public const int WM_CTLCOLORBTN = 0x135;
        public const int WM_CTLCOLORDLG = 0x136;
        public const int WM_CTLCOLORSCROLLBAR = 0x137;
        public const int WM_CTLCOLORSTATIC = 0x138;
        public const int MN_GETHMENU = 0x1E1;
        public const int WM_MOUSEFIRST = 0x200;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_XBUTTONDOWN = 0x20B;
        public const int WM_XBUTTONUP = 0x20C;
        public const int WM_XBUTTONDBLCLK = 0x20D;
        public const int WM_MOUSEHWHEEL = 0x20E;
        public const int WM_MOUSELAST = 0x20E;
        public const int WHEEL_DELTA = 120;

        public static short GET_WHEEL_DELTA_WPARAM(IntPtr wParam)
        {
            return (short)(wParam.ToInt32() >> 16);
        }

        public const uint WHEEL_PAGESCROLL = uint.MaxValue;

        public static short GET_KEYSTATE_WPARAM(IntPtr wParam)
        {
            return (short)(wParam.ToInt32() & 0xFFFF);
        }

        public static short GET_NCHITTEST_WPARAM(IntPtr wParam)
        {
            return (short)(wParam.ToInt32() & 0xFFFF);
        }

        public static short GET_XBUTTON_WPARAM(IntPtr wParam)
        {
            return (short)(wParam.ToInt32() >> 16);
        }

        public const int XBUTTON1 = 0x1;
        public const int XBUTTON2 = 0x2;
        public const int WM_PARENTNOTIFY = 0x210;
        public const int WM_ENTERMENULOOP = 0x211;
        public const int WM_EXITMENULOOP = 0x212;
        public const int WM_NEXTMENU = 0x213;
        public const int WM_SIZING = 0x214;
        public const int WM_CAPTURECHANGED = 0x215;
        public const int WM_MOVING = 0x216;
        public const int WM_POWERBROADCAST = 0x218;
        public const int PBT_APMQUERYSUSPEND = 0x0;
        public const int PBT_APMQUERYSTANDBY = 0x1;
        public const int PBT_APMQUERYSUSPENDFAILED = 0x2;
        public const int PBT_APMQUERYSTANDBYFAILED = 0x3;
        public const int PBT_APMSUSPEND = 0x4;
        public const int PBT_APMSTANDBY = 0x5;
        public const int PBT_APMRESUMECRITICAL = 0x6;
        public const int PBT_APMRESUMESUSPEND = 0x7;
        public const int PBT_APMRESUMESTANDBY = 0x8;
        public const int PBTF_APMRESUMEFROMFAILURE = 0x1;
        public const int PBT_APMBATTERYLOW = 0x9;
        public const int PBT_APMPOWERSTATUSCHANGE = 0xA;
        public const int PBT_APMOEMEVENT = 0xB;
        public const int PBT_APMRESUMEAUTOMATIC = 0x12;
        public const int PBT_POWERSETTINGCHANGE = 0x8013;

        public const int WM_MDICREATE = 0x220;
        public const int WM_MDIDESTROY = 0x221;
        public const int WM_MDIACTIVATE = 0x222;
        public const int WM_MDIRESTORE = 0x223;
        public const int WM_MDINEXT = 0x224;
        public const int WM_MDIMAXIMIZE = 0x225;
        public const int WM_MDITILE = 0x226;
        public const int WM_MDICASCADE = 0x227;
        public const int WM_MDIICONARRANGE = 0x228;
        public const int WM_MDIGETACTIVE = 0x229;
        public const int WM_MDISETMENU = 0x230;
        public const int WM_ENTERSIZEMOVE = 0x231;
        public const int WM_EXITSIZEMOVE = 0x232;
        public const int WM_DROPFILES = 0x233;
        public const int WM_MDIREFRESHMENU = 0x234;
        public const int WM_POINTERDEVICECHANGE = 0x238;
        public const int WM_POINTERDEVICEINRANGE = 0x239;
        public const int WM_POINTERDEVICEOUTOFRANGE = 0x23A;
        public const int WM_TOUCH = 0x240;
        public const int WM_NCPOINTERUPDATE = 0x241;
        public const int WM_NCPOINTERDOWN = 0x242;
        public const int WM_NCPOINTERUP = 0x243;
        public const int WM_POINTERUPDATE = 0x245;
        public const int WM_POINTERDOWN = 0x246;
        public const int WM_POINTERUP = 0x247;
        public const int WM_POINTERENTER = 0x249;
        public const int WM_POINTERLEAVE = 0x24A;
        public const int WM_POINTERACTIVATE = 0x24B;
        public const int WM_POINTERCAPTURECHANGED = 0x24C;
        public const int WM_TOUCHHITTESTING = 0x24D;
        public const int WM_POINTERWHEEL = 0x24E;
        public const int WM_POINTERHWHEEL = 0x24F;
        public const int DM_POINTERHITTEST = 0x250;
        public const int WM_IME_SETCONTEXT = 0x281;
        public const int WM_IME_NOTIFY = 0x282;
        public const int WM_IME_CONTROL = 0x283;
        public const int WM_IME_COMPOSITIONFULL = 0x284;
        public const int WM_IME_SELECT = 0x285;
        public const int WM_IME_CHAR = 0x286;
        public const int WM_IME_REQUEST = 0x288;
        public const int WM_IME_KEYDOWN = 0x290;
        public const int WM_IME_KEYUP = 0x291;
        public const int WM_MOUSEHOVER = 0x2A1;
        public const int WM_MOUSELEAVE = 0x2A3;
        public const int WM_NCMOUSEHOVER = 0x2A0;
        public const int WM_NCMOUSELEAVE = 0x2A2;
        public const int WM_WTSSESSION_CHANGE = 0x2B1;
        public const int WM_TABLET_FIRST = 0x2C0;
        public const int WM_TABLET_LAST = 0x2DF;
        public const int WM_DPICHANGED = 0x2E0;
        public const int WM_CUT = 0x300;
        public const int WM_COPY = 0x301;
        public const int WM_PASTE = 0x302;
        public const int WM_CLEAR = 0x303;
        public const int WM_UNDO = 0x304;
        public const int WM_RENDERFORMAT = 0x305;
        public const int WM_RENDERALLFORMATS = 0x306;
        public const int WM_DESTROYCLIPBOARD = 0x307;
        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_PAINTCLIPBOARD = 0x309;
        public const int WM_VSCROLLCLIPBOARD = 0x30A;
        public const int WM_SIZECLIPBOARD = 0x30B;
        public const int WM_ASKCBFORMATNAME = 0x30C;
        public const int WM_CHANGECBCHAIN = 0x30D;
        public const int WM_HSCROLLCLIPBOARD = 0x30E;
        public const int WM_QUERYNEWPALETTE = 0x30F;
        public const int WM_PALETTEISCHANGING = 0x310;
        public const int WM_PALETTECHANGED = 0x311;
        public const int WM_HOTKEY = 0x312;
        public const int WM_PRINT = 0x317;
        public const int WM_PRINTCLIENT = 0x318;
        public const int WM_APPCOMMAND = 0x319;
        public const int WM_THEMECHANGED = 0x31A;
        public const int WM_CLIPBOARDUPDATE = 0x31D;
        public const int WM_DWMCOMPOSITIONCHANGED = 0x31E;
        public const int WM_DWMNCRENDERINGCHANGED = 0x31F;
        public const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;
        public const int WM_DWMWINDOWMAXIMIZEDCHANGE = 0x321;
        public const int WM_DWMSENDICONICTHUMBNAIL = 0x323;
        public const int WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x326;
        public const int WM_GETTITLEBARINFOEX = 0x33F;
        public const int WM_HANDHELDFIRST = 0x358;
        public const int WM_HANDHELDLAST = 0x35F;
        public const int WM_AFXFIRST = 0x360;
        public const int WM_AFXLAST = 0x37F;
        public const int WM_PENWINFIRST = 0x380;
        public const int WM_PENWINLAST = 0x38F;
        public const int WM_APP = 0x8000;
        public const int WM_USER = 0x400;
        public const int MK_LBUTTON = 0x1;
        public const int MK_RBUTTON = 0x2;
        public const int MK_MBUTTON = 0x10;
        public const int MK_SHIFT = 0x4;
        public const int MK_CONTROL = 0x8;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int WM_DEVICECHANGE = 0x219;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int SC_SIZE = 0xF000;
        public const int SC_MOVE = 0xF010;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_NEXTWINDOW = 0xF040;
        public const int SC_PREVWINDOW = 0xF050;
        public const int SC_CLOSE = 0xF060;
        public const int SC_VSCROLL = 0xF070;
        public const int SC_HSCROLL = 0xF080;
        public const int SC_MOUSEMENU = 0xF090;
        public const int SC_KEYMENU = 0xF100;
        public const int SC_ARRANGE = 0xF110;
        public const int SC_RESTORE = 0xF120;
        public const int SC_TASKLIST = 0xF130;
        public const int SC_SCREENSAVE = 0xF140;
        public const int SC_HOTKEY = 0xF150;
        public const int SC_DEFAULT = 0xF160;
        public const int SC_MONITORPOWER = 0xF170;
        public const int SC_CONTEXTHELP = 0xF180;
        public const int SC_SEPARATOR = 0xF00F;
        public const int SCF_ISSECURE = 0x1;
        public const int SC_ICON = SC_MINIMIZE;
        public const int SC_ZOOM = SC_MAXIMIZE;

        public static int GET_SC_WPARAM(IntPtr wParam)
        {
            return wParam.ToInt32() & 0xFFF0;
        }

        /// <summary>
        /// Retrieves an ATOM value that uniquely identifies the window class. This is the same atom that the RegisterClassEx function returns.
        /// </summary>
        public const int GCW_ATOM = -32;
        /// <summary>
        /// Retrieves the size, in bytes, of the extra memory associated with the class.
        /// </summary>
        public const int GCL_CBCLSEXTRA = -20;
        /// <summary>
        /// Retrieves the size, in bytes, of the extra window memory associated with each window in the class. For information on how to access this memory, see GetWindowLongPtr.
        /// </summary>
        public const int GCL_CBWNDEXTRA = -18;
        /// <summary>
        /// Retrieves a handle to the background brush associated with the class.
        /// </summary>
        public const int GCLP_HBRBACKGROUND = -10;
        /// <summary>
        /// Retrieves a handle to the cursor associated with the class.
        /// </summary>
        public const int GCLP_HCURSOR = -12;
        /// <summary>
        /// Retrieves a handle to the icon associated with the class.
        /// </summary>
        public const int GCLP_HICON = -14;
        /// <summary>
        /// Retrieves a handle to the small icon associated with the class.
        /// </summary>
        public const int GCLP_HICONSM = -34;
        /// <summary>
        /// Retrieves a handle to the module that registered the class.
        /// </summary>
        public const int GCLP_HMODULE = -16;
        /// <summary>
        /// Retrieves the pointer to the menu name string. The string identifies the menu resource associated with the class.
        /// </summary>
        public const int GCLP_MENUNAME = -8;
        /// <summary>
        /// Retrieves the window-class style bits.
        /// </summary>
        public const int GCL_STYLE = -26;
        /// <summary>
        /// Retrieves the address of the window procedure, or a handle representing the address of the window procedure. You must use the CallWindowProc function to call the window procedure.
        /// </summary>
        public const int GCLP_WNDPROC = -24;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int GWL_WNDPROC = -4;

        /// <summary>
        /// Retrieves the extended window styles.
        /// </summary> 
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// Retrieves a handle to the application instance.
        /// </summary> 
        public const int GWLP_HINSTANCE = -6;

        /// <summary>
        /// Retrieves a handle to the parent window, if there is one.
        /// </summary> 
        public const int GWLP_HWNDPARENT = -8;

        /// <summary>
        /// Retrieves the identifier of the window.
        /// </summary> 
        public const int GWLP_ID = -12;

        /// <summary>
        /// Retrieves the window styles.
        /// </summary> 
        public const int GWL_STYLE = -16;

        /// <summary>
        /// Retrieves the user data associated with the window. This data is intended for use by the application that created the window. Its value is initially zero.
        /// </summary> 
        public const int GWLP_USERDATA = -21;

        /// <summary>
        /// Retrieves the pointer to the window procedure, or a handle representing the pointer to the window procedure. You must use the CallWindowProc function to call the window procedure.
        /// </summary> 
        public const int GWLP_WNDPROC = -4;

        // Window Creation

        // Window Styles 1

        public const long WS_OVERLAPPED = 0x0L;
        public const int WS_POPUP = unchecked((int)0x80000000);
        public const int WS_CHILD = 0x40000000;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_DISABLED = 0x8000000;
        public const int WS_CLIPSIBLINGS = 0x4000000;
        public const int WS_CLIPCHILDREN = 0x2000000;
        public const int WS_MAXIMIZE = 0x1000000;
        public const int WS_CAPTION = 0xC00000;
        public const int WS_BORDER = 0x800000;
        public const int WS_DLGFRAME = 0x400000;
        public const int WS_VSCROLL = 0x200000;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_SYSMENU = 0x80000;
        public const int WS_THICKFRAME = 0x40000;
        public const int WS_GROUP = 0x20000;
        public const int WS_TABSTOP = 0x10000;
        public const int WS_MINIMIZEBOX = 0x20000;
        public const int WS_MAXIMIZEBOX = 0x10000;
        public const long WS_TILED = WS_OVERLAPPED;
        public const int WS_ICONIC = WS_MINIMIZE;
        public const int WS_SIZEBOX = WS_THICKFRAME;

        // Window Styles 2

        public const long WS_OVERLAPPEDWINDOW = WS_OVERLAPPED + WS_CAPTION + WS_SYSMENU + WS_THICKFRAME + WS_MINIMIZEBOX + WS_MAXIMIZEBOX;
        public const int WS_POPUPWINDOW = WS_POPUP + WS_BORDER + WS_SYSMENU;
        public const int WS_CHILDWINDOW = WS_CHILD;
        public const long WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
        // 
        // Extended Window Styles
        // 
        public const long WS_EX_DLGMODALFRAME = 0x1L;
        public const long WS_EX_NOPARENTNOTIFY = 0x4L;
        public const long WS_EX_TOPMOST = 0x8L;
        public const long WS_EX_ACCEPTFILES = 0x10L;
        public const long WS_EX_TRANSPARENT = 0x20L;
        public const long WS_EX_MDICHILD = 0x40L;
        public const long WS_EX_TOOLWINDOW = 0x80L;
        public const long WS_EX_WINDOWEDGE = 0x100L;
        public const long WS_EX_CLIENTEDGE = 0x200L;
        public const long WS_EX_CONTEXTHELP = 0x400L;
        public const long WS_EX_RIGHT = 0x1000L;
        public const long WS_EX_LEFT = 0x0L;
        public const long WS_EX_RTLREADING = 0x2000L;
        public const long WS_EX_LTRREADING = 0x0L;
        public const long WS_EX_LEFTSCROLLBAR = 0x4000L;
        public const long WS_EX_RIGHTSCROLLBAR = 0x0L;
        public const int WS_EX_CONTROLPARENT = 0x10000;
        public const int WS_EX_STATICEDGE = 0x20000;
        public const int WS_EX_APPWINDOW = 0x40000;
        public const long WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE + WS_EX_CLIENTEDGE;
        public const long WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE + WS_EX_TOOLWINDOW + WS_EX_TOPMOST;

        // Windows 5.0& (2000/Millenium)

        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_NOINHERITLAYOUT = 0x100000;     // Disable inheritence of mirroring by children
        public const int WS_EX_LAYOUTRTL = 0x400000;           // Right to left mirroring

        // Windows NT 5.0& (Windows 2000) only

        public const int WS_EX_NOACTIVATE = 0x8000000;
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        public const int SW_FORCEMINIMIZE = 11;
        public const int SW_MAX = 11;

        public const int CCM_FIRST = 0x2000;      // Common control shared messages
        public const int CCM_LAST = CCM_FIRST + 0x200;
        public const int CCM_SETBKCOLOR = CCM_FIRST + 1; // lParam is bkColor
        public const int CCM_SETCOLORSCHEME = CCM_FIRST + 2; // lParam is color scheme
        public const int CCM_GETCOLORSCHEME = CCM_FIRST + 3; // fills in COLORSCHEME pointed to by lParam
        public const int CCM_GETDROPTARGET = CCM_FIRST + 4;
        public const int CCM_SETUNICODEFORMAT = CCM_FIRST + 5;
        public const int CCM_GETUNICODEFORMAT = CCM_FIRST + 6;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int LVS_ICON = 0x0;
        public const int LVS_REPORT = 0x1;
        public const int LVS_SMALLICON = 0x2;
        public const int LVS_LIST = 0x3;
        public const int LVS_TYPEMASK = 0x3;
        public const int LVS_SINGLESEL = 0x4;
        public const int LVS_SHOWSELALWAYS = 0x8;
        public const int LVS_SORTASCENDING = 0x10;
        public const int LVS_SORTDESCENDING = 0x20;
        public const int LVS_SHAREIMAGELISTS = 0x40;
        public const int LVS_NOLABELWRAP = 0x80;
        public const int LVS_AUTOARRANGE = 0x100;
        public const int LVS_EDITLABELS = 0x200;
        public const int LVS_OWNERDATA = 0x1000;
        public const int LVS_NOSCROLL = 0x2000;
        public const int LVS_TYPESTYLEMASK = 0xFC00;
        public const int LVS_ALIGNTOP = 0x0;
        public const int LVS_ALIGNLEFT = 0x800;
        public const int LVS_ALIGNMASK = 0xC00;
        public const int LVS_OWNERDRAWFIXED = 0x400;
        public const int LVS_NOCOLUMNHEADER = 0x4000;
        public const int LVS_NOSORTHEADER = 0x8000;

        // end_r_commctrl
        public const int LVM_FIRST = 0x1000;
        public const int LVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int LVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;

        public static IntPtr ListView_SetUnicodeFormat(IntPtr hwnd, int fUnicode)
        {
            return SendMessage(hwnd, LVM_SETUNICODEFORMAT, fUnicode, IntPtr.Zero);
        }

        public static IntPtr ListView_GetUnicodeFormat(IntPtr hwnd)
        {
            return SendMessage(hwnd, LVM_GETUNICODEFORMAT, IntPtr.Zero, IntPtr.Zero);
        }

        public const int LVM_GETBKCOLOR = LVM_FIRST + 0;
        public const int LVM_SETBKCOLOR = LVM_FIRST + 1;
        public const int LVM_GETIMAGELIST = LVM_FIRST + 2;

        public static Color ListView_SetBkColor(IntPtr hwnd)
        {
            return Color.FromArgb((int)SendMessage(hwnd, LVM_GETBKCOLOR, 0, 0));
        }

        public static IntPtr ListView_SetBkColor(IntPtr hwnd, Color clrBk)
        {
            return SendMessage(hwnd, LVM_SETBKCOLOR, 0, clrBk.ToArgb());
        }

        public static IntPtr ListView_GetImageList(IntPtr hwnd, int iImageList)
        {
            return SendMessage(hwnd, LVM_GETIMAGELIST, iImageList, 0);
        }

        public const int LVSIL_NORMAL = 0;
        public const int LVSIL_SMALL = 1;
        public const int LVSIL_STATE = 2;
        public const int LVSIL_GROUPHEADER = 3;
        public const int LVM_SETIMAGELIST = LVM_FIRST + 3;

        public static IntPtr ListView_SetImageList(IntPtr hwnd, int iImageList, IntPtr hImageList)
        {
            return SendMessage(hwnd, LVM_SETIMAGELIST, iImageList, hImageList);
        }

        public const int LVM_GETITEMCOUNT = LVM_FIRST + 4;

        public static IntPtr ListView_GetItemCount(IntPtr hwnd)
        {
            return SendMessage(hwnd, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
        }

        public const int LVIF_TEXT = 0x1;
        public const int LVIF_IMAGE = 0x2;
        public const int LVIF_PARAM = 0x4;
        public const int LVIF_STATE = 0x8;
        public const int LVIF_INDENT = 0x10;
        public const int LVIF_NORECOMPUTE = 0x800;
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int LVIF_GROUPID = 0x100;
        public const int LVIF_COLUMNS = 0x200;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int LVIF_COLFMT = 0x10000; // The piColFmt member is valid in addition to puColumns
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int LVIS_FOCUSED = 0x1;
        public const int LVIS_SELECTED = 0x2;
        public const int LVIS_CUT = 0x4;
        public const int LVIS_DROPHILITED = 0x8;
        public const int LVIS_GLOW = 0x10;
        public const int LVIS_ACTIVATING = 0x20;
        public const int LVIS_OVERLAYMASK = 0xF00;
        public const int LVIS_STATEIMAGEMASK = 0xF000;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // System Parameters info constants

        public const int SPI_GETACCESSTIMEOUT = 60;
        public const int SPI_GETANIMATION = 72;
        public const int SPI_GETBEEP = 1;
        public const int SPI_GETBORDER = 5;
        public const int SPI_GETDEFAULTINPUTLANG = 89;
        public const int SPI_GETDRAGFULLWINDOWS = 38;
        public const int SPI_GETFASTTASKSWITCH = 35;
        public const int SPI_GETFILTERKEYS = 50;
        public const int SPI_GETFONTSMOOTHING = 74;
        public const int SPI_GETGRIDGRANULARITY = 18;
        public const int SPI_GETHIGHCONTRAST = 66;
        public const int SPI_GETICONMETRICS = 45;
        public const int SPI_GETICONTITLELOGFONT = 31;
        public const int SPI_GETICONTITLEWRAP = 25;
        public const int SPI_GETKEYBOARDDELAY = 22;
        public const int SPI_GETKEYBOARDPREF = 68;
        public const int SPI_GETKEYBOARDSPEED = 10;
        public const int SPI_GETLOWPOWERACTIVE = 83;
        public const int SPI_GETLOWPOWERTIMEOUT = 79;
        public const int SPI_GETMENUDROPALIGNMENT = 27;
        public const int SPI_GETMINIMIZEDMETRICS = 43;
        public const int SPI_GETMOUSE = 3;
        public const int SPI_GETMOUSEKEYS = 54;
        public const int SPI_GETMOUSETRAILS = 94;
        public const int SPI_GETNONCLIENTMETRICS = 41;
        public const int SPI_GETPOWEROFFACTIVE = 84;
        public const int SPI_GETPOWEROFFTIMEOUT = 80;
        public const int SPI_GETSCREENREADER = 70;
        public const int SPI_GETSCREENSAVEACTIVE = 16;
        public const int SPI_GETSCREENSAVETIMEOUT = 14;
        public const int SPI_GETSERIALKEYS = 62;
        public const int SPI_GETSHOWSOUNDS = 56;
        public const int SPI_GETSOUNDSENTRY = 64;
        public const int SPI_GETSTICKYKEYS = 58;
        public const int SPI_GETTOGGLEKEYS = 52;
        public const int SPI_GETWINDOWSEXTENSION = 92;
        public const int SPI_GETWORKAREA = 48;
        public const int SPI_ICONHORIZONTALSPACING = 13;
        public const int SPI_ICONVERTICALSPACING = 24;
        public const int SPI_LANGDRIVER = 12;
        public const int SPI_SCREENSAVERRUNNING = 97;
        public const int SPI_SETACCESSTIMEOUT = 61;
        public const int SPI_SETANIMATION = 73;
        public const int SPI_SETBEEP = 2;
        public const int SPI_SETBORDER = 6;
        public const int SPI_SETCURSORS = 87;
        public const int SPI_SETDEFAULTINPUTLANG = 90;
        public const int SPI_SETDESKPATTERN = 21;
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPI_SETDOUBLECLICKTIME = 32;
        public const int SPI_SETDOUBLECLKHEIGHT = 30;
        public const int SPI_SETDOUBLECLKWIDTH = 29;
        public const int SPI_SETDRAGFULLWINDOWS = 37;
        public const int SPI_SETDRAGHEIGHT = 77;
        public const int SPI_SETDRAGWIDTH = 76;
        public const int SPI_SETFASTTASKSWITCH = 36;
        public const int SPI_SETFILTERKEYS = 51;
        public const int SPI_SETFONTSMOOTHING = 75;
        public const int SPI_SETGRIDGRANULARITY = 19;
        public const int SPI_SETHANDHELD = 78;
        public const int SPI_SETHIGHCONTRAST = 67;
        public const int SPI_SETICONMETRICS = 46;
        public const int SPI_SETICONS = 88;
        public const int SPI_SETICONTITLELOGFONT = 34;
        public const int SPI_SETICONTITLEWRAP = 26;
        public const int SPI_SETKEYBOARDDELAY = 23;
        public const int SPI_SETKEYBOARDPREF = 69;
        public const int SPI_SETKEYBOARDSPEED = 11;
        public const int SPI_SETLANGTOGGLE = 91;
        public const int SPI_SETLOWPOWERACTIVE = 85;
        public const int SPI_SETLOWPOWERTIMEOUT = 81;
        public const int SPI_SETMENUDROPALIGNMENT = 28;
        public const int SPI_SETMINIMIZEDMETRICS = 44;
        public const int SPI_SETMOUSE = 4;
        public const int SPI_SETMOUSEBUTTONSWAP = 33;
        public const int SPI_SETMOUSEKEYS = 55;
        public const int SPI_SETMOUSETRAILS = 93;
        public const int SPI_SETNONCLIENTMETRICS = 42;
        public const int SPI_SETPENWINDOWS = 49;
        public const int SPI_SETPOWEROFFACTIVE = 86;
        public const int SPI_SETPOWEROFFTIMEOUT = 82;
        public const int SPI_SETSCREENREADER = 71;
        public const int SPI_SETSCREENSAVEACTIVE = 17;
        public const int SPI_SETSCREENSAVETIMEOUT = 15;
        public const int SPI_SETSERIALKEYS = 63;
        public const int SPI_SETSHOWSOUNDS = 57;
        public const int SPI_SETSOUNDSENTRY = 65;
        public const int SPI_SETSTICKYKEYS = 59;
        public const int SPI_SETTOGGLEKEYS = 53;
        public const int SPI_SETWORKAREA = 47;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Frame Control

        public const int DFC_BUTTON = 4;
        public const int DFC_CAPTION = 1;
        public const int DFC_MENU = 2;
        public const int DFC_SCROLL = 3;
        public const int DFCS_ADJUSTRECT = 0x2000;
        public const int DFCS_BUTTON3STATE = 0x8;
        public const int DFCS_BUTTONCHECK = 0x0;
        public const int DFCS_BUTTONPUSH = 0x10;
        public const int DFCS_BUTTONRADIO = 0x4;
        public const int DFCS_BUTTONRADIOIMAGE = 0x1;
        public const int DFCS_BUTTONRADIOMASK = 0x2;
        public const int DFCS_CAPTIONCLOSE = 0x0;
        public const int DFCS_CAPTIONHELP = 0x4;
        public const int DFCS_CAPTIONMAX = 0x2;
        public const int DFCS_CAPTIONMIN = 0x1;
        public const int DFCS_CAPTIONRESTORE = 0x3;
        public const int DFCS_CHECKED = 0x400;
        public const int DFCS_FLAT = 0x4000;
        public const int DFCS_INACTIVE = 0x100;
        public const int DFCS_MENUARROW = 0x0;
        public const int DFCS_MENUARROWRIGHT = 0x4;
        public const int DFCS_MENUBULLET = 0x2;
        public const int DFCS_MENUCHECK = 0x1;
        public const int DFCS_MONO = 0x8000;
        public const int DFCS_PUSHED = 0x200;
        public const int DFCS_SCROLLCOMBOBOX = 0x5;
        public const int DFCS_SCROLLDOWN = 0x1;
        public const int DFCS_SCROLLLEFT = 0x2;
        public const int DFCS_SCROLLRIGHT = 0x3;
        public const int DFCS_SCROLLSIZEGRIP = 0x8;
        public const int DFCS_SCROLLSIZEGRIPRIGHT = 0x10;
        public const int DFCS_SCROLLUP = 0x0;
        public const int DC_ACTIVE = 0x1;
        public const int DC_SMALLCAP = 0x2;
        public const int DC_ICON = 0x4;
        public const int DC_TEXT = 0x8;
        public const int DC_INBUTTON = 0x10;
        public const int DC_GRADIENT = 0x20;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // System Colors

        public const int COLOR_ACTIVEBORDER = 10;
        public const int COLOR_ACTIVECAPTION = 2;
        public const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int COLOR_ADJ_MAX = 100;
        public const int COLOR_ADJ_MIN = -100;
        public const int COLOR_APPWORKSPACE = 12;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_BTNFACE = 15;
        public const int COLOR_BTNHIGHLIGHT = 20;
        public const int COLOR_BTNSHADOW = 16;
        public const int COLOR_BTNTEXT = 18;
        public const int COLOR_CAPTIONTEXT = 9;
        public const int COLOR_GRAYTEXT = 17;
        public const int COLOR_HIGHLIGHT = 13;
        public const int COLOR_HIGHLIGHTTEXT = 14;
        public const int COLOR_INACTIVEBORDER = 11;
        public const int COLOR_INACTIVECAPTION = 3;
        public const int COLOR_GRADIENTINACTIVECAPTION = 28;
        public const int COLOR_INACTIVECAPTIONTEXT = 19;
        public const int COLOR_MENU = 4;
        public const int COLOR_MENUTEXT = 7;
        public const int COLOR_SCROLLBAR = 0;
        public const int COLOR_WINDOW = 5;
        public const int COLOR_WINDOWFRAME = 6;
        public const int COLOR_WINDOWTEXT = 8;
        public const int COLOR_3DDKSHADOW = 21;
        public const int COLOR_3DLIGHT = 22;
        public const int COLOR_INFOTEXT = 23;
        public const int COLOR_INFOBK = 24;

        // Windows 98/2000

        public const int COLOR_HOTLIGHT = 26;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Scroll Bar Types

        public const int SB_BOTH = 3;
        public const int SB_BOTTOM = 7;
        public const int SB_CTL = 2;
        public const int SB_ENDSCROLL = 8;
        public const int SB_HORZ = 0;
        public const int SB_LEFT = 6;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINELEFT = 0;
        public const int SB_LINERIGHT = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGERIGHT = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_RIGHT = 7;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;

        // Scroll Bar Messages

        public const int SBM_ENABLE_ARROWS = 0xE4;
        public const int SBM_GETPOS = 0xE1;
        public const int SBM_GETRANGE = 0xE3;
        public const int SBM_SETPOS = 0xE0;
        public const int SBM_SETRANGE = 0xE2;
        public const int SBM_SETRANGEREDRAW = 0xE6;

        // Scroll Bar Window Styles

        public const int SBS_BOTTOMALIGN = 0x4;
        public const int SBS_HORZ = 0x0;
        public const int SBS_LEFTALIGN = 0x2;
        public const int SBS_RIGHTALIGN = 0x4;
        public const int SBS_SIZEBOX = 0x8;
        public const int SBS_SIZEBOXBOTTOMRIGHTALIGN = 0x4;
        public const int SBS_SIZEBOXTOPLEFTALIGN = 0x2;
        public const int SBS_TOPALIGN = 0x2;
        public const int SBS_VERT = 0x1;

        // EnableScrollBar() flags

        public const int ESB_DISABLE_BOTH = 0x3;
        public const int ESB_DISABLE_DOWN = 0x2;
        public const int ESB_DISABLE_LEFT = 0x1;
        public const int ESB_DISABLE_RIGHT = 0x2;
        public const int ESB_DISABLE_UP = 0x1;
        public const int ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT;
        public const int ESB_DISABLE_LTUP = ESB_DISABLE_LEFT;
        public const int ESB_ENABLE_BOTH = 0x0;
        public const int SIF_RANGE = 0x1;
        public const int SIF_PAGE = 0x2;
        public const int SIF_POS = 0x4;
        public const int SIF_DISABLENOSCROLL = 0x8;
        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const long TPM_LEFTBUTTON = 0x0L;
        public const long TPM_RIGHTBUTTON = 0x2L;
        public const long TPM_LEFTALIGN = 0x0L;
        public const long TPM_CENTERALIGN = 0x4L;
        public const long TPM_RIGHTALIGN = 0x8L;
        public const long TPM_TOPALIGN = 0x0L;
        public const long TPM_VCENTERALIGN = 0x10L;
        public const long TPM_BOTTOMALIGN = 0x20L;
        public const long TPM_HORIZONTAL = 0x0L;            // Horz alignment matters more ''
        public const long TPM_VERTICAL = 0x40L;             // Vert alignment matters more ''
        public const long TPM_NONOTIFY = 0x80L;             // Don't send any notification msgs ''
        public const long TPM_RETURNCMD = 0x100L;
        public const long TPM_RECURSE = 0x1L;

        // fMask flags

        public const int MIIM_STATE = 0x1;
        public const int MIIM_ID = 0x2;
        public const int MIIM_SUBMENU = 0x4;
        public const int MIIM_CHECKMARKS = 0x8;
        public const int MIIM_TYPE = 0x10;
        public const int MIIM_DATA = 0x20;

        // New for Windows 98/2000

        public const int MIIM_STRING = 0x40;
        public const int MIIM_BITMAP = 0x80;
        public const int MIIM_FTYPE = 0x100;

        // End fMask flags

        // Menu Flags

        public const long MF_INSERT = 0x0L;
        public const long MF_CHANGE = 0x80L;
        public const long MF_APPEND = 0x100L;
        public const long MF_DELETE = 0x200L;
        public const long MF_REMOVE = 0x1000L;
        public const long MF_BYCOMMAND = 0x0L;
        public const long MF_BYPOSITION = 0x400L;
        public const long MF_SEPARATOR = 0x800L;
        public const long MF_ENABLED = 0x0L;
        public const long MF_GRAYED = 0x1L;
        public const long MF_DISABLED = 0x2L;
        public const long MF_UNCHECKED = 0x0L;
        public const long MF_CHECKED = 0x8L;
        public const long MF_USECHECKBITMAPS = 0x200L;
        public const long MF_STRING = 0x0L;
        public const long MF_BITMAP = 0x4L;
        public const long MF_OWNERDRAW = 0x100L;
        public const long MF_POPUP = 0x10L;
        public const long MF_MENUBARBREAK = 0x20L;
        public const long MF_MENUBREAK = 0x40L;
        public const long MF_UNHILITE = 0x0L;
        public const long MF_HILITE = 0x80L;
        public const long MF_DEFAULT = 0x1000L;
        public const long MF_SYSMENU = 0x2000L;
        public const long MF_HELP = 0x4000L;
        public const long MF_RIGHTJUSTIFY = 0x4000L;
        public const long MF_MOUSESELECT = 0x8000L;
        public const long MF_END = 0x80L;                    // Obsolete -- only used by old RES files
        public const long MFT_STRING = MF_STRING;
        public const long MFT_BITMAP = MF_BITMAP;
        public const long MFT_MENUBARBREAK = MF_MENUBARBREAK;
        public const long MFT_MENUBREAK = MF_MENUBREAK;
        public const long MFT_OWNERDRAW = MF_OWNERDRAW;
        public const long MFT_RADIOGROUP = 0x200L;
        public const long MFT_SEPARATOR = MF_SEPARATOR;
        public const long MFT_RIGHTORDER = 0x2000L;
        public const long MFT_RIGHTJUSTIFY = MF_RIGHTJUSTIFY;
        public const long MFS_GRAYED = 0x3L;
        public const long MFS_DISABLED = MFS_GRAYED;
        public const long MFS_CHECKED = MF_CHECKED;
        public const long MFS_HILITE = MF_HILITE;
        public const long MFS_ENABLED = MF_ENABLED;
        public const long MFS_UNCHECKED = MF_UNCHECKED;
        public const long MFS_UNHILITE = MF_UNHILITE;
        public const long MFS_DEFAULT = MF_DEFAULT;

        // New for Windows 2000/98

        public const long MFS_MASK = 0x108BL;
        public const int MFS_HOTTRACKDRAWN = 0x10000000;
        public const int MFS_CACHEDBMP = 0x20000000;
        public const int MFS_BOTTOMGAPDROP = 0x40000000;
        public const int MFS_TOPGAPDROP = unchecked((int)0x80000000);
        public const int MFS_GAPDROP = unchecked((int)0xC0000000);

        // for the SetMenuInfo API function

        public const int MNS_NOCHECK = unchecked((int)0x80000000);
        public const int MNS_MODELESS = 0x40000000;
        public const int MNS_DRAGDROP = 0x20000000;
        public const int MNS_AUTODISMISS = 0x10000000;
        public const int MNS_NOTIFYBYPOS = 0x8000000;
        public const int MNS_CHECKORBMP = 0x4000000;
        public const int MIM_MAXHEIGHT = 0x1;
        public const int MIM_BACKGROUND = 0x2;
        public const int MIM_HELPID = 0x4;
        public const int MIM_MENUDATA = 0x8;
        public const int MIM_STYLE = 0x10;
        public const int MIM_APPLYTOSUBMENUS = unchecked((int)0x80000000);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int PARMNUM_BASE_INFOLEVEL = 1000;
        public const int SHARE_NETNAME_PARMNUM = 1;
        public const int SHARE_TYPE_PARMNUM = 3;
        public const int SHARE_REMARK_PARMNUM = 4;
        public const int SHARE_PERMISSIONS_PARMNUM = 5;
        public const int SHARE_MAX_USES_PARMNUM = 6;
        public const int SHARE_CURRENT_USES_PARMNUM = 7;
        public const int SHARE_PATH_PARMNUM = 8;
        public const int SHARE_PASSWD_PARMNUM = 9;
        public const int SHARE_FILE_SD_PARMNUM = 501;
        public const int SHARE_SERVER_PARMNUM = 503;

        //
        // Single-field infolevels for NetShareSetInfo.
        //

        public const int SHARE_REMARK_INFOLEVEL = PARMNUM_BASE_INFOLEVEL + SHARE_REMARK_PARMNUM;
        public const int SHARE_MAX_USES_INFOLEVEL = PARMNUM_BASE_INFOLEVEL + SHARE_MAX_USES_PARMNUM;
        public const int SHARE_FILE_SD_INFOLEVEL = PARMNUM_BASE_INFOLEVEL + SHARE_FILE_SD_PARMNUM;
        public const int SHI1_NUM_ELEMENTS = 4;
        public const int SHI2_NUM_ELEMENTS = 10;

        //
        // Share types (shi1_type and shi2_type fields).
        //

        public const int STYPE_DISKTREE = 0;
        public const int STYPE_PRINTQ = 1;
        public const int STYPE_DEVICE = 2;
        public const int STYPE_IPC = 3;
        public const int STYPE_MASK = 0xFF;  // AND with shi_type to
        public const int STYPE_RESERVED1 = 0x1000000;  // Reserved for internal processing
        public const int STYPE_RESERVED2 = 0x2000000;
        public const int STYPE_RESERVED3 = 0x4000000;
        public const int STYPE_RESERVED4 = 0x8000000;
        public const int STYPE_RESERVED_ALL = 0x3FFFFF00;
        public const int STYPE_TEMPORARY = 0x40000000;
        public const int STYPE_SPECIAL = unchecked((int)0x80000000);
        public const int SHI_USES_UNLIMITED = unchecked((int)0xFFFFFFFF);
        public const int ACCESS_READ = 0x1;
        public const int ACCESS_WRITE = 0x2;
        public const int ACCESS_CREATE = 0x4;
        public const int ACCESS_EXEC = 0x8;
        public const int ACCESS_DELETE = 0x10;
        public const int ACCESS_ATTRIB = 0x20;
        public const int ACCESS_PERM = 0x40;
        public const int ACCESS_GROUP = 0x8000;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // IContextMenu and shell

        //
        //  Begin ShellExecuteEx and family
        //

        // /* ShellExecute() and ShellExecuteEx() error codes */

        // /* regular WinExec() codes */
        public const int SE_ERR_FNF = 2;       // file not found
        public const int SE_ERR_PNF = 3;       // path not found
        public const int SE_ERR_ACCESSDENIED = 5;       // access denied
        public const int SE_ERR_OOM = 8;       // out of memory
        public const int SE_ERR_DLLNOTFOUND = 32;

        // /* error values for ShellExecute() beyond the regular WinExec() codes */
        public const int SE_ERR_SHARE = 26;
        public const int SE_ERR_ASSOCINCOMPLETE = 27;
        public const int SE_ERR_DDETIMEOUT = 28;
        public const int SE_ERR_DDEFAIL = 29;
        public const int SE_ERR_DDEBUSY = 30;
        public const int SE_ERR_NOASSOC = 31;

        // Note CLASSKEY overrides CLASSNAME
        public const int SEE_MASK_DEFAULT = 0x0;
        public const int SEE_MASK_CLASSNAME = 0x1;   // SHELLEXECUTEINFO.lpClass is valid
        public const int SEE_MASK_CLASSKEY = 0x3;   // SHELLEXECUTEINFO.hkeyClass is valid
        // Note SEE_MASK_INVOKEIDLIST(=&HC) implies SEE_MASK_IDLIST(=&H04)
        public const int SEE_MASK_IDLIST = 0x4;   // SHELLEXECUTEINFO.lpIDList is valid
        public const int SEE_MASK_INVOKEIDLIST = 0xC;   // enable IContextMenu based verbs
        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int SEE_MASK_HOTKEY = 0x20;   // SHELLEXECUTEINFO.dwHotKey is valid
        public const int SEE_MASK_NOCLOSEPROCESS = 0x40;   // SHELLEXECUTEINFO.hProcess
        public const int SEE_MASK_CONNECTNETDRV = 0x80;   // enables re-connecting disconnected network drives
        public const int SEE_MASK_NOASYNC = 0x100;   // block on the call until the invoke has completed, use for callers that exit after calling ShellExecuteEx()
        public const int SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC; // Use SEE_MASK_NOASYNC instead of SEE_MASK_FLAG_DDEWAIT as it more accuratly describes the behavior
        public const int SEE_MASK_DOENVSUBST = 0x200;   // indicates that SHELLEXECUTEINFO.lpFile contains env vars that should be expanded
        public const int SEE_MASK_FLAG_NO_UI = 0x400;   // disable UI including error messages
        public const int SEE_MASK_UNICODE = 0x4000;
        public const int SEE_MASK_NO_CONSOLE = 0x8000;
        public const int SEE_MASK_ASYNCOK = 0x100000;
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SEE_MASK_HMONITOR = 0x200000;   // SHELLEXECUTEINFO.hMonitor
        /* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SEE_MASK_NOZONECHECKS = 0x800000;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SEE_MASK_NOQUERYCLASSSTORE = 0x1000000;
        public const int SEE_MASK_WAITFORINPUTIDLE = 0x2000000;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SEE_MASK_FLAG_LOG_USAGE = 0x4000000;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        /* TODO ERROR: Skipped IfDirectiveTrivia */        // When SEE_MASK_FLAG_HINST_IS_SITE is specified SHELLEXECUTEINFO.hInstApp is used as an
                                                           // _In_ parameter and specifies a IUnknown* to be used as a site pointer. The site pointer
                                                           // is used to provide services to shell execute, the handler binding process and the verb handlers
                                                           // once they are invoked.
        public const int SEE_MASK_FLAG_HINST_IS_SITE = 0x8000000;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int CMF_NORMAL = 0x0;
        public const int CMF_DEFAULTONLY = 0x1;
        public const int CMF_VERBSONLY = 0x2;
        public const int CMF_EXPLORE = 0x4;
        public const int CMF_NOVERBS = 0x8;
        public const int CMF_CANRENAME = 0x10;
        public const int CMF_NODEFAULT = 0x20;
        public const int CMF_INCLUDESTATIC = 0x40;

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int CMF_ITEMMENU = 0x80;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int CMF_EXTENDEDVERBS = 0x100;
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int CMF_DISABLEDVERBS = 0x200;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int CMF_ASYNCVERBSTATE = 0x400;
        public const int CMF_OPTIMIZEFORINVOKE = 0x800;
        public const int CMF_SYNCCASCADEMENU = 0x1000;
        public const int CMF_DONOTPICKDEFAULT = 0x2000;
        public const int CMF_RESERVED = unchecked((int) 0xFFFF0000);

        // GetCommandString uFlags
        public const int GCS_VERBA = 0x0;     // canonical verb
        public const int GCS_HELPTEXTA = 0x1;     // help text (for status bar)
        public const int GCS_VALIDATEA = 0x2;     // validate command exists
        public const int GCS_VERBW = 0x4;     // canonical verb (unicode)
        public const int GCS_HELPTEXTW = 0x5;     // help text (unicode version)
        public const int GCS_VALIDATEW = 0x6;     // validate command exists (unicode)
        public const int GCS_VERBICONW = 0x14;     // icon string (unicode)
        public const int GCS_UNICODE = 0x4;     // for bit testing - Unicode string
        public const int GCS_VERB = GCS_VERBW;
        public const int GCS_HELPTEXT = GCS_HELPTEXTW;
        public const int GCS_VALIDATE = GCS_VALIDATEW;
        public const string CMDSTR_NEWFOLDERA = "NewFolder";
        public const string CMDSTR_VIEWLISTA = "ViewList";
        public const string CMDSTR_VIEWDETAILSA = "ViewDetails";
        public const string CMDSTR_NEWFOLDERW = "NewFolder";
        public const string CMDSTR_VIEWLISTW = "ViewList";
        public const string CMDSTR_VIEWDETAILSW = "ViewDetails";
        public const string CMDSTR_NEWFOLDER = CMDSTR_NEWFOLDERW;
        public const string CMDSTR_VIEWLIST = CMDSTR_VIEWLISTW;
        public const string CMDSTR_VIEWDETAILS = CMDSTR_VIEWDETAILSW;
        public const int CMIC_MASK_HOTKEY = SEE_MASK_HOTKEY;
        public const int CMIC_MASK_ICON = 0x10;
        public const int CMIC_MASK_FLAG_NO_UI = SEE_MASK_FLAG_NO_UI;
        public const int CMIC_MASK_UNICODE = SEE_MASK_UNICODE;
        public const int CMIC_MASK_NO_CONSOLE = SEE_MASK_NO_CONSOLE;
        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int CMIC_MASK_ASYNCOK = SEE_MASK_ASYNCOK;
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int CMIC_MASK_NOASYNC = SEE_MASK_NOASYNC;
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int CMIC_MASK_SHIFT_DOWN = 0x10000000;
        public const int CMIC_MASK_CONTROL_DOWN = 0x40000000;
        public const int CMIC_MASK_FLAG_LOG_USAGE = SEE_MASK_FLAG_LOG_USAGE;
        public const int CMIC_MASK_NOZONECHECKS = SEE_MASK_NOZONECHECKS;
        public const int CMIC_MASK_PTINVOKE = 0x20000000;
        public const long SHCIDS_ALLFIELDS = 0x80000000L;
        public const long SHCIDS_CANONICALONLY = 0x10000000L;
        public const long SHCIDS_BITMASK = 0xFFFF0000L;
        public const long SHCIDS_COLUMNMASK = 0xFFFFL;
        public const int SFGAO_CANCOPY = 0x1;        // Objects can be copied    (&H1)
        public const int SFGAO_CANMOVE = 0x2;        // Objects can be moved     (&H2)
        public const int SFGAO_CANLINK = 0x4;        // Objects can be linked    (&H4)
        public const long SFGAO_STORAGE = 0x8L;     // supports BindToObject(IID_IStorage)
        public const long SFGAO_CANRENAME = 0x10L;     // Objects can be renamed
        public const long SFGAO_CANDELETE = 0x20L;     // Objects can be deleted
        public const long SFGAO_HASPROPSHEET = 0x40L;     // Objects have property sheets
        public const long SFGAO_DROPTARGET = 0x100L;     // Objects are drop target
        public const long SFGAO_CAPABILITYMASK = 0x177L;
        public const long SFGAO_SYSTEM = 0x1000L;     // System object
        public const long SFGAO_ENCRYPTED = 0x2000L;     // Object is encrypted (use alt color)
        public const long SFGAO_ISSLOW = 0x4000L;     //Slow' object
        public const long SFGAO_GHOSTED = 0x8000L;     // Ghosted icon
        public const long SFGAO_LINK = 0x10000L;     // Shortcut (link)
        public const long SFGAO_SHARE = 0x20000L;     // Shared
        public const long SFGAO_READONLY = 0x40000L;     // Read-only
        public const long SFGAO_HIDDEN = 0x80000L;     // Hidden object
        public const long SFGAO_DISPLAYATTRMASK = 0xFC000L;
        public const long SFGAO_FILESYSANCESTOR = 0x10000000L;     // May contain children with SFGAO_FILESYSTEM
        public const long SFGAO_FOLDER = 0x20000000L;     // Support BindToObject(IID_IShellFolder)
        public const long SFGAO_FILESYSTEM = 0x40000000L;     // Is a win32 file system object (file/folder/root)
        public const long SFGAO_HASSUBFOLDER = 0x80000000L;     // May contain children with SFGAO_FOLDER (may be slow)
        public const long SFGAO_CONTENTSMASK = 0x80000000L;
        public const long SFGAO_VALIDATE = 0x1000000L;     // Invalidate cached information (may be slow)
        public const long SFGAO_REMOVABLE = 0x2000000L;     // Is this removeable media?
        public const long SFGAO_COMPRESSED = 0x4000000L;     // Object is compressed (use alt color)
        public const long SFGAO_BROWSABLE = 0x8000000L;     // Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
        public const long SFGAO_NONENUMERATED = 0x100000L;     // Is a non-enumerated object (should be hidden)
        public const long SFGAO_NEWCONTENT = 0x200000L;     // Should show bold in explorer tree
        public const long SFGAO_CANMONIKER = 0x400000L;     // Obsolete
        public const long SFGAO_HASSTORAGE = 0x400000L;     // Obsolete
        public const long SFGAO_STREAM = 0x400000L;     // Supports BindToObject(IID_IStream)
        public const long SFGAO_STORAGEANCESTOR = 0x800000L;     // May contain children with SFGAO_STORAGE or SFGAO_STREAM
        public const long SFGAO_STORAGECAPMASK = 0x70C50008L;     // For determining storage capabilities, ie for open/save semantics
        public const long SFGAO_PKEYSFGAOMASK = 0x81044000L;     // Attributes that are masked out for PKEY_SFGAOFlags because they are considered to cause slow calculations or lack context (SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER and others)
        public const int SHGFI_ICON = 0x100;     // get icon
        public const int SHGFI_DISPLAYNAME = 0x200;     // get display name
        public const int SHGFI_TYPENAME = 0x400;     // get type name
        public const int SHGFI_ATTRIBUTES = 0x800;     // get attributes
        public const int SHGFI_ICONLOCATION = 0x1000;     // get icon location
        public const int SHGFI_EXETYPE = 0x2000;     // return exe type
        public const int SHGFI_SYSICONINDEX = 0x4000;     // get system icon index
        public const int SHGFI_LINKOVERLAY = 0x8000;     // put a link overlay on icon
        public const int SHGFI_SELECTED = 0x10000;     // show icon in selected state
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SHGFI_ATTR_SPECIFIED = 0x20000;     // get only specified attributes
        /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public const int SHGFI_LARGEICON = 0x0;     // get large icon
        public const int SHGFI_SMALLICON = 0x1;     // get small icon
        public const int SHGFI_OPENICON = 0x2;     // get open icon
        public const int SHGFI_SHELLICONSIZE = 0x4;     // get shell size icon
        public const int SHGFI_PIDL = 0x8;     // pszPath is a pidl
        public const int SHGFI_USEFILEATTRIBUTES = 0x10;     // use passed dwFileAttribute
        public const int SHGFI_ADDOVERLAYS = 0x20;     // apply the appropriate overlays
        public const int SHGFI_OVERLAYINDEX = 0x40;     // Get the index of the overlay
        // in the upper 8 bits of the iIcon

        public const int BIF_RETURNONLYFSDIRS = 0x1;   // For finding a folder to start document searching
        public const int BIF_DONTGOBELOWDOMAIN = 0x2; // For starting the Find Computer
        public const int BIF_STATUSTEXT = 0x4;
        public const int BIF_RETURNFSANCESTORS = 0x8;
        public const int BIF_EDITBOX = 0x10;
        public const int BIF_VALIDATE = 0x20; // insist on valid result (or CANCEL)
        public const int BIF_BROWSEFORCOMPUTER = 0x1000;   // Browsing for Computers.
        public const int BIF_BROWSEFORPRINTER = 0x2000; // Browsing for Printers
        public const int BIF_BROWSEINCLUDEFILES = 0x4000;  // Browsing for Everything

        // message from browser

        public const int BFFM_INITIALIZED = 1;
        public const int BFFM_SELCHANGED = 2;
        public const int BFFM_VALIDATEFAILED = 3;  // lParam:szPath ret:1(cont),0(EndDialog)
        // Public Const BFFM_VALIDATEFAILEDW = 4  ' lParam:wzPath ret:1(cont),0(EndDialog)

        // messages to browser

        public const int BFFM_SETSTATUSTEXTA = WM_USER + 100;
        public const int BFFM_ENABLEOK = WM_USER + 101;
        public const int BFFM_SETSELECTIONA = WM_USER + 102;
        public const int BFFM_SETSELECTIONW = WM_USER + 103;
        public const int BFFM_SETSTATUSTEXTW = WM_USER + 104;

        // Get Data From ID List
        public const long SHGDFIL_FINDDATA = 1L;
        public const long SHGDFIL_NETRESOURCE = 2L;
        public const long SHGDFIL_DESCRIPTIONID = 3L;

        // Shell Description ID

        public const long SHDID_ROOT_REGITEM = 1L;
        public const long SHDID_FS_FILE = 2L;
        public const long SHDID_FS_DIRECTORY = 3L;
        public const long SHDID_FS_OTHER = 4L;
        public const long SHDID_COMPUTER_DRIVE35 = 5L;
        public const long SHDID_COMPUTER_DRIVE525 = 6L;
        public const long SHDID_COMPUTER_REMOVABLE = 7L;
        public const long SHDID_COMPUTER_FIXED = 8L;
        public const long SHDID_COMPUTER_NETDRIVE = 9L;
        public const long SHDID_COMPUTER_CDROM = 10L;
        public const long SHDID_COMPUTER_RAMDISK = 11L;
        public const long SHDID_COMPUTER_OTHER = 12L;
        public const long SHDID_NET_DOMAIN = 13L;
        public const long SHDID_NET_SERVER = 14L;
        public const long SHDID_NET_SHARE = 15L;
        public const long SHDID_NET_RESTOFNET = 16L;
        public const long SHDID_NET_OTHER = 17L;

        // FO_MOVE ''these need to be kept in sync with the ones in shlobj.h

        public const int FO_MOVE = 0x1;
        public const int FO_COPY = 0x2;
        public const int FO_DELETE = 0x3;
        public const int FO_RENAME = 0x4;
        public const int FOF_MULTIDESTFILES = 0x1;
        public const int FOF_CONFIRMMOUSE = 0x2;
        public const int FOF_SILENT = 0x4;                    // don't create progress/report
        public const int FOF_RENAMEONCOLLISION = 0x8;
        public const int FOF_NOCONFIRMATION = 0x10;           // Don't prompt the user.
        public const int FOF_WANTMAPPINGHANDLE = 0x20;        // Fill in SHFILEOPSTRUCT.hNameMappings
        // Must be freed using SHFreeNameMappings
        public const int FOF_ALLOWUNDO = 0x40;
        public const int FOF_FILESONLY = 0x80;                // on *.*, do only files
        public const int FOF_SIMPLEPROGRESS = 0x100;          // means don't show names of files
        public const int FOF_NOCONFIRMMKDIR = 0x200;          // don't confirm making any needed dirs
        public const int FOF_NOERRORUI = 0x400;               // don't put up error UI
        public const int FOF_NOCOPYSECURITYATTRIBS = 0x800;   // dont copy NT file Security Attributes
        public const int PO_DELETE = 0x13;         // printer is being deleted
        public const int PO_RENAME = 0x14;         // printer is being renamed
        public const int PO_PORTCHANGE = 0x20;     // port this printer connected to is being changed
        // if this id is set, the strings received by
        // the copyhook are a doubly-null terminated
        // list of strings.  The first is the printer
        // name and the second is the printer port.
        public const int PO_REN_PORT = 0x34;       // PO_RENAME and PO_PORTCHANGE at same time.
        public const int CSIDL_DESKTOP = 0x0;
        public const int CSIDL_INTERNET = 0x1;
        public const int CSIDL_PROGRAMS = 0x2;
        public const int CSIDL_CONTROLS = 0x3;
        public const int CSIDL_PRINTERS = 0x4;
        public const int CSIDL_PERSONAL = 0x5;
        public const int CSIDL_FAVORITES = 0x6;
        public const int CSIDL_STARTUP = 0x7;
        public const int CSIDL_RECENT = 0x8;
        public const int CSIDL_SENDTO = 0x9;
        public const int CSIDL_BITBUCKET = 0xA;
        public const int CSIDL_STARTMENU = 0xB;
        public const int CSIDL_DESKTOPDIRECTORY = 0x10;
        public const int CSIDL_DRIVES = 0x11;
        public const int CSIDL_NETWORK = 0x12;
        public const int CSIDL_NETHOOD = 0x13;
        public const int CSIDL_FONTS = 0x14;
        public const int CSIDL_TEMPLATES = 0x15;
        public const int CSIDL_COMMON_STARTMENU = 0x16;
        public const int CSIDL_COMMON_PROGRAMS = 0x17;
        public const int CSIDL_COMMON_STARTUP = 0x18;
        public const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x19;
        public const int CSIDL_APPDATA = 0x1A;
        public const int CSIDL_PRINTHOOD = 0x1B;
        public const int CSIDL_ALTSTARTUP = 0x1D;                          // DBCS
        public const int CSIDL_COMMON_ALTSTARTUP = 0x1E;                   // DBCS
        public const int CSIDL_COMMON_FAVORITES = 0x1F;
        public const int CSIDL_INTERNET_CACHE = 0x20;
        public const int CSIDL_COOKIES = 0x21;
        public const int CSIDL_HISTORY = 0x22;

        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SHIL_LARGE = 0;   // normally 32x32
        public const int SHIL_SMALL = 1;   // normally 16x16
        public const int SHIL_EXTRALARGE = 2;
        public const int SHIL_SYSSMALL = 3;   // like SHIL_SMALL, but tracks system small icon metric correctly
        /* TODO ERROR: Skipped IfDirectiveTrivia */
        public const int SHIL_JUMBO = 4;   // normally 256x256
        public const int SHIL_LAST = SHIL_JUMBO;
        /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int IMAGE_BITMAP = 0;
        public const int IMAGE_ICON = 1;
        public const int IMAGE_CURSOR = 2;
        public const int IMAGE_ENHMETAFILE = 3;
        public const int LR_DEFAULTCOLOR = 0x0;
        public const int LR_MONOCHROME = 0x1;
        public const int LR_COLOR = 0x2;
        public const int LR_COPYRETURNORG = 0x4;
        public const int LR_COPYDELETEORG = 0x8;
        public const int LR_LOADFROMFILE = 0x10;
        public const int LR_LOADTRANSPARENT = 0x20;
        public const int LR_DEFAULTSIZE = 0x40;
        public const int LR_VGACOLOR = 0x80;
        public const int LR_LOADMAP3DCOLORS = 0x1000;
        public const int LR_CREATEDIBSECTION = 0x2000;
        public const int LR_COPYFROMRESOURCE = 0x4000;
        public const int LR_SHARED = 0x8000;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Public Const Device = Parameters for GetDeviceCaps() ''
        public const int DRIVERVERSION = 0;     // Device driver version                    ''
        public const int TECHNOLOGY = 2;     // Device classification                    ''
        public const int HORZSIZE = 4;     // Horizontal size in millimeters           ''
        public const int VERTSIZE = 6;     // Vertical size in millimeters             ''
        public const int HORZRES = 8;     // Horizontal width in pixels               ''
        public const int VERTRES = 10;    // Vertical height in pixels                ''
        public const int BITSPIXEL = 12;    // Number of bits per pixel                 ''
        public const int PLANES = 14;    // Number of planes                         ''
        public const int NUMBRUSHES = 16;    // Number of brushes the device has         ''
        public const int NUMPENS = 18;    // Number of pens the device has            ''
        public const int NUMMARKERS = 20;    // Number of markers the device has         ''
        public const int NUMFONTS = 22;    // Number of fonts the device has           ''
        public const int NUMCOLORS = 24;    // Number of colors the device supports     ''
        public const int PDEVICESIZE = 26;    // Size required for device descriptor      ''
        public const int CURVECAPS = 28;    // Curve capabilities                       ''
        public const int LINECAPS = 30;    // Line capabilities                        ''
        public const int POLYGONALCAPS = 32;    // Polygonal capabilities                   ''
        public const int TEXTCAPS = 34;    // Text capabilities                        ''
        public const int CLIPCAPS = 36;    // Clipping capabilities                    ''
        public const int RASTERCAPS = 38;    // Bitblt capabilities                      ''
        public const int ASPECTX = 40;    // Length of the X leg                      ''
        public const int ASPECTY = 42;    // Length of the Y leg                      ''
        public const int ASPECTXY = 44;    // Length of the hypotenuse                 ''
        public const int LOGPIXELSX = 88;    // Logical pixels/inch in X                 ''
        public const int LOGPIXELSY = 90;    // Logical pixels/inch in Y                 ''
        public const int SIZEPALETTE = 104;    // Number of entries in physical palette    ''
        public const int NUMRESERVED = 106;    // Number of reserved entries in palette    ''
        public const int COLORRES = 108;    // Actual color resolution                  ''

        // Public Const Printing = related DeviceCaps. These replace the appropriate Escapes

        public const int PHYSICALWIDTH = 110; // Physical Width in device units           ''
        public const int PHYSICALHEIGHT = 111; // Physical Height in device units          ''
        public const int PHYSICALOFFSETX = 112; // Physical Printable Area x margin         ''
        public const int PHYSICALOFFSETY = 113; // Physical Printable Area y margin         ''
        public const int SCALINGFACTORX = 114; // Scaling factor x                         ''
        public const int SCALINGFACTORY = 115; // Scaling factor y                         ''

        // Public Const Display = driver specific

        public const int VREFRESH = 116;  // Current vertical refresh rate of the    ''
        // Public Const display = device (for displays only) in Hz ''
        public const int DESKTOPVERTRES = 117;  // Horizontal width of entire desktop in   ''
        // pixels                                  ''
        public const int DESKTOPHORZRES = 118;  // Vertical height of entire desktop in    ''
        // pixels                                  ''
        public const int BLTALIGNMENT = 119;  // Preferred blt alignment                 ''
        public const int SHADEBLENDCAPS = 120;  // Shading and blending caps               ''
        public const int COLORMGMTCAPS = 121;  // Color Management caps                   ''
        // WINVER >= &H0500 ''

        // Public Const Device = Capability Masks: ''

        // Public Const Device = Technologies ''
        public const int DT_PLOTTER = 0;   // Vector plotter                   ''
        public const int DT_RASDISPLAY = 1;   // Raster display                   ''
        public const int DT_RASPRINTER = 2;   // Raster printer                   ''
        public const int DT_RASCAMERA = 3;   // Raster camera                    ''
        public const int DT_CHARSTREAM = 4;   // Character-stream, PLP            ''
        public const int DT_METAFILE = 5;   // Metafile, VDM                    ''
        public const int DT_DISPFILE = 6;   // Display-file                     ''

        // Public Const Curve = Capabilities ''
        public const int CC_NONE = 0;   // Curves not supported             ''
        public const int CC_CIRCLES = 1;   // Can do circles                   ''
        public const int CC_PIE = 2;   // Can do pie wedges                ''
        public const int CC_CHORD = 4;   // Can do chord arcs                ''
        public const int CC_ELLIPSES = 8;   // Can do ellipese                  ''
        public const int CC_WIDE = 16;  // Can do wide lines                ''
        public const int CC_STYLED = 32;  // Can do styled lines              ''
        public const int CC_WIDESTYLED = 64;  // Can do wide styled lines         ''
        public const int CC_INTERIORS = 128; // Can do interiors                 ''
        public const int CC_ROUNDRECT = 256; //                                  ''

        // Public Const Line = Capabilities ''
        public const int LC_NONE = 0;   // Lines not supported              ''
        public const int LC_POLYLINE = 2;   // Can do polylines                 ''
        public const int LC_MARKER = 4;   // Can do markers                   ''
        public const int LC_POLYMARKER = 8;   // Can do polymarkers               ''
        public const int LC_WIDE = 16;  // Can do wide lines                ''
        public const int LC_STYLED = 32;  // Can do styled lines              ''
        public const int LC_WIDESTYLED = 64;  // Can do wide styled lines         ''
        public const int LC_INTERIORS = 128; // Can do interiors                 ''

        // Public Const Polygonal = Capabilities ''
        public const int PC_NONE = 0;   // Polygonals not supported         ''
        public const int PC_POLYGON = 1;   // Can do polygons                  ''
        public const int PC_RECTANGLE = 2;   // Can do rectangles                ''
        public const int PC_WINDPOLYGON = 4;   // Can do winding polygons          ''
        public const int PC_TRAPEZOID = 4;   // Can do trapezoids                ''
        public const int PC_SCANLINE = 8;   // Can do scanlines                 ''
        public const int PC_WIDE = 16;  // Can do wide borders              ''
        public const int PC_STYLED = 32;  // Can do styled borders            ''
        public const int PC_WIDESTYLED = 64;  // Can do wide styled borders       ''
        public const int PC_INTERIORS = 128; // Can do interiors                 ''
        public const int PC_POLYPOLYGON = 256; // Can do polypolygons              ''
        public const int PC_PATHS = 512; // Can do paths                     ''

        // Public Const Clipping = Capabilities ''
        public const int CP_NONE = 0;   // No clipping of output            ''
        public const int CP_RECTANGLE = 1;   // Output clipped to rects          ''
        public const int CP_REGION = 2;   // obsolete                         ''

        // Public Const Text = Capabilities ''
        public const int TC_OP_CHARACTER = 0x1;  // Can do OutputPrecision   CHARACTER      ''
        public const int TC_OP_STROKE = 0x2;  // Can do OutputPrecision   STROKE         ''
        public const int TC_CP_STROKE = 0x4;  // Can do ClipPrecision     STROKE         ''
        public const int TC_CR_90 = 0x8;  // Can do CharRotAbility    90             ''
        public const int TC_CR_ANY = 0x10;  // Can do CharRotAbility    ANY            ''
        public const int TC_SF_X_YINDEP = 0x20;  // Can do ScaleFreedom      X_YINDEPENDENT ''
        public const int TC_SA_DOUBLE = 0x40;  // Can do ScaleAbility      DOUBLE         ''
        public const int TC_SA_INTEGER = 0x80;  // Can do ScaleAbility      INTEGER        ''
        public const int TC_SA_CONTIN = 0x100;  // Can do ScaleAbility      CONTINUOUS     ''
        public const int TC_EA_DOUBLE = 0x200;  // Can do EmboldenAbility   DOUBLE         ''
        public const int TC_IA_ABLE = 0x400;  // Can do ItalisizeAbility  ABLE           ''
        public const int TC_UA_ABLE = 0x800;  // Can do UnderlineAbility  ABLE           ''
        public const int TC_SO_ABLE = 0x1000;  // Can do StrikeOutAbility  ABLE           ''
        public const int TC_RA_ABLE = 0x2000;  // Can do RasterFontAble    ABLE           ''
        public const int TC_VA_ABLE = 0x4000;  // Can do VectorFontAble    ABLE           ''
        public const int TC_RESERVED = 0x8000;
        public const int TC_SCROLLBLT = 0x10000;  // Don't do text scroll with blt           ''

        // NOGDICAPMASKS ''

        // Public Const Raster = Capabilities ''
        public const int RC_NONE = 0;
        public const int RC_BITBLT = 1;       // Can do standard BLT.             ''
        public const int RC_BANDING = 2;       // Device requires banding support  ''
        public const int RC_SCALING = 4;       // Device requires scaling support  ''
        public const int RC_BITMAP64 = 8;       // Device can support >64K bitmap   ''
        public const int RC_GDI20_OUTPUT = 0x10;      // has 2.0 output calls         ''
        public const int RC_GDI20_STATE = 0x20;
        public const int RC_SAVEBITMAP = 0x40;
        public const int RC_DI_BITMAP = 0x80;      // supports DIB to memory       ''
        public const int RC_PALETTE = 0x100;      // supports a palette           ''
        public const int RC_DIBTODEV = 0x200;      // supports DIBitsToDevice      ''
        public const int RC_BIGFONT = 0x400;      // supports >64K fonts          ''
        public const int RC_STRETCHBLT = 0x800;      // supports StretchBlt          ''
        public const int RC_FLOODFILL = 0x1000;      // supports FloodFill           ''
        public const int RC_STRETCHDIB = 0x2000;      // supports StretchDIBits       ''
        public const int RC_OP_DX_OUTPUT = 0x4000;
        public const int RC_DEVBITS = 0x8000;



        // Public Const Shading = and blending caps ''
        public const int SB_NONE = 0x0;
        public const int SB_CONST_ALPHA = 0x1;
        public const int SB_PIXEL_ALPHA = 0x2;
        public const int SB_PREMULT_ALPHA = 0x4;
        public const int SB_GRAD_RECT = 0x10;
        public const int SB_GRAD_TRI = 0x20;

        // Public Const Color = Management caps ''
        public const int CM_NONE = 0x0;
        public const int CM_DEVICE_ICM = 0x1;
        public const int CM_GAMMA_RAMP = 0x2;
        public const int CM_CMYK_COLOR = 0x4;

        // WINVER >= &H0500 ''


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Brush Styles

        public const int BS_DIBPATTERN = 5;
        public const int BS_DIBPATTERN8X8 = 8;
        public const int BS_DIBPATTERNPT = 6;
        public const int BS_HATCHED = 2;
        public const int BS_NULL = 1;
        public const int BS_HOLLOW = BS_NULL;
        public const int BS_PATTERN = 3;
        public const int BS_PATTERN8X8 = 7;
        public const int BS_SOLID = 0;

        // Hatch brush constants

        public const int HS_BDIAGONAL = 3;
        public const int HS_BDIAGONAL1 = 7;
        public const int HS_CROSS = 4;
        public const int HS_DIAGCROSS = 5;
        public const int HS_DITHEREDBKCLR = 24;
        public const int HS_DITHEREDCLR = 20;
        public const int HS_DITHEREDTEXTCLR = 22;
        public const int HS_FDIAGONAL = 2;
        public const int HS_FDIAGONAL1 = 6;
        public const int HS_HALFTONE = 18;
        public const int HS_HORIZONTAL = 0;
        public const int HS_NOSHADE = 17;
        public const int HS_SOLID = 8;
        public const int HS_SOLIDBKCLR = 23;
        public const int HS_SOLIDCLR = 19;
        public const int HS_SOLIDTEXTCLR = 21;
        public const int HS_VERTICAL = 1;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Pen Styles

        public const int PS_ALTERNATE = 8;
        public const int PS_COSMETIC = 0x0;
        public const int PS_DASH = 1;
        public const int PS_DASHDOT = 3;
        public const int PS_DASHDOTDOT = 4;
        public const int PS_DOT = 2;
        public const int PS_ENDCAP_FLAT = 0x200;
        public const int PS_ENDCAP_MASK = 0xF00;
        public const int PS_ENDCAP_ROUND = 0x0;
        public const int PS_ENDCAP_SQUARE = 0x100;
        public const int PS_GEOMETRIC = 0x10000;
        public const int PS_INSIDEFRAME = 6;
        public const int PS_JOIN_BEVEL = 0x1000;
        public const int PS_JOIN_MASK = 0xF000;
        public const int PS_JOIN_MITER = 0x2000;
        public const int PS_JOIN_ROUND = 0x0;
        public const int PS_NULL = 5;
        public const long PS_SOLID = 0L;
        public const int PS_STYLE_MASK = 0xF;
        public const int PS_PTCMASK = 0xF0000;
        public const int PS_USERSTYLE = 7;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // GetObject object constants
        public const long BI_RGB = 0L;
        public const long BI_RLE8 = 1L;
        public const long BI_RLE4 = 2L;
        public const long BI_BITFIELDS = 3L;
        public const long BI_JPEG = 4L;
        public const long BI_PNG = 5L;
        public const int OBJ_BITMAP = 7;
        public const int OBJ_BRUSH = 2;
        public const int OBJ_DC = 3;
        public const int OBJ_ENHMETADC = 12;
        public const int OBJ_ENHMETAFILE = 13;
        public const int OBJ_EXTPEN = 11;
        public const int OBJ_FONT = 6;
        public const int OBJ_MEMDC = 10;
        public const int OBJ_METADC = 4;
        public const int OBJ_METAFILE = 9;
        public const int OBJ_PAL = 5;
        public const int OBJ_PEN = 1;
        public const int OBJ_REGION = 8;



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // BitBlt flags

        public const int MERGEPAINT = 0xBB0226;
        public const int SRCERASE = 0x440328;
        public const int SRCINVERT = 0x660046;
        public const int SRCPAINT = 0xEE0086;
        public const int SRCCOPY = 0xCC0020;
        public const int MERGECOPY = 0xC000CA;
        public const int NOTSRCCOPY = 0x330008;
        public const int NOTSRCERASE = 0x1100A6;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Draw Modes

        public const int OPAQUE = 2;
        public const int TRANSPARENT = 1;

        // Text Alignment

        public const int TA_BASELINE = 2;
        public const int TA_BOTTOM = 8;
        public const int TA_CENTER = 6;
        public const int TA_LEFT = 0;
        public const int TA_NOUPDATECP = 0;
        public const int TA_RIGHT = 2;
        public const int TA_TOP = 0;
        public const int TA_UPDATECP = 1;
        public const int TA_MASK = TA_BASELINE + TA_CENTER + TA_UPDATECP;
        public const int ETO_GRAYED = 0x1;
        public const int ETO_OPAQUE = 0x2;
        public const int ETO_CLIPPED = 0x4;
        public const int ETO_GLYPH_INDEX = 0x10;
        public const int ETO_RTLREADING = 0x80;
        public const int ETO_NUMERICSLOCAL = 0x400;
        public const int ETO_NUMERICSLATIN = 0x800;
        public const int ETO_IGNORELANGUAGE = 0x1000;
        public const int ETO_PDY = 0x2000;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // Check states.

        public const int DRWCHK_NORMAL = 0;
        public const int DRWCHK_SELECTED = 1;
        public const int DRWCHK_DISABLED = 2;

        // Draw Types

        public const int DST_COMPLEX = 0x0;
        public const int DST_TEXT = 0x1;
        public const int DST_PREFIXTEXT = 0x2;
        public const int DST_ICON = 0x3;
        public const int DST_BITMAP = 0x4;

        // Draw states

        public const int DSS_NORMAL = 0x0;
        public const int DSS_UNION = 0x10;         // Gray string appearance '
        public const int DSS_DISABLED = 0x20;
        public const int DSS_MONO = 0x80;
        public const int DSS_RIGHT = 0x8000;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32", EntryPoint = "SystemParametersInfoW", CharSet = CharSet.Unicode)]
        public static extern int SystemParametersInfo(int uAction, int uParam, ref object lpvParam, int fuWinIni);
        [DllImport("user32", EntryPoint = "SystemParametersInfoW", CharSet = CharSet.Unicode)]
        public static extern int SystemParametersInfo(int uAction, int uParam, [MarshalAs(UnmanagedType.LPStruct)] ref NONCLIENTMETRICS lpvParam, int fuWinIni);
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, int wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, int lParam);
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "PostMessageW", CharSet = CharSet.Unicode)]
        public static extern bool PostMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetLastError();
        [DllImport("kernel32", EntryPoint = "FormatMessageW", CharSet = CharSet.Unicode)]
        public static extern int FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, IntPtr lpBuffer, uint dwSize, IntPtr va_list);

        // Executable assembly version functions for 16, 32 and 64 bit binaries.
        [DllImport("version.dll", EntryPoint = "GetFileVersionInfoW", CharSet = CharSet.Unicode)]
        public static extern bool GetFileVersionInfo([MarshalAs(UnmanagedType.LPTStr)] string lptstrFilename, IntPtr dwHandle, int dwLen, IntPtr lpData);
        [DllImport("Version.dll", EntryPoint = "GetFileVersionInfoSizeW", CharSet = CharSet.Unicode)]
        public static extern uint GetFileVersionInfoSize([MarshalAs(UnmanagedType.LPTStr)] string lptstrFilename, IntPtr lpdwHandle);
        [DllImport("version.dll", EntryPoint = "VerQueryValueW", CharSet = CharSet.Unicode)]
        public static extern bool VerQueryValue(IntPtr pBlock, [MarshalAs(UnmanagedType.LPTStr)] string lpSubBlock, ref IntPtr lplpVoid, ref uint puInt);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetCursorPos(ref W32POINT lpPoint);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CloseHandle(IntPtr hObject);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string name);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPWStr)] string lpProcName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern Delegate GetProcAddressDelegate(IntPtr hModule, [MarshalAs(UnmanagedType.LPWStr)] string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FreeLibrary(IntPtr hInst);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadImage(IntPtr hInst, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, int uType, int cxDesired, int cyDesired, int fuLoad);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadImage(IntPtr hInst, IntPtr lpszName, int uType, int cxDesired, int cyDesired, int fuLoad);
        [DllImport("coredll.dll")]
        public static extern IntPtr LoadIcon(IntPtr hinst, [MarshalAs(UnmanagedType.LPWStr)] string iconName);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode)]
        public static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int nIconIndex, ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIcons);
        [DllImport("shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode)]
        public static extern int ExtractIconEx2([MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int nIconIndex, IntPtr phiconLarge, IntPtr phiconSmall, uint nIcons);
        [DllImport("ntshrui.dll", EntryPoint = "ShowShareFolderUIW", CharSet = CharSet.Unicode)]
        public static extern int ShowShareFolderUI(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string lpszFolder);

        // Public Declare Unicode Function GetCurrentDirectory Lib "kernel32" Alias "GetCurrentDirectoryW" (bLen As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef lpszBuffer As String) As Integer

        // Public Declare Unicode Function GetFullPathName Lib "kernel32" Alias "GetFullPathNameW" (<MarshalAs(UnmanagedType.LPWStr)> lpFilename As String, nBufferLength As Integer, lpBuffer As IntPtr, ByRef lpFilepart As IntPtr) As Integer

        [DllImport("kernel32", EntryPoint = "ReplaceFileW", CharSet = CharSet.Unicode)]
        public static extern bool ReplaceFile([MarshalAs(UnmanagedType.LPWStr)] string lpReplacedFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpReplacementName, [MarshalAs(UnmanagedType.LPWStr)] string lpBackupFileName, ReplaceFileFlags dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);
        [DllImport("Shell32", CharSet = CharSet.Unicode)]
        public static extern int SHBrowseForFolder(BROWSEINFO bInfo);
        [DllImport("Shell32", EntryPoint = "SHGetSpecialFolderPathW", CharSet = CharSet.Unicode)]
        public static extern int SHGetSpecialFolderPath(IntPtr hWnd, string lpPath, int nFolder, bool fCreate);
        [DllImport("Shell32", CharSet = CharSet.Unicode)]
        public static extern int SHGetPathFromIDList(int pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPath);
        [DllImport("shell32.dll", EntryPoint = "ShellExecuteExW", CharSet = CharSet.Unicode)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpInfo);
        [DllImport("shell32.dll", EntryPoint = "ShellExecuteExW", CharSet = CharSet.Unicode)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFOPTR lpInfo);
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfoW", CharSet = CharSet.Unicode)]
        public static extern int SHGetFileInfo([MarshalAs(UnmanagedType.LPWStr)] string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfoW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetItemInfo(IntPtr pidl, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);
        [DllImport("shell32.dll", EntryPoint = "SHFileOperationW", CharSet = CharSet.Unicode)]
        public static extern int SHFileOperation(SHFILEOPSTRUCT lpFileOp);
        [DllImport("shell32.dll", EntryPoint = "SHGetDataFromIDListW", CharSet = CharSet.Unicode)]
        public static extern int SHGetDataFromIDList([MarshalAs(UnmanagedType.Interface)] object lpFolder, int pidl, int nFormat, IntPtr pv, int cb);
        [DllImport("shell32.dll", EntryPoint = "#660", CharSet = CharSet.Unicode)]
        public static extern bool FileIconInit([MarshalAs(UnmanagedType.Bool)] bool fRestoreCache);

        public static string mBrowseCurrent;
        public static int BrowserLastFolder;

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int CopyImage(IntPtr Handle, int un1, int n1, int n2, int un2);
        [DllImport("comctl32.dll", CharSet = CharSet.Unicode)]
        public static extern int ImageList_GetImageInfo(IntPtr himl, int i, ref IMAGEINFO pImageInfo);
        [DllImport("comctl32.dll", CharSet = CharSet.Unicode)]
        public static extern int ImageList_GetIcon(IntPtr himl, int i, uint flags);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IntPtr hIml);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int DestroyIcon(IntPtr hIcon);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW")]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int code, IntPtr value);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW")]
        public static extern WndProcDelegate SetWindowLongPtr(IntPtr hWnd, int code, WndProcDelegate value);


        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern bool MoveWindow(
          IntPtr hWnd,
          int X,
          int Y,
          int nWidth,
          int nHeight,
          bool bRepaint
        );

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref W32RECT rc);
        [DllImport("user32", EntryPoint = "GetWindowLongW", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int code);
        [DllImport("user32", EntryPoint = "GetWindowLongPtrW", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int code);


        [DllImport("user32", EntryPoint = "GetClassLongW", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetClassLong(IntPtr hWnd, int nIndex);

        [DllImport("user32", EntryPoint = "GetClassLongPtrW", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);


        public static System.Drawing.Icon GetWindowIcon(IntPtr hwnd, int iconType = 0)
        {
            IntPtr hIcon = SendMessage(hwnd, WM_GETICON, iconType, 96);
            if (hIcon != IntPtr.Zero)
            {
                var ico = (Icon)Icon.FromHandle(hIcon).Clone();
                return ico;
            }

            if (IntPtr.Size == 8)
            {
                hIcon = GetClassLongPtr(hwnd, GCLP_HICON);
            }
            else
            {
                hIcon = GetClassLong(hwnd, GCLP_HICON);
            }

            if (hIcon != IntPtr.Zero)
            {
                var ico = (Icon)Icon.FromHandle(hIcon).Clone();
                return ico;
            }

            return null;

        }



        [DllImport("user32", EntryPoint = "GetWindowModuleFileNameW", CharSet = CharSet.Unicode)]
        public static extern uint GetWindowModuleFileName(
                                  IntPtr hwnd,
                                  StringBuilder pszFileName,
                                  int cchFileNameMax
                                  );


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumChildWindows(
              IntPtr hWndParent,
              [MarshalAs(UnmanagedType.FunctionPtr)] 
              EnumWindowsProc lpEnumFunc,
              IntPtr lParam
            );

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumChildWindows(
              IntPtr hWndParent,
              [MarshalAs(UnmanagedType.FunctionPtr)]
              EnumWindowsProcObj lpEnumFunc,
              object lParam
            );

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);
        [DllImport("user32.dll")]
        public static extern uint RealGetWindowClass(IntPtr hWnd, StringBuilder pszType, uint cchType);

        [DllImport("user32", EntryPoint = "CreateWindowExW", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx(uint dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern bool DestroyWindow(IntPtr hwnd);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "FindWindowW")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "FindWindowExW")]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW")]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetSysColor(int nIndex);
        [DllImport("user32")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, [Optional, DefaultParameterValue(default(int))] ref int lpdwProcessId);

        public const int DF_ALLOWOTHERACCOUNTHOOK = 1;

        /// <summary>
        /// Opens the Input Desktop.
        /// </summary>
        /// <param name="dwFlags">Optional Flags</param>
        /// <param name="fInherit">If this is set, all child processes will inherit this handle.</param>
        /// <param name="dwDesiredAccess">Desktop access mask.</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern IntPtr OpenInputDesktop(int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, DESKTOP_ACCESS_MASK dwDesiredAccess);
        [DllImport("user32")]
        public static extern bool CloseDesktop(IntPtr hDesk);
        [DllImport("user32")]
        public static extern IntPtr GetThreadDesktop(int dwThreadId);

        public delegate bool EnumWindowsProcObj(IntPtr hwnd, object lParam);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern bool EnumDesktopWindows(IntPtr hDesk, [MarshalAs(UnmanagedType.FunctionPtr)] EnumWindowsProc lpfn, IntPtr lParam);

        /// <summary>
        /// Returns an array of all current top-level windows HWND pointers on the current desktop.
        /// </summary>
        /// <returns>Array of HWNDs as IntPtr</returns>
        public static IntPtr[] GetCurrentDesktopWindows()
        {
            return GetDesktopWindows(IntPtr.Zero);
        }

        /// <summary>
        /// Returns an array of all current top-level windows HWND pointers on the current desktop.
        /// </summary>
        /// <returns>Array of HWNDs as IntPtr</returns>
        public static IntPtr[] GetDesktopWindows(IntPtr hDesk)
        {
            var l = new List<IntPtr>();
            EnumDesktopWindows(hDesk, new EnumWindowsProc((hwnd, lParam) =>
            {
                l.Add(hwnd);
                return true;
            }), IntPtr.Zero);
            return l.ToArray();
        }

        public static IEnumerable<Process> GetDesktopProcesses(IntPtr hDesk = default)
        {
            var l = new List<Process>();
            var hwnd = GetDesktopWindows(hDesk);
            int pid = 0;
            foreach (var h in hwnd)
            {
                GetWindowThreadProcessId(h, ref pid);
                l.Add(Process.GetProcessById(pid));
            }

            l.Sort(new Comparison<Process>((x, y) => { try { return string.Compare(x.ProcessName, y.ProcessName); } catch { return 0; } }));
            var t = new List<Process>();
            Process c = null;
            foreach (var p in l)
            {
                if (c is null || c.Id != p.Id)
                {
                    c = p;
                    t.Add(c);
                }
            }

            return t;
        }

        public static Process GetWindowProcess(IntPtr hwnd)
        {
            int pid = 0;
            GetWindowThreadProcessId(hwnd, ref pid);
            return Process.GetProcessById(pid);
        }

        public static string GetChromeWindowUrl(string title)
        {
            IntPtr hwnd = FindWindow("Chrome_WidgetWin_1", title);

            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(hwnd);
            AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
              new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
            
            // if it can be found, get the value from the URL bar
            if (elmUrlBar != null)
            {
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                if (patterns.Length > 0)
                {
                    ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                    return val.Current.Value;
                }
            }

            return null;
        }

        public static string GetWindowName(IntPtr Hwnd)
        {
            // This function gets the name of a window from its handle
            StringBuilder Title = new StringBuilder(256);
            GetWindowText(Hwnd, Title, 256);

            return Title.ToString().Trim();
        }



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("user32", EntryPoint = "GetMessageW", CharSet = CharSet.Unicode)]
        public static extern int GetMessage([MarshalAs(UnmanagedType.LPStruct)] ref MSG lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);
        [DllImport("user32", EntryPoint = "PeekMessageW", CharSet = CharSet.Unicode)]
        public static extern int PeekMessage([MarshalAs(UnmanagedType.LPStruct)] ref MSG lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int TranslateMessage([MarshalAs(UnmanagedType.LPStruct)] ref MSG lpMsg);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetKeyState(int nVirtKey);
        [DllImport("user32", EntryPoint = "CopyAcceleratorTableW", CharSet = CharSet.Unicode)]
        public static extern int CopyAcceleratorTable(IntPtr hAccelSrc, IntPtr lpAccelDst, int cAccelEntries);
        [DllImport("user32", EntryPoint = "CreateAcceleratorTableW", CharSet = CharSet.Unicode)]
        public static extern int CreateAcceleratorTable(IntPtr lpaccl, int cEntries);
        [DllImport("user32", EntryPoint = "TranslateAcceleratorW", CharSet = CharSet.Unicode)]
        public static extern int TranslateAccelerator(IntPtr hWnd, IntPtr hAccTable, [MarshalAs(UnmanagedType.LPStruct)] ref MSG lpMsg);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DestroyAcceleratorTable(IntPtr haccel);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int TrackPopupMenu(IntPtr Handle, uint wFlags, short x, short y, short nReserved, IntPtr hWnd, IntPtr lprc);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr TrackPopupMenuEx(IntPtr Handle, uint wFlags, int x, int y, IntPtr hWnd, IntPtr lpTPMParams);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DrawMenuBar(IntPtr hWnd);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreatePopupMenu();
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateMenu();
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DeleteMenu(IntPtr Handle, int nPosition, int wFlags);
        [DllImport("user32", EntryPoint = "InsertMenuItemW", CharSet = CharSet.Unicode)]
        public static extern int InsertMenuItem(IntPtr Handle, int un, bool @bool, [MarshalAs(UnmanagedType.Struct)] ref MENUITEMINFO lpcMenuItemInfo);
        [DllImport("user32", EntryPoint = "AppendMenuW", CharSet = CharSet.Unicode)]
        public static extern int AppendMenu(IntPtr Handle, int wFlags, int wIDNewItem, IntPtr lpNewItem);
        [DllImport("user32", EntryPoint = "InsertMenuW", CharSet = CharSet.Unicode)]
        public static extern int InsertMenu(IntPtr Handle, int nPosition, int wFlags, int wIDNewItem, IntPtr lpNewItem);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int RemoveMenu(IntPtr Handle, int nPosition, int wFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DestroyMenu(IntPtr Handle);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetSystemMenu(int hWnd, int bRevert);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuItemCount(IntPtr Handle);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuItemID(IntPtr Handle, int nPos);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuDefaultItem(IntPtr Handle, [MarshalAs(UnmanagedType.Bool)] bool fByPos, int gmdiFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuContextHelpId(IntPtr Handle);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuCheckMarkDimensions();
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenu(int hWnd);
        [DllImport("user32", EntryPoint = "GetMenuItemInfoW", CharSet = CharSet.Unicode)]
        public static extern int GetMenuItemInfo(IntPtr Handle, int un, [MarshalAs(UnmanagedType.Bool)] bool fByItem, ref MENUITEMINFO lpMenuItemInfo);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuInfo(IntPtr hMenu, MENUINFO lpcmi);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuBarInfo(IntPtr hWnd, int idObject, int idItem, [MarshalAs(UnmanagedType.LPStruct)] ref MENUBARINFO pmbi);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuItemRect(IntPtr hWnd, IntPtr Handle, int uItem, [MarshalAs(UnmanagedType.LPStruct)] ref W32RECT lprcItem);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetMenuState(IntPtr Handle, int wID, int wFlags);
        [DllImport("user32", EntryPoint = "GetMenuStringW", CharSet = CharSet.Unicode)]
        public static extern int GetMenuString(IntPtr Handle, int wIDItem, [MarshalAs(UnmanagedType.LPWStr)] string lpString, int nMaxCount, int wFlag);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetMenuInfo(IntPtr hMenu, [MarshalAs(UnmanagedType.LPStruct)] MENUINFO lpcmi);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetMenu(IntPtr hWnd, IntPtr Handle);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetMenuContextHelpId(IntPtr Handle, int dw);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetMenuDefaultItem(IntPtr Handle, int uItem, int fByPos);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetMenuItemBitmaps(IntPtr Handle, int nPosition, int wFlags, int hBitmapUnchecked, int hBitmapChecked);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);
        [DllImport("user32", EntryPoint = "SetMenuItemInfoW", CharSet = CharSet.Unicode)]
        public static extern int SetMenuItemInfo(IntPtr Handle, int un, [MarshalAs(UnmanagedType.Bool)] bool fByItem, ref MENUITEMINFO lpcMenuItemInfo);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetScrollInfo(IntPtr hWnd, int n, [MarshalAs(UnmanagedType.LPStruct)] ref SCROLLINFO lpcScrollInfo, bool @bool);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, int bRedraw);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int SetScrollRange(IntPtr hWnd, int nBar, int nMinPos, int nMaxPos, int bRedraw);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetScrollInfo(IntPtr hWnd, int n, [MarshalAs(UnmanagedType.LPStruct)] ref SCROLLINFO lpScrollInfo);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int EnableScrollBar(IntPtr hWnd, int wSBflags, int wArrows);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentObject(IntPtr hDC, int uObjectType);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetStockObject(int nIndex);

        // Pen
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetObject(IntPtr hObject, int nCount, [MarshalAs(UnmanagedType.Struct)] LOGPEN lpObject);

        // Brush
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetObject(IntPtr hObject, int nCount, [MarshalAs(UnmanagedType.Struct)] LOGBRUSH lpObject);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int ExtCreatePen(int dwPenStyle, int dwWidth, [MarshalAs(UnmanagedType.Struct)] LOGBRUSH lplb, int dwStyleCount, int lpStyle);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int CreatePenIndirect([MarshalAs(UnmanagedType.Struct)] LOGPEN lpLogPen);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int CreatePen(int nPenStyle, int nWidth, int crColor);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int CreateBrushIndirect([MarshalAs(UnmanagedType.Struct)] LOGBRUSH lpLogBrush);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateIconIndirect([MarshalAs(UnmanagedType.Struct)] ref ICONINFO lpIconInfo);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateBitmapIndirect([MarshalAs(UnmanagedType.Struct)] ref BITMAPSTRUCT lpBitmap);

        // HDC CreateDC(
        // LPCTSTR lpszDriver,
        // _In_  LPCTSTR lpszDevice,
        // LPCTSTR lpszOutput,
        // _In_  const DEVMODE *lpInitData
        // );

        [DllImport("gdi32")]
        public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32", EntryPoint = "CreateDCW", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr devMode);
        [DllImport("gdi32")]
        public static extern int GetDeviceCaps(IntPtr hDc, int index);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int MoveToEx(IntPtr hDC, int nXOrg, int nYOrg, [MarshalAs(UnmanagedType.Struct)] ref W32POINT lppt);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int LineTo(IntPtr hDC, int x, int y);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GdiFlush();
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int FillRect(IntPtr hDC, [MarshalAs(UnmanagedType.Struct)] ref W32RECT lpRect, int hBrush);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int BeginPaint(IntPtr hWnd, [MarshalAs(UnmanagedType.Struct)] ref PAINTSTRUCT lpPaint);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int EndPaint(IntPtr hWnd, [MarshalAs(UnmanagedType.Struct)] ref PAINTSTRUCT lpPaint);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int SetTextAlign(IntPtr hDC, int wFlags);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetTextAlign(IntPtr hDC);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int SetBkMode(IntPtr hDC, int nMode);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DrawTextW(IntPtr hDC, [MarshalAs(UnmanagedType.LPWStr)] ref string lpStr, int nCount, [MarshalAs(UnmanagedType.Struct)] ref W32RECT lpRect, int wFormat);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int TextOutW(IntPtr hDC, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] ref string lpString, int nCount);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int ExtTextOutW(IntPtr hDC, int x, int y, int wOptions, [MarshalAs(UnmanagedType.Struct)] ref W32RECT lpRect, [MarshalAs(UnmanagedType.LPWStr)] ref string lpString, int nCount, int lpDx);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetTextExtentPoint32W(IntPtr hDC, [MarshalAs(UnmanagedType.LPWStr)] ref string lpsz, int cbString, [MarshalAs(UnmanagedType.LPStruct)] ref W32SIZE lpSize);
        [DllImport("user32", CharSet = CharSet.Ansi)]
        public static extern int DrawTextA(IntPtr hDC, [MarshalAs(UnmanagedType.LPStr)] ref string lpStr, int nCount, [MarshalAs(UnmanagedType.Struct)] ref W32RECT lpRect, int wFormat);
        [DllImport("gdi32", CharSet = CharSet.Ansi)]
        public static extern int TextOutA(IntPtr hDC, int x, int y, [MarshalAs(UnmanagedType.LPStr)] ref string lpString, int nCount);
        [DllImport("gdi32", CharSet = CharSet.Ansi)]
        public static extern int ExtTextOutA(IntPtr hDC, int x, int y, int wOptions, ref W32RECT lpRect, [MarshalAs(UnmanagedType.LPStr)] ref string lpString, int nCount, int lpDx);
        [DllImport("gdi32", CharSet = CharSet.Ansi)]
        public static extern int GetTextExtentPoint32A(IntPtr hDC, [MarshalAs(UnmanagedType.LPStr)] ref string lpsz, int cbString, [MarshalAs(UnmanagedType.Struct)] ref W32SIZE lpSize);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int GetFontLanguageInfo(IntPtr hDC);
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int SetTextColor(IntPtr hDC, int crColor);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DrawFrameControl(IntPtr hDC, [MarshalAs(UnmanagedType.Struct)] ref W32RECT lpRect, int un1, int un2);
        // draw caption text

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int DrawCaption(IntPtr hWnd, IntPtr hDC, [MarshalAs(UnmanagedType.Struct)] ref W32RECT pcRect, int un);
        
        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern int BitBlt(IntPtr dest, int x, int y, int cx, int cy, IntPtr src, int x2, int y2, int dwROP);

        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr pbmi, uint usage, ref IntPtr ppvBits, IntPtr hSection, int offset);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // int CALLBACK EnumFontFamExProc(
        // Const LOGFONT    *lpelfe,
        // Const TEXTMETRIC *lpntme,
        // DWORD      FontType,
        // LPARAM     lParam
        // );




        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32", EntryPoint = "RtlZeroMemory")]
        static extern void RtlZeroMemory(IntPtr pDst, long ByteLen);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */



        [DllImport("kernel32", EntryPoint = "GetModuleFileNameW")]
        public static extern int GetModuleFileName(
            IntPtr hModule,
            StringBuilder lpFilename,
            int nSize
        );



        //[DllImport("kernel32")]
        //public static extern int GetWindowThreadProcessId(
        //    IntPtr hWnd,
        //    out int lpdwProcessId
        //);


    }
}
