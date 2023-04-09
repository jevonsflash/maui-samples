using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MatoMusic.Converter
{
    public class SecondsToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var time = new TimeSpan();
            var result = string.Empty;
            var d = (double)value;
            if (d < 0)
            {
                return time.ToString(@"mm\:ss");
            }
            time = TimeSpan.FromSeconds(d);
            result = time.ToString(time.TotalHours >= 1.0 ? @"hh\:mm\:ss" : @"mm\:ss");
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan ts = (TimeSpan)value;
            return ts.TotalSeconds;
        }

    }
}
