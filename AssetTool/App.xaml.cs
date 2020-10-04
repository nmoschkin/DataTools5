using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DataTools.ExtendedMath;
using DataTools.ExtendedMath.ColorMath;

namespace AssetTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static Color ThemeLight { get; private set; }
        public static Color ThemeDark { get; private set; }
        public static Color ButtonText { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {

            Color cbase;
            
            if (Resources["ThemeBase"] is SolidColorBrush sb)
            {
                cbase = sb.Color;
            }
            else if (Resources["ThemeBase"] is Color col)
            {
                cbase = col;
            }
            
            var hsvl = new ColorStructs.HSVDATA();
            Color tc;

            ColorMath.ColorToHSV(cbase, ref hsvl);

            if (hsvl.Value < 0.5 || hsvl.Saturation > 0.5)
            {
                tc = Colors.White;
            }
            else
            {
                tc = Colors.Black;
            }

            ButtonText = tc;

            // make dark and light versions.

            var hsvd = hsvl;

            hsvl.Value += (hsvl.Value / 2);
            hsvl.Saturation -= (hsvl.Saturation / 4);

            hsvd.Value -= (hsvl.Value / 2);

            ThemeDark = ColorMath.HSVToMediaColor(hsvd);
            ThemeLight = ColorMath.HSVToMediaColor(hsvl);

            

            base.OnStartup(e);
        }

    }
}
