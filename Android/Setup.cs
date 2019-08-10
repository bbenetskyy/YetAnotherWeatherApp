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
using MvvmCross.Converters;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Localization;

namespace Android
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        //protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        //{
        //    base.FillValueConverters(registry);
        //    registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        //}
    }
}