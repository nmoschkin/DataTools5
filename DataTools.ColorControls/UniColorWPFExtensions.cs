using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphics
{
    public static class UniColorWPFExtensions
    {
        public static UniColor GetUniColor(this System.Windows.Media.Color c)
        {
            UniColor clr = new UniColor();

            clr.A = c.A;
            clr.R = c.R;
            clr.G = c.G;
            clr.B = c.B;

            return clr;
        }

        public static System.Windows.Media.Color GetWPFColor(this UniColor c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

    }
}
