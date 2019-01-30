using API;
using AutoMapper;
using Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

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

        public override void Prepare(WeatherDetails parameter)
        {
            weatherDetails = parameter;
        }
    }
}
