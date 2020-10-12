using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Hardware.Disk.Partitions
{
    /// <summary>
    /// Prints a friendly hexadecimal Mbr partition byte code.
    /// </summary>
    /// <remarks></remarks>
    public struct FriendlyPartitionId 
    {
        public byte Value;

        public FriendlyPartitionId(byte v)
        {
            Value = v;
        }

        public override string ToString()
        {
            return Value.ToString("X2") + "h";
        }

        public static implicit operator FriendlyPartitionId(byte operand)
        {
            return new FriendlyPartitionId(operand);
        }

        public static implicit operator byte(FriendlyPartitionId operand)
        {
            return operand.Value;
        }

        public static explicit operator string(FriendlyPartitionId operand)
        {
            return operand.ToString();
        }
    }

}
