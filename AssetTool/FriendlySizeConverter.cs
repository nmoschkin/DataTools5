using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AssetTool
{
    public class FriendlySizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long l)
            {
                return DataTools.Strings.TextTools.PrintFriendlySize((ulong)l, null, DataTools.Strings.TextTools.FriendlyKBType.Auto, false);
            }
            else
            {
                return value?.ToString();
            } 

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
