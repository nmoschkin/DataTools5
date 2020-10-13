// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: VirtualDisk
//         Create, Mount, and Unmount .vhd and .vhdx
//         Virtual Disks.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Text;
using DataTools.Hardware.Native;

namespace DataTools.Hardware.Disk
{

    /// <summary>
    /// Virtual storage type.
    /// </summary>
    /// <remarks></remarks>
    public enum VirtualStorageType : int
    {

        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks></remarks>
        Unknown = 0,

        /// <summary>
        /// CD-ROM/DVD-ROM ISO image file.
        /// </summary>
        /// <remarks></remarks>
        ISO = 1,

        /// <summary>
        /// Virtual drive (Windows 7)
        /// </summary>
        /// <remarks></remarks>
        VHD = 2,

        /// <summary>
        /// Virtual drive (Windows 8+)
        /// </summary>
        /// <remarks></remarks>
        VHDX = 3
    }

    /// <summary>
    /// Encapsulates a virtual disk device (iso, vhd, or vhdx).
    /// </summary>
    /// <remarks></remarks>
    public sealed class VirtualDisk
    {
        internal DiskDeviceInfo _info;
        internal IntPtr _Handle = IntPtr.Zero;
        private string _ImageFile;

        /// <summary>
        /// Indicates whether the virtual drive is attached.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Attached
        {
            get
            {
                return !string.IsNullOrEmpty(DevicePath);
            }
        }

