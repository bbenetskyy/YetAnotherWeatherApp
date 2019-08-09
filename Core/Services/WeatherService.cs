using API;
using Core.Services.Interfaces;
using OpenWeatherMap;
using System;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Core.Resources;
using MvvmCross.Logging;

namespace Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IMvxLog logger;
        private readonly IApiClient apiClient;

        public WeatherService(IMvxLog logger, IApiClient apiClient)
        {
            this.logger = logger;
            this.apiClient = apiClient;
        }

        public async Task<CurrentWeatherResponse> GetWeatherAsync(string cityName)
        {
            try
            {
                return await apiClient.GetWeatherByCityNameAsync(cityName);
            }
            catch (Exception ex) when (ex is AggregateException
                                       || ex is ArgumentException
                                       || ex is OpenWeatherMapException)
            {
                throw new WeatherException(AppResources.CityNameIsIncorrect, ex);
            }
            catch (Exception ex)
            {
                logger.Log(MvxLogLevel.Error, () => ex.Message, ex);
                throw new WeatherException(AppResources.SomethingIsWrong, ex);
            }
        }
    }
}
