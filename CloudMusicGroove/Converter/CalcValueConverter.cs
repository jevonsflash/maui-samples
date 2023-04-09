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
            double compensation;
            if (double.Parse((string)parameter)>=0)
            {
                compensation=((App.Current as App).PanContainerWidth+300)/2;
            }
            else
            {
                compensation=-1.5*(App.Current as App).PanContainerWidth+300/2;
            }
            return d+compensation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
