// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Miscellaneous enums to support devices.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


using System;
using System.ComponentModel;
using DataTools.Text;
using DataTools.Win32;

namespace DataTools.Win32
{
    /// <summary>
    /// Represents partition location information on a physical disk.
    /// </summary>
    /// <remarks></remarks>
    public struct DiskExtent
    {

        /// <summary>
        /// The physical device number
        /// </summary>
        /// <remarks></remarks>
        public int PhysicalDevice;

        /// <summary>
        /// Reserved
        /// </summary>
        /// <remarks></remarks>
        public int Space;

        /// <summary>
        /// Physical byte offset on disk
        /// </summary>
        /// <remarks></remarks>
        public long Offset;

        /// <summary>
        /// Physical size in bytes on disk
        /// </summary>
        /// <remarks></remarks>
        public long Size;

        /// <summary>
        /// Presents this object in a readable string.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return "Physical Device " + PhysicalDevice + ", " + TextTools.PrintFriendlySize(Size);
        }
    }
}
