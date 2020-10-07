using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using DataTools.Interop.Native;
using DataTools.Memory.Internal;
using MMAppHookLib;

namespace MMAppHook
{
    public partial class Form1 : Form
    {
        ShellHook hook = new ShellHook();



        public Form1()
        {
            InitializeComponent();

            hook.ShellHookProc += Hook_ShellHookProc;
            this.FormClosing += Form1_FormClosing;

            if (hook.InitHook())
            {
                MessageBox.Show(this, $"Hook Created With Handle: {((long)hook.Handle).ToString("x8")}");
            }
            else
            {
                MessageBox.Show(NativeErrorMethods.FormatLastError(hook.ErrorCode));
            }

        }

        private void Hook_ShellHookProc(ShellHookCodes code, IntPtr wParam, IntPtr lParam)
        {
            if (code == ShellHookCodes.WindowCreated)
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    PInvoke.RECT r = new PInvoke.RECT();
                    PInvoke.GetWindowRect(wParam, ref r);

                    listBox1.Items.Add($"Window {((long)wParam).ToString("x8")}, x: {r.left}, y: {r.top}");
                });
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hook.CloseHook();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

}
