using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LibraryManagementSystem.Utilities
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = (bool)value;
            bool inverse = (parameter != null) ? bool.Parse((string)parameter) : false;
            return (flag != inverse) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
    }
}