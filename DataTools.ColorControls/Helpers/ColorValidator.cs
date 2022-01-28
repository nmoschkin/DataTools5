using DataTools.Graphics;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataTools.ColorControls
{
    public class ColorValidator : ValidationRule
    {

        public bool ValidateSatVal { get; set; }

        public bool ValidateHue { get; set; }

        public bool ValidateWebHex { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ValidateSatVal || ValidateHue)
            {
                if (!double.TryParse(value.ToString(), NumberStyles.Any, cultureInfo, out double result))
                {
                    return new ValidationResult(false, "Does Not Parse As Double");
                }
                else if (ValidateSatVal && (result < 0d || result > 1))
                {
                    return new ValidationResult(false, "Is Not Within Range 0 to 1");
                }
                else if (ValidateHue && (result < 0d || result > 360d))
                {
                    return new ValidationResult(false, "Is Not Within Range 0 to 360");
                }
            }
            else if (ValidateWebHex)
            {
                try
                {
                    UniColor.Parse(value.ToString());
                }
                catch
                {
                    return new ValidationResult(false, "Is Not Parseable Color");
                }
            }
            else
            {
                if (!byte.TryParse(value.ToString(), NumberStyles.Any, cultureInfo, out _))
                {
                    return new ValidationResult(false, "Does Not Parse As Byte");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
