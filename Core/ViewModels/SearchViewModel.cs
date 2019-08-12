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
using Plugin.Connectivity.Abstractions;
using System.Resources;
using System.Globalization;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private readonly IMapper mapper;
        private readonly IMvxNavigationService navigationService;
        private readonly IWeatherService weatherService;
        private readonly ILocationService locationService;
        private readonly IConnectivity connectivity;
        private readonly IAlertService alertService;
        private readonly ResourceManager resourceManager;

        public SearchViewModel(
            IMapper mapper,
            IMvxNavigationService navigationService,
            IWeatherService weatherService,
            ILocationService locationService,
            IConnectivity connectivity,
            IAlertService alertService)
        {
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.weatherService = weatherService;
            this.locationService = locationService;
            this.connectivity = connectivity;
            this.alertService = alertService;
            resourceManager = AppResources.ResourceManager;

            CheckWeatherCommand = new MvxAsyncCommand(CheckWeather, () => !string.IsNullOrEmpty(CityName));

            GetLocationCityNameCommand = new MvxAsyncCommand(GetLocationCityName);
        }

        public string this[string key] => resourceManager.GetString(key, CultureInfo.CurrentCulture);

        private string cityName;

        public string CityName
        {
            get => cityName;
            set
            {
                if (SetProperty(ref cityName, value))
                {
                    CheckWeatherCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public IMvxAsyncCommand CheckWeatherCommand { get; }
        public IMvxAsyncCommand GetLocationCityNameCommand { get; }

        protected virtual async Task GetLocationCityName()
        {
            ShowActivityIndicator();
            try
            {
                CityName = await locationService.GetLocationCityNameAsync();
            }
            catch (Exception ex)
            {
                alertService.Show(ex.Message, AlertType.Warning);
            }
            HideActivityIndicator();
        }

        protected virtual async Task CheckWeather()
        {
            var currentWeather = await GetWeather();
            if (currentWeather != null)
                await NavigateToWeatherDetails(currentWeather);
        }

        protected virtual Task NavigateToWeatherDetails(CurrentWeatherResponse currentWeather)
        {
            return navigationService.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather));
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
                currentWeather = await weatherService.GetWeatherAsync(cityName);
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
    }
}
