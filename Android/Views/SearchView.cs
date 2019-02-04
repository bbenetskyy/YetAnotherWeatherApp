using Android.App;
using Android.OS;
using Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Android.Views
{
    [Activity(Label = "@string/search_view")]
    public class SearchView : MvxAppCompatActivity<SearchViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
#pragma warning disable CS0436 // Type conflicts with imported type
            SetContentView(Resource.Layout.SearchView);
#pragma warning restore CS0436 // Type conflicts with imported type
        }
    }
}