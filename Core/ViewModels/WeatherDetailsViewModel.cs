using API;
using AutoMapper;
using Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel<WeatherDetails>
    {
        private readonly IMapper mapper;
        private readonly IApiClient apiClient;
        private readonly IMvxNavigationService navigationService;
        private WeatherDetails weatherDetails;

        public WeatherDetailsViewModel(IApiClient apiClient, IMapper mapper, IMvxNavigationService navigationService)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
            this.navigationService = navigationService;
        }

        public string Description => weatherDetails.Description;
        public string CurrentTemperature => weatherDetails.CurrentTemperature;
        public string MinTemperature => weatherDetails.MinTemperature;
        public string MaxTemperature => weatherDetails.MaxTemperature;

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
                    IsLoading = true;
                    var currentWeather = await apiClient.GetWeatherByCityNameAsync(weatherDetails.CityName);
                    weatherDetails = mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather);
                    IsLoading = false;
                }));
            }
        }

        public override void Prepare(WeatherDetails parameter)
        {
            weatherDetails = parameter;
        }
    }
}
