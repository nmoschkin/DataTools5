using System;
using System.Collections.Generic;
using System.Linq;
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

}
