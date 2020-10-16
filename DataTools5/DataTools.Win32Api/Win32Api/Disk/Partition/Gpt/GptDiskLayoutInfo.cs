using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.Disk.Partition.Gpt
{

    /// <summary>
    /// Represents Gpt style disk layout information.
    /// </summary>
    /// <remarks></remarks>
    public class GptDiskLayoutInfo : DiskLayoutInfo, IGuid
    {

        /// <summary>
        /// Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="p"></param>
        /// <remarks></remarks>
        internal GptDiskLayoutInfo(Partitioning.DRIVE_LAYOUT_INFORMATION_EX li, Partitioning.PARTITION_INFORMATION_EX[] p) : base(li, p)
        {
        }

        /// <summary>
        /// Returns the Gpt disk Guid.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Guid Id
        {
            get
            {
                return _Layout.Gpt.DiskId;
            }
        }

        /// <summary>
        /// Returns the starting position of the first usable byte on the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long StartingUsableOffset
        {
            get
            {
                return _Layout.Gpt.StartingUsableOffset;
            }
        }

        /// <summary>
        /// Returns the actual usable length of the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long UsableLength
        {
            get
            {
                return _Layout.Gpt.UsableLength;
            }
        }

        /// <summary>
        /// Returns the maximum number of partitions allowed on the disk device.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint MaxPartitionCount
        {
            get
            {
                return _Layout.Gpt.MaxPartitionCount;
            }
        }
    }
}
