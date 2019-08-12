using Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using Core.Resources;
using UIKit;

namespace iOS.Views
{
    [MvxFromStoryboard]
    [MvxModalPresentation]
    public partial class WeatherDetailsView : MvxViewController<WeatherDetailsViewModel>
    {
        public WeatherDetailsView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<WeatherDetailsView, WeatherDetailsViewModel>();

            set.Bind(cityNameText).To(vm => vm.CityName);
            set.Bind(descriptionText).To(vm => vm.Description);
            set.Bind(currentTemperatureText).To(vm => vm.CurrentTemperature);
            set.Bind(minTemperatureText).To(vm => vm.MinTemperature);
            set.Bind(maxTemperatureText).To(vm => vm.MaxTemperature);
            set.Bind(refreshButton).To(vm => vm.RefreshWeatherCommand);
            set.Bind(backButton).To(vm => vm.BackCommand);

            set.Bind(loadingIndicator)
                .For("Visibility")
                .To(vm => vm.IsLoading)
                .WithConversion("Visibility");
            set.Bind(refreshButton)
                .For("Visibility")
                .To(vm => vm.IsLoading)
                .WithConversion("InvertedVisibility");

            set.Bind(currentTemperatureText)
                .For("TextColor")
                .To(vm => vm.CurrentTemperature)
                .WithConversion("TemperatureToColor");
            set.Bind(minTemperatureText)
                .For("TextColor")
                .To(vm => vm.MinTemperature)
                .WithConversion("TemperatureToColor");
            set.Bind(maxTemperatureText)
                .For("TextColor")
                .To(vm => vm.MaxTemperature)
                .WithConversion("TemperatureToColor");

            refreshButton.Layer.BorderColor = UIColor.White.CGColor;

            descriptionLabel.Text = AppResources.DescriptionLabel;
            currentTemperatureLabel.Text = AppResources.TemperatureLabel;
            minTemperatureLabel.Text = AppResources.MinLabel;
            maxTemperatureLabel.Text = AppResources.MaxLabel;
            refreshButton.SetTitle(AppResources.SearchButton, UIControlState.Normal);

            set.Apply();
        }
    }
}