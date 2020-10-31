using DataTools.Hardware.Display;
using DataTools.Win32Api;
using System;
using MMHLR64;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using DataTools.SystemInformation;

namespace MMWndT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        Dispatcher maindisp = System.Windows.Application.Current.Dispatcher;

        GlobalHooks gh;

        NotifyIcon notify;

        WindowInteropHelper hwndHelper;

        private ObservableCollection<EventViewModel> eventLog = new ObservableCollection<EventViewModel>();

        private System.Windows.Forms.ContextMenuStrip cmenu;
        private System.Windows.Forms.ToolStripMenuItem cmnuRestore;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmnuEnable;
        private System.Windows.Forms.ToolStripMenuItem cmnuDisable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmnuQuit;

        public ObservableCollection<EventViewModel> EventLog
        {
            get => eventLog;
            set
            {
                if (eventLog == value) return;
                eventLog = value;
            }
        }

        private void InitWinForms()
        {


            cmenu = new ContextMenuStrip();
            cmnuRestore = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            cmnuEnable = new ToolStripMenuItem();
            cmnuDisable = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            cmnuQuit = new ToolStripMenuItem();

            // 
            // cmenu
            // 
            this.cmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuRestore,
            this.toolStripSeparator1,
            this.cmnuEnable,
            this.cmnuDisable,
            this.toolStripSeparator2,
            this.cmnuQuit});
            this.cmenu.Name = "contextMenuStrip1";
            this.cmenu.Size = new System.Drawing.Size(223, 104);
            // 
            // mnuRestore
            // 
            this.cmnuRestore.Name = "mnuRestore";
            this.cmnuRestore.Size = new System.Drawing.Size(222, 22);
            this.cmnuRestore.Text = "&Restore Window";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuEnable
            // 
            this.cmnuEnable.Name = "mnuEnable";
            this.cmnuEnable.Size = new System.Drawing.Size(222, 22);
            this.cmnuEnable.Text = "Enable Window Positioning";
            // 
            // mnuDisable
            // 
            this.cmnuDisable.Name = "mnuDisable";
            this.cmnuDisable.Size = new System.Drawing.Size(222, 22);
            this.cmnuDisable.Text = "Disable Window Positioning";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuQuit
            // 
            this.cmnuQuit.Name = "mnuQuit";
            this.cmnuQuit.Size = new System.Drawing.Size(222, 22);
            this.cmnuQuit.Text = "Quit";

            notify = new NotifyIcon();
            notify.Visible = true;
            notify.ContextMenuStrip = cmenu;


            string icoUri = "pack://application:,,,/MMWndT;component/Resources/mmhelper.ico";

            var icoStream = System.Windows.Application.GetResourceStream(new Uri(icoUri));

            System.Drawing.Icon icon = new System.Drawing.Icon(icoStream.Stream);

            notify.Icon = icon;

            SetWindowTheme(hwndHelper.Handle, "explorer", null);

        }

        public MainWindow()
        {
            InitializeComponent();
            WindowList.ItemsSource = EventLog;

            hwndHelper = new WindowInteropHelper(this);
            hwndHelper.EnsureHandle();

            InitWinForms();
            
            Program.Work.WorkLogger += Work_WorkLogger;
            Program.Work.WorkNotify += Work_WorkNotify;

            gh = new GlobalHooks(hwndHelper.Handle);

            gh.MouseLL.MouseMove += MouseLL_MouseMove;
            gh.MouseLL.Start();

            notify.Text = InactiveText;
            
            this.SizeChanged += MainWindow_SizeChanged;
            this.StateChanged += MainWindow_StateChanged;
            
            mnuEnable.IsEnabled = true;
            mnuDisable.IsEnabled = false;

            notify.MouseClick += Notify_MouseClick;
            notify.MouseDoubleClick += notify_DoubleClick;

            btnToggle.Click += btnToggle_Click;

            btnQuit.Click += btnQuit_Click;

            btnClear.Click += BtnClear_Click;

            mnuQuit.Click += MnuQuit_Click;
            mnuEnable.Click += MnuEnable_Click;
            mnuDisable.Click += MnuDisable_Click;
            mnuRestore.Click += MnuRestore_Click;

            cmnuQuit.Click += MnuQuit_Click;
            cmnuEnable.Click += MnuEnable_Click;
            cmnuDisable.Click += MnuDisable_Click;
            cmnuRestore.Click += MnuRestore_Click;

            maindisp = Dispatcher.CurrentDispatcher;
           
            Program.Work.ActiveWindows.Add(hwndHelper.Handle, new ActWndInfo() { WindowName = Title, Timestamp = DateTime.Now });

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            eventLog.Clear();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        private bool isShutdown = false;

        private void Shutdown(bool close = true, bool exit = false)
        {
            StopService();

            notify.Visible = false;

            Program.CTS.Cancel();

            isShutdown = true;
            App.Current.Work.Log.Close();

            if (exit)
            {
                Environment.Exit(0);
            }

            while (!Program.Work.WorkerShutdown)
            {
                Thread.Yield();
            }

            if (close) Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!isShutdown) Shutdown(false);
            base.OnClosing(e);
        }

        private void Work_WorkNotify(object sender, WorkerNotifyEventArgs e)
        {

            maindisp.Invoke(async () =>
            {
                if (e.Message == Worker.MSG_START_MOVER)
                {
                    mnuEnable.IsEnabled = false;
                    mnuDisable.IsEnabled = true;
                    notify.Text = ActiveText;
                    btnToggle.Content = StopButtonText;
                    IsWatching = true;
                }
                else if (e.Message == Worker.MSG_STOP_MOVER)
                {
                    mnuEnable.IsEnabled = true;
                    mnuDisable.IsEnabled = false;
                    notify.Text = InactiveText;
                    btnToggle.Content = StartButtonText;
                    IsWatching = false;
                }
                else if (e.Message == Worker.MSG_SHUTDOWN)
                {
                    string shut = "System is shutting down.";

                    EndSessionTypes t = (EndSessionTypes)e.LongData2;

                    if (t == EndSessionTypes.Critical)
                    {
                        shut += "App is being forced closed.";
                    }
                    else if (t == EndSessionTypes.LogOff)
                    {
                        shut += "User is logging off. Closing gracefully.";
                    }
                    else if (t == EndSessionTypes.CloseApp) 
                    {
                        shut += "App has been asked to close by the system.";
                    }

                    eventLog.Add(new EventViewModel(e));

                    App.Current.Work.Log.Log(shut);

                    Shutdown(true, true);
                    return;
                }


                await UpdateConnectedStatus();

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

        private void Notify_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void MnuRestore_Click(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.Activate();
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
            Shutdown();
        }


        private void AddEvent(WorkerLogEventArgs e)
        {
            //if (eventLog.Count > 100)
            //{
            //    eventLog.RemoveAt(eventLog.Count - 1);
            //}

            var item = new EventViewModel(e);

            eventLog.Insert(0, item);

            if (e.Action == Worker.MSG_HW_CHANGE)
            {
                _ = UpdateConnectedStatus();
            }

        }

        private async Task UpdateConnectedStatus()
        {

            if (await SysInfo.GetHasInternetAsync())
            {
                tss2.Content = "Online";
            }
            else
            {
                tss2.Content = "Offline";
            }

        }

        private void MouseLL_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var p = new W32POINT(e.X, e.Y);
            mouseMon = monitors.GetMonitorFromPoint(p);

            if (mouseMon != null)
            {
                var idx = mouseMon.Index;
                tss1.Content = $"Monitor #{idx}, {e.X}, {e.Y}";
            }
            else
            {
                tss1.Content = $"{e.X}, {e.Y}";

            }

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

        private void notify_DoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

    }


}
