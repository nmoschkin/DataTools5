// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: MonitorInfo
//         Display monitor information encapsulation.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using DataTools.Win32;

using static DataTools.Hardware.MultiMon;
using System.Collections.Generic;
using DataTools.Win32.Memory;

namespace DataTools.Hardware
{
    internal static class MultiMon
    {
        public const int MONITORINFOF_PRIMARY = 0x00000001;
        public const int PHYSICAL_MONITOR_DESCRIPTION_SIZE = 128;
        public const int EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;


        [DllImport("Dxva2.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR
        (
            IntPtr hMonitor,
            out uint pdwNumberOfPhysicalMonitors
   
        );

        public static string[] GetPhysicalMonitorNames(IntPtr hMonitor)
        {
            var mm = new MemPtr();
            string[] sOut = null;
            PHYSICAL_MONITOR pm;
            
            uint nmon = 0;

            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out nmon)) return null;

            int cb = Marshal.SizeOf<PHYSICAL_MONITOR>();
            int size = cb * (int)nmon;

            mm.Alloc(size);

            try
            {
                if (GetPhysicalMonitorsFromHMONITOR(hMonitor, size, mm))
                {
                    sOut = new string[size];
                    int i;

                    for (i = 0; i < nmon; i++)
                    {
                        pm = mm.ToStructAt<PHYSICAL_MONITOR>(i * cb);
                        sOut[i] = pm.szPhysicalMonitorDescription;
                    }

                    DestroyPhysicalMonitors((uint)size, mm);
                }
                else
                {
                    sOut = new string[] { NativeErrorMethods.FormatLastError() };
                }

                mm.Free();
            }
            catch
            {
                mm.Free();
            }

            return sOut;

        }

        //[DllImport("Dxva2.dll")]
        //internal static extern bool GetNumberOfPhysicalMonitorsFromIDirect3DDevice9
        //    (
        //    IDirect3DDevice9* pDirect3DDevice9,
        //    out uint pdwNumberOfPhysicalMonitors

        //    );

