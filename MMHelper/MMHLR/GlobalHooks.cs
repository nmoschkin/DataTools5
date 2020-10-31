// Credit goes to Chris Wilson for pretty much all of this.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataTools.Win32Api;
using static DataTools.Win32Api.User32;
using static DataTools.Win32Api.DevNotify;
using System.Diagnostics;
using DataTools.Memory;

#if X64
namespace MMHLR64
#else
namespace MMHLR32
#endif
{
    public class GlobalHooks : NativeWindow
    {

        public delegate void HookReplacedEventHandler();
        public delegate void WindowEventHandler(IntPtr Handle);
        public delegate void WindowMoveSizeEventHandler(IntPtr Handle, W32RECT rc);
        public delegate void WindowReplacedEventHandler(IntPtr OldHandle, IntPtr NewHandle);
        public delegate void SysCommandEventHandler(int SysCommand, int lParam);
        public delegate void ActivateShellWindowEventHandler();
        public delegate void TaskmanEventHandler();
        public delegate void BasicHookEventHandler(IntPtr Handle1, IntPtr Handle2);
        public delegate void WndProcEventHandler(IntPtr Handle, IntPtr Message, IntPtr wParam, IntPtr lParam);

        public delegate void HardwareChangedEventHandler(object sender, HardwareChangedEventArgs e);
        public event HardwareChangedEventHandler HardwareChanged;


        public delegate void SystemShutdownEventHandler(object sender, SystemShutdownEventArgs e);
        public event SystemShutdownEventHandler SystemShutdown;

        // Functions imported from our unmanaged DLL

#if X64

        [DllImport("MMHookLib64.dll")]
        private static extern bool InitializeCbtHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeCbtHook();
        [DllImport("MMHookLib64.dll")]
        private static extern bool InitializeShellHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeShellHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeKeyboardHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeKeyboardHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeMouseHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeMouseHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeKeyboardLLHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeKeyboardLLHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeMouseLLHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeMouseLLHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeCallWndProcHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeCallWndProcHook();
        [DllImport("MMHookLib64.dll")]
        private static extern void InitializeGetMsgHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib64.dll")]
        private static extern void UninitializeGetMsgHook();

        
        private const string HookPrefix = "NNM_HOOK_64_";

#else

        [DllImport("MMHookLib.dll")]
        private static extern bool InitializeCbtHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeCbtHook();
        [DllImport("MMHookLib.dll")]
        private static extern bool InitializeShellHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeShellHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeKeyboardHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeKeyboardHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeMouseHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeMouseHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeKeyboardLLHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeKeyboardLLHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeMouseLLHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeMouseLLHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeCallWndProcHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeCallWndProcHook();
        [DllImport("MMHookLib.dll")]
        private static extern void InitializeGetMsgHook(int threadID, IntPtr DestWindow);
        [DllImport("MMHookLib.dll")]
        private static extern void UninitializeGetMsgHook();


        private const string HookPrefix = "NNM_HOOK_";

#endif

        // API call needed to retreive the value of the messages to intercept from the unmanaged DLL
        [DllImport("user32.dll")]
        private static extern int RegisterWindowMessage(string lpString);
        [DllImport("user32.dll")]
        private static extern IntPtr GetProp(IntPtr hWnd, string lpString);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        // Handle of the window intercepting messages
        private IntPtr _Handle;

        private CBTHook _CBT;
        private ShellHook _Shell;
        private KeyboardHook _Keyboard;
        private MouseHook _Mouse;
        private KeyboardLLHook _KeyboardLL;
        private MouseLLHook _MouseLL;
        private CallWndProcHook _CallWndProc;
        private GetMsgHook _GetMsg;

        private IntPtr dnHandle = IntPtr.Zero;

        public GlobalHooks(IntPtr Handle)
        {
            CreateParams cp = new CreateParams()
            {
#if X64
                Caption = "MMWndT MMHLR64 Window Shell Monitor"
#else
                Caption = "MMWndT MMHLR32 Window Shell Monitor"
#endif
            };

            CreateHandle(cp);

            _Handle = this.Handle;

            _CBT = new CBTHook(_Handle);
            _Shell = new ShellHook(_Handle);
            _Keyboard = new KeyboardHook(_Handle);
            _Mouse = new MouseHook(_Handle);
            _KeyboardLL = new KeyboardLLHook(_Handle);
            _MouseLL = new MouseLLHook(_Handle);
            _CallWndProc = new CallWndProcHook(_Handle);
            _GetMsg = new GetMsgHook(_Handle);

#if X64
            dnHandle = DoRegisterDeviceClassNotification(Handle, DevProp.GUID_DEVINTERFACE_MONITOR);
#endif
        }

        ~GlobalHooks()
        {
#if X64

            if (dnHandle != IntPtr.Zero)
                UnregisterDeviceNotification(dnHandle);
#endif

            DestroyHandle();

            _CBT?.Stop();
            _Shell?.Stop();
            _Keyboard?.Stop();
            _Mouse?.Stop();
            _KeyboardLL?.Stop();
            _MouseLL?.Stop();
            _CallWndProc?.Stop();
            _GetMsg?.Stop();
        }

        protected override void WndProc(ref Message m)
        {
#if X64
            if (m.Msg == WM_DEVICECHANGE)
            {
                string sDisplay = "";

                uint msg = (uint)m.WParam;

                if (m.LParam != IntPtr.Zero)
                {
                    //Debugger.Launch();

                    try
                    {
                        MemPtr mm = m.LParam;
                        var ofs = Marshal.SizeOf<DEV_BROADCAST_HDR>() + Marshal.SizeOf<DEV_BROADCAST_DEVICEINTERFACE>() - sizeof(char);
                        sDisplay = mm.GetString(ofs);
                    }
                    catch
                    {

                    }

                }

                HardwareChanged?.Invoke(this, new HardwareChangedEventArgs((uint)m.WParam, sDisplay));
            }
            else if (m.Msg == WM_QUERYENDSESSION)
            {
                SystemShutdown?.Invoke(this, new SystemShutdownEventArgs(m.WParam, m.LParam));                
            }
            else
            {
                ProcessWindowMessage(ref m);
            }
#else
            ProcessWindowMessage(ref m);
#endif


            if (m.Msg == WM_QUERYENDSESSION)
            {
                SystemShutdown?.Invoke(this, new SystemShutdownEventArgs(m.WParam, m.LParam));
            }

            base.WndProc(ref m);
        }

        public void ProcessWindowMessage(ref System.Windows.Forms.Message m)
        {
            _CBT?.ProcessWindowMessage(ref m);
            _Shell?.ProcessWindowMessage(ref m);
            _Keyboard?.ProcessWindowMessage(ref m);
            _Mouse?.ProcessWindowMessage(ref m);
            _KeyboardLL?.ProcessWindowMessage(ref m);
            _MouseLL?.ProcessWindowMessage(ref m);
            _CallWndProc?.ProcessWindowMessage(ref m);
            _GetMsg?.ProcessWindowMessage(ref m);
        }

#region Accessors

        public CBTHook CBT
        {
            get { return _CBT; }
        }

        public ShellHook Shell
        {
            get { return _Shell; }
        }

        public KeyboardHook Keyboard
        {
            get { return _Keyboard; }
        }

        public MouseHook Mouse
        {
            get { return _Mouse; }
        }

        public KeyboardLLHook KeyboardLL
        {
            get { return _KeyboardLL; }
        }

        public MouseLLHook MouseLL
        {
            get { return _MouseLL; }
        }

        public CallWndProcHook CallWndProc
        {
            get { return _CallWndProc; }
        }

        public GetMsgHook GetMsg
        {
            get { return _GetMsg; }
        }

#endregion

        public abstract class Hook
        {
            protected bool _IsActive = false;
            protected IntPtr _Handle;

            public Hook(IntPtr Handle)
            {
                _Handle = Handle;
            }

            public void Start()
            {
                if (!_IsActive)
                {
                    _IsActive = true;
                    OnStart();
                }
            }

            public void Stop()
            {
                if (_IsActive)
                {
                    OnStop();
                    _IsActive = false;
                }
            }

            ~Hook()
            {
                Stop();
            }

            public bool IsActive
            {
                get { return _IsActive; }
            }

            protected abstract void OnStart();
            protected abstract void OnStop();
            public abstract void ProcessWindowMessage(ref System.Windows.Forms.Message m);
        }

        public class CBTHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_CBT_HookReplaced {get; private set; }
            public int MsgID_CBT_Activate {get; private set; }
            public int MsgID_CBT_CreateWnd {get; private set; }
            public int MsgID_CBT_DestroyWnd {get; private set; }
            public int MsgID_CBT_MinMax {get; private set; }
            public int MsgID_CBT_MoveSize {get; private set; }
            public int MsgID_CBT_SetFocus {get; private set; }
            public int MsgID_CBT_SysCommand {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event WindowEventHandler Activate;
            public event WindowEventHandler CreateWindow;
            public event WindowEventHandler DestroyWindow;
            public event WindowEventHandler MinMax;
            public event WindowMoveSizeEventHandler MoveSize;
            public event WindowEventHandler SetFocus;
            public event SysCommandEventHandler SysCommand;

            public CBTHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_CBT_HookReplaced = RegisterWindowMessage(HookPrefix + "CBT_REPLACED");
                MsgID_CBT_Activate = RegisterWindowMessage(HookPrefix + "HCBT_ACTIVATE");
                MsgID_CBT_CreateWnd = RegisterWindowMessage(HookPrefix + "HCBT_CREATEWND");
                MsgID_CBT_DestroyWnd = RegisterWindowMessage(HookPrefix + "HCBT_DESTROYWND");
                MsgID_CBT_MinMax = RegisterWindowMessage(HookPrefix + "HCBT_MINMAX");
                MsgID_CBT_MoveSize = RegisterWindowMessage(HookPrefix + "HCBT_MOVESIZE");
                MsgID_CBT_SetFocus = RegisterWindowMessage(HookPrefix + "HCBT_SETFOCUS");
                MsgID_CBT_SysCommand = RegisterWindowMessage(HookPrefix + "HCBT_SYSCOMMAND");

                // Start the hook
                InitializeCbtHook(0, _Handle);
            }

            protected override void OnStop()
            {
                UninitializeCbtHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_CBT_HookReplaced)
                {
                    HookReplaced?.Invoke();
                }
                else if (m.Msg == MsgID_CBT_Activate)
                {
                    Activate?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_CBT_CreateWnd)
                {
                    CreateWindow?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_CBT_DestroyWnd)
                {
                    DestroyWindow?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_CBT_MinMax)
                {
                    MinMax?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_CBT_MoveSize)
                {
                    W32RECT rc = new W32RECT();
                    //try
                    //{
                    //    unsafe
                    //    {
                    //        rc = *((W32RECT*)m.LParam);
                    //    }

                    //    //rc = (W32RECT)m.GetLParam(typeof(W32RECT));
                    //    //rc = Marshal.PtrToStructure<W32RECT>(m.LParam);
                    //}
                    //catch(Exception ex)
                    //{
                    //    Console.WriteLine("Error: " + ex.Message);
                    //}

                    MoveSize?.Invoke(m.WParam, rc);
                }
                else if (m.Msg == MsgID_CBT_SetFocus)
                {
                    SetFocus?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_CBT_SysCommand)
                {
                    SysCommand?.Invoke(m.WParam.ToInt32(), m.LParam.ToInt32());
                }
            }
        }

        public class ShellHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_Shell_ActivateShellWindow {get; private set; }
            public int MsgID_Shell_GetMinRect {get; private set; }
            public int MsgID_Shell_Language {get; private set; }
            public int MsgID_Shell_Redraw {get; private set; }
            public int MsgID_Shell_Taskman {get; private set; }
            public int MsgID_Shell_HookReplaced {get; private set; }
            public int MsgID_Shell_WindowActivated {get; private set; }
            public int MsgID_Shell_WindowCreated {get; private set; }
            public int MsgID_Shell_WindowDestroyed {get; private set; }
            public int MsgID_Shell_WindowReplaced { get; private set; }
            public int MsgID_Shell_WindowReplacing { get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event ActivateShellWindowEventHandler ActivateShellWindow;
            public event WindowEventHandler GetMinRect;
            public event WindowEventHandler Language;
            public event WindowEventHandler Redraw;
            public event TaskmanEventHandler Taskman;
            public event WindowEventHandler WindowActivated;
            public event WindowEventHandler WindowCreated;
            public event WindowEventHandler WindowDestroyed;
            public event WindowReplacedEventHandler WindowReplaced;
            public event WindowReplacedEventHandler WindowReplacing;

            public ShellHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_Shell_HookReplaced = RegisterWindowMessage(HookPrefix + "SHELL_REPLACED");
                MsgID_Shell_ActivateShellWindow = RegisterWindowMessage(HookPrefix + "HSHELL_ACTIVATESHELLWINDOW");
                MsgID_Shell_GetMinRect = RegisterWindowMessage(HookPrefix + "HSHELL_GETMINRECT");
                MsgID_Shell_Language = RegisterWindowMessage(HookPrefix + "HSHELL_LANGUAGE");
                MsgID_Shell_Redraw = RegisterWindowMessage(HookPrefix + "HSHELL_REDRAW");
                MsgID_Shell_Taskman = RegisterWindowMessage(HookPrefix + "HSHELL_TASKMAN");
                MsgID_Shell_WindowActivated = RegisterWindowMessage(HookPrefix + "HSHELL_WINDOWACTIVATED");
                MsgID_Shell_WindowCreated = RegisterWindowMessage(HookPrefix + "HSHELL_WINDOWCREATED");
                MsgID_Shell_WindowDestroyed = RegisterWindowMessage(HookPrefix + "HSHELL_WINDOWDESTROYED");
                MsgID_Shell_WindowReplaced = RegisterWindowMessage(HookPrefix + "HSHELL_WINDOWREPLACED");
                MsgID_Shell_WindowReplacing = RegisterWindowMessage(HookPrefix + "HSHELL_WINDOWREPLACING");

                // Start the hook
                var b = InitializeShellHook(0, _Handle);

            }

            protected override void OnStop()
            {
                UninitializeShellHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_Shell_HookReplaced)
                {
                    HookReplaced?.Invoke();
                }
                else if (m.Msg == MsgID_Shell_ActivateShellWindow)
                {
                    ActivateShellWindow?.Invoke();
                }
                else if (m.Msg == MsgID_Shell_GetMinRect)				
                {
                    GetMinRect?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_Language)
                {
                    Language?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_Redraw)
                {
                    Redraw?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_Taskman)
                {
                    Taskman?.Invoke();
                }
                else if (m.Msg == MsgID_Shell_WindowActivated)
                {
                    WindowActivated?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_WindowCreated)
                {
                    WindowCreated?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_WindowDestroyed)
                {
                    WindowDestroyed?.Invoke(m.WParam);
                }
                else if (m.Msg == MsgID_Shell_WindowReplaced)
                {
                    WindowReplaced?.Invoke(m.WParam, m.LParam);
                }
                else if (m.Msg == MsgID_Shell_WindowReplacing)
                {
                    WindowReplacing?.Invoke(m.WParam, m.LParam);
                }

            }
        }

        public class KeyboardHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_Keyboard {get; private set; }
            public int MsgID_Keyboard_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event BasicHookEventHandler KeyboardEvent;

            public KeyboardHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_Keyboard = RegisterWindowMessage(HookPrefix + "KEYBOARD");
                MsgID_Keyboard_HookReplaced = RegisterWindowMessage(HookPrefix + "KEYBOARD_REPLACED");

                // Start the hook
                InitializeKeyboardHook(0, _Handle);
            }
            protected override void OnStop()
            {
                UninitializeKeyboardHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_Keyboard)
                {
                    if (KeyboardEvent != null)
                        KeyboardEvent(m.WParam, m.LParam);
                }
                else if (m.Msg == MsgID_Keyboard_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }

        public class MouseHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_Mouse {get; private set; }
            public int MsgID_Mouse_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event BasicHookEventHandler MouseEvent;

            public MouseHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_Mouse = RegisterWindowMessage(HookPrefix + "MOUSE");
                MsgID_Mouse_HookReplaced = RegisterWindowMessage(HookPrefix + "MOUSE_REPLACED");

                // Start the hook
                InitializeMouseHook(0, _Handle);
            }
            protected override void OnStop()
            {
                UninitializeMouseHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_Mouse)
                {
                    if (MouseEvent != null)
                        MouseEvent(m.WParam, m.LParam);
                }
                else if (m.Msg == MsgID_Mouse_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }

        public class KeyboardLLHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_KeyboardLL {get; private set; }
            public int MsgID_KeyboardLL_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event BasicHookEventHandler KeyboardLLEvent;

            public KeyboardLLHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_KeyboardLL = RegisterWindowMessage(HookPrefix + "KEYBOARDLL");
                MsgID_KeyboardLL_HookReplaced = RegisterWindowMessage(HookPrefix + "KEYBOARDLL_REPLACED");

                // Start the hook
                InitializeKeyboardLLHook(0, _Handle);
            }

            protected override void OnStop()
            {
                UninitializeKeyboardLLHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_KeyboardLL)
                {
                    if (KeyboardLLEvent != null)
                        KeyboardLLEvent(m.WParam, m.LParam);
                }
                else if (m.Msg == MsgID_KeyboardLL_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }

        public class MouseLLHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_MouseLL {get; private set; }
            public int MsgID_MouseLL_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event BasicHookEventHandler MouseLLEvent;
            public event MouseEventHandler MouseDown;
            public event MouseEventHandler MouseMove;
            public event MouseEventHandler MouseUp;

            private const int WM_MOUSEMOVE = 0x0200;
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_LBUTTONUP = 0x0202;
            private const int WM_LBUTTONDBLCLK = 0x0203;
            private const int WM_RBUTTONDOWN = 0x0204;
            private const int WM_RBUTTONUP = 0x0205;
            private const int WM_RBUTTONDBLCLK = 0x0206;
            private const int WM_MBUTTONDOWN = 0x0207;
            private const int WM_MBUTTONUP = 0x0208;
            private const int WM_MBUTTONDBLCLK = 0x0209;
            private const int WM_MOUSEWHEEL = 0x020A;

            struct MSLLHOOKSTRUCT
            {
                public System.Drawing.Point pt;
                public int mouseData;
                public int flags;
                public int time;
                public IntPtr dwExtraInfo;
            };

            public MouseLLHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_MouseLL = RegisterWindowMessage(HookPrefix + "MOUSELL");
                MsgID_MouseLL_HookReplaced = RegisterWindowMessage(HookPrefix + "MOUSELL_REPLACED");

                // Start the hook
                InitializeMouseLLHook(0, _Handle);
            }

            protected override void OnStop()
            {
                UninitializeMouseLLHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)

            {
                if (m.Msg == MsgID_MouseLL)
                {
                    if (MouseLLEvent != null)
                        MouseLLEvent(m.WParam, m.LParam);

                    MSLLHOOKSTRUCT M = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(m.LParam, typeof(MSLLHOOKSTRUCT));

                    if (m.WParam.ToInt32() == WM_MOUSEMOVE)
                    {
                        if (MouseMove != null)
                            MouseMove(this, new MouseEventArgs(MouseButtons.None, 0, M.pt.X, M.pt.Y, 0));
                    }
                    else if (m.WParam.ToInt32() == WM_LBUTTONDOWN)
                    {
                        if (MouseDown != null)
                            MouseDown(this, new MouseEventArgs(MouseButtons.Left, 0, M.pt.X, M.pt.Y, 0));
                    }
                    else if (m.WParam.ToInt32() == WM_RBUTTONDOWN)
                    {
                        if (MouseDown != null)
                            MouseDown(this, new MouseEventArgs(MouseButtons.Right, 0, M.pt.X, M.pt.Y, 0));
                    }
                    else if (m.WParam.ToInt32() == WM_LBUTTONUP)
                    {
                        if (MouseUp != null)
                            MouseUp(this, new MouseEventArgs(MouseButtons.Left, 0, M.pt.X, M.pt.Y, 0));
                    }
                    else if (m.WParam.ToInt32() == WM_RBUTTONUP)
                    {
                        if (MouseUp != null)
                            MouseUp(this, new MouseEventArgs(MouseButtons.Right, 0, M.pt.X, M.pt.Y, 0));
                    }
                }
                else if (m.Msg == MsgID_MouseLL_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }
        public class CallWndProcHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_CallWndProc {get; private set; }
            public int MsgID_CallWndProc_Params {get; private set; }
            public int MsgID_CallWndProc_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event WndProcEventHandler CallWndProc;

            private IntPtr _CacheHandle;
            private IntPtr _CacheMessage;

            public CallWndProcHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_CallWndProc_HookReplaced = RegisterWindowMessage(HookPrefix + "CALLWNDPROC_REPLACED");
                MsgID_CallWndProc = RegisterWindowMessage(HookPrefix + "CALLWNDPROC");
                MsgID_CallWndProc_Params = RegisterWindowMessage(HookPrefix + "CALLWNDPROC_PARAMS");

                // Start the hook
                InitializeCallWndProcHook(0, _Handle);
            }

            protected override void OnStop()
            {
                UninitializeCallWndProcHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_CallWndProc)
                {
                    _CacheHandle = m.WParam;
                    _CacheMessage = m.LParam;
                }
                else if (m.Msg == MsgID_CallWndProc_Params)
                {
                    if (CallWndProc != null && _CacheHandle != IntPtr.Zero && _CacheMessage != IntPtr.Zero)
                        CallWndProc(_CacheHandle, _CacheMessage, m.WParam, m.LParam);
                    _CacheHandle = IntPtr.Zero;
                    _CacheMessage = IntPtr.Zero;
                }
                else if (m.Msg == MsgID_CallWndProc_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }
        public class GetMsgHook : Hook
        {
            // Values retreived with RegisterWindowMessage
            public int MsgID_GetMsg {get; private set; }
            public int MsgID_GetMsg_Params {get; private set; }
            public int MsgID_GetMsg_HookReplaced {get; private set; }

            public event HookReplacedEventHandler HookReplaced;
            public event WndProcEventHandler GetMsg;

            private IntPtr _CacheHandle;
            private IntPtr _CacheMessage;

            public GetMsgHook(IntPtr Handle) : base(Handle)
            {
            }

            protected override void OnStart()
            {
                // Retreive the message IDs that we'll look for in WndProc
                MsgID_GetMsg_HookReplaced = RegisterWindowMessage(HookPrefix + "GETMSG_REPLACED");
                MsgID_GetMsg = RegisterWindowMessage(HookPrefix + "GETMSG");
                MsgID_GetMsg_Params = RegisterWindowMessage(HookPrefix + "GETMSG_PARAMS");

                // Start the hook
                InitializeGetMsgHook(0, _Handle);
            }

            protected override void OnStop()
            {
                UninitializeGetMsgHook();
            }

            public override void ProcessWindowMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == MsgID_GetMsg)
                {
                    _CacheHandle = m.WParam;
                    _CacheMessage = m.LParam;
                }
                else if (m.Msg == MsgID_GetMsg_Params)
                {
                    if (GetMsg != null && _CacheHandle != IntPtr.Zero && _CacheMessage != IntPtr.Zero)
                        GetMsg(_CacheHandle, _CacheMessage, m.WParam, m.LParam);
                    _CacheHandle = IntPtr.Zero;
                    _CacheMessage = IntPtr.Zero;
                }
                else if (m.Msg == MsgID_GetMsg_HookReplaced)
                {
                    if (HookReplaced != null)
                        HookReplaced();
                }
            }
        }
    }
}
