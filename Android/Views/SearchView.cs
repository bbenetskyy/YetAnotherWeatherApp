using Android.App;
using Android.OS;
using Core.ViewModels;
using InteractiveAlert;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Android.Views
{
    [Activity(Label = "Search", MainLauncher = true)]
    public class SearchView : MvxAppCompatActivity<SearchViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            InteractiveAlerts.Init(() => this);
            base.OnCreate(bundle);
#pragma warning disable CS0436 // Type conflicts with imported type
            SetContentView(Resource.Layout.SearchView);
#pragma warning restore CS0436 // Type conflicts with imported type
        }
    }
}