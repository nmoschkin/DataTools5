using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib
{
    public class LightMode
    {
        public const int UserStartIdx = 2000;


        private int code;
        private string description;

        private Pilot settings;

        public int Code
        {
            get => code;
            protected set
            {
                if (code == value) return;
                code = value;
            }
        }

        public string Description
        {
            get => description;
            protected set
            {
                if (description == value) return;
                description = value;
            }

        }

        public Pilot Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
            }
        }

        protected static Dictionary<int, LightMode> modes = new Dictionary<int, LightMode>();

        public static IReadOnlyDictionary<int, LightMode> LightModes
        {
            get => modes;
        }

        protected LightMode(int code, string desc, Pilot settings = null)
        {
            Code = code;
            Description = desc;
            Settings = settings;
        }

        public static LightMode RegisterLightMode(string desc, Pilot settings)
        {
            int i = UserStartIdx;
            while (modes.ContainsKey(i))
            {
                i++;
            }
            
            var scnew = new LightMode(i, desc, settings);

            modes.Add(i, scnew);
            return scnew;

        }

        public static LightMode RegisterLightMode(int code, string desc, Pilot settings = null)
        {
            // if that code already exists, it will be reassigned and returned
            // the alternatives are to throw an exception or return null
            // and I don't want to do that.


            if (modes.ContainsKey(code))
            {
                var sc = modes[code];

                sc.description = desc;
                if (settings != null)
                {
                    sc.settings = settings;
                }
                return sc;

            }

            var scnew = new LightMode(code, desc, settings);

            modes.Add(code, scnew);
            return scnew;
        }

        public static LightMode Off { get; } = RegisterLightMode(0, "Off");
        public static LightMode Ocean { get; } = RegisterLightMode(1, "Ocean");
        public static LightMode Romance { get; } = RegisterLightMode(2, "Romance");
        public static LightMode Sunset { get; } = RegisterLightMode(3, "Sunset");
        public static LightMode Party { get; } = RegisterLightMode(4, "Party");
        public static LightMode Fireplace { get; } = RegisterLightMode(5, "Fireplace");
        public static LightMode Cozy { get; } = RegisterLightMode(6, "Cozy");
        public static LightMode Forest { get; } = RegisterLightMode(7, "Forest");
        public static LightMode PastelColors { get; } = RegisterLightMode(8, "Pastel Colors");
        public static LightMode Wakeup { get; } = RegisterLightMode(9, "Wake up");
        public static LightMode Bedtime { get; } = RegisterLightMode(10, "Bedtime");
        public static LightMode WarmWhite { get; } = RegisterLightMode(11, "Warm White");
        public static LightMode Daylight { get; } = RegisterLightMode(12, "Daylight");
        public static LightMode Coolwhite { get; } = RegisterLightMode(13, "Cool white");
        public static LightMode Nightlight { get; } = RegisterLightMode(14, "Night light");
        public static LightMode Focus { get; } = RegisterLightMode(15, "Focus");
        public static LightMode Relax { get; } = RegisterLightMode(16, "Relax");
        public static LightMode TrueColors { get; } = RegisterLightMode(17, "True colors");
        public static LightMode TVTime { get; } = RegisterLightMode(18, "TV time");
        public static LightMode PlantGrowth { get; } = RegisterLightMode(19, "Plantgrowth");
        public static LightMode Spring { get; } = RegisterLightMode(20, "Spring");
        public static LightMode Summer { get; } = RegisterLightMode(21, "Summer");
        public static LightMode Fall { get; } = RegisterLightMode(22, "Fall");
        public static LightMode DeepDive { get; } = RegisterLightMode(23, "Deepdive");
        public static LightMode Jungle { get; } = RegisterLightMode(24, "Jungle");
        public static LightMode Mojito { get; } = RegisterLightMode(25, "Mojito");
        public static LightMode Club { get; } = RegisterLightMode(26, "Club");
        public static LightMode Christmas { get; } = RegisterLightMode(27, "Christmas");
        public static LightMode Halloween { get; } = RegisterLightMode(28, "Halloween");
        public static LightMode Candlelight { get; } = RegisterLightMode(29, "Candlelight");
        public static LightMode GoldenWhite { get; } = RegisterLightMode(30, "Golden white");
        public static LightMode Pulse { get; } = RegisterLightMode(31, "Pulse");
        public static LightMode Steampunk { get; } = RegisterLightMode(32, "Steampunk");
        public static LightMode Rhythm { get; } = RegisterLightMode(1000, "Rhythm");

        public override string ToString()
        {
            return $"{code}: {description}";
        }

        public static implicit operator int(LightMode val)
        {
            return val.code;
        }

        public static implicit operator string(LightMode val)
        {
            return val.description;
        }

    }
}
