using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.Disk.Partition
{



    /// <summary>
    /// Partition access feature flags.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum PartitionAccess
    {

        /// <summary>
        /// Nothing or N/A
        /// </summary>
        /// <remarks></remarks>
        [Description("Nothing or N/A")]
        None = 0,

        /// <summary>
        /// Logical Block Addressing
        /// </summary>
        /// <remarks></remarks>
        [Description("Logical Block Addressing")]
        LBA = 1,

        /// <summary>
        /// Cylinder-Head-Sector
        /// </summary>
        /// <remarks></remarks>
        [Description("Cylinder-Head-Sector")]
        CHS = 2
    }

}
