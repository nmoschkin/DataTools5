using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Hardware.Display;
using DataTools.Win32Api;
using static DataTools.Win32Api.User32;
using System.Text;

namespace MMWndSvc
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

    

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private CancellationToken ttok;

        private Process x86proc;
        private Process x64proc;

        private Thread x86Thread;
        private Thread x64Thread;

        MonitorInfo mouseMon;
        MonitorInfo wndMon;

        Dictionary<IntPtr, DateTime> actWnds = new Dictionary<IntPtr, DateTime>();

        private Monitors monitors = new Monitors();

        private readonly Guid MagicKey = Guid.Parse("{4D8E985F-79C1-4AF9-B481-8846FEB6C7DA}");
        int cb = Marshal.SizeOf<OUTPUT_STRUCT>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ttok = stoppingToken;

            var stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR32.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            x86proc = Process.Start(stinfo);

            x86proc.StandardInput.Write(MagicKey.ToByteArray());


            stinfo = new ProcessStartInfo()
            {
                FileName = "MMHLR64.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            x64proc = Process.Start(stinfo);

            x64proc.StandardInput.Write(MagicKey.ToByteArray());

            x86Thread = new Thread(ThreadRunner86);
            x64Thread = new Thread(ThreadRunner64);

            x86Thread.Start();
            x64Thread.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Yield();
            }

            Write(x86proc.StandardInput.BaseStream, new byte[] { 27 });
            Write(x64proc.StandardInput.BaseStream, new byte[] { 27 });

            x86proc.Close();
            x64proc.Close();
        }

        private void ThreadRunner64()
        {
            while (!ttok.IsCancellationRequested)
            {
                var os = new OUTPUT_STRUCT();
                var b = Read(x64proc.StandardOutput.BaseStream, cb);

                MemPtr.Union(ref b, out os);

                if (os.msg == 1)
                {
                    DoCreated((IntPtr)os.LongData1, true);
                }
                else if (os.msg == 2)
                {
                    DoActivate((IntPtr)os.LongData1, true);
                }
                else if (os.msg == 3)
                {
                    DoDestroyed((IntPtr)os.LongData1);
                }
                
            }
        }

        private void ThreadRunner86()
        {
            while (!ttok.IsCancellationRequested)
            {
                var os = new OUTPUT_STRUCT();
                var b = Read(x86proc.StandardOutput.BaseStream, cb);

                MemPtr.Union(ref b, out os);

                if (os.msg == 1)
                {
                    DoCreated((IntPtr)os.IntData1, false);
                }
                else if (os.msg == 2)
                {
                    DoActivate((IntPtr)os.IntData1, false);
                }
                else if (os.msg == 3)
                {
                    DoDestroyed((IntPtr)os.IntData1);
                }

            }
        }

        private void DoDestroyed(IntPtr Handle)
        {

            if (actWnds.ContainsKey(Handle))
            {
                actWnds.Remove(Handle);
            }

            AddEvent($"Window Destroyed {GetWindowName(Handle)}");
        }

        private void DoCreated(IntPtr Handle, bool x64)
        {
            //if (!actWnds.ContainsKey(Handle))
            //{
            //    actWnds.Add(Handle, DateTime.Now);
            //}

            wndMon = monitors.GetMonitorFromWindow(Handle);
            if (wndMon == null || mouseMon == null)
            {
                AddEvent($"Window Created {GetWindowName(Handle)} unknown monitor.");
                return;
            }

            AddEvent($"Window Created {GetWindowName(Handle)} on monitor #{wndMon.Index}");
        }

        private void DoActivate(IntPtr Handle, bool x64)
        {
            var dt = DateTime.Now;

            if (actWnds.TryGetValue(Handle, out dt))
            {
                if (DateTime.Now - dt < new TimeSpan(0, 0, 2))
                {
                    DoWindowMove(Handle, x64);
                }
            }
            else
            {
                actWnds.Add(Handle, DateTime.Now);
                DoWindowMove(Handle, x64);
            }

            var wndMon = monitors.GetMonitorFromWindow(Handle);
            if (wndMon == null || mouseMon == null)
            {
                AddEvent($"Window Activated {GetWindowName(Handle)} unknown monitor.");
                return;
            }

            AddEvent($"Window Activated {GetWindowName(Handle)} on monitor #{wndMon.Index}");
        }

        void AddEvent(string e) 
        {
            _logger.LogInformation(e);
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
                    AddEvent($"Window Created {GetWindowName(Handle)} unknown monitor.");
                    return;
                }

                AddEvent($"Window Created {GetWindowName(Handle)} on monitor #{wndMon.Index}");

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

                    wndMon = monitors.GetMonitorFromWindow(Handle);

                }

            });

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;

            t.Start();

        }


        /// <summary>
        /// Reads count bytes, blocks until count bytes are received
        /// </summary>
        /// <param name="count">The bytes to read</param>
        /// <returns></returns>
        private byte[] Read(Stream stdin, int count)
        {
            byte[] output = new byte[count];

            int t = 0, c = count;
            int r;

            while (t < c)
            {
                if (ttok.IsCancellationRequested) return null;
                r = stdin.Read(output, t, c);

                t += r;
                c -= r;

                if (t >= count) break;
            }

            return output;

        }

        private void Write(Stream stdout, byte[] buffer)
        {
            stdout.Write(buffer);
        }

      
    }
}
