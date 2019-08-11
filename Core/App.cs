using Core.IoC;
using Core.Services;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Plugin.Connectivity;

namespace Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            //todo move it to IoCRegistrar and add unit tests
            Mvx.IoCProvider.RegisterSingleton(MapService.ConfigureMapper);
            Mvx.IoCProvider.RegisterSingleton(CrossConnectivity.Current);
            new IoCRegistrar().RegisterServices();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
