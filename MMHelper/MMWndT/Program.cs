using DataTools.Win32Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Logging.EventSource;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace MMWndT
{
    internal static class Program
    {

        public static Worker Work;
        public static CancellationTokenSource CTS = new CancellationTokenSource();

        public static Dispatcher MainDispatcher;
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Work = new Worker();
            MainDispatcher = Dispatcher.CurrentDispatcher;

            _ = Work.StartAsync(CTS.Token);
            
            Application.Run(new frmMain());

            CTS.Cancel();
        }

    }


    internal class WindowLogEventArgs : EventArgs
    {
        public string Message { get; private set; }


    }

}
