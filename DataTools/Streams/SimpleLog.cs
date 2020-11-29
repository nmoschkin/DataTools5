using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTools.Streams
{
    public class SimpleLog
    {

        public bool DebugOnly { get; private set; }

        public bool IsOpened { get; private set; }

        public FileStream Stream { get; private set; }

        public string Filename { get; set; }

        public SimpleLog(bool debugOnly = true)
        {
            DebugOnly = debugOnly;
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

        public SimpleLog(string fileName, bool open = true, bool debugOnly = true)
        {
            DebugOnly = debugOnly;
            Filename = fileName;
            if (open) OpenLog();
        }

        public void OpenLog(string fileName = null)
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

        public void Close()
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

        public void Log(string message)
        {
#if !DEBUG 
            if (DebubOnly) return;
#endif
            try
            {
                if (!IsOpened) return;
                Stream.Write(Encoding.UTF8.GetBytes($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF")}]: {message}\r\n"));
            }
            catch
            {

            }
        }

    }
}
