using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WizLib
{
    public class PilotCommand : ViewModelBase
    {

        private string method = "setPilot";
        private Pilot outparam;
        private Pilot inparam;
        private string env;

        
        [JsonProperty("method")]
        public string Method
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
