using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamStarterKit.Helpers;
using XamStarterKit.Pages;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Navigation {
    public class LinearNavigationService<TPage, TModel> : BaseNavigationService<TPage, TModel>
        where TPage : IKitPage
        where TModel : KitViewModel {
        public List<Page> PagesList { get; } = new List<Page>();

        public override Page SetRoot(NavigationPushInfo pushInfo) {
            RootPage = new NavigationPage(GetInitializedPage(pushInfo));
            return RootPage;
        }

        protected virtual INavigation GetRootNavigation(Page rootPage) {
            return rootPage.Navigation;
        }

        protected virtual INavigation GetPickNavigation(Page rootPage, object parameter = null) {
            INavigation topNavigation = null;
            if (rootPage is NavigationPage navigationPage) {
                topNavigation = navigationPage.Navigation;
            }

            if (topNavigation == null) throw new ArgumentNullException(@"MainPage should be NavigationPage");
            return topNavigation;
        }

        protected override Task<bool> PushPopUp(NavigationPushInfo pushInfo) {
            return Task.FromResult(false);
        }

        protected override Task<bool> PushCustom(NavigationPushInfo pushInfo) {
            return Task.FromResult(false);
        }

        protected override Task<bool> PushRoot(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var globalNavigation = GetRootNavigation(RootPage);
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (globalNavigation.NavigationStack.Any()) {
                        var hasBackButton = NavigationPage.GetHasBackButton(newPage);
                        NavigationPage.SetHasBackButton(newPage, false);
                        await globalNavigation.PushAsync(newPage);
                        NavigationPage.SetHasBackButton(newPage, hasBackButton);

                        PushPage(newPage);
                        foreach (var page in globalNavigation.NavigationStack.Where(p => p != newPage).ToList()) {
                            globalNavigation.RemovePage(page);
                        }
                        foreach (var page in globalNavigation.ModalStack.ToList()) {
                            globalNavigation.RemovePage(page);
                        }
                        PopAll(newPage);
                    }
                    pushInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    pushInfo.OnCompletedTask?.TrySetResult(false);
                }

            });
            return pushInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PushNormal(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var topNavigation = GetPickNavigation(RootPage);
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    await topNavigation.PushAsync(newPage);
                    PushPage(newPage);
                    pushInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    pushInfo.OnCompletedTask?.TrySetResult(false);
                }

            });
            return pushInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PushModal(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var topNavigation = GetPickNavigation(RootPage);

            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (pushInfo.Argument is bool modalWithNavigation && modalWithNavigation) {
                        await topNavigation.PushModalAsync(new NavigationPage(newPage));
                    }
                    else {
                        await topNavigation.PushModalAsync(newPage);
                    }

                    PushPage(newPage);
                    pushInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    pushInfo.OnCompletedTask?.TrySetResult(false);
                }

            });
            return pushInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PopPopUp(NavigationPopInfo popInfo) {
            return Task.FromResult(false);
        }

        protected override Task<bool> PopCustom(NavigationPopInfo popInfo) {
            return Task.FromResult(false);
        }

        protected override Task<bool> PopModal(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = GetPickNavigation(RootPage);
                    var page = await navigation.PopModalAsync();
                    if (page is NavigationPage navigationPage)
                        PopPage(navigationPage.CurrentPage);
                    else
                        PopPage(page);

                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PopNormal(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = GetPickNavigation(RootPage);
                    var page = await navigation.PopAsync();
                    PopPage(page);
                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PopRoot(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = GetRootNavigation(RootPage);
                    while (navigation.ModalStack.Count > 0)
                        await navigation.PopModalAsync();
                    await navigation.PopToRootAsync();
                    PopAll(navigation.NavigationStack.First());
                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        void PopPage(Page page) {
            PagesList.Remove(page);
            DisposePage(page);
        }

        void PushPage(Page page) {
            PagesList.Add(page);
        }

        void PopAll(Page exceptPage = null) {
            foreach (var page in PagesList) {
                if (page == exceptPage)
                    continue;
                PagesList.Remove(page);
                DisposePage(page);
            }
        }
    }
}
