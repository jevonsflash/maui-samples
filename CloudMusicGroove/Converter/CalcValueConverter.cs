using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MatoMusic.Converter
{
    public class CalcValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return d+ double.Parse( (string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return -(d+(double)parameter);
        }

    }
}
