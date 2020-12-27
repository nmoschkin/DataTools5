using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DataTools.Graphics
{
    public class ColorHitEventArgs : EventArgs
    {
        public UniColor Color { get; private set; }

        public ColorHitEventArgs(int rawColor)
        {
            Color = rawColor;
        }

        public ColorHitEventArgs(Color color)
        {
            Color = new UniColor(color.ToArgb());
        }
    }

}
