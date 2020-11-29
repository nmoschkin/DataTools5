using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Memory;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DataTools.Win32Api.Network
{
    public static class IfTable
    {

        public const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Unicode)]
        public static extern int GetIfTable(
         IntPtr pIfTable,
         ref int pdwSize,
         bool bOrder
       );

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Unicode)]
        public static extern int GetIfTable(
         SafePtr pIfTable,
         ref int pdwSize,
         bool bOrder
       );

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Unicode)]
        public static extern int GetIfTable(
         LPMIB_IFTABLE pIfTable,
         ref int pdwSize,
         bool bOrder
       );


        public static List<MIB_IFROW> GetIfTable()
        {

            int x, cb = 0;
            var ret = new LPMIB_IFTABLE();

            x = GetIfTable(IntPtr.Zero, ref cb, false);

            if (x != ERROR_INSUFFICIENT_BUFFER) return null;

            if (cb <= 0) return null;

            if (!ret.Alloc(cb)) return null;

            x = GetIfTable(ret, ref cb, false);

            if (x != 0) return null;

            var l = new List<MIB_IFROW>();

            foreach (var inf in ret)
            {
                l.Add(inf);
            }

            ret.Dispose();

            return l;
        }

    }
}
