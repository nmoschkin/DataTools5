using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Interop.Native;
using DataTools.Interop;
using CoreCT.SystemInformation;

namespace DataTools.Interop.System
{
    public class ProcessorDeviceInfo : DeviceInfo
    {
        private IReadOnlyCollection<CacheInfo> cacheInfo;
        private SYSTEM_LOGICAL_PROCESSOR_INFORMATION source;

        internal SYSTEM_LOGICAL_PROCESSOR_INFORMATION Source
        {
            get => source;
            set => source = value;
        }

        private long totalLineSize;

        public long TotalLineSize
        {
            get => totalLineSize;
            internal set
            {
                totalLineSize = value;
            }
        }
        
        private long totalCacheSize;

        public long TotalCacheSize
        {
            get => totalCacheSize;
            internal set
            {
                totalCacheSize = value;
            }
        }

        public IReadOnlyCollection<CacheInfo> Caches
        {
            get => cacheInfo;
            internal set
            {
                cacheInfo = value;
            }
        }

        private bool hasL1cache;

        public bool HasL1Cache
        {
            get => hasL1cache;
            internal set
            {
                hasL1cache = value;
            }
        }


        private bool hasL2cache;

        public bool HasL2Cache
        {
            get => hasL2cache;
            internal set
            {
                hasL2cache = value;
            }
        }

        private bool hasL3cache;

        public bool HasL3Cache
        {
            get => hasL3cache;
            internal set
            {
                hasL3cache = value;
            }
        }

        private int core;

        public int Core
        {
            get => core;
            internal set
            {
                core = value;
            }
        }

        private int logProc;

        public int LogicalProcessor
        {
            get => logProc;
            internal set
            {
                logProc = value;
            }
        }

        public override string ToString()
        {
            try
            {
                string s = source.ToString();
                return s;
            }
            catch
            {

            }
            
            return base.ToString();
        }
    }
}
