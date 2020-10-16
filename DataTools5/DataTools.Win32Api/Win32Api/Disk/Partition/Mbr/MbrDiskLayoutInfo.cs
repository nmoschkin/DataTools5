using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.Disk.Partition.Mbr
{

    /// <summary>
    /// Represents Mbr-style disk layout information.
    /// </summary>
    /// <remarks></remarks>
    public class MbrDiskLayoutInfo : DiskLayoutInfo
    {

        /// <summary>
        /// Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="p"></param>
        /// <remarks></remarks>
        internal MbrDiskLayoutInfo(Partitioning.DRIVE_LAYOUT_INFORMATION_EX li, Partitioning.PARTITION_INFORMATION_EX[] p) : base(li, p)
        {
        }

        /// <summary>
        /// The Mbr Disk Signature
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint Signature
        {
            get
            {
                return _Layout.Mbr.Signature;
            }
        }
    }

}
