using API;
using MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private readonly IApiClient apiClient;

        public SearchViewModel(IApiClient apiClient)
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
