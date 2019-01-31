using Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace iOS.Views
{
    [MvxFromStoryboard("SearchView")]
    [MvxRootPresentation]
    public partial class SearchView : MvxViewController<SearchViewModel>
    {
        public SearchView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<SearchView, SearchViewModel>();

            set.Bind(searchTextField).To(vm => vm.CityName);
            set.Bind(searchButton).To(vm => vm.CheckWeatherCommand);

            set.Apply();
        }
    }
}