using DataTools.Win32Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Logging.EventSource;
using MMHLR64;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MMWndT
{
    
    public partial class App : Application
    {

        public Worker Work;
        public CancellationTokenSource CTS = new CancellationTokenSource();

        public Task WorkerTask;

        public Dispatcher MainDispatcher;
        
        public static new App Current
        {
            get
            {
                return (App)Application.Current;
            }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public App()
        {

            //InitializeComponent();

            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Work = new Worker();
            MainDispatcher = Dispatcher;


            //var rectConv = new StructConverter<W32RECT>();


            //var ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 43039);

            //var ob1 = Obex<OUTPUT_STRUCT>.CreateServer(ep);
            //var ob2 = Obex<OUTPUT_STRUCT>.CreateClient();

            //ob1.JsonSettings.Converters = new JsonConverter[] { (rectConv) };
            //ob2.JsonSettings.Converters = new JsonConverter[] { (rectConv) };

            //OUTPUT_STRUCT stest1 = new OUTPUT_STRUCT()
            //{
            //    cb = Marshal.SizeOf<OUTPUT_STRUCT>(),
            //    LongData1 = 0x49938277d92ef,
            //    LongData2 = 0xfde99a8eb0bb2,
            //    msg = 144,
            //    rect = new W32RECT(-500, 20, 640, 580)
            //};


            //ob1.ObjectReceived += Ob1_ObjectReceived;
            //ob2.ObjectReceived += Ob2_ObjectReceived;

            //ob1.StartListening();
            ////Thread.Sleep(1000);

            //ob2.Connect(ep);
            //ob2.StartListening();

            //ob1.SendObject(stest1);

            //Thread.Sleep(1000);

            //ob1.Dispose();
            //ob2.Dispose();


            //Environment.Exit(0);

            WorkerTask = Work.StartAsync(CTS.Token);
            
            //Application.Run(new frmMain());

            //CTS.Cancel();
        }

        OUTPUT_STRUCT stest;

        private void Ob2_ObjectReceived(object sender, ObjectReceivedEventArgs<OUTPUT_STRUCT> e)
        {
            stest = e.Object;
        }

        private void Ob1_ObjectReceived(object sender, ObjectReceivedEventArgs<OUTPUT_STRUCT> e)
        {
            
        }
    }


    internal class WindowLogEventArgs : EventArgs
    {
        public string Message { get; private set; }


    }

}
