// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: DiskDeviceInfo derived class for disks and
// '         volumes.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.ComponentModel;
using System.Linq;
using DataTools.Text;
using DataTools.Hardware.Native;
using DataTools.Hardware.Disk.Partitions;
using DataTools.Hardware.Disk.Partitions.Gpt;

namespace DataTools.Hardware.Disk
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// An object that represents a disk or volume device on the system.
    /// </summary>
    /// <remarks></remarks>
    public class DiskDeviceInfo : DeviceInfo
    {
        protected int _PhysicalDevice;
        protected int _PartitionNumber;
        protected DevClassPresenting.StorageType _Type;
        protected long _Size;
        protected DevClassPresenting.DeviceCapabilities _Capabilities;
        protected string[] _BackingStore;
        protected bool _IsVolume;
        protected uint _SerialNumber;
        protected string _FileSystem;
        protected DevClassPresenting.FileSystemFlags _VolumeFlags;
        protected string _VolumeGuidPath;
        protected string[] _VolumePaths;
        protected DevClassPresenting.DiskExtent[] _DiskExtents;
        protected IDiskLayout _DiskLayout;
        protected IDiskPartition _PartInfo;
        protected int _SectorSize;
        protected VirtualDisk _VirtualDrive;

        /// <summary>
        /// Access the virtual disk object (if any).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public VirtualDisk VirtualDisk
        {
            get
            {
                return _VirtualDrive;
            }

            internal set
            {
                _VirtualDrive = value;
            }
        }

        /// <summary>
        /// The sector size of this volume, in bytes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int SectorSize
        {
            get
            {
                return _SectorSize;
            }

            internal set
            {
                _SectorSize = value;
            }
        }

        /// <summary>
        /// Returns the disk layout and partition information on a physical disk where Type is not StorageType.Volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IDiskLayout DiskLayout
        {
            get
            {
                return _DiskLayout;
            }

            internal set
            {
                _DiskLayout = value;
            }
        }

        /// <summary>
        /// Returns partition information for a DiskDeviceInfo object whose Type = StorageType.Volume
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDiskPartition PartitionInfo
        {
            get
            {
                Partitioning.PARTITION_INFORMATION_EX[] p;

                if (Type != DevClassPresenting.StorageType.Volume)
                    return null;

                if (_PartInfo == null)
                {
                    try
                    {
                        if (!IsVolumeMounted)
                            return null;

                        Partitioning.DRIVE_LAYOUT_INFORMATION_EX arglayInfo = new Partitioning.DRIVE_LAYOUT_INFORMATION_EX();

                        p = Partitioning.GetPartitions(@"\\.\PhysicalDrive" + PhysicalDevice, IntPtr.Zero, layInfo: ref arglayInfo);

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                    if (p is object)
                    {
                        foreach (var x in p)
                        {
                            if (x.PartitionNumber == PartitionNumber)
                            {
                                _PartInfo = DiskPartitionInfo.CreateInfo(x);
                                break;
                            }
                        }
                    }
                }

                return _PartInfo;
            }

            internal set
            {
                _PartInfo = value;
            }
        }

        /// <summary>
        /// The physical disk drive number.
        /// </summary>
        /// <remarks></remarks>
        public int PhysicalDevice
        {
            get
            {
                return _PhysicalDevice;
            }

            internal set
            {
                _PhysicalDevice = value;
            }
        }

        /// <summary>
        /// If applicable, the partition number of the device.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int PartitionNumber
        {
            get
            {
                return _PartitionNumber;
            }

            internal set
            {
                _PartitionNumber = value;
            }
        }

        /// <summary>
        /// Total capacity of storage device.
        /// </summary>
        /// <remarks></remarks>
        public FriendlySizeLong Size
        {
            get
            {
                if (Type != DevClassPresenting.StorageType.Volume || string.IsNullOrEmpty(VolumeGuidPath))
                    return _Size;
                var a = default(ulong);
                var b = default(ulong);
                var c = default(ulong);
                try
                {
                    if (!IsVolumeMounted)
                        return new FriendlySizeLong(0);

                    FileApi.GetDiskFreeSpaceEx(VolumeGuidPath, ref a, ref b, ref c);
                }
                catch (Exception ex)
                {
                    return new FriendlySizeLong(0);
                }

                return b;
            }

            internal set
            {
                _Size = value;
            }
        }

        /// <summary>
        /// Available capacity of volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong SizeFree
        {
            get
            {
                if (Type != DevClassPresenting.StorageType.Volume || string.IsNullOrEmpty(VolumeGuidPath))
                    return _Size;
                var a = default(ulong);
                var b = default(ulong);
                var c = default(ulong);
                try
                {
                    if (!IsVolumeMounted)
                        return new FriendlySizeLong(0);

                    FileApi.GetDiskFreeSpaceEx(VolumeGuidPath, ref a, ref b, ref c);
                }
                catch (Exception ex)
                {
                    return new FriendlySizeLong(0);
                }

                return c;
            }
        }

        /// <summary>
        /// The total used space of the volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong SizeUsed
        {
            get
            {
                if (Type != DevClassPresenting.StorageType.Volume || string.IsNullOrEmpty(VolumeGuidPath))
                    return _Size;
                var a = default(ulong);
                var b = default(ulong);
                var c = default(ulong);
                try
                {
                    if (!IsVolumeMounted)
                        return new FriendlySizeLong(0);

                    FileApi.GetDiskFreeSpaceEx(VolumeGuidPath, ref a, ref b, ref c);

                    return b - c;
                }
                catch (Exception ex)
                {
                    return new FriendlySizeLong(0);
                }
            }
        }

        /// <summary>
        /// TYpe of storage.
        /// </summary>
        /// <remarks></remarks>
        public DevClassPresenting.StorageType Type
        {
            get
            {
                return _Type;
            }

            internal set
            {
                _Type = value;
            }
        }

        /// <summary>
        /// Physical device capabilities.
        /// </summary>
        /// <remarks></remarks>
        public DevClassPresenting.DeviceCapabilities Capabilities
        {
            get
            {
                DevClassPresenting.DeviceCapabilities CapabilitiesRet = default;
                CapabilitiesRet = _Capabilities;
                return CapabilitiesRet;
            }

            internal set
            {
                _Capabilities = value;
            }
        }
        /// <summary>
        /// Contains a list of VHD/VHDX files that make up a virtual hard drive.
        /// </summary>
        /// <remarks></remarks>
        public string[] BackingStore
        {
            get
            {
                string[] BackingStoreRet = default;
                BackingStoreRet = _BackingStore;
                return BackingStoreRet;
            }

            internal set
            {
                _BackingStore = value;
            }
        }
        // ' Volume information

        /// <summary>
        /// Indicates whether or not this structure refers to a volume or a device.
        /// </summary>
        /// <remarks></remarks>
        public bool IsVolume
        {
            get
            {
                return _IsVolume;
            }

            internal set
            {
                _IsVolume = value;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this volume is mounted, if it represents a volume of a removeable drive.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsVolumeMounted
        {
            get
            {
                return GetDriveFlag() != 0L & FileApi.GetLogicalDrives() != 0L;
            }
        }

        /// <summary>
        /// Returns the current logical drive flag.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private uint GetDriveFlag()
        {
            if (!IsVolume)
                return 0U;
            if (_VolumePaths is null || _VolumePaths.Length == 0)
                return 0U;
            char ch = '-';
            uint vl = 0U;
            foreach (var vp in _VolumePaths)
            {
                if (vp.Length <= 3)
                {
                    ch = vp.ToCharArray()[0];
                    break;
                }
            }

            if (ch == '-')
                return 0U;
            ch = ch.ToString().ToUpper()[0];
            vl = (uint)(ch - 'A');
            return (uint)(1 << (int)vl);
        }

        /// <summary>
        /// The volume serial number.
        /// </summary>
        /// <remarks></remarks>
        public uint SerialNumber
        {
            get
            {
                uint SerialNumberRet = default;
                SerialNumberRet = _SerialNumber;
                return SerialNumberRet;
            }

            internal set
            {
                _SerialNumber = value;
            }
        }

        /// <summary>
        /// The name of the file system for this volume.
        /// </summary>
        /// <remarks></remarks>
        public string FileSystem
        {
            get
            {
                string FileSystemRet = default;
                FileSystemRet = _FileSystem;
                return FileSystemRet;
            }

            internal set
            {
                _FileSystem = value;
            }
        }

        /// <summary>
        /// Volume flags and capabilities.
        /// </summary>
        /// <remarks></remarks>
        public DevClassPresenting.FileSystemFlags VolumeFlags
        {
            get
            {
                DevClassPresenting.FileSystemFlags VolumeFlagsRet = default;
                VolumeFlagsRet = _VolumeFlags;
                return VolumeFlagsRet;
            }

            internal set
            {
                _VolumeFlags = value;
            }
        }

        /// <summary>
        /// The Volume GUID (parsing) path.  This member can be used in a call to CreateFile for DeviceIoControl (for volumes).
        /// </summary>
        /// <remarks></remarks>
        public string VolumeGuidPath
        {
            get
            {
                string VolumeGuidPathRet = default;
                VolumeGuidPathRet = _VolumeGuidPath;
                return VolumeGuidPathRet;
            }

            set
            {
                _VolumeGuidPath = value;
            }
        }
        /// <summary>
        /// A list of all mount-points for the volume
        /// </summary>
        /// <remarks></remarks>
        public string[] VolumePaths
        {
            get
            {
                string[] VolumePathsRet = default;
                VolumePathsRet = _VolumePaths;
                return VolumePathsRet;
            }

            set
            {
                _VolumePaths = value;
            }
        }

        /// <summary>
        /// Partition locations on the physical disk or disks.
        /// </summary>
        /// <remarks></remarks>
        public DevClassPresenting.DiskExtent[] DiskExtents
        {
            get
            {
                DevClassPresenting.DiskExtent[] DiskExtentsRet = default;
                DiskExtentsRet = _DiskExtents;
                return DiskExtentsRet;
            }

            internal set
            {
                _DiskExtents = value;
            }
        }

        /// <summary>
        /// Print friendly device information, including friendly name, mount points and the device's friendly size.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            string ToStringRet = default;
            if (IsVolume)
            {
                if (VolumePaths is object && VolumePaths.Count() > 0)
                {
                    string slist = string.Join(", ", VolumePaths);
                    ToStringRet = "[" + slist + "] ";
                }
                else
                {
                    ToStringRet = "";
                }

                ToStringRet += FriendlyName + " (" + TextTools.PrintFriendlySize(Size) + ")";
            }
            else
            {
                ToStringRet = "[" + Type.ToString() + "] " + FriendlyName + " (" + TextTools.PrintFriendlySize(Size) + ")";
            }

            return ToStringRet;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}