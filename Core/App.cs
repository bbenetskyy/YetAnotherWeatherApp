using API;
using Core.IoC;
using Core.Services;
using Core.Services.Interfaces;
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
            Mvx.IoCProvider.RegisterSingleton(MapService.ConfigureMapper);
            Mvx.IoCProvider.RegisterSingleton(CrossConnectivity.Current);
            new IoCRegistrar().RegisterServices();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
