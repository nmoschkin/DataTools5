using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DataTools.Win32
{
    /// <summary>
    /// Helper static class to open files using the Windows shell.
    /// </summary>
    public static class ShellExecuteHelper
    {
        /// <summary>
        /// Execute a shell command on a file.
        /// </summary>
        /// <param name="filename">The filename of the shell command to execute on.</param>
        /// <param name="verb">The verb to execute</param>
        public static void Execute(string filename, string verb = "open")
        {
            if (!File.Exists(filename) && !Directory.Exists(filename)) throw new FileNotFoundException(filename);

            var shex = new SHELLEXECUTEINFO();
            var proc = Process.GetCurrentProcess();

            shex.cbSize = Marshal.SizeOf(shex);
            shex.nShow = User32.SW_SHOW;
            shex.hInstApp = proc.Handle;
            shex.hWnd = proc.MainWindowHandle;
            shex.lpVerb = verb;

            // Set the parsing name exactly this way.
            shex.lpDirectory = Path.GetDirectoryName(filename);

            shex.lpFile = filename;
            shex.fMask = User32.SEE_MASK_ASYNCOK | User32.SEE_MASK_FLAG_DDEWAIT | User32.SEE_MASK_UNICODE;
            User32.ShellExecuteEx(ref shex);
        }
    }
}
