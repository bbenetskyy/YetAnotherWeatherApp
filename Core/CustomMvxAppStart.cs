using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Core
{
    public class CustomMvxAppStart<TViewModel> : MvxAppStart<TViewModel>
        where TViewModel : IMvxViewModel
    {
        public CustomMvxAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint)
        {
            await NavigationService.Navigate<TViewModel>();
        }
    }
}
