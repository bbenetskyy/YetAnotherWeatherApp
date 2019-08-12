using System;
using AutoMapper;
using Core.Models;
using Core.Resources;
using Core.Services.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;
using System.Threading.Tasks;
using System.Resources;
using System.Globalization;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel<WeatherDetails>
    {
        private readonly IMapper mapper;
        private readonly IWeatherService weatherService;
        private readonly IConnectivityService connectivity;
        private readonly IAlertService alertService;
        private readonly IMvxNavigationService navigationService;
        private readonly ResourceManager resourceManager;
        private WeatherDetails weatherDetails;

        public WeatherDetailsViewModel(
            IMapper mapper,
            IMvxNavigationService navigationService,
            IWeatherService weatherService,
            IConnectivityService connectivity,
            IAlertService alertService)
        {
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.weatherService = weatherService;
            this.connectivity = connectivity;
            this.alertService = alertService;
            resourceManager = AppResources.ResourceManager;

            RefreshWeatherCommand = new MvxAsyncCommand(RefreshWeather);

            BackCommand = new MvxAsyncCommand(NavigateToSearch);
        }

        public string this[string key] => resourceManager.GetString(key, CultureInfo.CurrentCulture);

        public string CityName => weatherDetails?.CityName;
        public string Description => weatherDetails?.Description;
        public string CurrentTemperature => weatherDetails?.CurrentTemperature;
        public string MinTemperature => weatherDetails?.MinTemperature;
        public string MaxTemperature => weatherDetails?.MaxTemperature;

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

            CurrentWeatherResponse currentWeather = null;
            ShowActivityIndicator();
            try
            {
                currentWeather = await weatherService.GetWeatherAsync(weatherDetails?.CityName);
            }
            catch (Exception ex)
            {
                alertService.Show(ex.Message, AlertType.Error);
            }

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
    }
}
