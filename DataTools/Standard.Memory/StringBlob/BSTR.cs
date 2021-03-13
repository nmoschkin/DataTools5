using System;
using System.Collections.Generic;
using System.Text;

namespace DataTools.Standard.Memory.StringBlob
{
    public class BSTR
    {
        private SafePtr mem = null;

        internal IntPtr _ptr
        {
            get => (mem?.handle + 4) ?? IntPtr.Zero;
            set
            {
                if ((value - 4) == (mem?.handle ?? IntPtr.Zero)) return;

                if (mem != null)
                {
                    mem.Dispose();
                }

                mem = new SafePtr(value - 4);
            }
        }

        public BSTR()
        {
            mem = new SafePtr();
        }

        public int Length
        {
            get => (int)mem.Length - sizeof(char);
        }

        public BSTR(string s)
        {
            mem = new SafePtr((s.Length + 1) * sizeof(char));
            mem.SetString(0, s);
        }

        public void SetValue(string s)
        {
            if (mem == null)
                mem = new SafePtr((s.Length * sizeof(char)) + sizeof(int));
            else
                mem.Length = (s.Length * sizeof(char)) + sizeof(int);

            mem.IntAt(0) = (s.Length * sizeof(char));

            mem.SetString(sizeof(int), s, false);
        }

        public string GetValue()
        {
            if (mem == null || mem.Length == 0L) return null;

            var i = mem.IntAt(0) / 2;

            if (i <= 0) return null;

            return mem.GetString(sizeof(int), i);


        }

        public BSTR(IntPtr m)
        {
            mem = new SafePtr(m);
        }

        public unsafe BSTR(void* m)
        {
            mem = new SafePtr(m);
        }

        public override string ToString()
        {
            return GetValue();
        }

        public static explicit operator string(BSTR val)
        {
            return val.GetValue();
        }


        public static explicit operator BSTR(string val)
        {
            return new BSTR(val);
        }

    }
}
