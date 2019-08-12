using API;
using Core.Services;
using Core.Services.Interfaces;
using MvvmCross;
using Plugin.Connectivity;

namespace Core.IoC
{
    internal class IoCRegistrar
    {
        public void RegisterServices()
        {
            Mvx.IoCProvider.RegisterType<IAlertService, AlertService>();
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();
            Mvx.IoCProvider.RegisterType<IGeocodingService, GeocodingService>();
            Mvx.IoCProvider.RegisterType<IGeolocationService, GeolocationService>();
            Mvx.IoCProvider.RegisterType<ILocationService, LocationService>();
            Mvx.IoCProvider.RegisterType<IWeatherService, WeatherService>();
        }

        public void RegisterSingletons()
        {
            Mvx.IoCProvider.RegisterSingleton(MapService.ConfigureMapper);
            Mvx.IoCProvider.RegisterSingleton(CrossConnectivity.Current);
        }
    }
}
