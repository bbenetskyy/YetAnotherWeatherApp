using API;
using Core.Services;
using Core.ViewModels;
using InteractiveAlert;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            Mvx.IoCProvider.RegisterSingleton(MapService.ConfigureMapper);
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
