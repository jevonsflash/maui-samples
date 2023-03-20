using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MatoMusic.Converter
{
    internal class DecimalDigitVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var decDigitLength = (int)value;
            var digitIndex = System.Convert.ToInt32(parameter);

            return digitIndex < decDigitLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
