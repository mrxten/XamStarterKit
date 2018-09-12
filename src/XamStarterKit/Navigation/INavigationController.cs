using System;
using Xamarin.Forms;

namespace XamStarterKit.Navigation {
    public interface INavigationController {
        INavigation GetRootNavigation(Page mainPage);
        INavigation GetPickNavigation(Page mainPage, object parameter = null);

        void PushPage(Page page);
        void PopPage(Page page);
        void PopAll(Page exceptPage = null);
    }
}
