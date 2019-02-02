using System;
using Android.App;
using Android.Runtime;
using Android.Views;
using InteractiveAlert;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

namespace Android
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<Setup, Core.App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}