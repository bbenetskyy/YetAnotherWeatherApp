﻿using API;
using InteractiveAlert;
using MvvmCross;
using OpenWeatherMap;
using System;
using System.Threading.Tasks;

namespace Core.Services
{
    public class WeatherAlertService : IAlertService
    {
        public async Task<CurrentWeatherResponse> GetWeather(string cityName, string errorMessage)
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
                    IsCancellable = false
                };
                interactiveAlerts.ShowAlert(alertConfig);
            }
            return null;
        }
    }
}