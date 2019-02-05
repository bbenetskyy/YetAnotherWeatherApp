using Android.App;
using Android.OS;
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
            InteractiveAlerts.Init(() => this);
            Xamarin.Essentials.Platform.Init(this, bundle);
            Mvx.IoCProvider.RegisterSingleton(InteractiveAlerts.Instance);
            base.OnCreate(bundle);
#pragma warning disable CS0436 // Type conflicts with imported type
            SetContentView(Resource.Layout.SearchView);
#pragma warning restore CS0436 // Type conflicts with imported type
        }
    }
}