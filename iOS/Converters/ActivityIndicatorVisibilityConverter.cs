using MvvmCross.Converters;
using System;
using System.Globalization;

namespace iOS.Converters
{
    public class ActivityIndicatorVisibilityConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !bool.TryParse(value?.ToString(), out var isLoading) || !isLoading;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}