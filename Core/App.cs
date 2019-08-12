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
            var ioc = new IoCRegistrar();
            ioc.RegisterSingletons();
            ioc.RegisterServices();
            RegisterAppStart<SearchViewModel>();
        }
    }
}
