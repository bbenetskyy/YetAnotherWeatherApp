using Android.App;
using Android.OS;
using InteractiveAlert;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Core;

namespace Android
{

    [Activity(MainLauncher = true, Theme = "@style/SplashScreen", NoHistory = true)]
    public class SplashScreen : MvxSplashScreenAppCompatActivity
    {
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            InteractiveAlerts.Init(() => this);

            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.EnsureInitialized();

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SplashScreen);

        }
    }
}