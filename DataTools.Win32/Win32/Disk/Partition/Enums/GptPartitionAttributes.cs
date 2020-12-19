using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32.Disk.Partition
{

    /// <summary>
    /// Gpt Partition Flags
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum GptPartitionAttributes : ulong
    {

        /// <summary>
        /// The partition is required for the platform.
        /// </summary>
        /// <remarks></remarks>
        RequiredPartition = 1UL,

        /// <summary>
        /// UEFI does not produce a block IO device for this partition.
        /// </summary>
        /// <remarks></remarks>
        NoBlockIoProtocol = 2UL,

        /// <summary>
        /// The partition is visible as bootable to legacy software.
        /// </summary>
        /// <remarks></remarks>
        LegacyBiosBootable = 4UL,

        /// <summary>
        /// UEFI reserved bits 3-47.
        /// </summary>
        /// <remarks></remarks>
        UefiReservedMask = 0xFFFFFFFFFFF8UL,

        /// <summary>
        /// Platform reserved bits 48-63.
        /// </summary>
        /// <remarks></remarks>
        PlatformReservedMask = 0xFFFF000000000000UL
    }

}
