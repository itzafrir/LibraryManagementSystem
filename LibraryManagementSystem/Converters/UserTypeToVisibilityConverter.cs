using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Converters
{
    public class UserTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserType userType)
            {
                var requiredType = (UserType)Enum.Parse(typeof(UserType), parameter.ToString());
                return userType == requiredType ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}