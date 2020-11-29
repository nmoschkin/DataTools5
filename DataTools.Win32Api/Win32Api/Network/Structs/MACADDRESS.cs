// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api.Network
{
    /// <summary>
    /// Represents a network adapter MAC address.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct MACADDRESS
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH)]
        public byte[] Data;

        
        public static bool operator ==(MACADDRESS val1, MACADDRESS val2)
        {
            for (int i = 0; i < IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH; i++)
            {
                if (val1.Data[i] != val2.Data[i]) return false;
            }

            return true;
        }

        public static bool operator !=(MACADDRESS val1, MACADDRESS val2)
        {
            for (int i = 0; i < IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH; i++)
            {
                if (val1.Data[i] != val2.Data[i]) return true;
            }

            return false;
        }

        public static MACADDRESS Parse(string s, string partition)
        {
            s = s.Replace(partition, "").Trim();
            var sv = s.Partition(2);

            byte[] b = new byte[IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH];

            int i, c = sv.Length - 1;
            int j = b.Length - 1;

            for (i = c; i >= 0; i--)
            {
                b[j] = byte.Parse(sv[i], System.Globalization.NumberStyles.HexNumber);
                j--;
            }

            return new MACADDRESS(b);
        }

        public static bool TryParse(string s, string partition, out MACADDRESS value)
        {
            try
            {
                value = Parse(s, partition);
                return true;
            }
            catch
            {
                value = new MACADDRESS();
                return false;
            }
        }

        public MACADDRESS(byte[] hwaddr)
        {
            int i, c = hwaddr?.Length - 1 ?? throw new ArgumentNullException(nameof(hwaddr));

            Data = new byte[IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH];
            int j = Data.Length - 1;


            for (i = c; i >= 0; i--)
            {
                Data[j] = hwaddr[i];
                j--;
            }
        }

        public static explicit operator byte[](MACADDRESS obj) => obj.Data;

        public override bool Equals(object obj)
        {
            byte[] b;

            if (obj is MACADDRESS ma)
            {
                b = ma.Data;
            }
            else if (obj is byte[])
            {
                b = (byte[])obj;
            }
            else
            {
                return false;
            }

            for (int i = 0; i < IfDefApi.MAX_ADAPTER_ADDRESS_LENGTH; i++)
            {
                if (b[i] != Data[i]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (int)Crc32.Calculate(Data);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool delineate, bool upperCase = false)
        {
            int i, c = Data?.Length ?? 0;
            if (c == 0) return null;

            StringBuilder sb = new StringBuilder();
            string fmt = upperCase ? "X2" : "x2";
           
            bool sc = true;
            
            for (i = c - 1; i >= 0; i--)
            {
                if (Data[i] != 0)
                {
                    c = i + 1;
                    break;
                }
            }

            for (i = 0; i < c; i++)
            {
                if (sc)
                {
                    if (Data[i] == 0) continue;
                    sc = false;
                }

                if (delineate)
                {
                    if (sb.Length != 0) sb.Append(':');
                }

                sb.Append(Data[i].ToString(fmt));

            }

            return sb.ToString();

        }
    }
}
