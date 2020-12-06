using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTools.Streams
{
    public class SimpleLog
    {

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole")]
        protected static extern bool _allocConsole();

        public static bool HasConsole { get; protected set; }

        public virtual bool DebugOnly { get; private set; }

        public virtual bool IsOpened { get; private set; }

        public virtual FileStream Stream { get; private set; }

        public virtual bool LogToConsole { get; set; }

        public virtual string Filename { get; set; }


        static SimpleLog()
        {
            try
            {
                var str = Console.OpenStandardInput();
                HasConsole = !(str.Equals(System.IO.Stream.Null));
                str.Close();
            }
            catch
            {
                HasConsole = false;
            }
        }

        public static bool AllocConsole()
        {
            var b = _allocConsole();
            HasConsole = b;
            return b;
        }

        public SimpleLog(bool debugOnly = true, bool logToConsole = true)
        {
            DebugOnly = debugOnly;
            LogToConsole = logToConsole;
        }

        ~SimpleLog()
        {
            try
            {
                Stream?.Close();
            }
            catch
            {

            }
        }

        public SimpleLog(string fileName, bool open = true, bool debugOnly = true, bool logToConsole = true)
        {
            DebugOnly = debugOnly;
            Filename = fileName;
            LogToConsole = logToConsole;

            if (open) OpenLog();
        }

        public virtual void OpenLog(string fileName = null)
        {

            if (fileName != null)
            {
                Filename = fileName;
            }
            else if (Filename == null)
            {
                throw new ArgumentNullException(nameof(fileName), "Must specify filename if property is not set.");
            }
#if !DEBUG 
            if (DebubOnly) 
            {
                IsOpened = true;
                return;
            }
#endif

            Stream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.Read);
            IsOpened = true;
        }

        public virtual void Close()
        {
#if !DEBUG 
            if (DebubOnly) 
            {
                IsOpened = false;
                return;
            }
#endif
            Stream?.Close();
            Stream = null;
            IsOpened = false;
        }

        public virtual void Log(string message)
        {
#if !DEBUG 
            if (DebubOnly) return;
#endif
            try
            {
                //yyyy-MM-dd HH:mm:ss.FFFFFFF
                string msg = $"[{DateTime.Now.ToString("G")}]: {message}\r\n";

                if (LogToConsole && HasConsole)
                {
                    Console.Write(msg);
                }

                if (!IsOpened) return;
                Stream.Write(Encoding.UTF8.GetBytes(msg));


            }
            catch
            {

            }
        }

    }
}
