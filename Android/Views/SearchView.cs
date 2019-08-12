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
    [Activity(Label = "@string/search_view")]
    public class SearchView : MvxAppCompatActivity<SearchViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            Xamarin.Essentials.Platform.Init(this, bundle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SearchView);
            BindResources();
        }

        private void BindResources()
        {
            var searchEditText = FindViewById<EditText>(Resource.Id.searchEditText);
            searchEditText.Hint = AppResources.SearchHint;

            var searchButton = FindViewById<Button>(Resource.Id.searchButton);
            searchButton.Text = AppResources.SearchButton;

            var getLocationButton = FindViewById<Button>(Resource.Id.getLocationButton);
            getLocationButton.Text = AppResources.GetCityNameButton;
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