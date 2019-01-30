using API;
using AutoMapper;
using MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class WeatherDetailsViewModel : MvxViewModel
    {
        private readonly IMapper mapper;
        private readonly IApiClient apiClient;

        public WeatherDetailsViewModel(IApiClient apiClient, IMapper mapper)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private string currentTemperature;
        public string CurrentTemperature
        {
            get => currentTemperature;
            set => SetProperty(ref currentTemperature, value);
        }

        private string minTemperature;
        public string MinTemperature
        {
            get => minTemperature;
            set => SetProperty(ref minTemperature, value);
        }

        private string maxTemperature;
        public string MaxTemperature
        {
            get => maxTemperature;
            set => SetProperty(ref maxTemperature, value);
        }
    }
}
