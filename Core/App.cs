using Core.IoC;
using Core.ViewModels;
using MvvmCross.ViewModels;

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
