using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WizLib
{

    [JsonConverter(typeof(PilotMethodJsonConverter))]
    public struct PilotMethod
    {
        string cmd;
        
        public static readonly PilotMethod GetPilot = new PilotMethod("getPilot");
        public static readonly PilotMethod SetPilot = new PilotMethod("setPilot");
        public static readonly PilotMethod SyncPilot = new PilotMethod("syncPilot");
        public static readonly PilotMethod FirstBeat = new PilotMethod("firstBeat");
        public static readonly PilotMethod GetSystemConfig = new PilotMethod("getSystemConfig");
        public static readonly PilotMethod SetSystemConfig = new PilotMethod("setSystemConfig");
        public static readonly PilotMethod Registration = new PilotMethod("registration");
        public static readonly PilotMethod Pulse = new PilotMethod("pulse");

        public PilotMethod(string s)
        {
            cmd = s;
        }

        public static implicit operator string(PilotMethod src)
            => src.cmd;
        
        public static implicit operator PilotMethod(string src) 
            => new PilotMethod(src);

        public override string ToString() => cmd;

    }

    public class PilotMethodJsonConverter : JsonConverter<PilotMethod>
    {
        public override PilotMethod ReadJson(JsonReader reader, Type objectType, [AllowNull] PilotMethod existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string s)
            {
                return s;
            }
            else
            {
                return "";
            }
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] PilotMethod value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }

    public class PilotCommand : ViewModelBase
    {

        private PilotMethod method = PilotMethod.GetPilot;
        
        private Pilot outparam;
        private Pilot inparam;
        
        private string env;

        [JsonProperty("method")]
        public PilotMethod Method
        {
            get => method;
            set
            {
                SetProperty(ref method, value);
            }
        }

        [JsonProperty("params")]
        public Pilot Params
        {
            get => outparam;
            set
            {
                SetProperty(ref outparam, value);
            }
        }

        [JsonProperty("result")]
        public Pilot Result
        {
            get => inparam;
            set
            {
                SetProperty(ref inparam, value);
            }
        }

        [JsonProperty("env")]
        public string Environment
        {
            get => env;
            set
            {
                SetProperty(ref env, value);
            }
        }

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
            try
            {
                JsonConvert.PopulateObject(json, this);

            }
            catch
            {

            }
        }

    }

}
