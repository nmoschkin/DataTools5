using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataTools.Graphics;
using DataTools.MathTools;

namespace DataTools.XamarinForms.ColorControls
{
    public static class UniColorXamarinExtensions
    {
        public static UniColor GetUniColor(this Xamarin.Forms.Color c)
        {
            UniColor clr;
            
            var hx = MathLib.NumbersOnly(c.ToHex(), hex: true);
            clr = uint.Parse(hx, System.Globalization.NumberStyles.HexNumber);
            return clr;
        }

        public static Xamarin.Forms.Color GetXamarinColor(this UniColor c)
        {

            return Xamarin.Forms.Color.FromHex(c.ToString(UniColorFormatOptions.Hex));
        }

    }
}
