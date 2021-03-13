using System;
using System.Collections.Generic;
using System.Text;

using DataTools.Standard.Memory;

namespace DataTools.Standard.Memory.StringBlob
{
    public class LPWSTR 
    {
        private SafePtr mem = null;

        internal IntPtr _ptr
        {
            get => (mem?.handle) ?? IntPtr.Zero;
            set
            {
                if (value == (mem?.handle ?? IntPtr.Zero)) return;

                if (mem != null) 
                {
                    mem.Dispose();
                }

                mem = new SafePtr(value);
            }
        }

        public LPWSTR()
        {
            mem = new SafePtr();
        }

        public int Length
        {
            get => (int)mem.Length - sizeof(char);
        }


        public LPWSTR(string s)
        {
            mem = new SafePtr((s.Length + 1) * sizeof(char));
            mem.SetString(0, s);
        }

        public LPWSTR(IntPtr m)
        {
            mem = new SafePtr(m);
        }

        public unsafe LPWSTR(void *m)
        {
            mem = new SafePtr(m);
        }


        public override string ToString()
        {
            return mem?.ToString();
        }

        public static explicit operator string(LPWSTR val)
        {
            return val.mem.ToString();
        }


        public static explicit operator LPWSTR(string val)
        {
            return new LPWSTR(val);
        }

    }


}
