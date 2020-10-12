using System;
using DataTools.Hardware.Native;

namespace DataTools.Hardware
{
    interface IDeviceInfo
    {

        /// <summary>
    /// Gets the device instance id which can be passed to RunDLL property sheet functions.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string InstanceId { get; }

        /// <summary>
    /// Returns the type of bus that the device is hosted on.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        BusType BusType { get; }

        /// <summary>
    /// Get the physical device path that can be used by CreateFile
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string DevicePath { get; }

        /// <summary>
    /// Returns the parent instance id of this device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string Parent { get; }

        /// <summary>
    /// Returns all children instance ids of this device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string[] Children { get; }

        /// <summary>
    /// Retrieves the install date for the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        DateTime InstallDate { get; }
        /// <summary>
    /// Retrieves any device characteristcs associated with the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        DevClassPresenting.DeviceCharacteristcs Characteristics { get; }

        /// <summary>
    /// Specifies whether or not the device must be removed safely.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        bool SafeRemovalRequired { get; }

        /// <summary>
    /// Specifies the removal policy of the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        DevClassPresenting.DeviceRemovalPolicy RemovalPolicy { get; }

        /// <summary>
    /// Gets all hardware location paths.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string[] LocationPaths { get; }

        /// <summary>
    /// Gets location information.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string LocationInfo { get; }

        /// <summary>
    /// Gets all hardware Ids
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string[] HardwareIds { get; }

        /// <summary>
    /// Gets the PDO name.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string PDOName { get; }

        /// <summary>
    /// Gets the device interface class guid.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        Guid DeviceInterfaceClassGuid { get; }

        /// <summary>
    /// Gets the device class type.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        DevClassPresenting.DeviceInterfaceClassEnum DeviceInterfaceClass { get; }

        /// <summary>
    /// Gets the device class guid.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        Guid DeviceClassGuid { get; }

        /// <summary>
    /// Gets the device class type.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        DevClassPresenting.DeviceClassEnum DeviceClass { get; }

        /// <summary>
    /// Gets the device class name.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string ClassName { get; }

        /// <summary>
    /// Gets the device class description.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string ClassDescription { get; }

        /// <summary>
    /// Retrieve a WPF BitmapSource image for use in binding.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        System.Windows.Media.Imaging.BitmapSource DeviceIcon { get; }

        /// <summary>
    /// Gets the vendor Id hexadecimal string
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string VendorId { get; }

        /// <summary>
    /// Gets the product Id hexadecimal string
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string ProductId { get; }

        /// <summary>
    /// Display the system device properties dialog page for this device.
    /// </summary>
    /// <remarks></remarks>
        void ShowDevicePropertiesDialog(IntPtr hwnd = default);

        /// <summary>
    /// Retrieves a string suitable for display in a user interface.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string UIDescription { get; }

        /// <summary>
    /// Gets the friendly name for the device
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string FriendlyName { get; }

        /// <summary>
    /// Gets the description of the device.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string Description { get; }

        /// <summary>
    /// Gets the manufacturer.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        string Manufacturer { get; }
    }

    public class DeviceInfoBase : IDeviceInfo
    {
        internal string _InstanceId;
        internal BusType _BusType;
        internal string _DevicePath;
        internal string _Parent;
        internal string[] _Children;
        internal DateTime _InstallDate;
        internal DevClassPresenting.DeviceCharacteristcs _Characteristics;
        internal bool _SafeRemovalRequired;
        internal DevClassPresenting.DeviceRemovalPolicy _RemovalPolicy;
        internal string[] _LocationPaths;
        internal string _LocationInfo;
        internal string[] _HardwareIds;
        internal string _PDOName;
        internal Guid _DeviceInterfaceClassGuid;
        internal DevClassPresenting.DeviceInterfaceClassEnum _DeviceInterfaceClass;
        internal Guid _DeviceClassGuid;
        internal DevClassPresenting.DeviceClassEnum _DeviceClass;
        internal string _ClassName;
        internal string _ClassDescription;
        internal System.Windows.Media.Imaging.BitmapSource _DeviceIcon;
        internal string _VendorId;
        internal string _ProductId;
        internal string _UIDescription;
        internal string _FriendlyName;
        internal string _Description;
        internal string _Manufacturer;

        public BusType BusType
        {
            get
            {
                return _BusType;
            }
        }

        public DevClassPresenting.DeviceCharacteristcs Characteristics
        {
            get
            {
                return _Characteristics;
            }
        }

        public string[] Children
        {
            get
            {
                return _Children;
            }
        }

        public string ClassDescription
        {
            get
            {
                return _ClassDescription;
            }
        }

        public string ClassName
        {
            get
            {
                return _ClassName;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
        }

        public DevClassPresenting.DeviceClassEnum DeviceClass
        {
            get
            {
                return _DeviceClass;
            }
        }

        public Guid DeviceClassGuid
        {
            get
            {
                return _DeviceClassGuid;
            }
        }

        public System.Windows.Media.Imaging.BitmapSource DeviceIcon
        {
            get
            {
                return _DeviceIcon;
            }
        }

        public DevClassPresenting.DeviceInterfaceClassEnum DeviceInterfaceClass
        {
            get
            {
                return _DeviceInterfaceClass;
            }
        }

        public Guid DeviceInterfaceClassGuid
        {
            get
            {
                return _DeviceInterfaceClassGuid;
            }
        }

        public string DevicePath
        {
            get
            {
                return _DevicePath;
            }
        }

        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
        }

        public string[] HardwareIds
        {
            get
            {
                return _HardwareIds;
            }
        }

        public DateTime InstallDate
        {
            get
            {
                return _InstallDate;
            }
        }

        public string InstanceId
        {
            get
            {
                return _InstanceId;
            }
        }

        public string LocationInfo
        {
            get
            {
                return _LocationInfo;
            }
        }

        public string[] LocationPaths
        {
            get
            {
                return _LocationPaths;
            }
        }

        public string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }
        }

        public string Parent
        {
            get
            {
                return _Parent;
            }
        }

        public string PDOName
        {
            get
            {
                return _PDOName;
            }
        }

        public string ProductId
        {
            get
            {
                return _ProductId;
            }
        }

        public DevClassPresenting.DeviceRemovalPolicy RemovalPolicy
        {
            get
            {
                return _RemovalPolicy;
            }
        }

        public bool SafeRemovalRequired
        {
            get
            {
                return _SafeRemovalRequired;
            }
        }

        public void ShowDevicePropertiesDialog(IntPtr hwnd = default)
        {
            DevPropDialog.OpenDeviceProperties(InstanceId, hwnd);
        }

        public string UIDescription
        {
            get
            {
                return _UIDescription;
            }
        }

        public string VendorId
        {
            get
            {
                return _VendorId;
            }
        }
    }
}