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

using DataTools.Win32;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Network
{
    /// <summary>
    /// Pointerized IP_ADAPTER_DNS_SUFFIX structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_DNS_SUFFIX
    {
        public MemPtr Handle;

        public LPIP_ADAPTER_DNS_SUFFIX[] Chain
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return null;
                int c = 0;
                var mx = this;
                var ac = default(LPIP_ADAPTER_DNS_SUFFIX[]);
                do
                {
                    Array.Resize(ref ac, c + 1);
                    ac[c] = mx;
                    mx = mx.Next;
                    c += 1;
                }
                while (mx.Handle.Handle != IntPtr.Zero);
                return ac;
            }
        }

        public LPIP_ADAPTER_DNS_SUFFIX Next
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return default;
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return Struct.String;
        }

        public IP_ADAPTER_DNS_SUFFIX Struct
        {
            get
            {
                IP_ADAPTER_DNS_SUFFIX StructRet = default;
                if (Handle == IntPtr.Zero)
                    return default;
                StructRet = ToStruct();
                return StructRet;
            }
        }

        public IP_ADAPTER_DNS_SUFFIX ToStruct()
        {
            IP_ADAPTER_DNS_SUFFIX ToStructRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToStructRet = Handle.ToStruct<IP_ADAPTER_DNS_SUFFIX>();
            return ToStructRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(IntPtr operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(MemPtr operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPIP_ADAPTER_DNS_SUFFIX(IP_ADAPTER_DNS_SUFFIX operand)
        {
            var a = new LPIP_ADAPTER_DNS_SUFFIX();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_DNS_SUFFIX(LPIP_ADAPTER_DNS_SUFFIX operand)
        {
            return operand.Struct;
        }
    }
}
