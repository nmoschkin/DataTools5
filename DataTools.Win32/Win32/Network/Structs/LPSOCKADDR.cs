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
    /// Structure that encapsulates the marshaling of a live memory pointer to either a SOCKADDR or a SOCKADDRV6
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LPSOCKADDR
    {
        public MemPtr Handle;

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
                if (Data is null)
                    return null;
                return new IPAddress(Data);
            }
        }

        public SOCKADDR IPAddrV4
        {
            get
            {
                SOCKADDR IPAddrV4Ret = default;
                if (AddressFamily == AddressFamily.AfInet6)
                    return default;
                IPAddrV4Ret = ToSockAddr();
                return IPAddrV4Ret;
            }
        }

        public SOCKADDRV6 IPAddrV6
        {
            get
            {
                SOCKADDRV6 IPAddrV6Ret = default;
                if (AddressFamily == AddressFamily.AfInet)
                    return new SOCKADDRV6();
                IPAddrV6Ret = ToSockAddr6();
                return IPAddrV6Ret;
            }
        }

        public SOCKADDR ToSockAddr()
        {
            SOCKADDR ToSockAddrRet = default;
            if (Handle == IntPtr.Zero)
                return new SOCKADDR();
            ToSockAddrRet = Handle.ToStruct<SOCKADDR>();
            return ToSockAddrRet;
        }

        public SOCKADDRV6 ToSockAddr6()
        {
            SOCKADDRV6 ToSockAddr6Ret = default;
            if (Handle == IntPtr.Zero)
                return default;
            ToSockAddr6Ret = Handle.ToStruct<SOCKADDRV6>();
            return ToSockAddr6Ret;
        }

        public void Dispose()
        {
            Handle.Free();
        }

        public AddressFamily AddressFamily
        {
            get
            {
                if (Handle.Handle == IntPtr.Zero)
                    return AddressFamily.AfUnspecified;
                return ToSockAddr().AddressFamily;
            }
        }

        public byte[] Data
        {
            get
            {
                switch (AddressFamily)
                {
                    case AddressFamily.AfInet:
                        {
                            return IPAddrV4.Data;
                        }

                    default:
                        {
                            return IPAddrV6.Data;
                        }
                }
            }
        }

        public static implicit operator LPSOCKADDR(IntPtr operand)
        {
            var a = new LPSOCKADDR();
            a.Handle = operand;
            return a;
        }

        public static implicit operator IntPtr(LPSOCKADDR operand)
        {
            return operand.Handle.Handle;
        }

        public static implicit operator LPSOCKADDR(MemPtr operand)
        {
            var a = new LPSOCKADDR();
            a.Handle = operand;
            return a;
        }

        public static implicit operator MemPtr(LPSOCKADDR operand)
        {
            return operand.Handle;
        }

        public static implicit operator LPSOCKADDR(SOCKADDR operand)
        {
            var a = new LPSOCKADDR();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator SOCKADDR(LPSOCKADDR operand)
        {
            return operand.ToSockAddr();
        }

        public static implicit operator LPSOCKADDR(SOCKADDRV6 operand)
        {
            var a = new LPSOCKADDR();
            a.Handle.Alloc(Marshal.SizeOf(operand));
            Marshal.StructureToPtr(operand, a.Handle.Handle, true);
            return a;
        }

        public static implicit operator SOCKADDRV6(LPSOCKADDR operand)
        {
            return operand.ToSockAddr6();
        }
    }
}
