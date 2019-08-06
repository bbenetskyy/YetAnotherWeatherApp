using API;
using Core.Services;
using Core.Services.Interfaces;
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
            Mvx.IoCProvider.RegisterType<IWeatherService, WeatherService>();
            Mvx.IoCProvider.RegisterType<ILocationService, LocationService>();
            Mvx.IoCProvider.RegisterType<IGeolocationService, Geolocation>();
            Mvx.IoCProvider.RegisterType<IGeocodingService, Geocoding>();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
