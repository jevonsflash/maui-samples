using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MatoMusic.Converter
{
    public class SliderMaxValueConverter : IValueConverter
    {
        public SliderMaxValueConverter()
        {
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            if (val <= 0)
            {
                val = double.MaxValue;
            }
            return val;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}

