using Android.App;
using Android.OS;
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
#pragma warning disable CS0436 // Type conflicts with imported type
            SetContentView(Resource.Layout.WeatherDetailsView);
#pragma warning restore CS0436 // Type conflicts with imported type
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