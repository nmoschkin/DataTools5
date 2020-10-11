// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: MonitorInfo
// '         Display monitor information encapsulation.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using DataTools.Interop.Native;

namespace DataTools.Interop.Native
{
    static class MultiMon
    {
        public const int MONITORINFOF_PRIMARY = 0x00000001;
    }


    /// <summary>
    /// Represents the internal structure for display monitor information.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MONITORINFOEX
    {
        public uint cbSize;
        public PInvoke.RECT rcMonitor;
        public PInvoke.RECT rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }
}

namespace DataTools.Interop.Display
{

    /// <summary>
    /// Multi-monitor hit-test flags.
    /// </summary>
    public enum MultiMonFlags : uint
    {
        DefaultToNull = 0x00000000U,
        DefaultToPrimary = 0x00000001U,
        DefaultToNearest = 0x00000002U
    }

    /// <summary>
    /// Represents a collection of all monitors on the system.
    /// </summary>
    /// <remarks></remarks>
    public class Monitors : ObservableCollection<MonitorInfo>
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref PInvoke.RECT lpRect, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, [MarshalAs(UnmanagedType.FunctionPtr)] MonitorEnumProc lpfnEnum, IntPtr dwData);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromPoint(PInvoke.POINT pt, MultiMonFlags dwFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromRect(PInvoke.RECT pt, MultiMonFlags dwFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromWindow(IntPtr hWnd, MultiMonFlags dwFlags);

        private bool _enum(IntPtr hMonitor, IntPtr hdcMonitor, ref PInvoke.RECT lpRect, IntPtr lParamIn)
        {
            CoreCT.Memory.MemPtr lParam = lParamIn;
            Add(new MonitorInfo(hMonitor, lParam.IntAt(0L)));
            lParam.IntAt(0L) += 1;
            return true;
        }


        /// <summary>
        /// Retrieves the monitor at the given point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public MonitorInfo GetMonitorFromPoint(PInvoke.POINT pt, MultiMonFlags flags = MultiMonFlags.DefaultToNull)
        {
            if (Count == 0)
                Refresh();
            var h = MonitorFromPoint(pt, flags);
            if (h == IntPtr.Zero)
                return null;
            foreach (var m in this)
            {
                if (m.hMonitor == h)
                    return m;
            }

            return this[0];
        }

        /// <summary>
        /// Retrieves the monitor at the given point.
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public MonitorInfo GetMonitorFromRect(PInvoke.RECT rc, MultiMonFlags flags = MultiMonFlags.DefaultToNull)
        {
            if (Count == 0)
                Refresh();
            var h = MonitorFromRect(rc, flags);
            if (h == IntPtr.Zero)
                return null;
            foreach (var m in this)
            {
                if (m.hMonitor == h)
                    return m;
            }

            return this[0];
        }

        /// <summary>
        /// Retrieves the monitor at the specified native window handle.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public MonitorInfo GetMonitorFromWindow(IntPtr hwnd, MultiMonFlags flags = MultiMonFlags.DefaultToNull)
        {
            if (Count == 0)
                Refresh();
            var h = MonitorFromWindow(hwnd, flags);
            if (h == IntPtr.Zero)
                return null;
            foreach (var m in this)
            {
                if (m.hMonitor == h)
                    return m;
            }

            return this[0];
        }

        /// <summary>
        /// Retrieves the monitor at the specified WPF window.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public MonitorInfo GetMonitorFromWindow(System.Windows.Window window, MultiMonFlags flags = MultiMonFlags.DefaultToNull)
        {
            if (Count == 0)
                Refresh();
            var ih = new System.Windows.Interop.WindowInteropHelper(window);
            var h = MonitorFromWindow(ih.EnsureHandle(), flags);
            ih = null;
            if (h == IntPtr.Zero)
                return null;
            foreach (var m in this)
            {
                if (m.hMonitor == h)
                    return m;
            }

            return this[0];
        }

        /// <summary>
        /// Refresh the current monitor list.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Refresh()
        {
            bool RefreshRet = default;
            Clear();
            var mm = new CoreCT.Memory.MemPtr(IntPtr.Size);
            mm.IntAt(0L) = 1;
            int i = mm.IntAt(0L);
            RefreshRet = EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, _enum, mm.Handle);
            mm.Free();
            return RefreshRet;
        }

        public Monitors()
        {
            Refresh();
        }
    }

    /// <summary>
    /// Represents a monitor device.
    /// </summary>
    /// <remarks></remarks>
    public class MonitorInfo
    {
        private IntPtr _hMonitor;
        private MONITORINFOEX _data;
        private int _idx;

        [DllImport("user32", EntryPoint = "GetMonitorInfoW", CharSet = CharSet.Unicode)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX info);


        /// <summary>
        /// Returns the monitor index, or the order in which this monitor was reported to the monitor collection.
        /// </summary>
        /// <returns></returns>
        public int Index
        {
            get
            {
                return _idx;
            }

            internal set
            {
                _idx = value;
            }
        }

        /// <summary>
        /// Specifies the current monitor's device path.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DevicePath
        {
            get
            {
                return _data.szDevice;
            }
        }

        /// <summary>
        /// Gets the total desktop screen area and coordinates for this monitor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PInvoke.RECT MonitorBounds
        {
            get
            {
                {
                    var withBlock = _data.rcMonitor;
                    return new PInvoke.RECT(withBlock.left, withBlock.top, withBlock.right - withBlock.left, withBlock.bottom - withBlock.top);
                }
            }
        }

        /// <summary>
        /// Gets the available desktop area and screen coordinates for this monitor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PInvoke.RECT WorkBounds
        {
            get
            {
                {
                    var withBlock = _data.rcWork;
                    return new PInvoke.RECT(withBlock.left, withBlock.top, withBlock.right - withBlock.left, withBlock.bottom - withBlock.top);
                }
            }
        }

        /// <summary>
        /// True if this monitor is the primary monitor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPrimary
        {
            get
            {
                return (_data.dwFlags & 1L) == 1L;
            }
        }

        /// <summary>
        /// Gets the hMonitor handle for this device.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        internal IntPtr hMonitor
        {
            get
            {
                return _hMonitor;
            }
        }

        /// <summary>
        /// Refresh the monitor device information.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Refresh()
        {
            return GetMonitorInfo(_hMonitor, ref _data);
        }

        /// <summary>
        /// Create a new instance of a monitor object from the given hMonitor.
        /// </summary>
        /// <param name="hMonitor"></param>
        /// <remarks></remarks>
        internal MonitorInfo(IntPtr hMonitor, int idx)
        {
            _hMonitor = hMonitor;
            _data.cbSize = (uint)Marshal.SizeOf(_data);
            _idx = idx;
            Refresh();
        }
    }
}