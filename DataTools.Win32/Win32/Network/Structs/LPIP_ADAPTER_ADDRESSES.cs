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
// Licensed Under the MIT License   
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
    /// Creatively marshaled pointerized structure for the IP_ADAPTER_ADDRESSES structure.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPIP_ADAPTER_ADDRESSES
    {
        public MemPtr Handle;

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return Struct.FriendlyName;
        }

        public LPIP_ADAPTER_ADDRESSES Next
        {
            get
            {
                return Struct.Next;
            }
        }

        public IP_ADAPTER_ADDRESSES Struct
        {
            get
            {
                IP_ADAPTER_ADDRESSES StructRet = default;
                StructRet = ToAdapterStruct();
                return StructRet;
            }
        }

        public IP_ADAPTER_ADDRESSES ToAdapterStruct()
        {
            IP_ADAPTER_ADDRESSES ToAdapterStructRet = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToAdapterStructRet = Handle.ToStruct<IP_ADAPTER_ADDRESSES>();
            return ToAdapterStructRet;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public static implicit operator LPIP_ADAPTER_ADDRESSES(IP_ADAPTER_ADDRESSES operand)
        {
            var a = new LPIP_ADAPTER_ADDRESSES();
            int cb = Marshal.SizeOf(a);
            a.Handle.Alloc(cb);
            Marshal.StructureToPtr(operand, a.Handle, true);
            return a;
        }

        public static implicit operator IP_ADAPTER_ADDRESSES(LPIP_ADAPTER_ADDRESSES operand)
        {
            var a = operand.Handle.ToStruct<IP_ADAPTER_ADDRESSES>();
            return a;
        }

        //public static implicit operator LPIP_ADAPTER_ADDRESSES(IntPtr operand)
        //{
        
    }//    var a = new LPIP_ADAPTER_ADDRESSES();
        //    a.Handle = operand;
        //    return a;
        //}

        //public static implicit operator IntPtr(LPIP_ADAPTER_ADDRESSES operand)
        //{
        //    return operand.Handle.Handle;
        //}

        //public static implicit operator LPIP_ADAPTER_ADDRESSES(MemPtr operand)
        //{
        //    var a = new LPIP_ADAPTER_ADDRESSES();
        //    a.Handle = operand;
        //    return a;
        //}

        //public static implicit operator MemPtr(LPIP_ADAPTER_ADDRESSES operand)
        //{
        //    return operand.Handle;
        //}
}
