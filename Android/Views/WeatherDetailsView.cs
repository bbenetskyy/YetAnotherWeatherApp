using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Android.Views
{
    [Activity(Label = "Weather Details")]
    public class WeatherDetailsView : MvxAppCompatActivity<WeatherDetailsViewModel>
    {
    }
}