        [DllImport("Dxva2.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        public static extern bool GetPhysicalMonitorsFromHMONITOR
            (
            IntPtr hMonitor,
            int dwPhysicalMonitorArraySize,
            MemPtr pPhysicalMonitorArray
        
            );

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "EnumDisplayDevicesW")]
        public static extern bool EnumDisplayDevices(
            IntPtr lpDevice,
            uint iDevNum,
            MemPtr lpDisplayDevice,
            int dwFlags
            );

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "EnumDisplayDevicesW")]
        public static extern bool EnumDisplayDevices(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpDevice,
            uint iDevNum,
            MemPtr lpDisplayDevice,
            int dwFlags
            );


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "EnumDisplaySettingsExW")]
        public static extern bool EnumDisplaySettingsEx(
            IntPtr lpszDeviceName,
            uint iModeNum,
            ref DEVMODE lpDevMode,
            uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "EnumDisplaySettingsExW")]
        public static extern bool EnumDisplaySettingsEx(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszDeviceName,
            uint iModeNum,
            ref DEVMODE lpDevMode,
            uint dwFlags);

        //[DllImport("Dxva2.dll")]
        //internal static extern bool GetPhysicalMonitorsFromIDirect3DDevice9
        //    (
        //    _In_ IDirect3DDevice9* pDirect3DDevice9,
        //    _In_ DWORD dwPhysicalMonitorArraySize,
        //    _Out_writes_(dwPhysicalMonitorArraySize) LPPHYSICAL_MONITOR pPhysicalMonitorArray

        //    );

        [DllImport("Dxva2.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DestroyPhysicalMonitor(IntPtr hMonitor);


        [DllImport("Dxva2.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DestroyPhysicalMonitors
            (
                uint dwPhysicalMonitorArraySize,
                MemPtr pPhysicalMonitorArray
            );




    }





    /// <summary>
    /// Represents the internal structure for display monitor information.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MONITORINFOEX
    {
        public uint cbSize;
        public W32RECT rcMonitor;
        public W32RECT rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct DISPLAY_DEVICE
    {
        public uint cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;

        public uint StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PHYSICAL_MONITOR_DESCRIPTION_SIZE)]
        public string szPhysicalMonitorDescription;
    }

    public struct VIDEOPARAMETERS
    {
        public Guid guid;

        public uint dwOffset;
        public uint dwCommand;
        public uint dwFlags;
        public uint dwMode;
        public uint dwTVStandard;
        public uint dwAvailableModes;
        public uint dwAvailableTVStandard;
        public uint dwFlickerFilter;
        public uint dwOverScanX;
        public uint dwOverScanY;
        public uint dwMaxUnscaledX;
        public uint dwMaxUnscaledY;
        public uint dwPositionX;
        public uint dwPositionY;
        public uint dwBrightness;
        public uint dwContrast;
        public uint dwCPType;
        public uint dwCPCommand;
        public uint dwCPStandard;
        public uint dwCPKey;
        public uint bCP_APSTriggerBits;


        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string bOEMCopyProtection;
    }

}

namespace DataTools.Hardware.Display
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
    public class Monitors : List<MonitorInfo>
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref W32RECT lpRect, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, [MarshalAs(UnmanagedType.FunctionPtr)] MonitorEnumProc lpfnEnum, IntPtr dwData);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromPoint(W32POINT pt, MultiMonFlags dwFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromRect(W32RECT pt, MultiMonFlags dwFlags);
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr MonitorFromWindow(IntPtr hWnd, MultiMonFlags dwFlags);

        private bool _enum(IntPtr hMonitor, IntPtr hdcMonitor, ref W32RECT lpRect, IntPtr lParamIn)
        {
            MemPtr lParam = lParamIn;
            Add(new MonitorInfo(hMonitor, lParam.IntAt(0L)));

            //string[] ss = GetPhysicalMonitorNames(hMonitor);
            lParam.IntAt(0L) += 1;
            return true;
        }


        /// <summary>
        /// Retrieves the monitor at the given point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public MonitorInfo GetMonitorFromPoint(W32POINT pt, MultiMonFlags flags = MultiMonFlags.DefaultToNearest)
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
        public MonitorInfo GetMonitorFromRect(W32RECT rc, MultiMonFlags flags = MultiMonFlags.DefaultToNearest)
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
        public MonitorInfo GetMonitorFromWindow(IntPtr hwnd, MultiMonFlags flags = MultiMonFlags.DefaultToNearest)
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
        public MonitorInfo GetMonitorFromWindow(System.Windows.Window window, MultiMonFlags flags = MultiMonFlags.DefaultToNearest)
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
            bool ret;
            Clear();

            var mm = new SafePtr(IntPtr.Size);

            mm.IntAt(0L) = 1;

            int i = mm.IntAt(0L);

            ret = EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, _enum, mm.handle);

            mm.Free();

            return ret;
        }

        public static void TransformRect(ref W32RECT wndRect, MonitorInfo m1, MonitorInfo m2, bool resize = false)
        {

            double wm, hm; 

            wm = (double)(wndRect.left - m1.WorkBounds.left) / m1.WorkBounds.right;
            hm = (double)(wndRect.top - m1.WorkBounds.top) / m1.WorkBounds.bottom;

            int cx, cy;

            cx = Math.Abs(wndRect.right - wndRect.left);
            cy = Math.Abs(wndRect.bottom - wndRect.top);

            var newRect = new W32RECT();

            newRect.left = m2.WorkBounds.left + (int)(m2.WorkBounds.right * wm);
            newRect.top = m2.WorkBounds.top + (int)(m2.WorkBounds.bottom * hm);

            if (resize)
            {
                newRect.right = newRect.left + (int)((double)cx * Math.Abs((double)m2.WorkBounds.right / m1.WorkBounds.right));
                newRect.bottom = newRect.top + (int)((double)cy * Math.Abs((double)m2.WorkBounds.bottom / m1.WorkBounds.bottom));
            }
            else
            {
                newRect.right = newRect.left + cx;
                newRect.bottom = newRect.top + cy;
            }

            wndRect = newRect;

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
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX info);


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
        public W32RECT MonitorBounds
        {
            get
            {
                {
                    var withBlock = _data.rcMonitor;
                    return new W32RECT(withBlock.left, withBlock.top, withBlock.right - withBlock.left, withBlock.bottom - withBlock.top);
                }
            }
        }

        /// <summary>
        /// Gets the available desktop area and screen coordinates for this monitor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public W32RECT WorkBounds
        {
            get
            {
                {
                    var withBlock = _data.rcWork;
                    return new W32RECT(withBlock.left, withBlock.top, withBlock.right - withBlock.left, withBlock.bottom - withBlock.top);
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