using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCT
{
    public enum MemoryType
    {
        Invalid = 0,
        HGlobal = 1,
        Aligned = 2,
        Network = 3,
        CoTaskMem = 4,
        Virtual = 5
    }
}
