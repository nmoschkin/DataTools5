using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WizLib
{

    /// <summary>
    /// Bulb configuration parameters.
    /// </summary>
    public sealed class BulbParams : ViewModelBase, ICloneable
    {

        public static readonly ReadOnlyDictionary<int, string> BulbTypeCatalog
            = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>()
            {
                { 37, "Phillips Color & Tunable-White BR30" },
                { 50, "Phillips Color & Tunable-White BR30" },
                { 33, "Philips Color & Tunable-White A19" },
                { 60, "Philips Color & Tunable-White A19" },
                { 20, "Philips Color & Tunable-White A21" }
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

        private int? delta;

        private int? duration;

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

        private int[] drvConf;

        private string ewfHex;

        private int[] ewf;

        private string moduleName;

        /// <summary>
        /// Clone this paramater set to a new instance.
        /// </summary>
        /// <returns></returns>
        public object Clone() => MemberwiseClone();


        public BulbParams Clone(bool pilotOnly)
        {
            var p = (BulbParams)Clone();
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
                if (value > 100) value = 100;
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

        [JsonProperty("delta")]
        public int? Delta
        {
            get => delta;
            set
            {
                SetProperty(ref delta, value);
            }
        }

        [JsonProperty("duration")]
        public int? Duration
        {
            get => duration;
            set
            {
                SetProperty(ref duration, value);
            }
        }
                
        #region Returned Information


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

        [JsonProperty("drvConf")]
        public int[] DrvConf
        {
            get => drvConf;
            set
            {
                if (SetProperty(ref drvConf, value))
                {
                    OnPropertyChanged(nameof(DriverConfig));
                }
            }
        }

        [JsonIgnore]
        public string DriverConfig
        {
            get
            {
                if (drvConf != null && drvConf.Length == 2)
                {
                    return $"{drvConf[0]}, {drvConf[1]}";
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty("ewf")] 
        public int[] Ewf
        {
            get => ewf;
            set
            {
                SetProperty(ref ewf, value);
            }
        }

        [JsonProperty("ewfHex")]
        public string EwfHex
        {
            get => ewfHex;
            set
            {
                SetProperty(ref ewfHex, value);
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
                if (drvConf != null && drvConf.Length == 2 && BulbTypeCatalog.ContainsKey(drvConf[0]))
                {
                    return BulbTypeCatalog[drvConf[0]];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
    }

}
