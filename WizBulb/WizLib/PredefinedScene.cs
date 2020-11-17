﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib
{
    public class PredefinedScene
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

        protected static Dictionary<int, PredefinedScene> scenes = new Dictionary<int, PredefinedScene>();
        public static IReadOnlyDictionary<int, PredefinedScene> Scenes
        {
            get => scenes;
        }

        protected PredefinedScene(int code, string desc, Pilot settings = null)
        {
            Code = code;
            Description = desc;
            Settings = settings;
        }


        public static PredefinedScene RegisterScene(string desc, Pilot settings)
        {
            int i = UserStartIdx;
            while (scenes.ContainsKey(i))
            {
                i++;
            }
            
            var scnew = new PredefinedScene(i, desc, settings);

            scenes.Add(i, scnew);
            return scnew;

        }

        public static PredefinedScene RegisterScene(int code, string desc, Pilot settings = null)
        {
            // if that code already exists, it will be reassigned and returned
            // the alternatives are to throw an exception or return null
            // and I don't want to do that.


            if (scenes.ContainsKey(code))
            {
                var sc = scenes[code];

                sc.description = desc;
                if (settings != null)
                {
                    sc.settings = settings;
                }
                return sc;

            }

            var scnew = new PredefinedScene(code, desc, settings);

            scenes.Add(code, scnew);
            return scnew;
        }

        public static PredefinedScene Off { get; } = RegisterScene(0, "Off");
        public static PredefinedScene Ocean { get; } = RegisterScene(1, "Ocean");
        public static PredefinedScene Romance { get; } = RegisterScene(2, "Romance");
        public static PredefinedScene Sunset { get; } = RegisterScene(3, "Sunset");
        public static PredefinedScene Party { get; } = RegisterScene(4, "Party");
        public static PredefinedScene Fireplace { get; } = RegisterScene(5, "Fireplace");
        public static PredefinedScene Cozy { get; } = RegisterScene(6, "Cozy");
        public static PredefinedScene Forest { get; } = RegisterScene(7, "Forest");
        public static PredefinedScene PastelColors { get; } = RegisterScene(8, "Pastel Colors");
        public static PredefinedScene Wakeup { get; } = RegisterScene(9, "Wake up");
        public static PredefinedScene Bedtime { get; } = RegisterScene(10, "Bedtime");
        public static PredefinedScene WarmWhite { get; } = RegisterScene(11, "Warm White");
        public static PredefinedScene Daylight { get; } = RegisterScene(12, "Daylight");
        public static PredefinedScene Coolwhite { get; } = RegisterScene(13, "Cool white");
        public static PredefinedScene Nightlight { get; } = RegisterScene(14, "Night light");
        public static PredefinedScene Focus { get; } = RegisterScene(15, "Focus");
        public static PredefinedScene Relax { get; } = RegisterScene(16, "Relax");
        public static PredefinedScene TrueColors { get; } = RegisterScene(17, "True colors");
        public static PredefinedScene TVTime { get; } = RegisterScene(18, "TV time");
        public static PredefinedScene PlantGrowth { get; } = RegisterScene(19, "Plantgrowth");
        public static PredefinedScene Spring { get; } = RegisterScene(20, "Spring");
        public static PredefinedScene Summer { get; } = RegisterScene(21, "Summer");
        public static PredefinedScene Fall { get; } = RegisterScene(22, "Fall");
        public static PredefinedScene DeepDive { get; } = RegisterScene(23, "Deepdive");
        public static PredefinedScene Jungle { get; } = RegisterScene(24, "Jungle");
        public static PredefinedScene Mojito { get; } = RegisterScene(25, "Mojito");
        public static PredefinedScene Club { get; } = RegisterScene(26, "Club");
        public static PredefinedScene Christmas { get; } = RegisterScene(27, "Christmas");
        public static PredefinedScene Halloween { get; } = RegisterScene(28, "Halloween");
        public static PredefinedScene Candlelight { get; } = RegisterScene(29, "Candlelight");
        public static PredefinedScene GoldenWhite { get; } = RegisterScene(30, "Golden white");
        public static PredefinedScene Pulse { get; } = RegisterScene(31, "Pulse");
        public static PredefinedScene Steampunk { get; } = RegisterScene(32, "Steampunk");
        public static PredefinedScene Rhythm { get; } = RegisterScene(1000, "Rhythm");

        public override string ToString()
        {
            return $"{code}: {description}";
        }

        public static implicit operator int(PredefinedScene val)
        {
            return val.code;
        }

        public static implicit operator string(PredefinedScene val)
        {
            return val.description;
        }

    }
}
