using API;
using AutoMapper;
using MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private readonly IApiClient apiClient;
        private readonly IMapper mapper;

        public SearchViewModel(IApiClient apiClient, IMapper mapper)
        {
            this.mapper = mapper;
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
