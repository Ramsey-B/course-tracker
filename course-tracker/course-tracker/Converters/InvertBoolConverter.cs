using System;
using System.Globalization;
using Xamarin.Forms;

namespace coursetracker.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        public InvertBoolConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                return !valueAsBool;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool valueAsBool)
            {
                return valueAsBool;
            }
            return false;
        }
    }
}
