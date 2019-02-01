using API;
using AutoMapper;
using Core.Models;
using InteractiveAlert;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using OpenWeatherMap;
using System;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel<WeatherDetails>
    {
        private readonly IMapper mapper;
        private readonly IApiClient apiClient;
        private readonly IMvxNavigationService navigationService;
        private readonly IInteractiveAlerts interactiveAlerts;
        private WeatherDetails weatherDetails;

        public WeatherDetailsViewModel(IApiClient apiClient,
            IMapper mapper,
            IMvxNavigationService navigationService,
            IInteractiveAlerts interactiveAlerts)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
            this.navigationService = navigationService;
            this.interactiveAlerts = interactiveAlerts;
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
                    IsLoading = true;
                    try
                    {
                        var currentWeather = await apiClient.GetWeatherByCityNameAsync(weatherDetails?.CityName);
                        weatherDetails = mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather);
                    }
                    catch (Exception ex) when (ex is AggregateException || ex is ArgumentException)
                    {
                        var alertConfig = new InteractiveAlertConfig
                        {
                            OkButton = new InteractiveActionButton(),
                            Title = "Error",
                            Message = "Something is going wrong, don't worry we will navigate you to Search again!",
                            Style = InteractiveAlertStyle.Error,
                            IsCancellable = false
                        };
                        interactiveAlerts.ShowAlert(alertConfig);
                        await navigationService.Navigate<SearchViewModel>();
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }));
            }
        }

        public override void Prepare(WeatherDetails parameter)
        {
            weatherDetails = parameter;
        }
    }
}
