using Android.App;
using Android.OS;
using Android.Widget;
using Core.Resources;
using Core.ViewModels;
using InteractiveAlert;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Android.Views
{
    [Activity(Label = "@string/weather_details_view")]
    public class WeatherDetailsView : MvxAppCompatActivity<WeatherDetailsViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.WeatherDetailsView);
            BindResources();
        }

        private void BindResources()
        {
            var descriptionLabel = FindViewById<TextView>(Resource.Id.descriptionLabel);
            descriptionLabel.Text = AppResources.DescriptionLabel;

            var currentTemperatureLabel = FindViewById<TextView>(Resource.Id.currentTemperatureLabel);
            currentTemperatureLabel.Text = AppResources.TemperatureLabel;

            var minTemperatureLabel = FindViewById<TextView>(Resource.Id.minTemperatureLabel);
            minTemperatureLabel.Text = AppResources.MinLabel;

            var maxTemperatureLabel = FindViewById<TextView>(Resource.Id.maxTemperatureLabel);
            maxTemperatureLabel.Text = AppResources.MaxLabel;

            var refreshButton = FindViewById<Button>(Resource.Id.refreshButton);
            refreshButton.Text = AppResources.RefreshButton;
        }

        protected override void OnStart()
        {
            base.OnStart();
            InteractiveAlerts.Init(() => this);
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
        }

        protected override void OnResume()
        {
            base.OnResume();
            InteractiveAlerts.Init(() => this);
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
        }
    }
}