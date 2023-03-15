using System;
using System.Globalization;

namespace MSL4_Mobile.Converters;

public class PortTypeBoolConverter :IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value) ? parameter : Binding.DoNothing;
    }
}

