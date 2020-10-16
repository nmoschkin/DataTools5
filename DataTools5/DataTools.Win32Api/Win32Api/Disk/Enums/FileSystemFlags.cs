// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Miscellaneous enums to support devices.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    /// <summary>
    /// Flags that specify the capabilities of a volume file system.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum FileSystemFlags
    {

        /// <summary>
        /// The specified volume supports preserved case of file names when it places a name on disk.
        /// </summary>
        [Description("The specified volume supports preserved case of file names when it places a name on disk.")]
        CasePreservedNames = 0x2,

        /// <summary>
        /// The specified volume supports case-sensitive file names.
        /// </summary>
        [Description("The specified volume supports case-sensitive file names.")]
        CaseSensitiveSearch = 0x1,

        /// <summary>
        /// The specified volume supports file-based compression.
        /// </summary>
        [Description("The specified volume supports file-based compression.")]
        Compression = 0x10,

        /// <summary>
        /// The specified volume supports named streams.
        /// </summary>
        [Description("The specified volume supports named streams.")]
        NamedStreams = 0x40000,

        /// <summary>
        /// The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
        /// </summary>
        [Description("The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.")]
        PersistentACLs = 0x8,

        /// <summary>
        /// The specified volume is read-only.
        /// </summary>
        [Description("The specified volume is read-only.")]
        ReadOnlyVolume = 0x80000,

        /// <summary>
        /// The specified volume supports a single sequential write.
        /// </summary>
        [Description("The specified volume supports a single sequential write.")]
        SequentialWriteOnce = 0x100000,

        /// <summary>
        /// The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.
        /// </summary>
        [Description("The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.")]
        SupportsEncryption = 0x20000,

        /// <summary>
        /// The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.
        /// </summary>
        [Description("The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.")]
        SupportsExtendedAttributes = 0x800000,

        /// <summary>
        /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
        /// </summary>
        [Description("The specified volume supports hard links. For more information, see Hard Links and Junctions.")]
        SupportsHardLinks = 0x400000,

        /// <summary>
        /// The specified volume supports object identifiers.
        /// </summary>
        [Description("The specified volume supports object identifiers.")]
        SupportsObjectIds = 0x10000,


        /// <summary>
        /// The file system supports open by FileID. For more information, see IDBOTHDIRINFO.
        /// </summary>
        [Description("The file system supports open by FileID. For more information, see IDBOTHDIRINFO.")]
        SupportsOpenById = 0x1000000,


        /// <summary>
        /// The specified volume supports reparse points.
        /// </summary>
        [Description("The specified volume supports reparse points.")]
        SupportsReparsePoints = 0x80,


        /// <summary>
        /// The specified volume supports sparse files.
        /// </summary>
        [Description("The specified volume supports sparse files.")]
        SupportsSparseFiles = 0x40,


        /// <summary>
        /// The specified volume supports transactions. For more information, see About KTM.
        /// </summary>
        [Description("The specified volume supports transactions. For more information, see About KTM.")]
        SupportsTransactions = 0x200000,

        /// <summary>
        /// The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.
        /// </summary>
        [Description("The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.")]
        SupportsUSNJournal = 0x2000000,

        /// <summary>
        /// The specified volume supports Unicode in file names as they appear on disk.
        /// </summary>
        [Description("The specified volume supports Unicode in file names as they appear on disk.")]
        UnicodeOnDisk = 0x4,

        /// <summary>
        /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        [Description("The specified volume is a compressed volume, for example, a DoubleSpace volume.")]
        VolumeIsCompressed = 0x8000,

        /// <summary>
        /// The specified volume supports disk quotas.
        /// </summary>
        [Description("The specified volume supports disk quotas.")]
        VolumeQuotas = 0x20
    }
}
