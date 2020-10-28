using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTools.Streams
{
    public class SimpleLog
    {

        public bool IsOpened { get; private set; }

        public FileStream Stream { get; private set; }

        public string Filename { get; set; }

        public SimpleLog()
        {
           
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

        public SimpleLog(string fileName, bool open = true)
        {
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

            Stream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.Read);
            IsOpened = true;
        }

        public void Close()
        {
            Stream?.Close();
            Stream = null;
            IsOpened = false;
        }

        public void Log(string message)
        {
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
