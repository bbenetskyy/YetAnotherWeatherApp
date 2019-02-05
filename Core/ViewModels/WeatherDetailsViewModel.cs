using AutoMapper;
using Core.Models;
using Core.Services;
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

        private IMvxAsyncCommand refreshWeatherCommand;
        public IMvxAsyncCommand RefreshWeatherCommand
        {
            get
            {
                return refreshWeatherCommand ?? (refreshWeatherCommand = new MvxAsyncCommand(async () =>
                {
                    var currentWeather = await GetWeather();
                    if (currentWeather != null)
                        await MapWeatherToProperties(currentWeather);
                    else
                        await NavigateToSearch();
                }));
            }
        }

        private IMvxAsyncCommand backCommand;
        public IMvxAsyncCommand BackCommand
        {
            get
            {
                return backCommand ?? (backCommand = new MvxAsyncCommand(async () =>
                {
                    await navigationService.Navigate<SearchViewModel>();
                }));
            }
        }

        public override void Prepare(WeatherDetails parameter)
        {
            weatherDetails = parameter;
        }

        protected async Task<CurrentWeatherResponse> GetWeather()
        {
            IsLoading = true;
            var currentWeather = await alertService.GetWeather(weatherDetails?.CityName,
                "Something is going wrong, don't worry we will navigate you to Search again!");
            IsLoading = false;
            return currentWeather;
        }

        protected async Task MapWeatherToProperties(CurrentWeatherResponse currentWeather)
        {
            weatherDetails = mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather);
            await RaiseAllPropertiesChanged();
        }

        protected Task NavigateToSearch()
        {
            return navigationService.Navigate<SearchViewModel>();
        }
    }
}
