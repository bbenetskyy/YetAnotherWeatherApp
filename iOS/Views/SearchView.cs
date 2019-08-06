using Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using CoreGraphics;
using UIKit;

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

            var set = this.CreateBindingSet<SearchView, SearchViewModel>();

            set.Bind(searchTextField).To(vm => vm.CityName);
            set.Bind(searchButton).To(vm => vm.CheckWeatherCommand);
            set.Bind(getLocationButton).To(vm => vm.GetLocationCityNameCommand);

            set.Bind(loadingIndicator)
                .For("Visibility")
                .To(vm => vm.IsLoading)
                .WithConversion("Visibility");
            set.Bind(searchButton)
                .For("Visibility")
                .To(vm => vm.IsLoading)
                .WithConversion("InvertedVisibility");
            set.Bind(getLocationButton)
                .For("Visibility")
                .To(vm => vm.IsLoading)
                .WithConversion("InvertedVisibility");

            searchButton.Layer.BorderColor = UIColor.White.CGColor;
            getLocationButton.Layer.BorderColor = UIColor.White.CGColor;

            set.Apply();
        }
    }
}