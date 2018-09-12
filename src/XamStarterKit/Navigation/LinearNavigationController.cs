using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamStarterKit.Navigation {
    public class LinearNavigationController : INavigationController {
        public List<Page> PagesList { get; } = new List<Page>();

        public virtual INavigation GetRootNavigation(Page mainPage) {
            return mainPage.Navigation;
        }

        public virtual INavigation GetPickNavigation(Page mainPage, object parameter = null) {
            INavigation topNavigation = null;
            if (mainPage is NavigationPage navigationPage) {
                topNavigation = navigationPage.Navigation;
            }

            if (topNavigation == null) throw new ArgumentNullException(@"MainPage should be NavigationPage");
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
