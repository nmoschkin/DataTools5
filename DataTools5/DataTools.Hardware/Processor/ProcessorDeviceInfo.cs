using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Hardware.Native;
using DataTools.Hardware;
using DataTools.SystemInformation;
using DataTools.Text;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DataTools.Hardware.Processor
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

        public CacheInfo L1DataCache
        {
            get
            {
                foreach (var c in Caches)
                {
                    if (c.Level == 1 && c.Type == ProcessorCacheType.CacheData)
                    {
                        return c;
                    } 
                }

                return null;
            }
        }

        public CacheInfo L1InstructionCache
        {
            get
            {
                foreach (var c in Caches)
                {
                    if (c.Level == 1 && c.Type == ProcessorCacheType.CacheInstruction)
                    {
                        return c;
                    }
                }

                return null;
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


        public CacheInfo L2Cache
        {
            get
            {
                foreach (var c in Caches)
                {
                    if (c.Level == 2) return c;
                }

                return null;
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

        public CacheInfo L3Cache
        {
            get
            {
                foreach (var c in Caches)
                {
                    if (c.Level == 3) return c;
                }

                return null;
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

        public override string Description
        {
            get => ToString();
            internal set
            {
                base.Description = value;
            }
        }

        public override string ToString()
        {
            try
            {
                string s = FriendlyName + $", Logical Processor: {LogicalProcessor}, Core: {Core}";
                if (HasL1Cache)
                {
                    s += $", L1 ({TextTools.PrintFriendlySize(L1InstructionCache.Size)})";
                }
                if (HasL2Cache)
                {
                    s += $", L2 ({TextTools.PrintFriendlySize(L2Cache.Size)})";
                }
                if (HasL3Cache)
                {
                    s += $", L3 ({TextTools.PrintFriendlySize(L3Cache.Size)})";
                }
                return s;
            }
            catch
            {

            }
            
            return base.ToString();
        }
    }
}
