using DataTools.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI;

namespace DataTools.WinUI.ColorControls
{
    public static class UniColorWinUIExtension
    {


        public static UniColor GetUniColor(this Color color)
        {
            return new UniColor(color.A, color.R, color.G, color.B);
        }


        public static Color GetWinUIColor(this UniColor color)
        {
            return new Color()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }


    }
}
