using DataTools.Hardware.Disk;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static DataTools.Text.TextTools;

namespace SysInfoTool.Converters
{
    public class DiskToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DiskDeviceInfo ddi)
            {
                if (ddi.IsVolume)
                {
                    return $"{PrintFriendlySize(ddi.Size)} ({PrintFriendlySize(ddi.SizeFree)} free)";
                }
                else
                {
                    return $"{PrintFriendlySize(ddi.Size)}";
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
