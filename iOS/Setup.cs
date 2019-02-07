using Core;
using InteractiveAlert;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Color;
using MvvmCross.Plugin.Visibility;

namespace iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Visibility", new MvxVisibilityValueConverter());
            registry.AddOrOverwrite("NativeColor", new MvxNativeColorValueConverter());
        }
    }
}