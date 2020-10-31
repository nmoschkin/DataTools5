using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if X64
namespace MMHLR64
#else
namespace MMHLR32
#endif
{
    public class HardwareChangedEventArgs : EventArgs
    {
        protected uint msg;

        protected string deviceName;

        [JsonProperty("msg")]
        public uint Message
        {
            get => msg;
            protected set
            {
                msg = value;
            }
        }

        [JsonProperty("deviceName")]
        public string DeviceName
        {
            get => deviceName;
            protected set
            {
                deviceName = value;
            }
        }

        [JsonConstructor()]
        protected HardwareChangedEventArgs() { }

        internal HardwareChangedEventArgs(uint msg, string dev)
        {
            Message = msg;
            DeviceName = dev;
        }


    }
}
