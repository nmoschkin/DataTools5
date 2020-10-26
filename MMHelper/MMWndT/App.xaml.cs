using DataTools.Win32Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Logging.EventSource;

using System;
using System.Collections.Generic;
using System.Linq;
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

            WorkerTask = Work.StartAsync(CTS.Token);
            
            //Application.Run(new frmMain());

            //CTS.Cancel();
        }

    }


    internal class WindowLogEventArgs : EventArgs
    {
        public string Message { get; private set; }


    }

}
