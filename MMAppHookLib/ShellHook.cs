using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DataTools.Interop.Native;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Security.Permissions;
using System.Diagnostics;
using DataTools.Memory.Internal;
using System.Threading;

namespace MMAppHookLib
{

    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    internal class ShellHook
    {
        private IntPtr hook = IntPtr.Zero;
        private uint err = 0;

        public delegate void ShellHookProcEvent(ShellHookCodes code, IntPtr wParam, IntPtr lParam);

        public event ShellHookProcEvent ShellHookProc;

        public IntPtr Handle
        {
            get => hook;
        }

        public uint ErrorCode
        {
            get => err;
        }

        public bool InitHook()
        {
            var proc = new ShellCallback(ShellCallBack);

            var hMod = PInvoke.GetModuleHandle(typeof(ShellHook).Module.Name);

            hook = WinHooks.SetWindowsHookEx(WindowHookTypes.Shell, proc,
                            hMod, 0);

            if (hook == IntPtr.Zero)
            {
                err = PInvoke.GetLastError();
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CloseHook()
        {
            return WinHooks.UnhookWindowsHookEx(hook);
        }

        private IntPtr ShellCallBack(ShellHookCodes code, IntPtr wParam, IntPtr lParam)
        {
            Task.Run(() => ShellHookProc?.Invoke(code, wParam, lParam));
            return WinHooks.CallNextHookEx(hook, (int)code, wParam, lParam);
        }

    }
}
