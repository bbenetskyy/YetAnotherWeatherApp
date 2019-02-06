using API;
using InteractiveAlert;
using MvvmCross;
using OpenWeatherMap;
using Plugin.Connectivity.Abstractions;
using System;
using System.Threading.Tasks;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class WeatherAlertService : IAlertService
    {
        public async Task<CurrentWeatherResponse> GetWeatherAsync(string cityName, string errorMessage)
        {
            try
            {
                var currentWeather = await Mvx.IoCProvider.Resolve<IApiClient>().GetWeatherByCityNameAsync(cityName);
                return currentWeather;
            }
            catch (Exception ex) when (ex is AggregateException
                                       || ex is ArgumentException
                                       || ex is OpenWeatherMapException)
            {
                var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
                var alertConfig = new InteractiveAlertConfig
                {
                    OkButton = new InteractiveActionButton(),
                    Title = "Error",
                    Message = errorMessage,
                    Style = InteractiveAlertStyle.Error,
                    IsCancellable = true
                };
                interactiveAlerts.ShowAlert(alertConfig);
            }
            return null;
        }

        public bool IsInternetConnection()
        {
            var crossConnectivity = Mvx.IoCProvider.Resolve<IConnectivity>();
            if (!crossConnectivity.IsConnected)
            {
                var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
                var alertConfig = new InteractiveAlertConfig
                {
                    OkButton = new InteractiveActionButton(),
                    Title = "Warning",
                    Message = "Please check your internet connection",
                    Style = InteractiveAlertStyle.Warning,
                    IsCancellable = true
                };
                interactiveAlerts.ShowAlert(alertConfig);
                return false;
            }
            return true;
        }
    }
}
