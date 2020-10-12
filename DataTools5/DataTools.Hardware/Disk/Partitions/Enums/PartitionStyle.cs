using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Hardware.Disk.Partitions
{
    /// <summary>
    /// Partition styles.
    /// </summary>
    /// <remarks></remarks>
    public enum PartitionStyle : uint
    {

        /// <summary>
        /// Master Boot Record
        /// </summary>
        /// <remarks></remarks>
        [Description("Master Boot Record")]
        Mbr = 0U,

        /// <summary>
        /// Guid Partition Table
        /// </summary>
        /// <remarks></remarks>
        [Description("Guid Partition Table")]
        Gpt = 1U,

        /// <summary>
        /// Raw (unconfigured)
        /// </summary>
        /// <remarks></remarks>
        [Description("Raw (unconfigured)")]
        Raw = 2U
    }

}
