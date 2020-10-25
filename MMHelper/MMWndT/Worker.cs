using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Hardware.Display;
using DataTools.Win32Api;
using static DataTools.Win32Api.User32;
using System.Text;
using System.Windows.Threading;
using System.Windows.Forms;

namespace MMWndT
{


    [StructLayout(LayoutKind.Explicit)]
    public struct OUTPUT_STRUCT
    {
        [FieldOffset(0)]
        public int cb;

        [FieldOffset(4)]
        public int msg;

        [FieldOffset(8)]
        public int IntData1;

        [FieldOffset(12)]
        public int IntData2;

        [FieldOffset(8)]
        public long LongData1;

        [FieldOffset(16)]
        public long LongData2;
    }

    public enum WorkerKind
    {
        Is86Worker = 1,
        Is64Worker = 2
    }


    public class WorkerLogEventArgs : EventArgs
    {
        public string WindowName { get; private set; }

        public int Monitor { get; private set; }

        public IntPtr Handle { get; private set; }

        public int Action { get; private set; }

        public string Message { get; private set; }


        public WorkerLogEventArgs(string name, IntPtr handle, int monitor, int action) : this(null, name, handle, monitor, action)
        { 
        }

        public WorkerLogEventArgs(string smsg, string name, IntPtr handle, int monitor, int action)
        {
            Message = smsg;
            WindowName = name;
            Monitor = monitor;
            Handle = handle;
            Action = action;
        }

    }

    public class WorkerNotifyEventArgs : EventArgs
    {

        public WorkerKind Kind { get; private set; }
        public int Message { get; private set; }

        public int IntData1 { get; private set; }

        public int IntData2 { get; private set; }

        public long LongData1 { get; private set; }

        public long LongData2 { get; private set; }

        internal WorkerNotifyEventArgs(OUTPUT_STRUCT os, WorkerKind kind)
        {
            Kind = kind;

            Message = os.msg;
            IntData1 = os.IntData1;
            IntData2 = os.IntData2;

            LongData1 = os.LongData1;
            LongData2 = os.LongData2;
        }

    }

    public class Worker : BackgroundService
    {


        public const int MSG_QUERY_STATE = 411;
        public const int MSG_STOP_MOVER = 206;
        public const int MSG_START_MOVER = 204;
        public const int MSG_CREATED = 1;
        public const int MSG_ACTIVATED = 2;
        public const int MSG_DESTROYED = 3;
        public const int MSG_TERMINATE = 27;
        public const int MSG_INFORM_MY = 124;
        public const int MSG_HW_CHANGE = 129;

        private Socket x64Sock;
        private Socket x86Sock;
        private Dispatcher disp;

        public delegate void WorkerLoggerEvent(object sender, WorkerLogEventArgs e);
        public event WorkerLoggerEvent WorkLogger;

        public delegate void WorkerNotifyEvent(object sender, WorkerNotifyEventArgs e);
        public event WorkerNotifyEvent WorkNotify;

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public Worker()
        {
            _logger = null;
        }

        private CancellationToken ttok;

        private Process x86proc;
        private Process x64proc;

        private Thread x86Thread;
        private Thread x64Thread;

        MonitorInfo mouseMon;
        MonitorInfo wndMon;

        public Dictionary<IntPtr, DateTime> ActiveWindows = new Dictionary<IntPtr, DateTime>();

        private Monitors monitors = new Monitors();

        private readonly Guid MagicKey = Guid.Parse("{4D8E985F-79C1-4AF9-B481-8846FEB6C7DA}");
        int cb = Marshal.SizeOf<OUTPUT_STRUCT>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            disp = Program.MainDispatcher;
            ttok = stoppingToken;

            var dwnds = GetCurrentDesktopWindows();

            foreach (var wnd in dwnds)
            {
                ActiveWindows.Add(wnd, DateTime.Now);
            }

            var stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR32.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            x86proc = Process.Start(stinfo);
            
            stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR64.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            x64proc = Process.Start(stinfo);

            await Task.Delay(1000);

            x64Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            x64Sock.Blocking = true;

