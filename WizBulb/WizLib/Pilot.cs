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
        public Pilot Params { get; set; } = new Pilot();

        public string AssembleCommand()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(this, settings);
        }

    }

    public class Pilot : INotifyPropertyChanged, ICloneable
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
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

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
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

    }
}
