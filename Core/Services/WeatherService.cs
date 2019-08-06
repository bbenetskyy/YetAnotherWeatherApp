using API;
using Core.Services.Interfaces;
using OpenWeatherMap;
using System;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IApiClient apiClient;
        private readonly IAlertService alertService;

        public WeatherService(IApiClient apiClient, IAlertService alertService)
        {
            this.apiClient = apiClient;
            this.alertService = alertService;
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
                alertService.Show(errorMessage, AlertType.Error);
            }
            return null;
        }
    }
}
