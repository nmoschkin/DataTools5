using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataTools.Desktop;
using DataTools.Desktop.Unified;
using DataTools.Text;
using DataTools.Win32Api;

using static DataTools.MathTools.BinarySearch;
using static DataTools.MathTools.QuickSort;

namespace DataTools.ColorControls
{

    public class NamedColor
    {
        private static NamedColor[] catalog;

        private UniColor color;

        private string name;

        private string extraInfo;

        public static NamedColor FindColor(UniColor value, out int index)
        {
            NamedColor r;
            index = Search(catalog, value, "Color", out r, true);

            return r;
        }

        public static NamedColor FindColor(UniColor value, bool closest = false)
        {
            var r = FindColor(value, out _);

            if (r == null && closest)
            {
                r = GetClosestColor(value);
            }

            return r;
        }


        public static NamedColor GetClosestColor(UniColor value)
        {
            var hsv1 = new HSVDATA();

            ColorMath.ColorToHSV(value, ref hsv1);

            NamedColor closest = null;
            
            double lhue = 360;
            double lsat = 1;
            double lval = 1;

            foreach (var vl in catalog)
            {
                var hsv2 = new HSVDATA();

                ColorMath.ColorToHSV(vl.Color, ref hsv2);

                if (Math.Abs(hsv2.Hue - hsv1.Hue) <= lhue &&
                    Math.Abs(hsv2.Value - hsv1.Value) <= lval &&
                    Math.Abs(hsv2.Saturation - hsv1.Saturation) <= lsat)
                {
                    lhue = Math.Abs(hsv2.Hue - hsv1.Hue);
                    lsat = Math.Abs(hsv2.Saturation - hsv1.Saturation);
                    lval = Math.Abs(hsv2.Value - hsv1.Value);

                    closest = vl;
                }
            }

            return closest;
        }

        public static List<NamedColor> SearchByName(string search, bool anywhere = false)
        {
            List<NamedColor> output = new List<NamedColor>();

            foreach (var nc in catalog)
            {
                if (anywhere)
                {
                    if (nc.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase))
                    {
                        output.Add(nc);
                    }
                }
                else
                {
                    if (nc.Name.StartsWith(search, StringComparison.InvariantCultureIgnoreCase))
                    {
                        output.Add(nc);
                    }
                }
            }

            return output;
        }

        public static List<NamedColor> SearchByExtra(string search, bool anywhere = false)
        {
            List<NamedColor> output = new List<NamedColor>();

            foreach (var nc in catalog)
            {
                if (string.IsNullOrEmpty(nc.ExtraInfo)) continue;

                if (anywhere)
                {
                    if (nc.ExtraInfo.Contains(search, StringComparison.InvariantCultureIgnoreCase))
                    {
                        output.Add(nc);
                    }
                }
                else
                {
                    if (nc.ExtraInfo.StartsWith(search, StringComparison.InvariantCultureIgnoreCase))
                    {
                        output.Add(nc);
                    }
                }
            }

            return output;
        }

        public static IReadOnlyList<NamedColor> Catalog
        {
            get
            {
                return catalog;
                //return new ReadOnlyCollection<NamedColor>(catalog);
            }
        }

        static NamedColor()
        {

            var cl = new List<NamedColor>();
            var craw = AppResources.ColorList.Replace("\r\n", "\n").Split("\n");

            foreach (var cen in craw)
            {
                if (string.IsNullOrEmpty(cen.Trim())) continue;
                var et = cen.Split("|");
                UniColor cr = uint.Parse("ff" + et[0], System.Globalization.NumberStyles.HexNumber);

                string text = TextTools.TitleCase(et[1], false).Replace("'S", "'s");
                string extra = null;

                int x = text.IndexOf("(");
                if (x != -1)
                {
                    et = text.Split("(");
                    text = et[0].Trim();
                    extra = et[1].Trim().Trim(')');
                }

                cl.Add(new NamedColor(text, cr, extra));
            }

            catalog = cl.ToArray();
            Sort(ref catalog, (a, b) => a.Color.CompareTo(b.Color));

            //var pantone = SearchByExtra("Pantone");
            //var crayola = SearchByExtra("Crayola");
            //var web = SearchByExtra("Web");
        }

        public UniColor Color
        {
            get => color;
            private set
            {
                color = value;
            }
        }

        public string Name
        {
            get => name;
            private set
            {
                name = value;
            }
        }

        public string ExtraInfo
        {
            get => extraInfo;
            set
            {
                extraInfo = value;
            }
        }

        public override string ToString()
        {
            if (extraInfo != null)
            {
                return $"{name} ({extraInfo})";
            }
            else
            {
                return name;
            }
        }

        public NamedColor(string name, UniColor color, string extra = null)
        {
            Name = name;
            Color = color;
            ExtraInfo = extra;
        }
    }
}
