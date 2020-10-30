using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMHLR
{

    public enum EndSessionTypes : uint
    {
        /// <summary>
        /// If wParam is TRUE, the application must shut down. Any data should be saved automatically without prompting the user (for more information, see Remarks). The Restart Manager sends this message when the application is using a file that needs to be replaced, when it must service the system, or when system resources are exhausted. The application will be restarted if it has registered for restart using the RegisterApplicationRestart function. For more information, see Guidelines for Applications.
        /// If wParam is FALSE, the application should not shut down.
        /// </summary>
        CloseApp = 0x1,
        /// <summary>
        /// The application is forced to shut down.
        /// </summary>
        Critical = 0x40000000,
        /// <summary>
        /// The user is logging off. For more information, see Logging Off.
        /// </summary>
        LogOff = 0x80000000
    }

    public class SystemShutdownEventArgs : EventArgs
    {

        public bool IsShuttingDown { get; private set; }

        public EndSessionTypes EndSessionType { get; private set; }


        public SystemShutdownEventArgs(bool isd, EndSessionTypes est)
        {
            IsShuttingDown = isd;
            EndSessionType = est;
        }

        public SystemShutdownEventArgs(IntPtr wParam, IntPtr lParam)
        {
            IsShuttingDown = wParam != IntPtr.Zero;
            EndSessionType = (EndSessionTypes)lParam;
        }


    }
}
