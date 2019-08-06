using System;
using AutoMapper;
using Core.Constants;
using Core.Models;
using Core.Resources;
using Core.Services.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.UI;
using MvvmCross.ViewModels;
using OpenWeatherMap;
using System.Threading.Tasks;
using Plugin.Connectivity.Abstractions;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel<WeatherDetails>
    {
        private readonly IMapper mapper;
        private readonly IWeatherService weatherService;
        private readonly IConnectivity connectivity;
        private readonly IAlertService alertService;
        private readonly IMvxNavigationService navigationService;
        private WeatherDetails weatherDetails;

        public WeatherDetailsViewModel(
            IMapper mapper,
            IMvxNavigationService navigationService,
            IWeatherService weatherService,
            IConnectivity connectivity,
            IAlertService alertService)
        {
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.weatherService = weatherService;
            this.connectivity = connectivity;
            this.alertService = alertService;

            RefreshWeatherCommand = new MvxAsyncCommand(async () =>
            {
                await RefreshWeather();
            });

            BackCommand = new MvxAsyncCommand(async () =>
            {
                await NavigateToSearch();
            });
        }

        public string CityName => weatherDetails?.CityName;
        public string Description => weatherDetails?.Description;
        public string CurrentTemperature => weatherDetails?.CurrentTemperature;
        public string MinTemperature => weatherDetails?.MinTemperature;
        public string MaxTemperature => weatherDetails?.MaxTemperature;
        public MvxColor CurrentTemperatureColor => GetColorByTemperature(weatherDetails?.CurrentTemperature);
        public MvxColor MinTemperatureColor => GetColorByTemperature(weatherDetails?.MinTemperature);
        public MvxColor MaxTemperatureColor => GetColorByTemperature(weatherDetails?.MaxTemperature);

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public IMvxAsyncCommand RefreshWeatherCommand { get; }
        public IMvxAsyncCommand BackCommand { get; }

        public override void Prepare(WeatherDetails parameter)
        {
            weatherDetails = parameter;
        }

        protected virtual async Task<CurrentWeatherResponse> GetWeather()
        {
            if (!connectivity.IsConnected)
            {
                alertService.Show(AppResources.CheckInternetConnection, AlertType.Warning);
                return null;
            }

            ShowActivityIndicator();
            var currentWeather = await weatherService.GetWeatherAsync(weatherDetails?.CityName, AppResources.SomethingIsWrong);
            HideActivityIndicator();
            return currentWeather;
        }

        protected virtual void ShowActivityIndicator()
        {
            IsLoading = true;
        }

        protected virtual void HideActivityIndicator()
        {
            IsLoading = false;
        }

        protected virtual async Task RefreshWeather()
        {
            var currentWeather = await GetWeather();
            if (currentWeather != null)
                await MapWeatherToProperties(currentWeather);
            else
                await NavigateToSearch();
        }

        protected virtual Task MapWeatherToProperties(CurrentWeatherResponse currentWeather)
        {
            weatherDetails = mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather);
            return RaiseAllPropertiesChanged();
        }

        protected virtual Task NavigateToSearch()
        {
            return navigationService.Navigate<SearchViewModel>();
        }

        protected virtual MvxColor GetColorByTemperature(string tempString)
        {
            if (!string.IsNullOrEmpty(tempString)
                && tempString.Contains(" ")
                && double.TryParse(
                    tempString.Substring(0, tempString.IndexOf(' ')),
                    out var tempValue))
            {
                if (tempValue <= 0)
                {
                    return Colors.Cold;
                }

                if (tempValue <= 10)
                {
                    return Colors.Chilly;
                }

                if (tempValue <= 20)
                {
                    return Colors.Warm;
                }

                return Colors.Hotly;
            }

            return Colors.Default;
        }
    }
}
