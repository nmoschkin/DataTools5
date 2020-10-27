using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using DataTools.Win32Api;

namespace MMHLR
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


    public static class Program
    {


        public const int MSG_QUERY_STATE = 411;
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

        private static Socket listenSock;
        private static Socket connSock = null;

        private static readonly Guid MagicKey = Guid.Parse("{4D8E985F-79C1-4AF9-B481-8846FEB6C7DA}");

        public static readonly CancellationTokenSource Canceller = new CancellationTokenSource();

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        static GlobalHooks gh;

        [STAThread]
        static void Main(string[] args)
        {
//#if X64
//#else
//            Debugger.Launch();
//#endif
//            AllocConsole();

            gh = new GlobalHooks(IntPtr.Zero);

            gh.Shell.WindowCreated += Shell_WindowCreated;
            gh.Shell.WindowActivated += Shell_WindowActivated;
            gh.Shell.WindowDestroyed += Shell_WindowDestroyed;
            gh.Shell.WindowReplaced += Shell_WindowReplaced;
            
            gh.CBT.MoveSize += CBT_MoveSize;
            gh.HardwareChanged += HardwareChanged;

#if X64
            var ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 53112);
#else
            var ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 53113);
#endif

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            while (!Canceller.Token.IsCancellationRequested)
            {
                Application.DoEvents();
                Thread.Sleep(100);

                try
                {
                    if (connSock == null || !connSock.Connected)
                    {
                        if (gh.Shell.IsActive) gh.Shell.Stop();
                        if (gh.CBT.IsActive) gh.CBT.Stop();

                        listenSock = new Socket(SocketType.Stream, ProtocolType.Tcp);

                        listenSock.Blocking = true;
                        listenSock.Bind(ep);

                        listenSock.Listen();

                        connSock = listenSock.Accept();

                        gh.Shell.Start();
                        gh.CBT.Start();
                    }


                    if (connSock.Available >= 1)
                    {
                        var b = ReadAuto();
                        GCHandle gch = GCHandle.Alloc(b, GCHandleType.Pinned);

                        var dcmd = Marshal.PtrToStructure<OUTPUT_STRUCT>(gch.AddrOfPinnedObject());
                        gch.Free();
                        if (dcmd.msg == MSG_TERMINATE) break;

                        else if (dcmd.msg == MSG_START_MOVER)
                        {
                            if (!gh.Shell.IsActive) gh.Shell.Start();
                            if (!gh.CBT.IsActive) gh.CBT.Start();

                        }
                        else if (dcmd.msg == MSG_STOP_MOVER)
                        {
                            if (gh.Shell.IsActive) gh.Shell.Stop();
                            if (gh.CBT.IsActive) gh.CBT.Stop();
                        }

                        else if (dcmd.msg == MSG_QUERY_STATE)
                        {
                            if (gh.Shell.IsActive)
                            {

                                Monitor.Enter(connSock);
                                SendShell(MSG_START_MOVER, IntPtr.Zero);
                                Monitor.Exit(connSock);
                            }
                            else
                            {
                                Monitor.Enter(connSock);
                                SendShell(MSG_STOP_MOVER, IntPtr.Zero);
                                Monitor.Exit(connSock);
                            }
                        }
                    }

                }
                catch(Exception ex)
                {
                    SendShell(MSG_ERROR, IntPtr.Zero, ex.Message);
                }
            }
            
            try
            {
                connSock?.Close();

                gh?.Shell?.Stop();
                gh?.CBT?.Stop();

                gh?.DestroyHandle();

                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        private static void SendShell(int msg, IntPtr Handle, string text = null, IntPtr? extra = null, W32RECT? rect = null)
        {

            try
            {

                int ss = Marshal.SizeOf<OUTPUT_STRUCT>();
                byte[] tbytes = null;

                if (text != null && text.Length > 0)
                {
                    tbytes = Encoding.Unicode.GetBytes(text);
                    ss += tbytes.Length;
                }

                if (extra == null) extra = IntPtr.Zero;
                if (rect == null)
                {
                    rect = new W32RECT();
                }

                var os = new OUTPUT_STRUCT()
                {
                    cb = ss,
                    msg = msg,
#if X64
                    LongData1 = (long)Handle,
                    LongData2 = (long)extra,
#else
                    IntData1 = (int)Handle,
                    IntData2 = (int)extra,
#endif

                    rect = (W32RECT)rect
                };

                var bcpy = new byte[ss];
                var gch = GCHandle.Alloc(bcpy, GCHandleType.Pinned);

                Marshal.StructureToPtr(os, gch.AddrOfPinnedObject(), false);
                gch.Free();

                Write(bcpy);
                if (tbytes != null) Write(tbytes);
            }
            catch(Exception ex)
            {

                // usually we'd log an error, but whatfor the logger?
                Console.WriteLine(ex.Message);
            }
        }

        private static void HardwareChanged(object sender, HardwareChangedEventArgs e)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_HW_CHANGE, (IntPtr)e.Message, e.DeviceName);
                Monitor.Exit(connSock);
            }
            else
            {
                HardwareChanged(sender, e);
            }
        }

        private static void CBT_MoveSize(IntPtr Handle, W32RECT rc)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_MOVESIZE, Handle, null, null, rc);
                Monitor.Exit(connSock);
            }
            else
            {
                CBT_MoveSize(Handle, rc);
            }
        }



        private static void Shell_WindowReplaced(IntPtr OldHandle, IntPtr NewHandle)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_REPLACED, OldHandle, null, (IntPtr?)NewHandle);
                Monitor.Exit(connSock);
            }
            else
            {
                Shell_WindowReplaced(OldHandle, NewHandle);
            }
        }

        private static void Shell_WindowCreated(IntPtr Handle)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_CREATED, Handle);
                Monitor.Exit(connSock);
            }
            else
            {
                Shell_WindowCreated(Handle);
            }
        }

        private static void Shell_WindowActivated(IntPtr Handle)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_ACTIVATED, Handle);
                Monitor.Exit(connSock);
            }
            else
            {
                Shell_WindowActivated(Handle);
            }
        }
        private static void Shell_WindowDestroyed(IntPtr Handle)
        {
            if (Monitor.TryEnter(connSock))
            {
                SendShell(MSG_DESTROYED, Handle);
                Monitor.Exit(connSock);
            }
            else
            {
                Shell_WindowDestroyed(Handle);
            }
        }

        /// <summary>
        /// Reads count bytes, blocks until count bytes are received
        /// </summary>
        /// <param name="count">The bytes to read</param>
        /// <returns></returns>
        private static byte[] Read(int count)
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

        private static byte[] ReadAuto()
        {
            if (connSock == null || (connSock != null && connSock.Connected == false))
            {
                Thread.Yield();
                return null;
            }

            Monitor.Enter(connSock);

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

            Array.Resize(ref input, count);

            t = 4;
            c = count - 4;

            while (t < c)
            {
                r = connSock.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count) break;
                Thread.Sleep(0);
            }

            Monitor.Exit(connSock);
 
            return input;

        }

        private static void Write(byte[] buffer)
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
