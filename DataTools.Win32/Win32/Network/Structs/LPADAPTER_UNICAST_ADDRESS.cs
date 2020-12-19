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
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

using DataTools.Win32;
using DataTools.Win32.Memory;

namespace DataTools.Win32.Network
{
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPADAPTER_UNICAST_ADDRESS
    {
        private MemPtr Handle;

        public ADAPTER_UNICAST_ADDRESS[] AddressChain
        {
            get
            {
                var mx = this;                

                List<ADAPTER_UNICAST_ADDRESS> ac = new List<ADAPTER_UNICAST_ADDRESS>();

                while(true)
                {
                    if (mx.Handle == MemPtr.Empty) break;

                    ac.Add(mx.Struct);
                    mx = mx.Next;
                }               

                return ac.ToArray();
            }
        }

        public LPADAPTER_UNICAST_ADDRESS Next
        {
            get
            {
                return Struct.Next;
            }
        }

        public override string ToString()
        {
            if (Handle.Handle == IntPtr.Zero)
                return "NULL";
            return "" + IPAddress.ToString() + " (" + AddressFamily.ToString() + ")";
        }

        public IPAddress IPAddress
        {
            get
            {
                return Struct.Address.lpSockaddr.IPAddress;
            }
        }

        public ADAPTER_UNICAST_ADDRESS Struct
        {
            get
            {
                if (Handle == MemPtr.Empty)
                    return new ADAPTER_UNICAST_ADDRESS();

                try
                {
                    var a = Handle.ToStruct<ADAPTER_UNICAST_ADDRESS>();
                    return a;
                }
                catch
                {
                    return new ADAPTER_UNICAST_ADDRESS();
                }
            }
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                return Struct.Address.lpSockaddr.AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                return Struct.Address.lpSockaddr.Data;
            }
        }

        //public static implicit operator LPADAPTER_UNICAST_ADDRESS(IntPtr operand)
        //{
        //    var a = new LPADAPTER_UNICAST_ADDRESS();
        //    a.Handle = operand;
        //    return a;
        //}

        //public static implicit operator IntPtr(LPADAPTER_UNICAST_ADDRESS operand)
        //{
        //    return operand.Handle;
        //}

        //public static implicit operator LPADAPTER_UNICAST_ADDRESS(MemPtr operand)
        //{
        //    var a = new LPADAPTER_UNICAST_ADDRESS();
        //    a.Handle = operand;
        //    return a;
        //}

        //public static implicit operator MemPtr(LPADAPTER_UNICAST_ADDRESS operand)
        //{
        //    return operand.Handle;
        //}

        //public static implicit operator LPADAPTER_UNICAST_ADDRESS(ADAPTER_UNICAST_ADDRESS operand)
        //{
        //    var a = new LPADAPTER_UNICAST_ADDRESS();
        //    a.Handle = new MemPtr(Marshal.SizeOf(operand));

        //    Marshal.StructureToPtr(operand, a.Handle, true);

        //    return a;
        //}

        //public static implicit operator ADAPTER_UNICAST_ADDRESS(LPADAPTER_UNICAST_ADDRESS operand)
        //{
        //    return operand.ToAddress();
        //}
    }
}
