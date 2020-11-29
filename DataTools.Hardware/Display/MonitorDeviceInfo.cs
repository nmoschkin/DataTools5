using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Hardware;
using DataTools.Win32Api;

namespace DataTools.Hardware.Display
{
    public class MonitorDeviceInfo : DeviceInfo
    {

        MonitorInfo source;

        public MonitorInfo Source
        {
            get => source;
            internal set
            {
                source = value;
            }
        }

        /// <summary>
        /// Returns the monitor index, or the order in which this monitor was reported to the monitor collection.
        /// </summary>
        /// <returns></returns>
        public int Index
        {
            get
            {
                return (int)source?.Index;
            }
            internal set
            {
                if (source != null) source.Index = value;
            }
        }

        /// <summary>
        /// Specifies the current monitor's device path.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string MonitorDevicePath
        {
            get
            {
                return source?.DevicePath;
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
                    return (W32RECT)source?.MonitorBounds;
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
                    return (W32RECT)source?.WorkBounds;
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
                return (bool)source?.IsPrimary;
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
                return (IntPtr)source?.hMonitor;
            }
        }

        /// <summary>
        /// Refresh the monitor device information.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Refresh()
        {
            return (bool)source?.Refresh();
        }

        /// <summary>
        /// Create a new instance of a monitor object from the given hMonitor.
        /// </summary>
        /// <param name="hMonitor"></param>
        /// <remarks></remarks>
        internal MonitorDeviceInfo(MonitorInfo source)
        {
            this.source = source;
        }

        public MonitorDeviceInfo()
        {

        }
    }







}
