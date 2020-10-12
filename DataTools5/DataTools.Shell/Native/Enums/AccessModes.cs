// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: NativeShell
// '         Wrappers for native and COM shell interfaces.
// '
// ' Some enum documentation copied from the MSDN (and in some cases, updated).
// ' Some classes and interfaces were ported from the WindowsAPICodePack.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

//using DataTools.Hardware.MessageResources;
//using DataTools.Hardware;
//using DataTools.Hardware.Native;

namespace DataTools.Shell.Native
{
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Follows native api.")]
    public enum AccessModes
    {
        /// <summary>
        /// Indicates that, in direct mode, each change to a storage
        /// or stream element is written as it occurs.
        /// </summary>
        Direct = 0x0,

        /// <summary>
        /// Indicates that, in transacted mode, changes are buffered
        /// and written only if an explicit commit operation is called.
        /// </summary>
        Transacted = 0x10000,

        /// <summary>
        /// Provides a faster implementation of a compound file
        /// in a limited, but frequently used, case.
        /// </summary>
        Simple = 0x8000000,

        /// <summary>
        /// Indicates that the object is read-only,
        /// meaning that modifications cannot be made.
        /// </summary>
        Read = 0x0,

        /// <summary>
        /// Enables you to save changes to the object,
        /// but does not permit access to its data.
        /// </summary>
        Write = 0x1,

        /// <summary>
        /// Enables access and modification of object data.
        /// </summary>
        ReadWrite = 0x2,

        /// <summary>
        /// Specifies that subsequent openings of the object are
        /// not denied read or write access.
        /// </summary>
        ShareDenyNone = 0x40,

        /// <summary>
        /// Prevents others from subsequently opening the object in Read mode.
        /// </summary>
        ShareDenyRead = 0x30,

        /// <summary>
        /// Prevents others from subsequently opening the object
        /// for Write or ReadWrite access.
        /// </summary>
        ShareDenyWrite = 0x20,

        /// <summary>
        /// Prevents others from subsequently opening the object in any mode.
        /// </summary>
        ShareExclusive = 0x10,

        /// <summary>
        /// Opens the storage object with exclusive access to the most
        /// recently committed version.
        /// </summary>
        Priority = 0x40000,

        /// <summary>
        /// Indicates that the underlying file is to be automatically destroyed when the root
        /// storage object is released. This feature is most useful for creating temporary files.
        /// </summary>
        DeleteOnRelease = 0x4000000,

        /// <summary>
        /// Indicates that, in transacted mode, a temporary scratch file is usually used
        /// to save modifications until the Commit method is called.
        /// Specifying NoScratch permits the unused portion of the original file
        /// to be used as work space instead of creating a new file for that purpose.
        /// </summary>
        NoScratch = 0x100000,

        /// <summary>
        /// Indicates that an existing storage object
        /// or stream should be removed before the new object replaces it.
        /// </summary>
        Create = 0x1000,

        /// <summary>
        /// Creates the new object while preserving existing data in a stream named "Contents".
        /// </summary>
        Convert = 0x20000,

        /// <summary>
        /// Causes the create operation to fail if an existing object with the specified name exists.
        /// </summary>
        FailIfThere = 0x0,

        /// <summary>
        /// This flag is used when opening a storage object with Transacted
        /// and without ShareExclusive or ShareDenyWrite.
        /// In this case, specifying NoSnapshot prevents the system-provided
        /// implementation from creating a snapshot copy of the file.
        /// Instead, changes to the file are written to the end of the file.
        /// </summary>
        NoSnapshot = 0x200000,

        /// <summary>
        /// Supports direct mode for single-writer, multireader file operations.
        /// </summary>
        DirectSingleWriterMultipleReader = 0x400000
    }
}
