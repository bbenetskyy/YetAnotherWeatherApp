using Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;

namespace iOS.Views
{
    [MvxFromStoryboard]
    [MvxRootPresentation]
    public partial class SearchView : MvxViewController<SearchViewModel>
    {
        public SearchView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //var set = this.CreateBindingSet<SearchView, SearchViewModel>();

            //set.Bind(searchTextField).To(vm => vm.CityName);
            //set.Bind(searchButton).To(vm => vm.CheckWeatherCommand);

            //set.Apply();
        }
    }
}