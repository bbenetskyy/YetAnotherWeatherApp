using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        //private readonly IMapper mapper;
        //private readonly IApiClient apiClient;
        //private readonly IMvxNavigationService navigationService;
        //private readonly IInteractiveAlerts interactiveAlerts;

        public SearchViewModel()
        //IApiClient apiClient,
        //IMapper mapper,
        //IMvxNavigationService navigationService,
        //IInteractiveAlerts interactiveAlerts)
        {
            //this.mapper = mapper;
            //this.apiClient = apiClient;
            //this.navigationService = navigationService;
            //this.interactiveAlerts = interactiveAlerts;
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
                    //try
                    //{
                    //    var currentWeather = await apiClient.GetWeatherByCityNameAsync(cityName);
                    //    await navigationService.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                    //        mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather));
                    //}
                    //catch (Exception ex) when (ex is AggregateException || ex is ArgumentException)
                    //{
                    //    var alertConfig = new InteractiveAlertConfig
                    //    {
                    //        OkButton = new InteractiveActionButton(),
                    //        Title = "Error During Weather Checking",
                    //        Message = "City Name is incorrect!",
                    //        Style = InteractiveAlertStyle.Error,
                    //        IsCancellable = false
                    //    };
                    //    interactiveAlerts.ShowAlert(alertConfig);
                    //}
                    //finally
                    //{
                    //    IsLoading = false;
                    //}
                }, () => !string.IsNullOrEmpty(CityName)));
            }
        }
    }
}
