using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KinectBoundingBox
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (bool)value;
            var flip = parameter != null ? bool.Parse((string)parameter) : false;

            if (!flip)
            {
                return input ? Visibility.Visible : Visibility.Collapsed;
            }
            return input ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (Visibility)value;
            var flip = parameter != null ? bool.Parse((string)parameter) : false;

            if (!flip)
            {
                return input == Visibility.Visible ? true : false;
            }
            return input == Visibility.Visible ? false : true;
        }
    }
}
