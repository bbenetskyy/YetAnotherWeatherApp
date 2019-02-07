using Core.Constants;
using MvvmCross.Converters;
using MvvmCross.UI;
using System;
using System.Globalization;

namespace Core.Converters
{
    public class TemperatureToColorConverter : MvxValueConverter<string, MvxColor>
    {
        protected override MvxColor Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.Substring(0, value.IndexOf(' ')), out var temperature))
            {
                if (temperature <= 0)
                {
                    return Colors.Cold;
                }

                if (temperature <= 10)
                {
                    return Colors.Chilly;
                }

                if (temperature <= 20)
                {
                    return Colors.Warm;
                }

                return Colors.Hotly;
            }

            return Colors.Default;
        }
    }
}