        /// <summary>
        /// Indicates whether the virtual drive is open.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsOpen
        {
            get
            {
                return _Handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Displays drive information.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = TextTools.PrintFriendlySize(Size) + " Virtual Drive [" + (Attached ? "Attached" : "Not Attached") + "]";
            return ToStringRet;
        }

        /// <summary>
        /// Returns the actual physical size of the the virtual drive, as stored on the primary media.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long PhysicalSize
        {
            get
            {
                long PhysicalSizeRet = default;
                var info = default(VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE);
                uint iSize;
                var sizeUSed = default(uint);
                var mm = new MemPtr();
                info.Version = VDiskDecl.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_SIZE;
                iSize = (uint)Marshal.SizeOf<VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE>();
                mm.FromStruct(info);
                uint r = VDiskDecl.GetVirtualDiskInformation(_Handle, ref iSize, mm, ref sizeUSed);
                info = mm.ToStruct<VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE>();
                PhysicalSizeRet = (long)info.PhysicalSize;
                mm.Free();
                return PhysicalSizeRet;
            }
        }

        /// <summary>
        /// Returns the virtual size of the drive.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Size
        {
            get
            {
                long SizeRet = default;
                var info = default(VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE);
                uint iSize;
                var sizeUSed = default(uint);
                var mm = new MemPtr();
                info.Version = VDiskDecl.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_SIZE;
                iSize = (uint)Marshal.SizeOf(info);
                mm.FromStruct(info);
                uint r = VDiskDecl.GetVirtualDiskInformation(_Handle, ref iSize, mm, ref sizeUSed);
                info = mm.ToStruct<VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE>();
                SizeRet = (long)info.VirtualSize;
                mm.Free();
                return SizeRet;
            }
        }

        /// <summary>
        /// Returns the physical hardware path of the device.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PhysicalPath
        {
            get
            {
                string PhysicalPathRet = default;
                uint szpath = FileApi.MAX_PATH;
                var mm = new MemPtr();
                mm.Alloc(FileApi.MAX_PATH * 2);
                uint r = VDiskDecl.GetVirtualDiskPhysicalPath(_Handle, ref szpath, mm.Handle);
                PhysicalPathRet = mm.ToString();
                mm.Free();
                return PhysicalPathRet;
            }
        }

        /// <summary>
        /// Returns the unique Guid identifier of the virtual drive.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Guid Identifier
        {
            get
            {
                Guid IdentifierRet = default;
                var info = default(VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE);
                uint iSize;
                var sizeUSed = default(uint);
                var mm = new MemPtr();
                info.Version = VDiskDecl.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_IDENTIFIER;
                iSize = (uint)Marshal.SizeOf<VDiskDecl.GET_VIRTUAL_DISK_INFO_SIZE>();
                mm.FromStruct(info);
                uint r = VDiskDecl.GetVirtualDiskInformation(_Handle, ref iSize, mm, ref sizeUSed);
                IdentifierRet = mm.GuidAtAbsolute(8L);
                mm.Free();
                return IdentifierRet;
            }
        }

        /// <summary>
        /// Returns the handle to the open virtual disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr Handle
        {
            get
            {
                return _Handle;
            }
        }

        /// <summary>
        /// Gets or sets the VHD/VHDX/ISO image file for the current object.
        /// If set, any current image is closed.
        /// </summary>
        /// <returns></returns>
        public string ImageFile
        {
            get
            {
                return _ImageFile;
            }

            set
            {
                if (Open())
                    Close();
                _ImageFile = value;
            }
        }

        /// <summary>
        /// Creates a new virtual disk device file.
        /// </summary>
        /// <param name="imageFile">Full path to the destination image, including file extension (.vhd or .vhdx)</param>
        /// <param name="diskSize">The virtual size of the new disk.</param>
        /// <param name="fixedSize">Indicates whether or not the size is fixed or virtual.</param>
        /// <param name="id">Receives the Guid for the new drive.</param>
        /// <param name="resiliencyId">The Resiliancy Guid.</param>
        /// <param name="blockSize">The transfer block size.</param>
        /// <param name="sectorSize">The virtual sector size.</param>
        /// <param name="sourcePath">The cloning source (if any).</param>
        /// <param name="sourceDiskType">The cloning source disk type (if applicable).</param>
        /// <returns>A new VirtualDisk object.</returns>
        /// <remarks></remarks>
        public static VirtualDisk Create(string imageFile, long diskSize, bool fixedSize, ref Guid id, ref Guid resiliencyId, int blockSize = 2097152, int sectorSize = 512, string sourcePath = null, VirtualStorageType sourceDiskType = VirtualStorageType.Unknown)
        {
            VirtualDisk CreateRet = default;

            string ext = Path.GetExtension(imageFile).ToLower();

            var cp2 = new VDiskDecl.CREATE_VIRTUAL_DISK_PARAMETERS_V2();
            var cp1 = new VDiskDecl.CREATE_VIRTUAL_DISK_PARAMETERS_V1();

            VDiskDecl.VIRTUAL_STORAGE_TYPE vst;

            var r = default(uint);
            IntPtr handleNew = new IntPtr();

            vst.VendorId = VDiskDecl.VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT;

            switch (ext ?? "")
            {
                case ".vhd":
                    {
                        cp1.BlockSizeInBytes = (uint)blockSize;
                        cp1.Version = VDiskDecl.CREATE_VIRTUAL_DISK_VERSION.CREATE_VIRTUAL_DISK_VERSION_1;
                        cp1.MaximumSize = (ulong)diskSize;
                        cp1.UniqueId = id;
                        cp1.SourcePath = sourcePath;
                        cp1.SectorSizeInBytes = 512;
                        vst.DeviceId = VDiskDecl.VIRTUAL_STORAGE_TYPE_DEVICE_VHD;

                        r = VDiskDecl.CreateVirtualDisk(vst, imageFile, VDiskDecl.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_ALL, IntPtr.Zero, fixedSize ? VDiskDecl.CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION : VDiskDecl.CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_NONE, 0U, cp1, IntPtr.Zero, ref handleNew);

                        break;
                    }

                case ".vhdx":
                    {
                        cp2.BlockSizeInBytes = (uint)blockSize;
                        cp2.Version = VDiskDecl.CREATE_VIRTUAL_DISK_VERSION.CREATE_VIRTUAL_DISK_VERSION_2;
                        cp2.MaximumSize = (ulong)diskSize;
                        cp2.UniqueId = id;
                        cp2.SourcePath = sourcePath;
                        if (sourcePath is object)
                        {
                            cp2.SourceVirtualStorageType.DeviceId = (uint)sourceDiskType;
                            cp2.SourceVirtualStorageType.VendorId = VDiskDecl.VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT;
                        }

                        int x = Marshal.SizeOf(cp2);

                        cp2.SectorSizeInBytes = sectorSize;

                        if (resiliencyId != Guid.Empty)
                        {
                            cp2.ResiliencyGuid = resiliencyId;
                            // Else
                            // cp2.ResiliencyGuid = Guid.NewGuid
                            // resiliencyId = cp2.ResiliencyGuid
                        }

                        cp2.OpenFlags = VDiskDecl.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE;
                        vst.DeviceId = VDiskDecl.VIRTUAL_STORAGE_TYPE_DEVICE_VHDX;

                        r = VDiskDecl.CreateVirtualDisk(vst, imageFile, VDiskDecl.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_NONE, IntPtr.Zero, fixedSize ? VDiskDecl.CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION : VDiskDecl.CREATE_VIRTUAL_DISK_FLAGS.CREATE_VIRTUAL_DISK_FLAG_NONE, 0U, cp2, IntPtr.Zero, ref handleNew);
                        break;
                    }
            }

            if (r != 0L)
            {
                CreateRet = null;
            }
            else
            {
                CreateRet = new VirtualDisk();
                CreateRet._Handle = handleNew;
                CreateRet._ImageFile = imageFile;
            }

            return CreateRet;
        }

        /// <summary>
        /// Open the virtual disk.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Open()
        {
            bool OpenRet = default;
            OpenRet = Open(false);
            return OpenRet;
        }

        /// <summary>
        /// Open the virtual disk.
        /// </summary>
        /// <param name="openReadOnly">Open as read-only.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Open(bool openReadOnly)
        {
            bool OpenRet = default;
            OpenRet = Open(_ImageFile, openReadOnly);
            return OpenRet;
        }

        /// <summary>
        /// Open the specified virtual disk image file.
        /// </summary>
        /// <param name="imageFile">The vhd or vhdx file to open.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Open(string imageFile)
        {
            bool OpenRet = default;
            OpenRet = Open(imageFile, false);
            return OpenRet;
        }

        /// <summary>
        /// Open the specified disk image file.
        /// </summary>
        /// <param name="imageFile">The vhd or vhdx file to open.</param>
        /// <param name="openReadOnly">Open as read-only.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Open(string imageFile, bool openReadOnly)
        {
            bool OpenRet = default;
            string ext = Path.GetExtension(imageFile).ToLower();
            VDiskDecl.VIRTUAL_STORAGE_TYPE vst;
            if (_Handle != IntPtr.Zero)
                Close();

            var vdp1 = new VDiskDecl.OPEN_VIRTUAL_DISK_PARAMETERS_V1();
            var vdp2 = new VDiskDecl.OPEN_VIRTUAL_DISK_PARAMETERS_V2();

            uint r;

            var am = VDiskDecl.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_GET_INFO | VDiskDecl.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_DETACH;

            if (!openReadOnly)
            {
                am = am | VDiskDecl.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_ATTACH_RW;
            }

            vdp2.Version = VDiskDecl.OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_2;
            vdp2.ResiliencyGuid = Guid.NewGuid();
            vdp2.ReadOnly = false;
            vdp2.GetInfoOnly = false;

            vdp1.RWDepth = 1U;
            vdp1.Version = VDiskDecl.OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_1;

            vst.VendorId = VDiskDecl.VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT;

            switch (ext.ToLower() ?? "")
            {
                case ".vhd":
                    vst.DeviceId = VDiskDecl.VIRTUAL_STORAGE_TYPE_DEVICE_VHD;
                    r = VDiskDecl.OpenVirtualDisk(vst, imageFile, am, VDiskDecl.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE, vdp1, ref _Handle);
                    break;

                case ".vhdx":
                    vst.DeviceId = VDiskDecl.VIRTUAL_STORAGE_TYPE_DEVICE_VHDX;
                    r = VDiskDecl.OpenVirtualDisk(vst, imageFile, am, VDiskDecl.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE, vdp1, ref _Handle);
                    break;

                default:
                    return false;
            }

            if (r == 0L)
            {
                _ImageFile = imageFile;
            }

            OpenRet = r == 0L;
            return OpenRet;
        }

        /// <summary>
        /// Mount the virtual drive.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Attach()
        {
            return Attach(true);
        }

        /// <summary>
        /// Mount the virtual drive, permanently.  The virtual drive will stay mounted beyond the lifetime of this instance.
        /// </summary>
        /// <param name="makePermanent"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Attach(bool makePermanent)
        {
            if (_Handle == IntPtr.Zero || Attached)
                return false;

            var mm = new MemPtr(8L);

            mm.ByteAt(0L) = 1;

            uint r = VDiskDecl.AttachVirtualDisk(_Handle, IntPtr.Zero, makePermanent ? VDiskDecl.ATTACH_VIRTUAL_DISK_FLAG.ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME : VDiskDecl.ATTACH_VIRTUAL_DISK_FLAG.ATTACH_VIRTUAL_DISK_FLAG_NONE, 0U, mm.Handle, IntPtr.Zero);

            mm.Free();

            return r == 0L;
        }

        /// <summary>
        /// Dismount the drive.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Detach()
        {
            if (!Attached) return false;

            uint r = VDiskDecl.DetachVirtualDisk(_Handle, VDiskDecl.DETACH_VIRTUAL_DISK_FLAG.DETACH_VIRTUAL_DISK_FLAG_NONE, 0U);

            return r == 0L;
        }

        /// <summary>
        /// Close the disk instance.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Close()
        {
            if (_Handle == IntPtr.Zero)
                return false;

            bool r = PInvoke.CloseHandle(_Handle);

            if (r)
                _Handle = IntPtr.Zero;

            return r;
        }

        /// <summary>
        /// Initialize the virtual disk object with an existing virtual disk.
        /// </summary>
        /// <param name="imageFile">The disk image file to load.</param>
        /// <remarks></remarks>
        public VirtualDisk(string imageFile) : this(imageFile, false, false)
        {
        }

        /// <summary>
        /// Initialize the virtual disk object with an existing virtual disk, and optionally open the disk.
        /// </summary>
        /// <param name="imageFile">The disk image file to load.</param>
        /// <param name="openDisk">Whether or not to open the disk.</param>
        /// <remarks></remarks>
        public VirtualDisk(string imageFile, bool openDisk) : this(imageFile, openDisk, false)
        {
        }

        /// <summary>
        /// Initialize the virtual disk object with an existing virtual disk, and optionally open the disk.
        /// </summary>
        /// <param name="imageFile">The disk image file to load.</param>
        /// <param name="openDisk">Whether or not to open the disk.</param>
        /// <param name="openReadOnly">Whether or not to open the disk read-only.</param>
        /// <remarks></remarks>
        public VirtualDisk(string imageFile, bool openDisk, bool openReadOnly)
        {
            _ImageFile = imageFile;
            if (openDisk)
                Open(openReadOnly);
        }

        /// <summary>
        /// Initializes a new instance of a virtual drive with the specified DiskDeviceInfo object.
        /// </summary>
        /// <param name="inf">DiskDeviceInfo object.</param>
        /// <param name="open">Whether or not to open the drive.</param>
        /// <remarks></remarks>
        public VirtualDisk(DiskDeviceInfo inf, bool open) : this(inf.BackingStore[0], open)
        {
            _info = inf;
        }

        /// <summary>
        /// Returns the DeviceCapabilities enumeration.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DeviceCapabilities Capabilities
        {
            get
            {
                return _info.Capabilities;
            }
        }

        /// <summary>
        /// Returns the device friendly name.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FriendlyName
        {
            get
            {
                return _info.FriendlyName;
            }
        }

        /// <summary>
        /// Returns the PhysicalDriveX number.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int PhysicalDevice
        {
            get
            {
                return _info.PhysicalDevice;
            }
        }

        /// <summary>
        /// Returns the device path.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DevicePath
        {
            get
            {
                return _info.DevicePath;
            }
        }

        /// <summary>
        /// Returns the backing store paths.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] BackingStore
        {
            get
            {
                return _info.BackingStore;
            }
        }

        public object Tag { get; set; }

        /// <summary>
        /// Returns the storage device type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public StorageType Type
        {
            get
            {
                return StorageType.Virtual;
            }
        }

        /// <summary>
        /// Returns the hardware instance id.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string InstanceId
        {
            get
            {
                return _info.InstanceId;
            }
        }

        /// <summary>
        /// Returns the device type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DeviceType DeviceType
        {
            get
            {
                return DeviceType.Disk;
            }
        }

        private bool disposedValue;

        /// <summary>
        /// Dispose of the current instance and close all handles.
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Close();
            }

            disposedValue = true;
        }

        ~VirtualDisk()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private VirtualDisk()
        {
        }
    }
}