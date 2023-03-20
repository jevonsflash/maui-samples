using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MatoMusic.Converter
{
    public class IsValid2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value != null)
            {
                if (value is string)
                {
                    result =
                    !string.IsNullOrEmpty(value as string);
                }
                else if (value is IList)
                {
                    result =
                        (value as IList).Count != 0;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return !result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
