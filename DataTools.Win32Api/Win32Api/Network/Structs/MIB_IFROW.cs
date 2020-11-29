using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Memory;
using System.Runtime.InteropServices;

namespace DataTools.Win32Api.Network
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MIB_IFROW
    {

        public const int MAX_INTERFACE_NAME_LEN = 256;
        public const int MAX_TRANSPORT_NAME_LEN = 40;
        public const int MAX_MEDIA_NAME         = 16;
        public const int MAX_PORT_NAME          = 16;
        public const int MAX_DEVICE_NAME        = 128;
        public const int MAX_PHONE_NUMBER_LEN   = 128;
        public const int MAX_DEVICETYPE_NAME    = 16;
        public const int MAXLEN_PHYSADDR = 8;

        public const int MAXLEN_IFDESCR = 256;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_INTERFACE_NAME_LEN)]
        public string wszName;
        public int dwIndex;
        public IFTYPE dwType;
        public int dwMtu;
        public int dwSpeed;
        public int dwPhysAddrLen;

        public MACADDRESS bPhysAddr;

        public int dwAdminStatus;
        public INTERNAL_IF_OPER_STATUS dwOperStatus;
        public int dwLastChange;
        public int dwInOctets;
        public int dwInUcastPkts;
        public int dwInNUcastPkts;
        public int dwInDiscards;
        public int dwInErrors;
        public int dwInUnknownProtos;
        public int dwOutOctets;
        public int dwOutUcastPkts;
        public int dwOutNUcastPkts;
        public int dwOutDiscards;
        public int dwOutErrors;
        public int dwOutQLen;
        public int dwDescrLen;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = MAXLEN_IFDESCR)]
        public byte[] bDescr;

        public string Description
        {
            get
            {
                return Encoding.ASCII.GetString(bDescr).Trim('\x0');
            }
        }

        public override string ToString()
        {
            return $"{Description} [{dwOperStatus}]";
        }
    }

}
