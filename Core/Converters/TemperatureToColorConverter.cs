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
            {                
            } else if (tempValue <= 0)
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

            return mvxColor;
        }
    }
}
