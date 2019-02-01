using Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;

namespace iOS.Views
{
    [MvxFromStoryboard]
    public partial class WeatherDetailsView : MvxViewController<WeatherDetailsViewModel>
    {
        public WeatherDetailsView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<WeatherDetailsView, WeatherDetailsViewModel>();

            set.Bind(cityNameLabel).To(vm => vm.CityName);
            set.Bind(descriptionLabel).To(vm => vm.Description);
            set.Bind(currentTemperatureLabel).To(vm => vm.CurrentTemperature);
            set.Bind(minTemperatureLabel).To(vm => vm.MinTemperature);
            set.Bind(maxTemperatureLabel).To(vm => vm.MaxTemperature);
            set.Bind(refreshButton).To(vm => vm.RefreshWeatherCommand);

            set.Apply();
        }
    }
}