using DataTools.Hardware.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Hardware.Disk.Partitions.Gpt
{


    /// <summary>
    /// Represents GPT style partition information.
    /// </summary>
    /// <remarks></remarks>
    public class GptDiskPartitionInfo : DiskPartitionInfo, IGuid
    {

        /// <summary>
        /// Creates a new instance of this DiskPartitionInfo-derived class and populates it with information from the operating system.
        /// </summary>
        /// <param name="pe"></param>
        /// <remarks></remarks>
        internal GptDiskPartitionInfo(Partitioning.PARTITION_INFORMATION_EX pe) : base(pe)
        {
        }

        /// <summary>
        /// Returns the UEFI attributes for the device.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public GptPartitionAttributes Attributes
        {
            get
            {
                return _partex.Gpt.Attributes;
            }
        }

        /// <summary>
        /// Returns the Partition Guid.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Guid Id
        {
            get
            {
                return _partex.Gpt.PartitionId;
            }
        }

        /// <summary>
        /// Returns the PartitionType Guid.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Guid PartitionType
        {
            get
            {
                return _partex.Gpt.PartitionType;
            }
        }

        /// <summary>
        /// Returns detailed partition type information.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public GptCodeInfo PartitionTypeInfo
        {
            get
            {
                return _partex.Gpt.PartitionCode;
            }
        }

        /// <summary>
        /// Returns the Guid of the Partition Type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string TypeString
        {
            get
            {
                return PartitionType.ToString("B");
            }
        }
    }
}
