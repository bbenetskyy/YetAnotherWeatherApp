using API;
using MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class WeatherViewModel : MvxViewModel
    {
        private readonly IApiClient apiClient;

        public WeatherViewModel(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        private string cityName;

        public string CityName
        {
            get => cityName;
            set => SetProperty(ref cityName, value);
        }
    }
}
