using DataTools.Win32Api;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Win32Api.PartitionInfo.Gpt;
using DataTools.Win32Api.PartitionInfo.Mbr;

namespace DataTools.Win32Api.PartitionInfo
{
    /// <summary>
    /// Base class for disk device layout information.
    /// </summary>
    /// <remarks></remarks>
    public abstract class DiskLayoutInfo : IDiskLayout
    {
        internal Partitioning.DRIVE_LAYOUT_INFORMATION_EX _Layout;
        internal IDiskPartition[] _Parts;

        /// <summary>
        /// Populates disk layout information from an open disk handle.
        /// </summary>
        /// <param name="disk"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static IDiskLayout CreateLayout(IntPtr disk)
        {
            IDiskLayout CreateLayoutRet = default;
            Partitioning.DRIVE_LAYOUT_INFORMATION_EX lay = default;
            Partitioning.PARTITION_INFORMATION_EX[] p;
            p = Partitioning.GetPartitions(null, disk, ref lay);
            if (p is null)
                return null;
            if (lay.PartitionStyle == PartitionStyle.Gpt)
            {
                CreateLayoutRet = new GptDiskLayoutInfo(lay, p);
            }
            else
            {
                CreateLayoutRet = new MbrDiskLayoutInfo(lay, p);
            }

            return CreateLayoutRet;
        }

        /// <summary>
        /// Populates disk layout information from a device path.
        /// </summary>
        /// <param name="disk"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static IDiskLayout CreateLayout(string disk)
        {
            IDiskLayout CreateLayoutRet = default;
            Partitioning.DRIVE_LAYOUT_INFORMATION_EX lay = default;
            Partitioning.PARTITION_INFORMATION_EX[] p;
            p = Partitioning.GetPartitions(disk, IntPtr.Zero, ref lay);
            if (p is null)
                return null;
            if (lay.PartitionStyle == PartitionStyle.Gpt)
            {
                CreateLayoutRet = new GptDiskLayoutInfo(lay, p);
            }
            else
            {
                CreateLayoutRet = new MbrDiskLayoutInfo(lay, p);
            }

            return CreateLayoutRet;
        }

        /// <summary>
        /// Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="p"></param>
        /// <remarks></remarks>
        internal DiskLayoutInfo(Partitioning.DRIVE_LAYOUT_INFORMATION_EX li, Partitioning.PARTITION_INFORMATION_EX[] p)
        {
            _Layout = li;
            var pts = new List<IDiskPartition>();
            foreach (var i in p)
                pts.Add(DiskPartitionInfo.CreateInfo(i));
            _Parts = pts.ToArray();
        }


        /// <summary>
        /// Returns the partition style of the disk (MBR or GPT).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionStyle PartitionStyle
        {
            get
            {
                PartitionStyle PartitionStyleRet = default;
                PartitionStyleRet = _Layout.PartitionStyle;
                return PartitionStyleRet;
            }
        }

        /// <summary>
        /// Returns the number of partitions on the disk.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Count
        {
            get
            {
                int CountRet = default;
                CountRet = (int)_Layout.ParititionCount;
                return CountRet;
            }
        }

        /// <summary>
        /// Returns a specific partition by its index in the collection.
        /// </summary>
        /// <param name="index"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDiskPartition this[int index]
        {
            get
            {
                return _Parts[index];
            }
            set
            {
                _Parts[index] = value;
            }
        }

        
        /// <summary>
        /// Converts this object into its string representation.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = _Layout.PartitionStyle.ToString() + " disk, " + Count + " partitions.";
            return ToStringRet;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        IEnumerator<IDiskPartition> IEnumerable<IDiskPartition>.GetEnumerator()
        {
            IEnumerator<IDiskPartition> GetEnumeratorRet = default;
            GetEnumeratorRet = new Enumerator(this);
            return GetEnumeratorRet;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator GetEnumeratorRet = default;
            GetEnumeratorRet = new Enumerator(this);
            return GetEnumeratorRet;
        }

        //public IEnumerator GetEnumerator1() => GetEnumerator();

        private class Enumerator : IEnumerator<IDiskPartition>
        {
            private DiskLayoutInfo subj;
            private int pos = -1;

            public Enumerator(DiskLayoutInfo subject)
            {
                subj = subject;
            }

            public IDiskPartition Current
            {
                get
                {
                    IDiskPartition CurrentRet = default;
                    CurrentRet = subj._Parts[pos];
                    return CurrentRet;
                }
            }

            object IEnumerator.Current 
            {
                get
                {
                    return subj._Parts[pos];
                }
            }

            public bool MoveNext()
            {
                pos += 1;
                if (pos >= subj.Count)
                    return false;
                else
                    return true;
            }

            public void Reset()
            {
                pos = -1;
            }

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            private bool disposedValue; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        pos = -1;
                        subj = null;
                    }
                }

                disposedValue = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


        }


    }

}
