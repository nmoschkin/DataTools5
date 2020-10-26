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
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Threading;
using DataTools.Desktop;
using DataTools.Hardware;
using DataTools.Hardware.Display;
using DataTools.Win32Api;
using static DataTools.Win32Api.User32;

namespace MMWndT
{
    public partial class frmMain : Form
    {

        [DllImport("uxtheme.dll")]
        public extern static int SetWindowTheme(
            IntPtr hWnd,
            [MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName,
            [MarshalAs(UnmanagedType.LPWStr)] string pszSubIdList);

        public bool IsWatching { get; private set; }

        Monitors monitors = new Monitors();
        MonitorInfo mouseMon = null;

        public const string ActiveText = "Enabled - Multi-Monitor Dynamic Window Positioner";
        public const string InactiveText = "Disabled - Multi-Monitor Dynamic Window Positioner";

        public const string StartButtonText = "Start Watching Desktop";
        public const string StopButtonText = "Stop Watching Desktop";

        App Program = (App)System.Windows.Application.Current;

        CancellationTokenSource cts = null;
        CancellationToken ct = new CancellationToken();
        Dispatcher maindisp;

        GlobalHooks gh;

        public frmMain()
        {
            InitializeComponent();
            
            SetWindowTheme(lstEvents.Handle, "explorer", null);

            Program.Work.WorkLogger += Work_WorkLogger;
            Program.Work.WorkNotify += Work_WorkNotify;

            gh = new GlobalHooks(Handle);

            gh.MouseLL.MouseMove += MouseLL_MouseMove;
            gh.MouseLL.Start();

            notify.Text = InactiveText;

            this.FormClosing += Form1_FormClosing;
            this.Resize += Form1_Resize;

            mnuEnable.Enabled = true;
            mnuDisable.Enabled = false;

            notify.MouseClick += Notify_MouseClick;
            mnuQuit.Click += MnuQuit_Click;

            mnuEnable.Click += MnuEnable_Click;
            mnuDisable.Click += MnuDisable_Click;

            mnuRestore.Click += MnuRestore_Click;

            maindisp = Dispatcher.CurrentDispatcher;

            Program.Work.ActiveWindows.Add(Handle, new ActWndInfo() { WindowName = Text, Timestamp = DateTime.Now });
            WindowState = FormWindowState.Minimized;

        }

        private void Work_WorkNotify(object sender, WorkerNotifyEventArgs e)
        {

            maindisp.Invoke(() =>
            {
                if (e.Message == Worker.MSG_START_MOVER)
                {
                    mnuEnable.Enabled = false;
                    mnuDisable.Enabled = true;
                    notify.Text = ActiveText;
                    btnWatch.Text = StopButtonText;
                    IsWatching = true;
                }
                else if (e.Message == Worker.MSG_STOP_MOVER)
                {
                    mnuEnable.Enabled = true;
                    mnuDisable.Enabled = false;
                    notify.Text = InactiveText;
                    btnWatch.Text = StartButtonText;
                    IsWatching = false;
                }
                
            });

        }

        private void Work_WorkLogger(object sender, WorkerLogEventArgs e)
        {
            maindisp.Invoke(() =>
            {
                AddEvent(e);
            });
        }

        private void StartService()
        {
            Program.Work.StartDeskMover();
            Program.Work.QueryState();
        }

        private void StopService()
        {
            Program.Work.StopDeskMover();
            Program.Work.QueryState();
        }

        private void Notify_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void MnuRestore_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.BringToFront();
                this.Focus();
            }
        }

        private void MnuDisable_Click(object sender, EventArgs e)
        {
            btnToggle_Click(this, e);
        }

        private void MnuEnable_Click(object sender, EventArgs e)
        {
            btnToggle_Click(this, e);
        }

