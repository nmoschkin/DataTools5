using DataTools.Desktop;
using DataTools.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static DataTools.Win32Api.User32;

namespace MMWndT
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Dictionary<string, ImageSource> cache = new Dictionary<string, ImageSource>();

        private WorkerLogEventArgs source;

        private ImageSource icon;

        public ImageSource Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged();
            }
        }

        public WorkerLogEventArgs Source
        {
            get => source;
            private set
            {
                source = value;
                OnPropertyChanged();
            }
        }

        private IntPtr hWnd;

        public IntPtr Handle
        {
            get => hWnd;
            set
            {
                if (hWnd == value) return;
                hWnd = value;
                OnPropertyChanged();
            }
        }


        private string name;

        public string WindowName
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
            }
        }


        private string module;

        public string Module
        {
            get => module;
            set
            {
                if (module == value) return;
                module = value;
                OnPropertyChanged();
            }
        }


        private string msg;

        public string Message
        {
            get => msg;
            set
            {
                if (msg == value) return;
                msg = value;
                OnPropertyChanged();
            }
        }





        private int monitor;

        public int Monitor
        {
            get => monitor;
            set
            {
                if (monitor == value) return;
                monitor = value;
                OnPropertyChanged();
                OnPropertyChanged("MonitorText");
            }
        }

        public string MonitorText
        {
            get
            {
                if (Event == Worker.MSG_HW_CHANGE)
                {
                    return $"Event #{Monitor}";
                }
                else
                {
                    return $"Monitor {monitor}";
                }
            }
        }


        private DateTime timeStamp;

        public DateTime Timestamp
        {
            get => timeStamp;
            set
            {
                if (timeStamp == value) return;
                timeStamp = value;
                OnPropertyChanged();
            }
        }

        private int ev;

        public int Event
        {
            get => ev;
            set
            {
                if (ev == value) return;
                ev = value;

                string s;
                switch (ev)
                {
                    case Worker.MSG_HW_CHANGE:


                        if ((uint)Monitor == DevNotify.DBT_DEVICEARRIVAL)
                        {
                            s = "Monitor Plugged In";
                        }
                        else if ((uint)Monitor == DevNotify.DBT_DEVICEREMOVECOMPLETE)
                        {
                            s = "Monitor Unplugged";
                        }
                        else
                        {
                            s = "Hardware Changed";
                        }

                        break;

                    case Worker.MSG_REPLACED:
                        s = "Window Replaced";
                        break;

                    case Worker.MSG_MOVESIZE:
                        s = "Window Move/Size";
                        break;

                    case Worker.MSG_ACTIVATED:
                        s = "Activated";
                        break;

                    case Worker.MSG_CREATED:
                        s = "Created";
                        break;

                    case Worker.MSG_DESTROYED:
                        s = "Destroyed";
                        break;

                    case Worker.MSG_ERROR:
                        s = "Error";
                        break;

                    default:
                        s = "Unknown Event";
                        break;
                }

                eventText = s;

                OnPropertyChanged();
                OnPropertyChanged("EventText");
            }
        }

        private string eventText;

        public string EventText
        {
            get => eventText;
        }




        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EventViewModel(WorkerLogEventArgs e)
        {
            Source = e;

            _ = Task.Run(() =>
            {
                try
                {

                    //string sPath;

                    //if (lstEvents.Items.Count > 100)
                    //{
                    //    lstEvents.Items.RemoveAt(lstEvents.Items.Count - 1);
                    //}

                    Timestamp = DateTime.Now;

                    Monitor = e.Monitor;
                    Event = e.Action;

                    Message = e.Message;
                    WindowName = e.WindowName;
                    Handle = e.Handle;

                    // nothing more to do if there's no handle.
                    if (e.Handle == IntPtr.Zero) return;

                    if (e.Action != Worker.MSG_DESTROYED)
                    {
                        ImageSource icon = null;

                        var sb = new StringBuilder();
                        sb.Capacity = 512;

                        Process p = GetWindowProcess(e.Handle);

                        var sp = p.MainModule.FileName;
                        Module = sp;

                        //if (sp != null)
                        //{
                        //    sp = Path.GetFileName(sp);

                        //    if (sp == "chrome.exe")
                        //    {
                        //        //List<IntPtr> lchild = new List<IntPtr>();

                        //        //EnumChildWindows(e.Handle, (hwnd, lParam) =>
                        //        //{
                        //        //    lchild.Add(hwnd);
                        //        //    return true;
                        //        //}, IntPtr.Zero);

                        //        Module = GetChromeWindowUrl(WindowName);
                        //        var url = Module;
                        //        int i = url.IndexOf("/");
                        //        if (i != -1) url = url.Substring(0, i);
                        //        url = "https://" + url + "/favicon.ico";

                        //        Task.Run(async () =>
                        //        {
                        //            if (!cache.ContainsKey(url))
                        //            {
                        //                HttpClient cli = new HttpClient();

                        //                var res = cli.GetAsync(new Uri(url));
                        //                Stream stream = null;

                        //                stream = await res.Result.Content.ReadAsStreamAsync();

                        //                if (stream != null)
                        //                {
                        //                    icon = BitmapTools.MakeWPFImage(new System.Drawing.Icon(stream));
                        //                }

                        //                cache.Add(url, icon);
                        //            }
                        //            else
                        //            {
                        //                icon = cache[url];
                        //            }

                        //            Icon = icon;

                        //        });

                        //        return;
                        //    }

                        //}

                        if (!cache.ContainsKey(e.Handle.ToString()))
                        {
                            icon = BitmapTools.MakeWPFImage(GetWindowIcon(e.Handle, 1));
                            cache.Add(e.Handle.ToString(), icon);
                        }
                        else
                        {
                            icon = cache[e.Handle.ToString()];
                        }

                        Icon = icon;
                    }
                    else
                    {
                        if (cache.ContainsKey(e.Handle.ToString()))
                        {
                            cache.Remove(e.Handle.ToString());
                        }
                    }

                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }

            });


        }

    }



    public class ActionToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int act)
            {
                switch (act)
                {
                    case Worker.MSG_HW_CHANGE:

                        return new SolidColorBrush(Colors.DarkGray);

                    case Worker.MSG_REPLACED:
                        return new SolidColorBrush(Colors.Blue);

                    case Worker.MSG_MOVESIZE:
                        return new SolidColorBrush(Colors.DarkSlateBlue);

                    case Worker.MSG_ACTIVATED:
                        return new SolidColorBrush(Colors.Blue);

                    case Worker.MSG_CREATED:
                        return new SolidColorBrush(Colors.Green);

                    case Worker.MSG_DESTROYED:
                        return new SolidColorBrush(Colors.Red);

                    default:

                        return value;
                }

            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
