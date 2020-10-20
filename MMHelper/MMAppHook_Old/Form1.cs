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
using DataTools.Interop;
using DataTools.Interop.Display;
using DataTools.Interop.Native;

namespace MMAppHook
{
    public partial class Form1 : Form
    {

        private Monitors monitors = new Monitors();

        // API calls to give us a bit more information about the data we get from the hook
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);
        [DllImport("user32.dll")]
        private static extern uint RealGetWindowClass(IntPtr hWnd, StringBuilder pszType, uint cchType);

        GlobalHooks gh;

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += Form1_FormClosing;

            gh = new GlobalHooks(Handle);

            gh.Shell.WindowCreated += Shell_WindowCreated;
            gh.Shell.WindowActivated += Shell_WindowActivated;
            gh.Shell.WindowDestroyed += Shell_WindowDestroyed;
            gh.MouseLL.MouseMove += MouseLL_MouseMove;
            gh.MouseLL.Start();

        }

        private void Shell_WindowDestroyed(IntPtr Handle)
        {
            listBox1.Items.Add($"Window Destroyed {GetWindowName(Handle)}");
        }

        private void Shell_WindowActivated(IntPtr Handle)
        {
            var mon = monitors.GetMonitorFromWindow(Handle);
            listBox1.Items.Add($"Window Activated {GetWindowName(Handle)} on monitor #{mon.Index}");
        }

        private void Shell_WindowCreated(IntPtr Handle)
        {
            var mon = monitors.GetMonitorFromWindow(Handle);
            listBox1.Items.Add($"Window Created {GetWindowName(Handle)} on monitor #{mon.Index}");
        }


        #region Windows API Helper Functions

        private string GetWindowName(IntPtr Hwnd)
        {
            // This function gets the name of a window from its handle
            StringBuilder Title = new StringBuilder(256);
            GetWindowText(Hwnd, Title, 256);

            return Title.ToString().Trim();
        }

        private string GetWindowClass(IntPtr Hwnd)
        {
            // This function gets the name of a window class from a window handle
            StringBuilder Title = new StringBuilder(256);
            RealGetWindowClass(Hwnd, Title, 256);

            return Title.ToString().Trim();
        }

        #endregion

        private void MouseLL_MouseMove(object sender, MouseEventArgs e)
        {
            var p = new PInvoke.POINT(e.X, e.Y);
            var mon = monitors.GetMonitorFromPoint(p);
            
            toolStripStatusLabel1.Text = $"Monitor #{mon.Index}, {e.X}, {e.Y}";
        }

        protected override void WndProc(ref Message m)
        {
            gh?.ProcessWindowMessage(ref m);
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gh.Shell.Stop();
            gh.MouseLL.Stop();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start Watching Shell")
            {
                gh.Shell.Start();
                button1.Text = "Stop Watching Shell";
            }
            else
            {

                gh.Shell.Stop();
                button1.Text = "Start Watching Shell";
            }

        }
    }

}
