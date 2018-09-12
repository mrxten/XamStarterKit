using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamStarterKit.Navigation {
    public class KitNavigationController : INavigationController {
        public List<Page> PagesList { get; } = new List<Page>();

        public virtual INavigation GetGlobalNavigation(Page mainPage) {
            return mainPage.Navigation;
        }

        public virtual INavigation GetTopNavigation(Page mainPage, object parameter = null) {
            INavigation topNavigation = null;
            if (mainPage is NavigationPage navigationPage) {
                topNavigation = navigationPage.Navigation;
            }

            if (topNavigation == null) throw new ArgumentNullException(@"MainPage should be NavigationPage");

            if (!topNavigation.ModalStack.Any(page => page is NavigationPage)) return topNavigation;

            var navPage = topNavigation.ModalStack?.LastOrDefault(item => item is NavigationPage);
            if (navPage != null) topNavigation = navPage.Navigation;

            return topNavigation;
        }

        public void PopPage(Page page) {
            PagesList.Remove(page);
            DisposePage(page);
        }

        public void PushPage(Page page) {
            PagesList.Add(page);
        }

        public void PopAll(Page exceptPage = null) {
            foreach(var page in PagesList) {
                if (page == exceptPage) 
                    continue;
                PagesList.Remove(page);
                DisposePage(page);
            }
        }

        static void DisposePage(Page page) {
            if (page?.BindingContext is IDisposable disposableVm)
                disposableVm.Dispose();

            if (page is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
