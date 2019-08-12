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


        protected override object Convert(string temperature, Type targetType, object parameter, CultureInfo culture)
        {
            return NativeColor.ToNative(GetColor(temperature));
        }

        public MvxColor GetColor(string temperature)
        {
            var mvxColor = Colors.Default;
            if (string.IsNullOrEmpty(temperature) || !temperature.Contains(" ") || !double.TryParse(
                    temperature.Substring(0, temperature.IndexOf(' ')),
                    out var tempValue))
            {                //todo move it to constants
            }
            else if (tempValue <= MaxTemperature.Cold)
            {
                mvxColor = Colors.Cold;
            }
            else if (tempValue <= MaxTemperature.Chilly)
            {
                mvxColor = Colors.Chilly;
            }
            else if (tempValue <= MaxTemperature.Warm)
            {
                mvxColor = Colors.Warm;
            }
            else
            {
                mvxColor = Colors.Hotly;
            }

            return mvxColor;
        }
    }
}
