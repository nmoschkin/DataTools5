using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32.Disk.Partition
{
    /// <summary>
    /// Represents the partition configuration for an entire disk device.
    /// </summary>
    /// <remarks></remarks>
    public interface IDiskLayout : IEnumerable<IDiskPartition>
    {

        /// <summary>
        /// Returns the style of the disk or partition (MBR or GPT).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        PartitionStyle PartitionStyle { get; }

        /// <summary>
        /// Returns a specific partition by its index in the collection.
        /// </summary>
        /// <param name="index"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        IDiskPartition this[int index] { get; set; }

        /// <summary>
        /// Returns the number of partitions on the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        int Count { get; }
    }
}
