using DataTools.Graphics;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataTools.ColorControls
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Media.Color cc)
            {
                UniColor ac = cc.GetUniColor();
                return ac.ToString("awH");
            }
            else if (value is UniColor uc)
            {
                return uc.ToString("awH");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (targetType == typeof(UniColor))
                {
                    return UniColor.Parse(s);
                }
                else if (targetType == typeof(System.Windows.Media.Color))
                {
                    return UniColor.Parse(s).GetWPFColor();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
