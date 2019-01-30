using API;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();

            RegisterAppStart<SearchViewModel>();
        }
    }
}
