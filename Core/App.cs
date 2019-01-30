using API;
using Core.Services;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton(MapService.ConfigureMapper);
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();

            RegisterAppStart<SearchViewModel>();
        }
    }
}
