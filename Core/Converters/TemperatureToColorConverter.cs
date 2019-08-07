using System;
using System.Globalization;
using Core.Constants;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.UI;

namespace Core.Converters
{
    public class TemperatureToColorConverter : MvxValueConverter<string, object>
    {
        private IMvxNativeColor nativeColor;
        private IMvxNativeColor NativeColor => nativeColor ?? (nativeColor = Mvx.IoCProvider.Resolve<IMvxNativeColor>());


        protected override object Convert(string tempString, Type targetType, object parameter, CultureInfo culture)
        {
            var mvxColor = Colors.Default;
            if (string.IsNullOrEmpty(tempString) || !tempString.Contains(" ") || !double.TryParse(
                    tempString.Substring(0, tempString.IndexOf(' ')),
                    out var tempValue))
            {
                return NativeColor.ToNative(mvxColor);
            }

            if (tempValue <= 0)
            {
                mvxColor = Colors.Cold;
            }
            else if (tempValue <= 10)
            {
                mvxColor = Colors.Chilly;
            }
            else if (tempValue <= 20)
            {
                mvxColor = Colors.Warm;
            }
            else
            {
                mvxColor = Colors.Hotly;
            }

            return NativeColor.ToNative(mvxColor);
        }
    }
}
