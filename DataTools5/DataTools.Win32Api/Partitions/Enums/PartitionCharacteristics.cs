using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.PartitionInfo
{
    /// <summary>
    /// Partition characteristics.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum PartitionCharacteristics
    {

        /// <summary>
        /// None or no information.
        /// </summary>
        /// <remarks></remarks>
        None = 0,

        /// <summary>
        /// File System Partition
        /// </summary>
        /// <remarks></remarks>
        Filesystem = 0x1,

        /// <summary>
        /// Container Partition
        /// </summary>
        /// <remarks></remarks>
        Container = 0x2,

        /// <summary>
        /// Hidden Partition
        /// </summary>
        /// <remarks></remarks>
        Hidden = 0x4,

        /// <summary>
        /// Hibernation Partition
        /// </summary>
        /// <remarks></remarks>
        Hibernaton = 0x8,

        /// <summary>
        /// Service Partition
        /// </summary>
        /// <remarks></remarks>
        Service = 0x10,

        /// <summary>
        /// Secured Partition
        /// </summary>
        /// <remarks></remarks>
        Secured = 0x20,

        /// <summary>
        /// Policy Partition
        /// </summary>
        /// <remarks></remarks>
        Policy = 0x40,

        /// <summary>
        /// Blocker Partition
        /// </summary>
        /// <remarks></remarks>
        Blocker = 0x80
    }

}
