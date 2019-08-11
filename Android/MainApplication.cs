using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;
using System;
using MvvmCross.Converters;
using MvvmCross.Localization;

namespace Android
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<MvxAppCompatSetup<Core.App>, Core.App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}