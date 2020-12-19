// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DeviceInfo Hardware Information Class
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DataTools.Text;
using DataTools.Desktop;
using DataTools.Win32;

namespace DataTools.Hardware
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
/// An object that represents a hardware device on the system.
/// </summary>
/// <remarks></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DeviceInfo
    {
        internal SP_DEVINFO_DATA _devInfo;
        protected string[] _HardwareIds;
        internal ushort _Vid;
        internal ushort _Pid;
        protected string _DevicePath;
        protected string _FriendlyName;
        protected string _InstanceId;
        protected string[] _LocationPaths;
        protected string _LocationInfo;
        protected byte[] _PhysicalPath;
        protected int _UINumber;
        protected string _Description;
        protected Guid _ContainerId;
        protected string _PDOName;
        protected string _Manufacturer;
        protected Guid _ModelId;
        protected string _BusReportedDeviceDesc;
        protected string _ClassName;
        protected Guid _DeviceInterfaceClassGuid;
        protected DeviceInterfaceClassEnum _DeviceInterfaceClass;
        protected Guid _DeviceClassGuid;
        protected DeviceClassEnum _DeviceClass;
        protected System.Drawing.Icon _DeviceClassIcon;
        protected string _Parent;
        protected string[] _Children;
        protected DeviceRemovalPolicy _RemovalPolicy;
        protected bool _SafeRemovalRequired;
        protected DeviceCharacteristcs _Characteristics;
        protected DateTime _InstallDate;
        protected BusType _BusType;
        protected string _VenderId = null;
        protected string _ProductId = null;
        protected System.Windows.Media.Imaging.BitmapSource _DeviceIcon;
        protected DeviceInfo[] _LinkedChildren;
        protected DeviceInfo _LinkedParent;

        /// <summary>
    /// Create a DeviceInfo-based class based upon the given class, populating common members.
    /// </summary>
    /// <typeparam name="T">A DeviceInfo-derived class.</typeparam>
    /// <param name="inf">The object to duplicate.</param>
    /// <returns>A new instance of T.</returns>
    /// <remarks></remarks>
        internal static T Duplicate<T>(DeviceInfo inf) where T : DeviceInfo, new()
        {
            var fi = inf.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo fo;
            var objT = new T();
            foreach (var fe in fi)
            {
                fo = typeof(T).GetField(fe.Name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (fo is object)
                    fo.SetValue(objT, fe.GetValue(inf));
            }

            return objT;
        }

        /// <summary>
    /// Link all devices in an array of DeviceInfo objects with their relative parent and child objects.
    /// </summary>
    /// <param name="devInfo"></param>
    /// <remarks></remarks>
        public static void LinkDevices(ref DeviceInfo[] devInfo)
        {
            if (devInfo is null)
                return;

            foreach (var dprep in devInfo)
            {
                dprep.LinkedParent = null;
                dprep.LinkedChildren = null;
            }

            foreach (var de in devInfo)
            {
                foreach (var fe in devInfo)
                {
                    if ((de.InstanceId.ToUpper().Trim() ?? "") == (fe.Parent.ToUpper().Trim() ?? ""))
                    {
                        fe.LinkedParent = de;
                        if (de._LinkedChildren is null)
                        {
                            de._LinkedChildren = new DeviceInfo[1];
                        }
                        else
                        {
                            Array.Resize(ref de._LinkedChildren, de._LinkedChildren.Count() + 1);
                        }

                        de._LinkedChildren[de._LinkedChildren.Count() - 1] = fe;
                    }
                }
            }

            
        }

        /// <summary>
    /// The linked parent DeviceInfo object.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public DeviceInfo LinkedParent
        {
            get
            {
                return _LinkedParent;
            }

            internal set
            {
                _LinkedParent = value;
            }
        }

        /// <summary>
    /// Array of linked child DeviceInfo objects.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DeviceInfo[] LinkedChildren
        {
            get
            {
                return _LinkedChildren;
            }

            internal set
            {
                _LinkedChildren = value;
            }
        }

        /// <summary>
    /// Returns the type of bus that the device is hosted on.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public BusType BusType
        {
            get
            {
                return _BusType;
            }

            set
            {
                _BusType = value;
            }
        }

        /// <summary>
    /// Retrieves the install date for the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public DateTime InstallDate
        {
            get
            {
                return _InstallDate;
            }

            internal set
            {
                _InstallDate = value;
            }
        }

        /// <summary>
    /// Retrieves any device characteristcs associated with the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public DeviceCharacteristcs Characteristics
        {
            get
            {
                return _Characteristics;
            }

            internal set
            {
                _Characteristics = value;
            }
        }

        /// <summary>
    /// Specifies whether or not the device must be removed safely.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public bool SafeRemovalRequired
        {
            get
            {
                return _SafeRemovalRequired;
            }

            internal set
            {
                _SafeRemovalRequired = value;
            }
        }

        /// <summary>
    /// Specifies the removal policy of the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public DeviceRemovalPolicy RemovalPolicy
        {
            get
            {
                return _RemovalPolicy;
            }

            internal set
            {
                _RemovalPolicy = value;
            }
        }

        /// <summary>
    /// Returns all child instance ids of this device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string[] Children
        {
            get
            {
                return _Children;
            }

            internal set
            {
                _Children = value;
            }
        }

        /// <summary>
    /// Returns the parent instance id of this device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string Parent
        {
            get
            {
                return _Parent;
            }

            internal set
            {
                _Parent = value;
            }
        }

        /// <summary>
    /// Get the physical device path that can be used by CreateFile
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string DevicePath
        {
            get
            {
                return _DevicePath;
            }

            internal set
            {
                _DevicePath = value;
            }
        }

        /// <summary>
    /// Gets the friendly name for the device
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }

            internal set
            {
                _FriendlyName = value;
            }
        }

        /// <summary>
    /// Gets the device instance id which is unique and can be passed to RunDLL property sheet functions and to match children with their parents.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string InstanceId
        {
            get
            {
                return _InstanceId;
            }

            internal set
            {
                _InstanceId = value;
            }
        }

        /// <summary>
    /// Gets all hardware location paths.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string[] LocationPaths
        {
            get
            {
                return _LocationPaths;
            }

            internal set
            {
                _LocationPaths = value;
            }
        }

        /// <summary>
    /// Gets location information.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string LocationInfo
        {
            get
            {
                return _LocationInfo;
            }

            internal set
            {
                _LocationInfo = value;
            }
        }

        /// <summary>
    /// Gets all hardware Ids. Hardware Ids contain extractable data, including but not limited to
    /// device vendor id, device product id and USB HID page implementations.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string[] HardwareIds
        {
            get
            {
                return _HardwareIds;
            }

            internal set
            {
                _HardwareIds = value;
                ParseHw();
            }
        }

        /// <summary>
    /// Gets the binary physical path.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public byte[] PhysicalPath
        {
            get
            {
                return _PhysicalPath;
            }

            internal set
            {
                _PhysicalPath = value;
            }
        }

        /// <summary>
    /// Gets the UINumber
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public int UINumber
        {
            get
            {
                return _UINumber;
            }

            internal set
            {
                _UINumber = value;
            }
        }

        /// <summary>
    /// Gets the description of the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string Description
        {
            get
            {
                return _Description;
            }

            internal set
            {
                _Description = value;
            }
        }

        /// <summary>
    /// Gets the hardware container Id.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public Guid ContainerId
        {
            get
            {
                return _ContainerId;
            }

            internal set
            {
                _ContainerId = value;
            }
        }

        /// <summary>
    /// Gets the PDO name. This value is used to match physical disk device instances with their respective interfaces.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string PDOName
        {
            get
            {
                return _PDOName;
            }

            internal set
            {
                _PDOName = value;
            }
        }

        /// <summary>
    /// Gets the manufacturer.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }

            internal set
            {
                _Manufacturer = value;
            }
        }

        /// <summary>
    /// Gets the Model Id.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual Guid ModelId
        {
            get
            {
                return _ModelId;
            }

            internal set
            {
                _ModelId = value;
            }
        }

        /// <summary>
    /// Get the Bus-Reported device description.
    /// </summary>
    /// <returns></returns>
        public virtual string BusReportedDeviceDesc
        {
            get
            {
                return _BusReportedDeviceDesc;
            }

            set
            {
                _BusReportedDeviceDesc = value;
            }
        }


        /// <summary>
    /// Gets the device class name.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string ClassName
        {
            get
            {
                return _ClassName;
            }

            internal set
            {
                _ClassName = value;
            }
        }

        /// <summary>
    /// Gets the device interface class guid.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public Guid DeviceInterfaceClassGuid
        {
            get
            {
                return _DeviceInterfaceClassGuid;
            }

            internal set
            {
                _DeviceInterfaceClassGuid = value;
            }
        }

        /// <summary>
    /// Gets the device class type.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual DeviceInterfaceClassEnum DeviceInterfaceClass
        {
            get
            {
                return _DeviceInterfaceClass;
            }

            internal set
            {
                _DeviceInterfaceClass = value;
            }
        }

        /// <summary>
    /// Gets the device class guid.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public Guid DeviceClassGuid
        {
            get
            {
                return _DeviceClassGuid;
            }

            internal set
            {
                _DeviceClassGuid = value;
            }
        }

        /// <summary>
    /// Gets the device class type.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual DeviceClassEnum DeviceClass
        {
            get
            {
                return _DeviceClass;
            }

            internal set
            {
                _DeviceClass = value;
            }
        }

        /// <summary>
    /// Returns a detailed description of the device class.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string DeviceClassDescription
        {
            get
            {
                return FileTools.GetEnumDescription(_DeviceClass);
            }
        }

        /// <summary>
    /// Gets the device class icon.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        [Browsable(false)]
        public virtual System.Drawing.Icon DeviceClassIcon
        {
            get
            {
                return _DeviceClassIcon;
            }

            internal set
            {
                _DeviceClassIcon = value;
            }
        }

        /// <summary>
    /// Retrieve a WPF BitmapSource image for use in binding.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        [Browsable(false)]
        public virtual System.Windows.Media.Imaging.BitmapSource DeviceIcon
        {
            get
            {
                if (_DeviceIcon is null)
                {
                    if (_DeviceClassIcon is null)
                        return null;
                    _DeviceIcon = Resources.MakeWPFImage(Resources.IconToTransparentBitmap(_DeviceClassIcon));
                }

                return _DeviceIcon;
            }

            internal set
            {
                _DeviceIcon = value;
            }
        }

        /// <summary>
    /// Gets the device class description.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string ClassDescription
        {
            get
            {
                return FileTools.GetEnumDescription(DeviceClass);
            }
        }

        /// <summary>
    /// Gets the vendor Id string, which may or may not be a number.  If it is a number, it is returned as a 4-character (WORD) hexadecimal string.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string VendorId
        {
            get
            {
                if (_VenderId is object)
                    return _VenderId;
                return _Vid.ToString("X4");
            }
        }

        /// <summary>
    /// Gets the product Id string, which may or may not be a number.  If it is a number, it is returned as a 4-character (WORD) hexadecimal string.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public string ProductId
        {
            get
            {
                if (_ProductId is object)
                    return _ProductId;
                return _Pid.ToString("X4");
            }
        }

        /// <summary>
    /// Gets the vendor id raw number.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public ushort Vid
        {
            get
            {
                return _Vid;
            }
        }

        /// <summary>
    /// Gets the product id raw number.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public ushort Pid
        {
            get
            {
                return _Pid;
            }
        }

        /// <summary>
    /// Display the system device properties dialog page for this device.
    /// </summary>
    /// <remarks></remarks>
        public void ShowDevicePropertiesDialog(IntPtr hwnd = default)
        {
            DevPropDialog.OpenDeviceProperties(InstanceId, hwnd);
        }

        /// <summary>
    /// Parses the hardware Id.
    /// </summary>
    /// <remarks></remarks>
        protected virtual void ParseHw()
        {
            var s = TextTools.Split(_HardwareIds[0], @"\");
            if (s is null || s.Length < 2)
                return;
            string[] v;
            if (s[1].IndexOf(";") > 0)
            {
                v = TextTools.Split(s[1], ";");
            }
            else
            {
                v = TextTools.Split(s[1], "&");
            }

            foreach (string vp in v)
            {
                var x = TextTools.Split(vp, "_");
                if (x.Length < 2)
                    continue;
                switch (x[0] ?? "")
                {
                    case "VID":
                        {
                            if (!ushort.TryParse(x[1], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out _Vid))
                            {
                                _VenderId = x[1];
                            }

                            break;
                        }

                    case "PID":
                        {
                            if (!ushort.TryParse(x[1], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out _Pid))
                            {
                                _ProductId = x[1];
                            }

                            break;
                        }

                    case "VEN":
                        {
                            if (!ushort.TryParse(x[1], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out _Vid))
                            {
                                _VenderId = x[1];
                            }

                            break;
                        }

                    case "DEV":
                        {
                            if (!ushort.TryParse(x[1], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out _Pid))
                            {
                                _ProductId = x[1];
                            }

                            break;
                        }
                }
            }
        }

        /// <summary>
    /// Returns a description of the device. The default is the value of ToString().
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public virtual string UIDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Description) == false)
                {
                    return Description;
                }
                else if (string.IsNullOrEmpty(FriendlyName) == false)
                {
                    return FriendlyName;
                }
                else
                {
                    return ToString();
                }
            }
        }

        /// <summary>
    /// Returns a string representation of this object
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public override string ToString()
        {
            string d = Description;
            string f = FriendlyName;
            if (d is null && f is null)
            {
                return InstanceId;
            }
            else if (d is null && f is object)
            {
                return f;
            }
            else
            {
                return d;
            }
        }
    }
}

/* TODO ERROR: Skipped EndRegionDirectiveTrivia */