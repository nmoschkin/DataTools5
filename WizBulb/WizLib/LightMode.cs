﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using WizLib.Localization;

namespace WizLib
{
    public class LightMode : IComparable<LightMode>
    {
        public const int UserStartIdx = 2000;

        protected static Dictionary<uint, string> namedColors = new Dictionary<uint, string>();

        protected static Dictionary<int, LightMode> modes = new Dictionary<int, LightMode>();

        private int code;
        private string name;

        private Pilot settings;

        private bool jsonInit = false;

        static LightMode()
        {

            var cl = new Dictionary<uint, string>();
            var craw = AppResources.ColorList.Replace("\r\n", "\n").Split("\n");

            foreach (var cen in craw)
            {
                if (string.IsNullOrEmpty(cen.Trim())) continue;
                var et = cen.Split("|");
                uint cr = uint.Parse("ff" + et[0], System.Globalization.NumberStyles.HexNumber);

                if (!cl.ContainsKey(cr)) 
                    cl.Add(cr, et[1]);

            }

            namedColors = cl;
        }

        /// <summary>
        /// Find a name for the given color.
        /// </summary>
        /// <param name="color">The color to find a name for.</param>
        /// <returns>The name of the color or null if nothing is found.</returns>
        public static string FindNameForColor(System.Drawing.Color color)
        {
            uint c = (uint)color.ToArgb();
            if (namedColors.ContainsKey(c))
            {
                return namedColors[c];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Global catalog of named colors.
        /// </summary>
        public static IReadOnlyDictionary<uint, string> NamedColors
        {
            get => namedColors;
        }

        /// <summary>
        /// Global catalog of pre-defined and user-defined lighting modes.
        /// </summary>
        public static IReadOnlyDictionary<int, LightMode> LightModes
        {
            get => modes;
        }

        [JsonConstructor]
        internal LightMode()
        {
            jsonInit = true;
        }

        /// <summary>
        /// The lighting mode code.
        /// </summary>
        [JsonProperty("code")]
        public int Code
        {
            get => code;
            protected set
            {
                if (code == value) return;
                code = value;

                if (jsonInit)
                {
                    jsonInit = false;
                    modes.Add(Code, this);
                }
            }
        }

        /// <summary>
        /// Name of the lighting mode.
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get => name;
            protected set
            {
                if (name == value) return;
                name = value;
            }

        }

        /// <summary>
        /// Customing lighting mode settings.
        /// </summary>
        [JsonProperty("settings")]
        public Pilot Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
            }
        }

        protected LightMode(int code, string name, Pilot settings = null)
        {
            Code = code;
            Name = name;
            Settings = settings;
        }

        /// <summary>
        /// Register a new custom lighting mode with an automatically assigned code (recommended).
        /// </summary>
        /// <param name="name">The name of the new lighting mode.</param>
        /// <param name="settings">The settings object used to configure the bulb.</param>
        /// <returns></returns>
        public static LightMode RegisterLightMode(string name, Pilot settings)
        {
            int i = UserStartIdx;
            while (modes.ContainsKey(i))
            {
                i++;
            }
            
            var scnew = new LightMode(i, name, settings);

            modes.Add(i, scnew);
            return scnew;

        }

        /// <summary>
        /// Register a new lighting mode as a custom saved favorite.
        /// </summary>
        /// <param name="code">The new code.  Must be >= <see cref="UserStartIdx"/></param>
        /// <param name="name">The name of the new lighting mode.</param>
        /// <param name="settings">The settings object used to configure the bulb.</param>
        /// <returns></returns>
        public static LightMode RegisterLightMode(int code, string name, Pilot settings)
            => RegisterLightMode(code, name, false, settings);
        

        protected static LightMode RegisterLightMode(int code, string name, bool builtin, Pilot settings = null)
        {
            // if that code already exists, it will be reassigned and returned
            // the alternatives are to throw an exception or return null
            // and I don't want to do that.

            if (!builtin && code < UserStartIdx) 
                throw new ArgumentOutOfRangeException(nameof(code), $"{nameof(code)} < {UserStartIdx}");

            if (modes.ContainsKey(code))
            {
                var sc = modes[code];

                sc.name = name;
                if (settings != null)
                {
                    sc.settings = settings;
                }
                return sc;

            }

            var scnew = new LightMode(code, name, settings);

            modes.Add(code, scnew);
            return scnew;
        }

