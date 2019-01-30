using API;
using AutoMapper;
using Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private readonly IMapper mapper;
        private readonly IApiClient apiClient;
        private readonly IMvxNavigationService navigationService;

        public SearchViewModel(IApiClient apiClient, IMapper mapper, IMvxNavigationService navigationService)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
            this.navigationService = navigationService;
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
                    IsLoading = true;
                    var currentWeather = await apiClient.GetWeatherByCityNameAsync(cityName);
                    await navigationService.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                        mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather));
                    IsLoading = false;
                }, () => !string.IsNullOrEmpty(CityName)));
            }
        }
    }
}
