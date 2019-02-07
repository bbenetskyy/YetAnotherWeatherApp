using Core.Converters;
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Android
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("TemperatureToColor", new TemperatureToColorConverter());
        }
    }
}