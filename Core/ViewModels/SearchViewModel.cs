using AutoMapper;
using Core.Models;
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

        private IMvxAsyncCommand checkWeatherCommand;
        public IMvxAsyncCommand CheckWeatherCommand
        {
            get
            {
                return checkWeatherCommand ?? (checkWeatherCommand = new MvxAsyncCommand(async () =>
                {
                    var currentWeather = await GetWeather();
                    if (currentWeather != null)
                        NavigateToWeatherDetails(currentWeather);
                }, () => !string.IsNullOrEmpty(CityName)));
            }
        }

        private IMvxAsyncCommand getLocationCityNameCommand;
        public IMvxAsyncCommand GetLocationCityNameCommand
        {
            get
            {
                return getLocationCityNameCommand ?? (getLocationCityNameCommand = new MvxAsyncCommand(async () =>
                {
                    await GetLocationCityName();
                }));
            }
        }

        protected virtual async Task GetLocationCityName()
        {
            IsLoading = true;
            CityName = await locationService.GetLocationCityNameAsync();
            IsLoading = false;
        }

        protected virtual void NavigateToWeatherDetails(CurrentWeatherResponse currentWeather)
        {
            navigationService.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather));
        }

        protected virtual async Task<CurrentWeatherResponse> GetWeather()
        {
            IsLoading = true;
            var currentWeather = alertService.IsInternetConnection()
                ? await alertService.GetWeatherAsync(cityName, "City name is incorrect!")
                : null;
            IsLoading = false;
            return currentWeather;
        }
    }
}