        private void MnuQuit_Click(object sender, EventArgs e)
        {
            StopService();

            this.Close();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        
        private void AddEvent(WorkerLogEventArgs e)
        {
            string sPath;
            
            if (lstEvents.Items.Count > 100)
            {
                lstEvents.Items.RemoveAt(lstEvents.Items.Count - 1);
            }

            if (e.Action != Worker.MSG_DESTROYED)
            {
                StringBuilder sb = new StringBuilder(512);
                
                GetWindowModuleFileName(e.Handle, sb, 512);

                sPath = sb.ToString();
                int? ixx = null;

                if (!imageList1.Images.ContainsKey(e.Handle.ToString()))
                {
                    var icon = Resources.GetFileIcon(sPath, Resources.SystemIconSizes.Small, ref ixx);
                    
                    imageList1.Images.Add(e.Handle.ToString(), icon);
                                    
                }

            }

            ListViewItem item;
            
            if (e.Message != null)
            {
                item = new ListViewItem(e.Message, e.Handle.ToString());
            }
            else
            {
                item = new ListViewItem(e.WindowName, e.Handle.ToString());
            }

            item.Tag = e;

            var sitems = item.SubItems;
            string s = "";
            
            switch (e.Action)
            {
                case Worker.MSG_HW_CHANGE:


                    if ((uint)e.Monitor == DevNotify.DBT_DEVICEARRIVAL)
                    {
                        s = "Monitor Plugged In";
                        item.ForeColor = Color.Green;
                    }
                    else if ((uint)e.Monitor == DevNotify.DBT_DEVICEREMOVECOMPLETE)
                    {
                        s = "Monitor Unplugged";
                        item.ForeColor = Color.Red;
                    }
                    else
                    {
                        s = "Hardware Changed";
                        item.ForeColor = Color.DarkGray;
                    }

                    break;

                case Worker.MSG_REPLACED:
                    s = "Window Replaced";
                    item.ForeColor = Color.Blue;
                    break;

                case Worker.MSG_MOVESIZE:
                    s = "Window Move/Size";
                    item.ForeColor = Color.DarkSlateBlue;
                    break;

                case Worker.MSG_ACTIVATED:
                    s = "Activated";
                    item.ForeColor = Color.Blue;
                    break;

                case Worker.MSG_CREATED:
                    s = "Created";
                    item.ForeColor = Color.Green;
                    break;

                case Worker.MSG_DESTROYED:
                    s = "Destroyed";
                    item.ForeColor = Color.Red;
                    break;
            }

            sitems.Add(s);
            sitems.Add(e.Monitor.ToString());
            sitems.Add(DateTime.Now.ToString("G"));

            lstEvents.Items.Insert(0, item);
        }

        private void MouseLL_MouseMove(object sender, MouseEventArgs e)
        {
            var p = new W32POINT(e.X, e.Y);
            mouseMon = monitors.GetMonitorFromPoint(p);
            
            if (mouseMon != null)
            {
                var idx = mouseMon.Index;
                tss1.Text = $"Monitor #{idx}, {e.X}, {e.Y}";
            }
            else
            {
                tss1.Text = $"{e.X}, {e.Y}";

            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopService();
        }

        private void ToggleWatching()
        {

            if (!IsWatching)
            {
                StartService();
            }
            else
            {

                StopService();
            }

        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            ToggleWatching();
        }

        private void notify_DoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}



//private async void WndEnumThread()
//{
//    try
//    {
//        while (true)
//        {

//            if (ct.IsCancellationRequested) return;

//            await maindisp.BeginInvoke(() =>
//            {
//                if (ct.IsCancellationRequested) return;

//                var desk = WndEnumer.DoEnum();

//                lstWnd.Items.Clear();
//                lstWnd.Items.Add(desk);

//                foreach (var wnd in desk.Children)
//                {
//                    if (ct.IsCancellationRequested) return;
//                    lstWnd.Items.Add(wnd);
//                    //EnumChildren(wnd);
//                    if (ct.IsCancellationRequested) return;
//                }

//            });

//            if (ct.IsCancellationRequested) return;

//            await Task.Delay(30000, ct);

//            if (ct.IsCancellationRequested) return;
//        }

//    }
//    catch
//    {

//    }
//}

//private void EnumChildren(WndEnumer e)
//{
//    e.Refresh();
//    foreach (var ec in e.Children)
//    {
//        lstWnd.Items.Add(ec);
//        EnumChildren(ec);
//    }
//}
