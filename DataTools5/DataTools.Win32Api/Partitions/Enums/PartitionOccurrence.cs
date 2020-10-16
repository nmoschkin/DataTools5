using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.PartitionInfo
{
    /// <summary>
    /// Partition occurrence flags (where on the disk a partition entry may occur).
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum PartitionOccurrence
    {
        /// <summary>
        /// None or no information.
        /// </summary>
        /// <remarks></remarks>
        [Description("No Information")]
        None = 0,

        /// <summary>
        /// Master Boot Record
        /// </summary>
        /// <remarks></remarks>
        [Description("Master Boot Record")]
        MBR = 1,

        /// <summary>
        /// Extended Boot Record
        /// </summary>
        /// <remarks></remarks>
        [Description("Extended Boot Record")]
        EBR = 2,

        /// <summary>
        /// Virtual Master Boot Record
        /// </summary>
        /// <remarks></remarks>
        [Description("Virtual Master Boot Record")]
        VirtualMBR = 4
    }

}
