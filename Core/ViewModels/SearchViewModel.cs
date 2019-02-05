using API;
using AutoMapper;
using Core.Models;
using InteractiveAlert;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;
using System;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private readonly IMapper mapper;
        private readonly IApiClient apiClient;
        private readonly IMvxNavigationService navigationService;

        public SearchViewModel(
            IApiClient apiClient,
            IMapper mapper,
            IMvxNavigationService navigationService)
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
                    var currentWeather = await GetWeather();
                    if (currentWeather != null)
                        await NavigateToWeatherDetails(currentWeather);
                }, () => !string.IsNullOrEmpty(CityName)));
            }
        }

        private Task NavigateToWeatherDetails(CurrentWeatherResponse currentWeather)
        {
            return navigationService.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather));
        }

        protected async Task<CurrentWeatherResponse> GetWeather()
        {
            IsLoading = true;
            try
            {
                var currentWeather = await apiClient.GetWeatherByCityNameAsync(cityName);
                return currentWeather;
            }
            catch (Exception ex) when (ex is AggregateException
                                       || ex is ArgumentException
                                       || ex is OpenWeatherMapException)
            {
                var interactiveAlerts = MvvmCross.Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
                var alertConfig = new InteractiveAlertConfig
                {
                    OkButton = new InteractiveActionButton(),
                    Title = "Error",
                    Message = "City name is incorrect!",
                    Style = InteractiveAlertStyle.Error,
                    IsCancellable = false
                };
                interactiveAlerts.ShowAlert(alertConfig);
                return null;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
