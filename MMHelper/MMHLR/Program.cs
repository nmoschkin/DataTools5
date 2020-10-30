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
using DataTools.Streams;

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

        public static string PrintMsg(int msg)
        {

            switch(msg)
            {

                case MSG_QUERY_STATE:
                    return "MSG_QUERY_STATE";

                case MSG_HOOK_REPLACED:
                    return "MSG_HOOK_REPLACED";

                case MSG_STOP_MOVER:
                    return "MSG_STOP_MOVER";

                case MSG_START_MOVER:
                    return "MSG_START_MOVER";

                case MSG_CREATED:
                    return "MSG_CREATED";

                case MSG_ACTIVATED:
                    return "MSG_ACTIVATED";

                case MSG_DESTROYED:
                    return "MSG_DESTROYED";

                case MSG_MOVESIZE:
                    return "MSG_MOVESIZE";

                case MSG_REPLACED:
                    return "MSG_REPLACED";

                case MSG_TERMINATE:
                    return "MSG_TERMINATE";
                case MSG_SHUTDOWN:
                    return "MSG_SHUTDOWN";

                case MSG_INFORM_MY:
                    return "MSG_INFORM_MY";

                case MSG_HW_CHANGE:
                    return "MSG_HW_CHANGE";

                case MSG_ERROR:
                    return "MSG_ERROR";

                default:
                    return msg.ToString();
            }
        }

        private static Socket listenSock;
        private static Socket connSock = null;

        private static readonly Guid MagicKey = Guid.Parse("{4D8E985F-79C1-4AF9-B481-8846FEB6C7DA}");

        public static readonly CancellationTokenSource Canceller = new CancellationTokenSource();

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        static GlobalHooks gh;

#if X64
        public static SimpleLog Log { get; set; } = new SimpleLog("MMHLR64.log");
#else
        public static SimpleLog Log { get; set; } = new SimpleLog("MMHLR32.log");
//            Debugger.Launch();
#endif


        [STAThread]
        static void Main(string[] args)
        {
//            AllocConsole();

            gh = new GlobalHooks(IntPtr.Zero);
            
            gh.Shell.HookReplaced += Shell_HookReplaced;
            
            gh.Shell.WindowCreated += Shell_WindowCreated;
            gh.Shell.WindowActivated += Shell_WindowActivated;
            gh.Shell.WindowDestroyed += Shell_WindowDestroyed;
            gh.Shell.WindowReplaced += Shell_WindowReplaced;

            gh.CBT.HookReplaced += CBT_HookReplaced;

            gh.CBT.MoveSize += CBT_MoveSize;
            gh.HardwareChanged += HardwareChanged;
            gh.SystemShutdown += SystemShutdown;

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
                        Log.Log($"Opening Listening Socket on port {ep.Port}");
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

                        if (dcmd.msg == MSG_TERMINATE)
                        {
                            Log.Log("Got termination event.  Shutting down.");
                            break;
                        }

                        else if (dcmd.msg == MSG_START_MOVER)
                        {
                            Log.Log("Starting mover.");

                            if (!gh.Shell.IsActive) gh.Shell.Start();
                            if (!gh.CBT.IsActive) gh.CBT.Start();

                        }
                        else if (dcmd.msg == MSG_STOP_MOVER)
                        {
                            Log.Log("Stopping mover.");

                            if (gh.Shell.IsActive) gh.Shell.Stop();
                            if (gh.CBT.IsActive) gh.CBT.Stop();
                        }

                        else if (dcmd.msg == MSG_QUERY_STATE)
                        {
                            if (gh.Shell.IsActive && gh.CBT.IsActive)
                            {

                                Log.Log("Got query state. State: Is Running.");
                                Monitor.Enter(connSock);
                                SendShell(MSG_START_MOVER, IntPtr.Zero);
                                Monitor.Exit(connSock);
                            }
                            else
                            {
                                Log.Log("Got query state. State: Is Stopped.");
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
                    Log.Log(ex.Message);

                    if (gh.Shell.IsActive)
                    {
                        gh.Shell.Stop();
                        gh.Shell.Start();
                    }
                    if (gh.CBT.IsActive)
                    {
                        gh.CBT.Stop();
                        gh.CBT.Start();
                    }

                }
            }
            
            try
            {
                Log.Close();
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

        private static void SystemShutdown(object sender, SystemShutdownEventArgs e)
        {
            SendShell(MSG_SHUTDOWN, IntPtr.Zero);
        }

        private static void CBT_HookReplaced()
        {
            SendShell(MSG_HOOK_REPLACED, IntPtr.Zero);
        }

        private static void Shell_HookReplaced()
        {
            SendShell(MSG_HOOK_REPLACED, IntPtr.Zero);
        }

        private static void SendShell(int msg, IntPtr Handle, string text = null, IntPtr? extra = null, W32RECT? rect = null)
        {

            try
            {

                Log.Log($"SendShell: Message {PrintMsg(msg)}, Handle: {Handle}, Text: {text}, Extra: {extra}, rect: {rect}");

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
                Log.Log(ex.Message);
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
