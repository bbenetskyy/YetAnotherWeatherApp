using AutoMapper;
using Core.Models;
using Core.Resources;
using Core.Services.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel<WeatherDetails>
    {
        private readonly IMapper mapper;
        private readonly IAlertService alertService;
        private readonly IMvxNavigationService navigationService;
        private WeatherDetails weatherDetails;

        public WeatherDetailsViewModel(
            IMapper mapper,
            IMvxNavigationService navigationService,
            IAlertService alertService)
        {
            this.mapper = mapper;
            this.navigationService = navigationService;
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
            ShowActivityIndicator();
            var currentWeather = alertService.IsInternetConnection()
                ? await alertService.GetWeatherAsync(weatherDetails?.CityName, AppResources.SomethingIsWrong)
                : null;
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
