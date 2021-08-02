using System;
using System.Globalization;
using System.Windows.Data;

namespace Pamaxie.Wpf.Converters
{
    public class BooleanInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolVal)
                throw new ArgumentException("Invalid object passed into boolean inverter");

            return !boolVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
