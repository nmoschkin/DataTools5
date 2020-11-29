using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Win32Api.Disk.Partition.Gpt;
using DataTools.Win32Api.Disk.Partition.Mbr;

namespace DataTools.Win32Api.Disk.Partition
{

    /// <summary>
    /// Base class for disk partition information.
    /// </summary>
    /// <remarks></remarks>
    public abstract class DiskPartitionInfo : IDiskPartition
    {
        internal Partitioning.PARTITION_INFORMATION_EX _partex;

        /// <summary>
        /// Create a new object that implements the IDiskPartition interface from operating system information.
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static IDiskPartition CreateInfo(Partitioning.PARTITION_INFORMATION_EX pe)
        {
            if (pe.PartitionStyle == PartitionStyle.Gpt)
            {
                return new GptDiskPartitionInfo(pe);
            }
            else
            {
                return new MbrDiskPartitionInfo(pe);
            }
        }

        /// <summary>
        /// Creates a new instance of this DiskPartitionInfo-derived class and populates it with information from the operating system.
        /// </summary>
        /// <param name="pe"></param>
        /// <remarks></remarks>
        internal DiskPartitionInfo(Partitioning.PARTITION_INFORMATION_EX pe)
        {
            _partex = pe;
        }

        /// <summary>
        /// Returns the starting offset, in bytes, of the partition relative to the beginning of the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Offset
        {
            get
            {
                return _partex.StartingOffset;
            }
        }

        public abstract string TypeString { get; }

        /// <summary>
        /// Returns the logical partition number of the partition on the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int PartitionNumber
        {
            get
            {
                return (int)_partex.PartitionNumber;
            }
        }

        /// <summary>
        /// Returns the size, in bytes, of the partition.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Size
        {
            get
            {
                return _partex.PartitionLength;
            }
        }

        /// <summary>
        /// Returns the friendly type name of the partition type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string TypeName
        {
            get
            {
                if (_partex.PartitionStyle == PartitionStyle.Gpt)
                {
                    return _partex.Gpt.Name;
                }
                else
                {
                    return _partex.Mbr.PartitionCode.Name;
                }
            }
        }

        /// <summary>
        /// Returns the style of the partition (MBR or GPT).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionStyle PartitionStyle
        {
            get
            {
                return _partex.PartitionStyle;
            }
        }

        public override string ToString()
        {
            return _partex.ToString();
        }
    }

}
