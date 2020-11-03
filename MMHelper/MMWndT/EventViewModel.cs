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
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static DataTools.Win32Api.User32;
using MMHLR64;

namespace MMWndT
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Dictionary<string, ImageSource> cache = new Dictionary<string, ImageSource>();

        private EventArgs source;


        private WorkerKind origin;

        public WorkerKind Origin
        {
            get => origin;
            set
            {
                if (origin == value) return;
                origin = value;
                OnPropertyChanged("OriginText");
                OnPropertyChanged();
            }
        }

        public string OriginText
        {
            get
            {
                switch (origin)
                {
                    case WorkerKind.Is64Worker:
                        return "64-bit Worker";

                    case WorkerKind.Is86Worker:
                        return "32-bit Worker";

                    case WorkerKind.IsMainProgram:
                        return "Main Program";

                    default:
                        return origin.ToString();
                }
            }
        }

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

        public WorkerLogEventArgs LogSource
        {
            get
            {
                if (source is WorkerLogEventArgs e)
                    return e;
                else
                    return null;
            }
            private set
            {
                source = value;
                OnPropertyChanged();
            }
        }

        public WorkerNotifyEventArgs NotifySource
        {
            get
            {
                if (source is WorkerNotifyEventArgs e)
                    return e;
                else
                    return null;
            }
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
                else if (Event == Worker.MSG_SHUTDOWN)
                {
                    EndSessionTypes t = (EndSessionTypes)NotifySource.LongData2;

                    if (t == EndSessionTypes.Critical)
                    {
                        return "Force Close";
                    }
                    else if (t == EndSessionTypes.LogOff)
                    {
                        return "Logging Off";
                    }
                    else if (t == EndSessionTypes.CloseApp)
                    {
                        return "Shutting Down App";
                    }
                    else
                    {
                        return $"Shutdown Event #{t}";
                    }
                }
                else if (Event == Worker.MSG_START_MOVER)
                {
                    return "Start Event";
                }
                else if (Event == Worker.MSG_STOP_MOVER)
                {
                    return "Stop Event";
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

                    case Worker.MSG_TERMINATE:
                        s = "Terminate";
                        break;

                    case Worker.MSG_SHUTDOWN:
                        s = "Shutdown";
                        break;

                    case Worker.MSG_HOOK_REPLACED:
                        s = "Hook Replaced";
                        break;

                    case Worker.MSG_START_MOVER:
                        s = "Worker Started";
                        break;

                    case Worker.MSG_STOP_MOVER:
                        s = "Worker Stopped";
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

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EventViewModel(WorkerNotifyEventArgs e)
        {
            NotifySource = e;

            if (e.Message == Worker.MSG_SHUTDOWN)
            {

                string shut = "";

                if (e.LongData1 == 1)
                {
                    shut += "System is shutting down.";
                }

                EndSessionTypes t = (EndSessionTypes)e.LongData2;

                if (t == EndSessionTypes.Critical)
                {
                    shut += " App is being forced closed.";
                }
                else if (t == EndSessionTypes.LogOff)
                {
                    shut += " User is logging off. Closing gracefully.";
                }
                else if (t == EndSessionTypes.CloseApp)
                {
                    shut += " App has been asked to close by the system for a reason other than shutdown. Closing gracefully.";
                }

                Message = shut;
            }
            else if (e.Message == Worker.MSG_START_MOVER)
            {
                if (e.Kind == WorkerKind.Is64Worker)
                {
                    Message = "64-bit Worker Started";
                }
                else if (e.Kind == WorkerKind.Is86Worker)
                {
                    Message = "32-bit Worker Started";
                }
            }
            else if (e.Message == Worker.MSG_STOP_MOVER)
            {
                if (e.Kind == WorkerKind.Is64Worker)
                {
                    Message = "64-bit Worker Stopped";
                }
                else if (e.Kind == WorkerKind.Is86Worker)
                {
                    Message = "32-bit Worker Stopped";
                }

            }

            Event = e.Message;
            Origin = e.Kind;

            Timestamp = DateTime.Now;
        }

        public EventViewModel(WorkerLogEventArgs e)
        {
            LogSource = e;

            Timestamp = DateTime.Now;

            Monitor = e.Monitor;
            Event = e.Action;

            Message = e.Message;
            Module = WindowName = e.WindowName;
            Handle = e.Handle;

            Origin = e.Origin;

            // nothing more to do if there's no handle.
            if (e.Handle == IntPtr.Zero) return;

            _ = Task.Run(async () =>
            {
                try
                {
                    if (e.Action != Worker.MSG_DESTROYED)
                    {
                        ImageSource icon = null;

                        var sb = new StringBuilder();
                        sb.Capacity = 512;

                        Process p = GetWindowProcess(e.Handle);

                        var sp = p.MainModule.FileName;
                        Module = sp;

                        App.Current.Work.Log.Log($"Event {EventText}, {MonitorText}, {OriginText}, {WindowName}, {Message}, {Module}");


                        if (sp != null)
                        {
                            sp = Path.GetFileName(sp);

                            if (App.Current.Work.HasInternet && sp == "chrome.exe")
                            {
                                Module = GetChromeWindowUrl(WindowName);
                                App.Current.Work.Log.Log($"Detected Chrome Window URL: {Module}");

                                var url = Module;

                                int i = url.IndexOf("/");
                                if (i != -1) url = url.Substring(0, i);

                                if (!url.StartsWith("https://")) url = "https://" + url;
                                
                                url += "/favicon.ico";
                                App.Current.Work.Log.Log($"Attempting Favicon URL: {url}");

                                while (!System.Threading.Monitor.TryEnter(cache))
                                {
                                    Thread.Yield();
                                }

                                if (!cache.ContainsKey(url))
                                {
                                    System.Threading.Monitor.Exit(cache);

                                    HttpClient cli = new HttpClient();

                                    var res = await cli.GetAsync(new Uri(url));
                                    res.EnsureSuccessStatusCode();

                                    Stream stream = null;

                                    stream = await res.Content.ReadAsStreamAsync();

                                    if (stream != null)
                                    {
                                        icon = BitmapTools.MakeWPFImage(new System.Drawing.Icon(stream));
                                    }

                                    while (!System.Threading.Monitor.TryEnter(cache))
                                    {
                                        Thread.Yield();
                                    }

                                    App.Current.Work.Log.Log($"Favicon retrieval succeeded");
                                    cache.Add(url, icon);
                                }
                                else
                                {
                                    icon = cache[url];
                                }

                                System.Threading.Monitor.Exit(cache);
                                Icon = icon;

                                return;
                            }

                        }
            
                        while (!System.Threading.Monitor.TryEnter(cache))
                        {
                            Thread.Yield();
                        }

                        if (!cache.ContainsKey(Module))
                        {
                            icon = BitmapTools.MakeWPFImage(GetWindowIcon(e.Handle, 1));
                            cache.Add(Module, icon);
                        }
                        else
                        {
                            icon = cache[Module];
                        }

                        Icon = icon;
                        System.Threading.Monitor.Exit(cache);

                    }
                    else
                    {
                        while (!System.Threading.Monitor.TryEnter(cache))
                        {
                            Thread.Yield();
                        }

                        System.Threading.Monitor.Exit(cache);
                    }

                }
                catch (Exception ex)
                {

                    try
                    {
                        App.Current?.Work?.Log?.Log(ex.Message);
                        if (System.Threading.Monitor.IsEntered(cache))
                            System.Threading.Monitor.Exit(cache);

                        Message = ex.Message;
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            if (System.Threading.Monitor.IsEntered(cache))
                                System.Threading.Monitor.Exit(cache);

                            App.Current?.Work?.Log?.Log(ex2.Message);
                            Message = ex2.Message;
                        }
                        catch
                        {

                        }
                    }
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
                    case Worker.MSG_HOOK_REPLACED:

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
