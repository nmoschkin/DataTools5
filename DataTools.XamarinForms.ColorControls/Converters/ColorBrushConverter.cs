using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataTools.Graphics;

using Xamarin.Forms;

namespace DataTools.XamarinForms.ColorControls
{
    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (typeof(Brush).IsAssignableFrom(targetType))
            {

                if (value is Color c)
                {
                    return new SolidColorBrush(c);
                }
                else
                {
                    return null;
                }

            }
            else if (targetType == typeof(string))
            {
                if (value is Color c) 
                {
                    return c.ToHex();
                }
                else
                {
                    return value?.ToString();
                } 
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush br)
            {
                return br.Color;
            }
            else
            {
                return new Color();
            }
        }
    }
}
