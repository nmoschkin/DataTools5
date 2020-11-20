using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WizLib
{

    public class PilotCommand
    {
        [JsonProperty("method")]
        public string Method { get; set; } = "setPilot";

        [JsonProperty("params")]
        public Pilot Params { get; set; }

        [JsonProperty("result")]
        public Pilot Result { get; set; }

        [JsonProperty("env")]
        public string Environment { get; set; }



        public string AssembleCommand()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(this, settings);
        }

        public PilotCommand()
        {
            Params = new Pilot();
        }

        public PilotCommand(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

    }

    public class Pilot : INotifyPropertyChanged, ICloneable
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? state;

        private byte? r;

        private byte? g;

        private byte? b;

        private byte? w;

        private byte? c;

        private byte? speed;

        private int? sceneId;

        private int? temp;

        private byte? dimming;

        // registration params

        private string phoneMac;

        private bool? register;

        private string phoneIp;

        private string id;

        // results

        private int? rssi;

        private string src;

        private string macaddr;

        private bool? success;


        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(propertyName);
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        [JsonProperty("dimming")]
        public byte? Brightness
        {
            get => dimming;
            set
            {
                double pctval;

                if (value != null)
                {
                    pctval = ((double)value / 255) * 100;
                    if (pctval < 10) pctval = 10;

                    value = (byte?)pctval;
                }

                SetProperty(ref dimming, value);
            }
        }

        [JsonProperty("state")]
        public bool? State
        {
            get => state;
            set
            {
                SetProperty(ref state, value);
            }
        }

        [JsonProperty("speed")]
        public byte? Speed
        {
            get => speed;
            set
            {
                if (value != null)
                {
                    if (value < 1) value = 1;
                    else if (value > 100) value = 100;
                }

                SetProperty(ref speed, value);
            }
        }

        [JsonProperty("temp")]
        public int? Temperature
        {
            get => temp;
            set
            {
                if (value != null)
                {
                    if (value > 10000) value = 10000;
                    else if (value < 1000) value = 1000;
                }

                SetProperty(ref temp, value);
            }
        }

        [JsonProperty("sceneId")]
        public int? Scene
        {
            get => sceneId;
            set
            {
                SetProperty(ref sceneId, value);
            }
        }

        [JsonProperty("r")]
        public byte? Red
        {
            get => r;
            set
            {
                SetProperty(ref r, value);
            }
        }

        [JsonProperty("g")]
        public byte? Green
        {
            get => g;
            set
            {
                SetProperty(ref g, value);
            }
        }

        [JsonProperty("b")]
        public byte? Blue
        {
            get => b;
            set
            {
                SetProperty(ref b, value);
            }
        }

        [JsonIgnore]
        public System.Drawing.Color? Color
        {
            get
            {
                if (r != null && g != null & b != null)
                {
                    return System.Drawing.Color.FromArgb((byte)r, (byte)g, (byte)b);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    Red = Green = Blue = null;
                }
                else
                {
                    var c = (System.Drawing.Color)value;

                    Red = c.R;
                    Green = c.G;
                    Blue = c.B;
                }

                OnPropertyChanged();
            }
        }

        [JsonProperty("w")]
        public byte? WarmWhite
        {
            get => w;
            set
            {
                SetProperty(ref w, value);
            }
        }

        [JsonProperty("c")]
        public byte? ColdWhite
        {
            get => c;
            set
            {
                SetProperty(ref c, value);
            }
        }

        [JsonProperty("phoneMac")]
        public string PhoneMac
        {
            get => phoneMac;
            set
            {
                SetProperty(ref phoneMac, value);
            }
        }

        [JsonProperty("register")]
        public bool? Register
        {
            get => register;
            set
            {
                SetProperty(ref register, value);
            }
        }

        [JsonProperty("phoneIp")]
        public string PhoneIp
        {
            get => phoneIp;
            set
            {
                SetProperty(ref phoneIp, value);
            }
        }

        [JsonProperty("id")]
        public string ID
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
            }
        }

        [JsonProperty("rssi")]
        public int? Rssi
        {
            get => rssi;
            set
            {
                SetProperty(ref rssi, value);
            }
        }

        [JsonProperty("src")]
        public string Source
        {
            get => src;
            set
            {
                SetProperty(ref src, value);
            }
        }

        [JsonProperty("mac")]
        public string MACAddress
        {
            get => macaddr;
            set
            {
                SetProperty(ref macaddr, value);
            }
        }

        [JsonProperty("success")]
        public bool? Success
        {
            get => success;
            set
            {
                SetProperty(ref success, value);
            }
        }

    }
}