            x86Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            x86Sock.Blocking = true;

            var ep64 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 53112);
            var ep32 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 53113);

            //x64Sock.ReceiveTimeout = 100;
            //x86Sock.ReceiveTimeout = 100;

            x64Sock.Connect(ep64);
            x86Sock.Connect(ep32);

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            x86Thread = new Thread(ThreadRunner86);
            x64Thread = new Thread(ThreadRunner64);

            x86Thread.Priority = ThreadPriority.Lowest;
            x64Thread.Priority = ThreadPriority.Lowest;

            x86Thread.Start();
            x64Thread.Start();
            
            QueryState();

            while (!stoppingToken.IsCancellationRequested)
            {
                Application.DoEvents();
                Thread.Sleep(100);

                if (x64proc.HasExited)
                {
                    stinfo = new ProcessStartInfo()
                    {
                        FileName = "MMHLR64.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    x64proc = Process.Start(stinfo);

                    await Task.Delay(1000);
                    x64Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);

                    //x64Sock.ReceiveTimeout = 100;
                    x64Sock.Connect(ep64);
                }

                if (x86proc.HasExited)
                {
                    stinfo = new ProcessStartInfo()
                    {
                        FileName = "MMHLR32.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    x86proc = Process.Start(stinfo);

                    await Task.Delay(1000);
                    x86Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);

                    //x86Sock.ReceiveTimeout = 100;
                    x86Sock.Connect(ep32);
                }

            }


            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_TERMINATE
            };

            BroadcastMessage(os);

            x64proc.WaitForExit();
            x86proc.WaitForExit();

            Environment.Exit(0);
        }

        public void StartDeskMover()
        {

            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_START_MOVER
            };

            BroadcastMessage(os);
        }

        public void StopDeskMover()
        {

            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_STOP_MOVER
            };

            BroadcastMessage(os);
        }

        public void QueryState()
        {
            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_QUERY_STATE
            };

            BroadcastMessage(os);
        }

        private void BroadcastMessage(OUTPUT_STRUCT os)
        {

            var by = new byte[os.cb];

            MemPtr.Union(ref os, ref by);

            if (x64Sock != null && x64Sock.Connected)
            {
                Write(x64Sock, by);
            }

            if (x86Sock != null && x86Sock.Connected)
            {
                Write(x86Sock, by);
            }

        }


        private void ThreadRunner64()
        {
            while (!ttok.IsCancellationRequested)
            {
                Thread.Sleep(0);

                try
                {
                    var os = new OUTPUT_STRUCT();
                    var b = Read(x64Sock, cb);

                    MemPtr.Union(ref b, out os);

                    if (os.msg == MSG_CREATED)
                    {
                        DoCreated((IntPtr)os.LongData1, true);
                    }
                    else if (os.msg == MSG_ACTIVATED)
                    {
                        DoActivate((IntPtr)os.LongData1, true);
                    }
                    else if (os.msg == MSG_DESTROYED)
                    {
                        DoDestroyed((IntPtr)os.LongData1);
                    }
                    else if (os.msg == MSG_HW_CHANGE)
                    {
                        disp.Invoke(() => {
                            mouseMon = wndMon = null;
                            monitors = new Monitors();
                            int x = Marshal.SizeOf<OUTPUT_STRUCT>();
                            string sname = "";
                            string smsg = ""; 

                            if (os.cb > x)
                            {
                                sname = Encoding.Unicode.GetString(b, x, os.cb - x);
                            }

                            if ((uint)os.LongData1 == DevNotify.DBT_DEVICEARRIVAL)
                            {
                                smsg = "Installed";
                            }
                            else if ((uint)os.LongData1 == DevNotify.DBT_DEVICEREMOVECOMPLETE)
                            {
                                smsg = "Removed";
                            }
                            else
                            {
                                smsg = "Changed";
                            }

                            AddEvent(smsg, sname, (int)os.LongData1, os.msg);
                           
                        });
                    }
                    else
                    {
                        disp.Invoke(() =>
                        {
                            WorkNotify?.Invoke(this, new WorkerNotifyEventArgs(os, WorkerKind.Is64Worker));
                        });
                    }
                }
                catch
                {

                }

            }
        }

        private void ThreadRunner86()
        {
            while (!ttok.IsCancellationRequested)
            {
                Thread.Sleep(0);

                try
                {
                    var os = new OUTPUT_STRUCT();
                    var b = Read(x86Sock, cb);

                    MemPtr.Union(ref b, out os);

                    if (os.msg == MSG_CREATED)
                    {
                        DoCreated((IntPtr)os.IntData1, false);
                    }
                    else if (os.msg == MSG_ACTIVATED)
                    {
                        DoActivate((IntPtr)os.IntData1, false);
                    }
                    else if (os.msg == MSG_DESTROYED)
                    {
                        DoDestroyed((IntPtr)os.IntData1);
                    }
                    else
                    {
                        disp.Invoke(() =>
                        {
                            WorkNotify?.Invoke(this, new WorkerNotifyEventArgs(os, WorkerKind.Is86Worker));
                        });
                    }
                }
                catch
                {

                }
                

            }
        }

        private void DoDestroyed(IntPtr Handle)
        {
            disp.Invoke(() =>
            {
                if (ActiveWindows.ContainsKey(Handle))
                {
                    ActiveWindows.Remove(Handle);
                }

                AddEvent(Handle, 0, MSG_DESTROYED);
            });
        }

        private void DoCreated(IntPtr Handle, bool x64)
        {
            //if (!actWnds.ContainsKey(Handle))
            //{
            //    actWnds.Add(Handle, DateTime.Now);
            //}

            disp.Invoke(() =>
            {
                wndMon = monitors.GetMonitorFromWindow(Handle);

                W32POINT pt = new W32POINT();
                GetCursorPos(ref pt);

                mouseMon = monitors.GetMonitorFromPoint(pt);

                if (wndMon == null || mouseMon == null)
                {
                    AddEvent(Handle, 0, MSG_CREATED);
                    return;
                }

                AddEvent(Handle, wndMon.Index, MSG_CREATED);
            });

        }

        private void DoActivate(IntPtr Handle, bool x64)
        {
            disp.Invoke(() =>
            {
                var dt = DateTime.Now;

                if (ActiveWindows.TryGetValue(Handle, out dt))
                {
                    if (DateTime.Now - dt < new TimeSpan(0, 0, 2))
                    {
                        DoWindowMove(Handle, x64);
                        return;
                    }
                    else
                    {
                        wndMon = monitors.GetMonitorFromWindow(Handle);
                    }
                }
                else
                {
                    ActiveWindows.Add(Handle, DateTime.Now);
                    DoWindowMove(Handle, x64);
                    return;
                }


                W32POINT pt = new W32POINT();
                GetCursorPos(ref pt);

                mouseMon = monitors.GetMonitorFromPoint(pt);

                if (wndMon == null || mouseMon == null)
                {
                    AddEvent(Handle, 0, MSG_ACTIVATED);
                    return;
                }

                AddEvent(Handle, wndMon.Index, MSG_ACTIVATED);
            });
        }

        void AddEvent(string smsg, string wname, int monitor, int action)
        {
            _logger?.LogInformation(smsg);

            var e = new WorkerLogEventArgs(smsg, wname, IntPtr.Zero, monitor, action);
            WorkLogger?.Invoke(this, e);

        }

        void AddEvent(IntPtr handle, int monitor, int action) 
        {
            string smsg;
            string wname = GetWindowName(handle);

            switch(action)
            {
                case MSG_CREATED:

                    smsg = $"Window Created {wname}";
                    if (monitor > 0) smsg += $"on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_ACTIVATED:
                    smsg = $"Window Activated {wname}";
                    if (monitor > 0) smsg += $"on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_DESTROYED:
                    smsg = $"Window Destroyed {wname}";
                    if (monitor > 0) smsg += $"on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_HW_CHANGE:
                    smsg = "Monitor Plugged In/Unplugged";

                    break;

                default:
                    smsg = $"Unknown Window Event {action} on {wname}";
                    if (monitor > 0) smsg += $"on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;
            }

            _logger?.LogInformation(smsg);

            var e = new WorkerLogEventArgs(wname, handle, monitor, action);
            WorkLogger?.Invoke(this, e);
        }

        private string GetWindowName(IntPtr Hwnd)
        {
            // This function gets the name of a window from its handle
            StringBuilder Title = new StringBuilder(256);
            GetWindowText(Hwnd, Title, 256);

            return Title.ToString().Trim();
        }


        private void DoWindowMove(IntPtr Handle, bool x64)
        {
            var t = new Thread(() =>
            {
                long res64;
                int res32;
                W32POINT pt = new W32POINT();
                GetCursorPos(ref pt);

                mouseMon = monitors.GetMonitorFromPoint(pt);
                wndMon = monitors.GetMonitorFromWindow(Handle);

                if (wndMon == null || mouseMon == null)
                {
                    return;
                }

                if (wndMon.DevicePath != mouseMon.DevicePath)
                {
                    W32RECT rc = new W32RECT();
                    GetWindowRect(Handle, ref rc);

                    if (x64)
                    {
                        res64 = (long)GetWindowLongPtr(Handle, GWL_STYLE);

                        if ((res64 & WS_SIZEBOX) == WS_SIZEBOX)
                        {
                            Monitors.TransformRect(ref rc, wndMon, mouseMon, true);
                        }
                        else
                        {
                            Monitors.TransformRect(ref rc, wndMon, mouseMon);
                        }


                    }

                    else
                    {
                        res32 = (int)GetWindowLong(Handle, GWL_STYLE);

                        if ((res32 & WS_SIZEBOX) == WS_SIZEBOX)
                        {
                            Monitors.TransformRect(ref rc, wndMon, mouseMon, true);
                        }
                        else
                        {
                            Monitors.TransformRect(ref rc, wndMon, mouseMon);
                        }


                    }

                    MoveWindow(Handle, rc.left, rc.top, Math.Abs(rc.right - rc.left) + 1, Math.Abs(rc.bottom - rc.top) + 1, true);

                    //wndMon = monitors.GetMonitorFromWindow(Handle);
                    wndMon = mouseMon;

                }


                disp.Invoke(() =>
                {
                    mouseMon = monitors.GetMonitorFromPoint(pt);

                    if (wndMon == null || mouseMon == null)
                    {
                        AddEvent(Handle, 0, MSG_ACTIVATED);
                        return;
                    }

                    AddEvent(Handle, wndMon.Index, MSG_ACTIVATED);

                });

            });

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;

            t.Start();

        }

        public override void Dispose()
        {
            x64Sock?.Close();
            x86Sock?.Close();
            x64proc?.Close();
            x86proc?.Close();
            base.Dispose();
        }

        /// <summary>
        /// Reads count bytes, blocks until count bytes are received
        /// </summary>
        /// <param name="count">The bytes to read</param>
        /// <returns></returns>
        private static byte[] Read(Socket connSock, int count)
        {
            byte[] input = new byte[count];

            int t = 0, c = count;
            int r;

            while (t < c)
            {
                r = connSock.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count) break;
            }

            return input;

        }

        /// <summary>
        /// Receives a 32 bit integer (4 bytes) and reads that many total bytes (including the 4 bytes of the 32 bit integer)
        /// </summary>
        /// <param name="connSock">An active socket</param>
        /// <returns></returns>

        private static byte[] ReadAuto(Socket connSock)
        {
            int count = 4;
            byte[] input = new byte[count];

            int t = 0, c = count;
            int r;

            while (t < c)
            {
                r = connSock.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count) break;
                Thread.Sleep(0);
            }


            count = BitConverter.ToInt32(input);

            t = 0;
            c = count - 4;

            while (t < c)
            {
                r = connSock.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count) break;
                Thread.Sleep(0);
            }

            return input;

        }

        private static void Write(Socket connSock, byte[] buffer)
        {
            connSock.Send(buffer);
        }

    }
}
