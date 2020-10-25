using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMHLR
{
    public class HardwareChangedEventArgs : EventArgs
    {

        public uint Message { get; private set; }

        public string DeviceName { get; private set; }


        internal HardwareChangedEventArgs(uint msg, string dev)
        {
            Message = msg;
            DeviceName = dev;
        }


    }
}
