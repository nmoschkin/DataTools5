using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.Network
{
    public enum INTERNAL_IF_OPER_STATUS
    {
        IF_OPER_STATUS_NON_OPERATIONAL = 0,
        IF_OPER_STATUS_UNREACHABLE = 1,
        IF_OPER_STATUS_DISCONNECTED = 2,
        IF_OPER_STATUS_CONNECTING = 3,
        IF_OPER_STATUS_CONNECTED = 4,
        IF_OPER_STATUS_OPERATIONAL = 5,
    }
}
