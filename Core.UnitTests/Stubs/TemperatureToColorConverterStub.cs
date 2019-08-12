using Core.Converters;
using MvvmCross.UI;

namespace Core.UnitTests.Stubs
{
    public class TemperatureToColorConverterStub : TemperatureToColorConverter
    {
        public new MvxColor GetColor(string temperature)
            => base.GetColor(temperature);
    }
}
