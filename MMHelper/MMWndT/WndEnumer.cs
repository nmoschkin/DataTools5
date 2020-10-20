using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DataTools.Win32Api;
using static DataTools.Win32Api.User32;

namespace MMWndT
{
    public class WndEnumer
    {
        private WndEnumer parent;
        private string name;

        private IntPtr handle = IntPtr.Zero;

        private List<WndEnumer> children = new List<WndEnumer>();


        public bool IsDesktopWindow
        {
            get => handle == IntPtr.Zero;
        }

        public IReadOnlyCollection<WndEnumer> Children
        {
            get
            {
                return new ReadOnlyCollection<WndEnumer>(children);
            }
        }

        public string Name
        {
            get => name;
        }

        public IntPtr Handle
        {
            get => handle;
        }

        public WndEnumer Parent
        {
            get => parent;
            private set
            {
                parent = value;
            }
        }

        private WndEnumer(IntPtr handle, WndEnumer parent = null)
        {
            this.handle = handle;
            this.parent = parent;
            name = GetWindowName(handle);
        }

        public void Refresh()
        {
            DoEnum(this);
        }

        private EnumWindowsProcObj enumer = new EnumWindowsProcObj((hwnd, lparam) =>
        {
            if (lparam is WndEnumer e)
            {
                var n = GetWindowName(hwnd);
                if (string.IsNullOrEmpty(n.Trim().Trim('\x0'))) return true;
                e.children.Add(new WndEnumer(hwnd));
            }
            return true;
        });
        
        private static void DoEnum(WndEnumer enumer)
        {
            enumer.children.Clear();

            if (enumer.handle == IntPtr.Zero)
            {
                var l = GetDesktopWindows(IntPtr.Zero);

                foreach (var x in l)
                {
                    var n = GetWindowName(x);
                    if (string.IsNullOrEmpty(n.Trim().Trim('\x0'))) continue;
                    var e = new WndEnumer(x, enumer);
                    enumer.children.Add(e);
                }

            }
            else
            {
                EnumChildWindows(enumer.handle, enumer.enumer, IntPtr.Zero);
            }

        }

        public static WndEnumer DoEnum()
        {
            WndEnumer desktop = new WndEnumer(IntPtr.Zero);

            DoEnum(desktop);

            return desktop;
        }

        public static string GetWindowName(IntPtr Hwnd)
        {
            // This function gets the name of a window from its handle
            StringBuilder Title = new StringBuilder(256);
            GetWindowText(Hwnd, Title, 256);

            return Title.ToString().Trim();
        }


        public override string ToString()
        {
            return name;
        }


    }
}
