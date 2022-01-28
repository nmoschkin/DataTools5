using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Graphics
{
    public static class UniColorWPFExtensions
    {
        public static UniColor GetUniColor(this System.Windows.Media.Color c)
        {
            UniColor clr = new UniColor();
            clr.SetValue(c.A, c.R, c.G, c.B);

            return clr;
        }

        /// <summary>
        /// Gets the WPF <see cref="System.Windows.Media.Color"/> structure for this <see cref="UniColor"/> structure.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>A WPF color structure.</returns>
        public static System.Windows.Media.Color GetWPFColor(this UniColor c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Sets the value of this <see cref="UniColor"/> structure to the color value provided by the WPF <see cref="System.Windows.Media.Color"/> structure.
        /// </summary>
        /// <param name="color">The color to set.</param>
        public static void SetValue(this UniColor unicolor, System.Windows.Media.Color color)
        {
            unicolor.SetValue(color.A, color.R, color.G, color.B);
        }

    }
}
