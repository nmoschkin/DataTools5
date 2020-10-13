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
        DeviceCharacteristcs Characteristics { get; }

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
        DeviceRemovalPolicy RemovalPolicy { get; }

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
        DeviceInterfaceClassEnum DeviceInterfaceClass { get; }

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
        DeviceClassEnum DeviceClass { get; }

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

    public abstract class DeviceInfoBase : IDeviceInfo
    {
        protected string _InstanceId;
        protected BusType _BusType;
        protected string _DevicePath;
        protected string _Parent;
        protected string[] _Children;
        protected DateTime _InstallDate;
        protected DeviceCharacteristcs _Characteristics;
        protected bool _SafeRemovalRequired;
        protected DeviceRemovalPolicy _RemovalPolicy;
        protected string[] _LocationPaths;
        protected string _LocationInfo;
        protected string[] _HardwareIds;
        protected string _PDOName;
        protected Guid _DeviceInterfaceClassGuid;
        protected DeviceInterfaceClassEnum _DeviceInterfaceClass;
        protected Guid _DeviceClassGuid;
        protected DeviceClassEnum _DeviceClass;
        protected string _ClassName;
        protected string _ClassDescription;
        protected System.Windows.Media.Imaging.BitmapSource _DeviceIcon;
        protected string _VendorId;
        protected string _ProductId;
        protected string _UIDescription;
        protected string _FriendlyName;
        protected string _Description;
        protected string _Manufacturer;

        public virtual BusType BusType
        {
            get => _BusType;
            internal set => _BusType = value;
        }

        public virtual DeviceCharacteristcs Characteristics
        {
            get => _Characteristics;
            internal set => _Characteristics = value;
        }

        public virtual string[] Children
        {
            get => _Children;
            internal set => _Children = value;
        }

        public virtual string ClassDescription
        {
            get => _ClassDescription;
            internal set => _ClassDescription = value;
        }

        public virtual string ClassName
        {
            get => _ClassName;
            internal set => _ClassName = value;
        }

        public virtual string Description
        {
            get => _Description;
            internal set => _Description = value;
        }

        public virtual DeviceClassEnum DeviceClass
        {
            get => _DeviceClass;
            internal set => _DeviceClass = value;
        }

        public virtual Guid DeviceClassGuid
        {
            get => _DeviceClassGuid;
            internal set => _DeviceClassGuid = value;
        }

        public virtual System.Windows.Media.Imaging.BitmapSource DeviceIcon
        {
            get => _DeviceIcon;
            internal set => _DeviceIcon = value;
        }

        public virtual DeviceInterfaceClassEnum DeviceInterfaceClass
        {
            get => _DeviceInterfaceClass;
            internal set => _DeviceInterfaceClass = value;
        }

        public virtual Guid DeviceInterfaceClassGuid
        {
            get => _DeviceInterfaceClassGuid;
            internal set => _DeviceInterfaceClassGuid = value;
        }

        public virtual string DevicePath
        {
            get => _DevicePath;
            internal set => _DevicePath = value;
        }

        public virtual string FriendlyName
        {
            get => _FriendlyName;
            internal set => _FriendlyName = value;
        }

        public virtual string[] HardwareIds
        {
            get => _HardwareIds;
            internal set => _HardwareIds = value;
        }

        public virtual DateTime InstallDate
        {
            get => _InstallDate;
            internal set => _InstallDate = value;
        }

        public virtual string InstanceId
        {
            get => _InstanceId;
            internal set => _InstanceId = value;
        }

        public virtual string LocationInfo
        {
            get => _LocationInfo;
            internal set => _LocationInfo = value;
        }

        public virtual string[] LocationPaths
        {
            get => _LocationPaths;
            internal set => _LocationPaths = value;
        }

        public virtual string Manufacturer
        {
            get => _Manufacturer;
            internal set => _Manufacturer = value;
        }

        public virtual string Parent
        {
            get => _Parent;
            internal set => _Parent = value;
        }

        public virtual string PDOName
        {
            get => _PDOName;
            internal set => _PDOName = value;
        }

        public virtual string ProductId
        {
            get => _ProductId;
            internal set => _ProductId = value;
        }

        public virtual DeviceRemovalPolicy RemovalPolicy
        {
            get => _RemovalPolicy;
            internal set => _RemovalPolicy = value;
        }

        public virtual bool SafeRemovalRequired
        {
            get => _SafeRemovalRequired;
            internal set => _SafeRemovalRequired = value;
        }

        public virtual void ShowDevicePropertiesDialog(IntPtr hwnd = default)
        {
            DevPropDialog.OpenDeviceProperties(InstanceId, hwnd);
        }

        public virtual string UIDescription
        {
            get => _UIDescription;
            internal set => _UIDescription = value;
        }

        public virtual string VendorId
        {
            get => _VendorId;
            internal set => _VendorId = value;
        }
    }
}