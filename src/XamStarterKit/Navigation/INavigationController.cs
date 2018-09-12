using System;
using Xamarin.Forms;

namespace XamStarterKit.Navigation {
    public interface INavigationController {
        INavigation GetGlobalNavigation(Page mainPage);
        INavigation GetTopNavigation(Page mainPage, object parameter = null);

        void PushPage(Page page);
        void PopPage(Page page);
        void PopAll(Page exceptPage = null);
    }
}
