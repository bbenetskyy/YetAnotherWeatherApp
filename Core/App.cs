using API;
using Core.Services;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Plugin.Connectivity;

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
            Mvx.IoCProvider.RegisterSingleton(CrossConnectivity.Current);
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();
            Mvx.IoCProvider.RegisterType<IAlertService, WeatherAlertService>();
            Mvx.IoCProvider.RegisterType<ILocationService, LocationService>();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
