using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WizLib
{
    public class Pilot : ViewModelBase, ICloneable
    {

        public static readonly ReadOnlyDictionary<string, string> BulbTypeCatalog 
            = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
            {
                { "ESP01_SHDW_01", "" },
                { "ESP01_SHRGB1C_31", "Phillips Color & Tunable-White BR30 Recessed" },
                { "ESP01_SHTW1C_31", "Phillips 555599 recessed" },
                { "ESP56_SHTW3_01", "" },
                { "ESP01_SHRGB_03", "" },
                { "ESP01_SHDW1_31", "" },
                { "ESP15_SHTW1_01I", "" },
                { "ESP03_SHRGB1C_01", "Philips Color & Tunable-White A19" },
                //{ "ESP03_SHRGB1C_01", "WiZ LED EAN 8718699787059" },
                { "ESP03_SHRGB1W_01", "Philips Color & Tunable-White A21" },
                { "ESP06_SHDW9_01", "Philips Soft White A19" },
                { "ESP03_SHRGBP_31", "Trio Leuchten WiZ LED" },
                { "ESP17_SHTW9_01", "WiZ Filament Bulb EAN 8718699786793" }
            });

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

        private int? homeId;

        private int? roomId;

        private string fwVersion;

        private string moduleName;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public Pilot Clone(bool pilotOnly)
        {
            var p = (Pilot)MemberwiseClone();
            if (!pilotOnly) return p;

            // registration params

            p.phoneMac = null;

            p.register = null;

            p.phoneIp = null;

            p.id = null;

            // results

            p.rssi = null;

            p.src = null;

            p.macaddr = null;

            p.success = null;

            p.homeId = null;

            p.roomId = null;

            p.fwVersion = null;

            p.moduleName = null;

            return p;
        }

        [JsonProperty("dimming")]
        public byte? Brightness
        {
            get => dimming;
            set
            {
                //double pctval;

                //if (value != null)
                //{
                //    pctval = ((double)value / 255) * 100;
                //    if (pctval < 10) pctval = 10;

                //    value = (byte?)pctval;
                //}

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
                OnPropertyChanged("LightModeInfo");
            }
        }

        [JsonIgnore]
        public LightMode LightModeInfo
        {
            get
            {
                if (LightMode.LightModes.ContainsKey(sceneId ?? 0))
                {
                    return LightMode.LightModes[sceneId ?? 0];
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty("r")]
        public byte? Red
        {
            get => r;
            set
            {
                if (r == value) return;
                SetProperty(ref r, value);
                OnPropertyChanged("Color");
            }
        }

        [JsonProperty("g")]
        public byte? Green
        {
            get => g;
            set
            {
                if (g == value) return;
                SetProperty(ref g, value);
                OnPropertyChanged("Color");
            }
        }

        [JsonProperty("b")]
        public byte? Blue
        {
            get => b;
            set
            {
                if (b == value) return;
                SetProperty(ref b, value);
                OnPropertyChanged("Color");
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
                    return;
                }
                
                if (r != null && g != null && b != null)
                {
                    System.Drawing.Color? cur = System.Drawing.Color.FromArgb((byte)r, (byte)g, (byte)b);
                    if (value == cur) return;
                }

                var c = (System.Drawing.Color)value;

                r = c.R;
                g = c.G;
                b = c.B;

                OnPropertyChanged("Red");
                OnPropertyChanged("Green");
                OnPropertyChanged("Blue");
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

        [JsonProperty("homeId")]
        public int? HomeId
        {
            get => homeId;
            set
            {
                SetProperty(ref homeId, value);
            }
        }


        [JsonProperty("roomId")]
        public int? RoomId
        {
            get => roomId;
            set
            {
                SetProperty(ref roomId, value);
            }
        }


        [JsonProperty("fwVersion")]
        public string FirmwareVersion
        {
            get => fwVersion;
            set
            {
                SetProperty(ref fwVersion, value);
            }
        }

        [JsonProperty("moduleName")] 
        public string ModuleName
        {
            get => moduleName;
            set
            {
                SetProperty(ref moduleName, value);
                OnPropertyChanged("TypeDescription");
            }
        }

        [JsonIgnore] 
        public string TypeDescription
        {
            get
            {
                if (BulbTypeCatalog.ContainsKey(moduleName))
                {
                    return BulbTypeCatalog[ModuleName];
                }
                else
                {
                    return moduleName;
                }
            }
        }


    }
}
