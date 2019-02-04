using Core;
using InteractiveAlert;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;

namespace iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
        }
    }
}