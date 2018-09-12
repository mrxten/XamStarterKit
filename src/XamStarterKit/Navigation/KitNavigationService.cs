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
    public class KitNavigationService<TPage, TModel>
        where TPage : IKitPage
        where TModel : KitViewModel {
        protected INavigationController Controller { get; set; }
        protected Dictionary<string, Type> PageTypes { get; set; }
        protected Dictionary<string, Type> ViewModelTypes { get; set; }
        protected Page RootPage { get; set; }

        public KitNavigationService(INavigationController controller) {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));

            PageTypes = GetAssemblyPageTypes();
            ViewModelTypes = GetAssemblyViewModelTypes();
        }

        public virtual Page SetRoot(NavigationPushInfo pushInfo) {
            Controller.PopAll();
            RootPage = new NavigationPage(GetInitializedPage(pushInfo));
            return RootPage;
        }

        public virtual Task Push(NavigationPushInfo pushInfo) {
            switch (pushInfo.Mode) {
                case NavigationMode.Normal:
                    return PushNormal(pushInfo);
                case NavigationMode.Modal:
                    return PushModal(pushInfo);
                case NavigationMode.Custom:
                    return PushCustom(pushInfo);
                case NavigationMode.Root:
                    return PushRoot(pushInfo);
                case NavigationMode.PopUp:
                    return PushPopUp(pushInfo);
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual Task<bool> PushPopUp(NavigationPushInfo pushInfo) {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> PushCustom(NavigationPushInfo pushInfo) {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> PushRoot(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var globalNavigation = Controller.GetRootNavigation(RootPage);
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (globalNavigation.NavigationStack.Any()) {
                        var hasBackButton = NavigationPage.GetHasBackButton(newPage);
                        NavigationPage.SetHasBackButton(newPage, false);
                        await globalNavigation.PushAsync(newPage);
                        NavigationPage.SetHasBackButton(newPage, hasBackButton);

                        Controller.PushPage(newPage);
                        foreach (var page in globalNavigation.NavigationStack.Where(p => p != newPage).ToList()) {
                            globalNavigation.RemovePage(page);
                        }
                        foreach (var page in globalNavigation.ModalStack.ToList()) {
                            globalNavigation.RemovePage(page);
                        }
                        Controller.PopAll(newPage);
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

        protected virtual Task<bool> PushNormal(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var topNavigation = Controller.GetPickNavigation(RootPage);
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    await topNavigation.PushAsync(newPage);
                    Controller.PushPage(newPage);
                    pushInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    pushInfo.OnCompletedTask?.TrySetResult(false);
                }

            });
            return pushInfo.OnCompletedTask?.Task;
        }

        protected virtual Task<bool> PushModal(NavigationPushInfo pushInfo) {
            var newPage = GetInitializedPage(pushInfo);
            var topNavigation = Controller.GetPickNavigation(RootPage);

            Device.BeginInvokeOnMainThread(async () => {
                try {
                    if (pushInfo.NewNavigation) {
                        await topNavigation.PushModalAsync(new NavigationPage(newPage));
                    }
                    else {
                        await topNavigation.PushModalAsync(newPage);
                    }

                    Controller.PushPage(newPage);
                    pushInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    pushInfo.OnCompletedTask?.TrySetResult(false);
                }

            });
            return pushInfo.OnCompletedTask?.Task;
        }

        public virtual Task Pop(NavigationPopInfo popInfo) {
            switch (popInfo.Mode) {
                case NavigationMode.Normal:
                    return PopNormal(popInfo);
                case NavigationMode.Modal:
                    return PopModal(popInfo);
                case NavigationMode.Custom:
                    return PopCustom(popInfo);
                case NavigationMode.PopUp:
                    return PopPopUp(popInfo);
                case NavigationMode.Root:
                    return PopRoot(popInfo);
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual Task<bool> PopPopUp(NavigationPopInfo popInfo) {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> PopCustom(NavigationPopInfo popInfo) {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> PopModal(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = Controller.GetPickNavigation(RootPage);
                    var page = await navigation.PopModalAsync();
                    if (page is NavigationPage navigationPage)
                        Controller.PopPage(navigationPage.CurrentPage);
                    else
                        Controller.PopPage(page);

                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        protected virtual Task<bool> PopNormal(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = Controller.GetPickNavigation(RootPage);
                    var page = await navigation.PopAsync();
                    Controller.PopPage(page);
                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        protected virtual Task<bool> PopRoot(NavigationPopInfo popInfo) {
            Device.BeginInvokeOnMainThread(async () => {
                try {
                    var navigation = Controller.GetRootNavigation(RootPage);
                    while (navigation.ModalStack.Count > 0)
                        await navigation.PopModalAsync();
                    await navigation.PopToRootAsync();
                    Controller.PopAll(navigation.NavigationStack.First());
                    popInfo.OnCompletedTask?.TrySetResult(true);
                }
                catch (Exception e) {
                    Trace.WriteLine(e);
                    popInfo.OnCompletedTask?.TrySetResult(false);
                }
            });
            return popInfo.OnCompletedTask?.Task;
        }

        static string GetTypeBaseName(MemberInfo info) {
            if (info == null) throw new ArgumentNullException(nameof(info));
            return info.Name.Replace(@"Page", "").Replace(@"ViewModel", "");
        }

        static Dictionary<string, Type> GetAssemblyPageTypes() {
            return typeof(TPage).GetTypeInfo().Assembly.DefinedTypes
                .Where(ti =>
                       ti.IsClass && !ti.IsAbstract && ti.Name.EndsWith(@"Page", StringComparison.Ordinal) && ti.BaseType.Name.Contains(@"Page"))
                .ToDictionary(GetTypeBaseName, ti => ti.AsType());
        }

        static Dictionary<string, Type> GetAssemblyViewModelTypes() {
            return typeof(TModel).GetTypeInfo().Assembly.DefinedTypes.Where(
                ti => ti.IsClass && !ti.IsAbstract && ti.Name.EndsWith(@"ViewModel", StringComparison.Ordinal) &&
                      ti.BaseType.Name.Contains(@"ViewModel"))
                .ToDictionary(GetTypeBaseName, ti => ti.AsType());
        }

        Page GetInitializedPage(NavigationPushInfo navigationPushInfo) {
            var page = GetPage(navigationPushInfo.To.ToString());
            var viewModel = GetViewModel(navigationPushInfo.To.ToString());
            viewModel.DataToLoad = navigationPushInfo.DataToLoad;
            viewModel.DataToPreload = navigationPushInfo.DataToPreload;
            page.BindingContext = viewModel;
            viewModel.StartPreloadingData();
            return page;
        }

        Page GetPage(string pageName) {
            if (!PageTypes.ContainsKey(pageName)) throw new KeyNotFoundException($@"Page for {pageName} not found");
            Page page;
            try {
                var pageType = PageTypes[pageName];
                var pageObject = Activator.CreateInstance(pageType);
                page = pageObject as Page;
            }
            catch (Exception e) {
                throw new TypeLoadException($@"Unable create instance for {pageName}Page", e);
            }

            return page;
        }

        KitViewModel GetViewModel(string pageName) {
            if (!ViewModelTypes.ContainsKey(pageName))
                throw new KeyNotFoundException($@"ViewModel for {pageName} not found");
            KitViewModel viewModel;
            try {
                viewModel = Activator.CreateInstance(ViewModelTypes[pageName]) as KitViewModel;
            }
            catch (Exception e) {
                throw new TypeLoadException($@"Unable create instance for {pageName}ViewModel", e);
            }

            return viewModel;
        }
    }

    public class NavigationPushInfo {
        public object To { get; set; }
        public Dictionary<string, object> DataToLoad { get; set; }
        public Dictionary<string, object> DataToPreload { get; set; }
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public bool NewNavigation { get; set; }
        public object CustomData { get; set; }
        public TaskCompletionSource<bool> OnCompletedTask { get; set; } = new TaskCompletionSource<bool>();
    }

    public class NavigationPopInfo {
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public object CustomData { get; set; }
        public TaskCompletionSource<bool> OnCompletedTask { get; set; } = new TaskCompletionSource<bool>();
    }
}
