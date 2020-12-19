using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CTL_CODE
    {
        public uint Value;

        public uint DeviceType
        {
            get
            {
                return (uint)(Value & 0xFFFF0000L) >> 16;
            }
        }

        public uint Method
        {
            get
            {
                return Value & 3;
            }
        }

        public CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            Value = DeviceType << 16 | Access << 14 | Function << 2 | Method;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static explicit operator CTL_CODE(uint operand)
        {
            CTL_CODE c;
            c.Value = operand;
            return c;
        }

        public static implicit operator uint(CTL_CODE operand)
        {
            return operand.Value;
        }
    }
}
