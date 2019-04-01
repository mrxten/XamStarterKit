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
using XamStarterKit.Extensions.FastUI;

namespace XamStarterKit.Navigation {
    public class LinearNavigationService<TAssembly> : BaseNavigationService<TAssembly> {
        public override Page SetRoot(NavigationPushInfo pushInfo) {
            RootPage = new NavigationPage(GetInitializedPage(pushInfo));
            return RootPage;
        }

        protected virtual INavigation GetRootNavigation() {
            return RootPage.Navigation;
        }

        protected virtual INavigation GetPickNavigation() {
            INavigation topNavigation = null;
            if (RootPage is NavigationPage navigationPage) {
                topNavigation = navigationPage.Navigation;
            }

            if (topNavigation == null) throw new ArgumentNullException(@"MainPage should be NavigationPage");
            return topNavigation;
        }

        protected override Task<bool> PushPopUp(NavigationPushInfo pushInfo) {
            pushInfo.OnCompletedTask?.TrySetResult(false);
            return pushInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PushCustom(NavigationPushInfo pushInfo) {
            pushInfo.OnCompletedTask?.TrySetResult(false);
            return pushInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PushRoot(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var navigation = GetRootNavigation();
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (navigation.NavigationStack.Any()) {
                        while (navigation.ModalStack.Count > 0) {
                            var page = await navigation.PopModalAsync(true);
                            DisposeModalPage(page);
                        }

                        var hasBackButton = NavigationPage.GetHasBackButton(newPage);
                        NavigationPage.SetHasBackButton(newPage, false);
                        await navigation.PushAsync(newPage);

                        foreach (var page in navigation.NavigationStack.Where(p => p != newPage).ToList()) {
                            navigation.RemovePage(page);
                        }

                        NavigationPage.SetHasBackButton(newPage, hasBackButton);
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
            var topNavigation = GetPickNavigation();
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    await topNavigation.PushAsync(newPage);
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
            var topNavigation = GetRootNavigation();

            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (pushInfo.Argument is bool modalWithNavigation && modalWithNavigation) {
                        await topNavigation.PushModalAsync(new NavigationPage(newPage));
                    }
                    else {
                        await topNavigation.PushModalAsync(newPage);
                    }

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
            popInfo.OnCompletedTask?.TrySetResult(false);
            return popInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PopCustom(NavigationPopInfo popInfo) {
            popInfo.OnCompletedTask?.TrySetResult(false);
            return popInfo.OnCompletedTask?.Task;
        }

        protected override Task<bool> PopModal(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = GetPickNavigation();
                    var page = await navigation.PopModalAsync();
                    DisposeModalPage(page);

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
                    var navigation = GetPickNavigation();
                    var page = await navigation.PopAsync();
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
                    var navigation = GetRootNavigation();
                    while (navigation.ModalStack.Count > 0) {
                        var page = await navigation.PopModalAsync(true);
                        DisposeModalPage(page);
                    }

                    await navigation.PopToRootAsync();
                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        void DisposeModalPage(Page page) {
            //bug XF
            if (page is NavigationPage navPage && navPage?.CurrentPage is IDisposable disposable)
                disposable?.Dispose();
        }
    }
}
