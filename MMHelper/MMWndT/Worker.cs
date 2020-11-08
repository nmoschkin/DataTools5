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
using DataTools.Streams;
using DataTools.SystemInformation;
using MMHLR64;
using Newtonsoft.Json.Linq;

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

        [FieldOffset(24)]
        public W32RECT rect;

    }

    [Flags]
    public enum WorkerKind
    {
        IsMainProgram = 0,
        Is86Worker = 1,
        Is64Worker = 2
    }

    public struct ActWndInfo 
    {
        public DateTime Timestamp;

        public string WindowName;
    
    }

    public class WorkerLogEventArgs : EventArgs
    {
        public string WindowName { get; private set; }

        public int Monitor { get; private set; }

        public IntPtr Handle { get; private set; }

        public int Action { get; private set; }

        public string Message { get; private set; }

        public WorkerKind Origin { get; private set; }

        public WorkerLogEventArgs(string name, IntPtr handle, int monitor, int action, WorkerKind origin) : this(null, name, handle, monitor, action, origin)
        { 
        }

        public WorkerLogEventArgs(string smsg, string name, IntPtr handle, int monitor, int action, WorkerKind origin)
        {
            Origin = origin;
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

        //private uint GetIdleTime()
        //{
        //    LastInputInfo info = new LastInputInfo();
        //    info.cbSize = Marshal.SizeOf(info);
        //    GetLastInputInfo(ref info);
        //    uint tick = GetTickCount();
        //    uint msec = tick - info.dwTime;
        //    return msec;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //private struct LastInputInfo
        //{
        //    public int cbSize;
        //    public uint dwTime;
        //}

        //[DllImport("user32.dll")]
        //private static extern bool GetLastInputInfo(ref LastInputInfo info);

        //[DllImport("kernel32.dll")]
        //private static extern uint GetTickCount();

        public const int X64Port = 53112;
        public const int X86Port = 53113;

        public const int MSG_QUERY_STATE = 411;
        public const int MSG_HOOK_REPLACED = 339;
        public const int MSG_STOP_MOVER = 206;
        public const int MSG_START_MOVER = 204;
        public const int MSG_CREATED = 1;
        public const int MSG_ACTIVATED = 2;
        public const int MSG_DESTROYED = 3;
        public const int MSG_MOVESIZE = 4;
        public const int MSG_REPLACED = 5;
        public const int MSG_TERMINATE = 27;
        public const int MSG_INFORM_MY = 124;
        public const int MSG_HW_CHANGE = 129;
        public const int MSG_ERROR = 255;
        public const int MSG_SHUTDOWN = 600;

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

        public bool HasInternet { get; private set; } = false;

        public bool WorkerShutdown { get; private set; } = false;
        public SimpleLog Log { get; set; } = new SimpleLog("MMWndT.log");

        private CancellationToken GlobalCT;

        private CancellationTokenSource X86CTS;
        private CancellationTokenSource X64CTS;

        private Process x86proc;
        private Process x64proc;

        private Thread x86Thread;
        private Thread x64Thread;

        MonitorInfo mouseMon;
        MonitorInfo wndMon;

        public Dictionary<IntPtr, ActWndInfo> ActiveWindows = new Dictionary<IntPtr, ActWndInfo>();

        private Monitors monitors = new Monitors();

        private readonly Guid MagicKey = Guid.Parse("{4D8E985F-79C1-4AF9-B481-8846FEB6C7DA}");
        int osLen = Marshal.SizeOf<OUTPUT_STRUCT>();

        private bool RefreshWindows()
        {
            if (Monitor.TryEnter(ActiveWindows))
            {

                ActiveWindows.Clear();
                var dwnds = GetCurrentDesktopWindows();

                foreach (var wnd in dwnds)
                {
                    ActiveWindows.Add(wnd, new ActWndInfo() { WindowName = GetWindowName(wnd), Timestamp = DateTime.Now });
                }

                Monitor.Exit(ActiveWindows);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            disp = System.Windows.Application.Current.Dispatcher;
            
            GlobalCT = stoppingToken;

            RefreshWindows();

            var stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR32.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Log.Log("Starting x86 process");
            x86proc = Process.Start(stinfo);

            stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR64.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Log.Log("Starting x64 process");
            x64proc = Process.Start(stinfo);

            await Task.Delay(1000);

            var ep64 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), X64Port);
            var ep32 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), X86Port);

            _ = Task.Run(async () =>
            {
                HasInternet = await SysInfo.GetHasInternetAsync();
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                Application.DoEvents();
                Thread.Sleep(100);

                // watch the worker threads and try to repair anything that has gone awry.
                try
                {

                    #region X64 Worker

                    if (x64proc == null || (x64proc != null && x64proc.HasExited))
                    {
                        x64Sock = null;
                        stinfo = new ProcessStartInfo()
                        {
                            FileName = "MMHLR64.exe",
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };

                        Log.Log("Starting 64-bit Service Process");

                        x64proc = Process.Start(stinfo);
                        await Task.Delay(1000);
                    }

                    if (x64Sock == null || (x64Sock != null && x64Sock.Connected == false))
                    {
                        x64Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);

                        Log.Log($"Opening 64-bit Service Port {X64Port}");

                        x64Sock.Connect(ep64);
                    }

                    if (x64Thread == null || !x64Thread.IsAlive)
                    {
                        Log.Log("Starting 64-bit Service Thread");

                        X64CTS = new CancellationTokenSource();
                        x64Thread = new Thread(ThreadRunner64);
                        x64Thread.Priority = ThreadPriority.Lowest;
                        x64Thread.Start();

                        QueryState(WorkerKind.Is64Worker); 
                    }

                    #endregion


                    #region X86 Worker

                    if (x86proc == null || (x86proc != null && x86proc.HasExited))
                    {
                        x86Sock = null;
                        stinfo = new ProcessStartInfo()
                        {
                            FileName = "MMHLR32.exe",
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };


                        Log.Log("Starting 32-bit Service Process");

                        x86proc = Process.Start(stinfo);

                        await Task.Delay(1000);

                        x86Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                        x86Sock.Connect(ep32);
                    }

                    if (x86Sock == null || (x86Sock != null && x86Sock.Connected == false))
                    {
                        x86Sock = new Socket(SocketType.Stream, ProtocolType.Tcp);

                        Log.Log($"Opening 32-bit Service Port {X64Port}");

                        x86Sock.Connect(ep32);
                    }


                    if (x86Thread == null || !x86Thread.IsAlive)
                    {
                        Log.Log("Starting 32-bit Service Thread");

                        X86CTS = new CancellationTokenSource();
                        x86Thread = new Thread(ThreadRunner86);
                        x86Thread.Priority = ThreadPriority.Lowest;
                        x86Thread.Start();

                        QueryState(WorkerKind.Is86Worker);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    AddEvent(ex.Message, null, 0, MSG_ERROR, WorkerKind.IsMainProgram);
                    Log.Log(ex.Message);
                }

            }

            Log.Log("Shutting down.  Broadcasting shutdown command to child processes.");
            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_TERMINATE
            };

            BroadcastMessage(os);

            Log.Log("Waiting for child processes to exit.");
            x64proc.WaitForExit();
            x86proc.WaitForExit();

            Log.Log("Exiting.");
            Log.Close();

            WorkerShutdown = true;
            //Environment.Exit(0);
        }

        public void StartDeskMover(WorkerKind kind = WorkerKind.Is64Worker | WorkerKind.Is86Worker)
        {

            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_START_MOVER
            };

            BroadcastMessage(os, kind);
        }

        public void StopDeskMover(WorkerKind kind = WorkerKind.Is64Worker | WorkerKind.Is86Worker)
        {

            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_STOP_MOVER
            };

            BroadcastMessage(os, kind);
        }

        public void QueryState(WorkerKind kind = WorkerKind.Is64Worker | WorkerKind.Is86Worker)
        {
            OUTPUT_STRUCT os = new OUTPUT_STRUCT()
            {
                cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
                msg = MSG_QUERY_STATE
            };

            BroadcastMessage(os, kind);

        }

        private void BroadcastMessage(OUTPUT_STRUCT os, WorkerKind kind = WorkerKind.Is64Worker | WorkerKind.Is86Worker)
        {
            var by = new byte[os.cb];

            MemPtr.Union(ref os, ref by);

            Parallel.Invoke(
                () => {
                    if ((kind & WorkerKind.Is64Worker) == WorkerKind.Is64Worker)
                    {
                        if (x64Sock != null && x64Sock.Connected)
                        {
                            Write(x64Sock, by);
                        }
                    }
                },
                () => {
                    if ((kind & WorkerKind.Is86Worker) == WorkerKind.Is86Worker)
                    {
                        if (x86Sock != null && x86Sock.Connected)
                        {
                            Write(x86Sock, by);
                        }
                    }
                }
            );

        }


        private void ThreadRunner64()
        {
            Log.Log("Starting x64 Thread");

            //var ob = new Obex(x64Sock);

            //var example = ob.ReceiveObject();

            //if (example is HardwareChangedEventArgs e)
            //{
            //    Log.Log("Received HardwareChangedEvent");
            //}

            while (!GlobalCT.IsCancellationRequested && !X64CTS.Token.IsCancellationRequested)
            {
                Thread.Sleep(0);

                if (x64Sock == null || (x64Sock != null && x64Sock.Connected == false))
                {
                    Thread.Yield();
                    continue;
                }

                try
                {
                    var os = new OUTPUT_STRUCT();
                    var b = Read(x64Sock, osLen);

                    if (b == null || b.Length == 0 || x64Sock == null || (x64Sock != null && x64Sock.Connected == false))
                    {
                        Thread.Yield();
                        continue;
                    }


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
                        DoDestroyed((IntPtr)os.LongData1, true);
                    }
                    else if (os.msg == MSG_REPLACED)
                    {
                        DoReplaced((IntPtr)os.LongData1, (IntPtr)os.LongData2, true);
                    }
                    else if (os.msg == MSG_MOVESIZE)
                    {
                        DoMoveSize((IntPtr)os.LongData1, os.rect, true);
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

                            AddEvent(smsg, sname, (int)os.LongData1, os.msg, WorkerKind.Is64Worker);
                        });

                        _ = Task.Run(async () =>
                        {
                            HasInternet = await SysInfo.GetHasInternetAsync();
                        });

                    }
                    else
                    {
                        if (os.msg == MSG_HOOK_REPLACED)
                        {
                            while (!RefreshWindows())
                            {
                                Thread.Yield();
                            }
                        }

                        disp.Invoke(() =>
                        {
                            WorkNotify?.Invoke(this, new WorkerNotifyEventArgs(os, WorkerKind.Is64Worker));
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Log("64 thread: " + ex.Message);

                }

            }
        }

        private void ThreadRunner86()
        {
            Log.Log("Starting x86 Thread");
            while (!GlobalCT.IsCancellationRequested && !X86CTS.Token.IsCancellationRequested)
            {
                Thread.Sleep(0);
                if (x86Sock == null || (x86Sock != null && x86Sock.Connected == false))
                {
                    Thread.Yield();
                    continue;
                }

                try
                {
                    var os = new OUTPUT_STRUCT();
                    var b = Read(x86Sock, osLen);
                    if (b == null || b.Length == 0 || x86Sock == null || (x86Sock != null && x86Sock.Connected == false))
                    {
                        Thread.Yield();
                        continue;
                    }

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
                        DoDestroyed((IntPtr)os.IntData1, false);
                    }
                    else if (os.msg == MSG_REPLACED)
                    {
                        DoReplaced((IntPtr)os.IntData1, (IntPtr)os.IntData2, false);
                    }
                    else if (os.msg == MSG_MOVESIZE)
                    {
                        DoMoveSize((IntPtr)os.IntData1, os.rect, false);
                    }
                    else
                    {
                        if (os.msg == MSG_HOOK_REPLACED)
                        {
                            while (!RefreshWindows())
                            {
                                Thread.Yield();
                            }
                        }

                        disp.Invoke(() =>
                        {
                            WorkNotify?.Invoke(this, new WorkerNotifyEventArgs(os, WorkerKind.Is86Worker));
                        });
                    }
                }
                catch(Exception ex)
                {
                    Log.Log("86 thread: " + ex.Message);

                }

            }
        }


        private void DoMoveSize(IntPtr handle, W32RECT rc, bool x64)
        {
            disp.Invoke(() =>
            {
                wndMon = monitors.GetMonitorFromWindow(handle);

                W32POINT pt = new W32POINT();
                GetCursorPos(ref pt);

                mouseMon = monitors.GetMonitorFromPoint(pt);

                if (wndMon == null || mouseMon == null)
                {
                    AddEvent(handle, 0, MSG_MOVESIZE, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                    return;
                }

                AddEvent(handle, wndMon.Index, MSG_MOVESIZE, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
            });


        }

        private void DoReplaced(IntPtr oldHandle, IntPtr newHandle, bool x64)
        {
            disp.Invoke(() =>
            {
                if (ActiveWindows.ContainsKey(oldHandle))
                {
                    var actWnd = ActiveWindows[oldHandle];

                    ActiveWindows.Remove(oldHandle);

                    actWnd.Timestamp = DateTime.Now;
                    ActiveWindows.Add(newHandle, actWnd);

                    AddEvent(null, actWnd.WindowName, 0, MSG_REPLACED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                }
                else
                {
                    AddEvent(oldHandle, 0, MSG_REPLACED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                }

            });

        }

        private void DoDestroyed(IntPtr Handle, bool x64)
        {
            disp.Invoke(() =>
            {
                if (ActiveWindows.ContainsKey(Handle))
                {
                    var actWnd = ActiveWindows[Handle];

                    ActiveWindows.Remove(Handle);
                    AddEvent(null, actWnd.WindowName, 0, MSG_DESTROYED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                }
                else
                {
                    AddEvent(Handle, 0, MSG_DESTROYED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                }

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
                    AddEvent(Handle, 0, MSG_CREATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                    return;
                }

                AddEvent(Handle, wndMon.Index, MSG_CREATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
            });

        }

        private void DoActivate(IntPtr Handle, bool x64)
        {
            disp.Invoke(() =>
            {
                ActWndInfo actWnd = default;

                if (ActiveWindows.TryGetValue(Handle, out actWnd))
                {
                    if (DateTime.Now - actWnd.Timestamp < new TimeSpan(0, 0, 2))
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
                    ActiveWindows.Add(Handle, new ActWndInfo() { WindowName = GetWindowName(Handle), Timestamp = DateTime.Now });
                    DoWindowMove(Handle, x64);
                    return;
                }


                W32POINT pt = new W32POINT();
                GetCursorPos(ref pt);

                mouseMon = monitors.GetMonitorFromPoint(pt);

                if (wndMon == null || mouseMon == null)
                {
                    AddEvent(Handle, 0, MSG_ACTIVATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                    return;
                }

                AddEvent(Handle, wndMon.Index, MSG_ACTIVATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
            });
        }

        void AddEvent(string smsg, string wname, int monitor, int action, WorkerKind origin)
        {
            _logger?.LogInformation(smsg);
            Log.Log(smsg);

            var e = new WorkerLogEventArgs(smsg, wname, IntPtr.Zero, monitor, action, origin);
            WorkLogger?.Invoke(this, e);

        }

        void AddEvent(IntPtr handle, int monitor, int action, WorkerKind origin) 
        {
            string smsg;
            string wname = GetWindowName(handle);

            switch(action)
            {
                case MSG_CREATED:

                    smsg = $"Window Created {wname}";
                    if (monitor > 0) smsg += $" on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_ACTIVATED:
                    smsg = $"Window Activated {wname}";
                    if (monitor > 0) smsg += $" on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_DESTROYED:
                    smsg = $"Window Destroyed {wname}";
                    if (monitor > 0) smsg += $" on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;

                case MSG_HW_CHANGE:
                    smsg = "Monitor Plugged In/Unplugged";

                    break;

                default:
                    smsg = $"Unknown Window Event {action} on {wname}";
                    if (monitor > 0) smsg += $" on monitor #{monitor}";
                    else smsg += "on unknown monitor";

                    break;
            }

            _logger?.LogInformation(smsg);
            Log.Log(smsg);

            var e = new WorkerLogEventArgs(wname, handle, monitor, action, origin);
            WorkLogger?.Invoke(this, e);
        }

        private string GetWindowName(IntPtr hwnd)
        {
            try
            {
                StringBuilder Title = new StringBuilder(256);
                GetWindowText(hwnd, Title, 256);

                return Title.ToString().Trim();
            }
            catch
            {
                return "";
            }
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
                        AddEvent(Handle, 0, MSG_ACTIVATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);
                        return;
                    }

                    AddEvent(Handle, wndMon.Index, MSG_ACTIVATED, x64 ? WorkerKind.Is64Worker : WorkerKind.Is86Worker);

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
                if (connSock == null || (connSock != null && connSock.Connected == false))
                {
                    Thread.Yield();
                    return null;
                }

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
                if (connSock == null || (connSock != null && connSock.Connected == false))
                {
                    Thread.Yield();
                    return null;
                }

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
            if (connSock == null || (connSock != null && connSock.Connected == false))
            {
                Thread.Yield();
                return;
            }

            connSock.Send(buffer);
        }

    }
}
