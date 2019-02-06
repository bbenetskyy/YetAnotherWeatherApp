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
    public class SearchViewModel : MvxViewModel
    {
        private readonly IMapper mapper;
        private readonly IMvxNavigationService navigationService;
        private readonly IAlertService alertService;
        private readonly ILocationService locationService;

        public SearchViewModel(
            IMapper mapper,
            IMvxNavigationService navigationService,
            IAlertService alertService,
            ILocationService locationService)
        {
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.alertService = alertService;
            this.locationService = locationService;

            CheckWeatherCommand = new MvxAsyncCommand(async () =>
            {
                await CheckWeather();
            }, () => !string.IsNullOrEmpty(CityName));

            GetLocationCityNameCommand = new MvxAsyncCommand(async () =>
            {
                await GetLocationCityName();
            });
        }

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
            CityName = await locationService.GetLocationCityNameAsync();
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
            ShowActivityIndicator();
            var currentWeather = alertService.IsInternetConnection()
                ? await alertService.GetWeatherAsync(cityName, AppResources.CityNameIsIncorrect)
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
    }
}