        /// <summary>
        /// Adds a collection of user-defined light mode scenes.
        /// Any indexes that exist in the global light mode catalog will be overwritten.
        /// </summary>
        /// <param name="lms"></param>
        public static void AddUserLightModes(IEnumerable<LightMode> lms)
        {            
            foreach (var l in lms)
            {
                if (l.Code >= UserStartIdx)
                {
                    if (modes.ContainsKey(l.Code))
                    {
                        modes[l.Code] = l;
                    }
                    else
                    {
                        modes.Add(l.Code, l);
                    }
                }
            }
        }

        /// <summary>
        /// Returns all user-defined light modes in the global catalog.
        /// </summary>
        /// <returns></returns>
        public static List<LightMode> GetUserLightModes()
        {

            List<LightMode> ret = new List<LightMode>();

            foreach (var l in modes)
            {

                if (l.Value.Code >= UserStartIdx)
                {
                    ret.Add(l.Value);
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{code}: {name}";
        }

        /// <summary>
        /// Compare this lighting mode to another for sorting in lists.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(LightMode other)
        {
            if (name == other.name)
            {
                if (code == other.code)
                {

                    if (settings == null && other.settings != null)
                    {
                        return -1;
                    }
                    else if (settings != null && other.settings == null)
                    {
                        return 1;
                    }
                    else if (settings != null && other.settings != null)
                    {
                        if (settings.Color != null && other.settings.Color != null)
                        {
                            var c1 = (System.Drawing.Color)settings.Color;
                            var c2 = (System.Drawing.Color)other.settings.Color;

                            return c1.ToArgb() - c2.ToArgb();
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return code - other.code;
                }
            }
            else
            {
                return string.Compare(name, other.name);
            }
        }

        public static implicit operator int(LightMode val)
        {
            return val.code;
        }

        public static implicit operator string(LightMode val)
        {
            return val.name;
        }



        #region Built-In Lighting Modes
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Off { get; } = RegisterLightMode(0, "Custom", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Ocean { get; } = RegisterLightMode(1, "Ocean", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Romance { get; } = RegisterLightMode(2, "Romance", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Sunset { get; } = RegisterLightMode(3, "Sunset", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Party { get; } = RegisterLightMode(4, "Party", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Fireplace { get; } = RegisterLightMode(5, "Fireplace", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Cozy { get; } = RegisterLightMode(6, "Cozy", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Forest { get; } = RegisterLightMode(7, "Forest", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode PastelColors { get; } = RegisterLightMode(8, "Pastel Colors", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Wakeup { get; } = RegisterLightMode(9, "Wake up", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Bedtime { get; } = RegisterLightMode(10, "Bedtime", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode WarmWhite { get; } = RegisterLightMode(11, "Warm White", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Daylight { get; } = RegisterLightMode(12, "Daylight", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Coolwhite { get; } = RegisterLightMode(13, "Cool White", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Nightlight { get; } = RegisterLightMode(14, "Night Light", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Focus { get; } = RegisterLightMode(15, "Focus", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Relax { get; } = RegisterLightMode(16, "Relax", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode TrueColors { get; } = RegisterLightMode(17, "True Colors", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode TVTime { get; } = RegisterLightMode(18, "TV Time", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode PlantGrowth { get; } = RegisterLightMode(19, "Plant Growth", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Spring { get; } = RegisterLightMode(20, "Spring", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Summer { get; } = RegisterLightMode(21, "Summer", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Fall { get; } = RegisterLightMode(22, "Fall", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode DeepDive { get; } = RegisterLightMode(23, "Deep Dive", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Jungle { get; } = RegisterLightMode(24, "Jungle", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Mojito { get; } = RegisterLightMode(25, "Mojito", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Club { get; } = RegisterLightMode(26, "Club", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Christmas { get; } = RegisterLightMode(27, "Christmas", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Halloween { get; } = RegisterLightMode(28, "Halloween", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Candlelight { get; } = RegisterLightMode(29, "Candlelight", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode GoldenWhite { get; } = RegisterLightMode(30, "Golden White", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Pulse { get; } = RegisterLightMode(31, "Pulse", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Steampunk { get; } = RegisterLightMode(32, "Steam Punk", true);
        /// <summary>
        /// Built-In Lighting Mode
        /// </summary>
        public static LightMode Rhythm { get; } = RegisterLightMode(1000, "Rhythm", true);

        #endregion


    }

}
