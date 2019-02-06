using API;
using Core.Resources;
using Core.Services.Interfaces;
using InteractiveAlert;
using MvvmCross;
using OpenWeatherMap;
using Plugin.Connectivity.Abstractions;
using System;
using System.Threading.Tasks;

namespace Core.Services
{
    public class WeatherAlertService : IAlertService
    {
        private readonly IApiClient apiClient;
        private readonly IConnectivity connectivity;

        public WeatherAlertService(
            IApiClient apiClient,
            IConnectivity connectivity)
        {
            this.apiClient = apiClient;
            this.connectivity = connectivity;
        }

        public async Task<CurrentWeatherResponse> GetWeatherAsync(string cityName, string errorMessage)
        {
            try
            {
                var currentWeather = await apiClient.GetWeatherByCityNameAsync(cityName);
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
                    Title = AppResources.Error,
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
            if (!connectivity.IsConnected)
            {
                var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
                var alertConfig = new InteractiveAlertConfig
                {
                    OkButton = new InteractiveActionButton(),
                    Title = AppResources.Warning,
                    Message = AppResources.CheckInternetConnection,
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
