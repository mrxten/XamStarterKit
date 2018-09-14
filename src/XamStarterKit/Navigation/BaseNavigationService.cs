using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamStarterKit.Pages;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Navigation {
    public abstract class BaseNavigationService<TAssebly> {
        protected Dictionary<string, Type> PageTypes { get; set; }
        protected Dictionary<string, Type> ViewModelTypes { get; set; }
        protected Page RootPage { get; set; }

        protected BaseNavigationService() {
            PageTypes = GetAssemblyPageTypes();
            ViewModelTypes = GetAssemblyViewModelTypes();
        }

        public abstract Page SetRoot(NavigationPushInfo pushInfo);

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

        protected abstract Task<bool> PushPopUp(NavigationPushInfo pushInfo);
        protected abstract Task<bool> PushCustom(NavigationPushInfo pushInfo);
        protected abstract Task<bool> PushRoot(NavigationPushInfo pushInfo);
        protected abstract Task<bool> PushNormal(NavigationPushInfo pushInfo);
        protected abstract Task<bool> PushModal(NavigationPushInfo pushInfo);

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

        protected abstract Task<bool> PopPopUp(NavigationPopInfo popInfo);
        protected abstract Task<bool> PopNormal(NavigationPopInfo popInfo);
        protected abstract Task<bool> PopModal(NavigationPopInfo popInfo);
        protected abstract Task<bool> PopCustom(NavigationPopInfo popInfo);
        protected abstract Task<bool> PopRoot(NavigationPopInfo popInfo);

        protected static string GetTypeBaseName(MemberInfo info) {
            if (info == null) throw new ArgumentNullException(nameof(info));
            return info.Name.Replace(@"Page", "").Replace(@"ViewModel", "");
        }

        protected Dictionary<string, Type> GetAssemblyPageTypes() {
            return typeof(TAssebly).GetTypeInfo().Assembly.DefinedTypes
                .Where(ti =>
                       ti.IsClass && !ti.IsAbstract && ti.Name.EndsWith(@"Page", StringComparison.Ordinal) && ti.BaseType.Name.Contains(@"Page"))
                .ToDictionary(GetTypeBaseName, ti => ti.AsType());
        }

        protected Dictionary<string, Type> GetAssemblyViewModelTypes() {
            return typeof(TAssebly).GetTypeInfo().Assembly.DefinedTypes.Where(
                ti => ti.IsClass && !ti.IsAbstract && ti.Name.EndsWith(@"ViewModel", StringComparison.Ordinal) &&
                      ti.BaseType.Name.Contains(@"ViewModel"))
                .ToDictionary(GetTypeBaseName, ti => ti.AsType());
        }

        protected Page GetInitializedPage(NavigationPushInfo navigationPushInfo) {
            var page = GetPage(navigationPushInfo.To.ToString());
            var viewModel = GetViewModel(navigationPushInfo.To.ToString());
            viewModel.NavigationParams = navigationPushInfo.NavigationParams;
            page.BindingContext = viewModel;
            return page;
        }

        protected Page GetPage(string pageName) {
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

        protected KitViewModel GetViewModel(string pageName) {
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

        protected static void DisposePage(Page page) {
            if (page?.BindingContext is IDisposable disposableVm)
                disposableVm.Dispose();

            if (page is IDisposable disposable)
                disposable.Dispose();
        }
    }

    public class NavigationPushInfo {
        public object To { get; set; }
        public Dictionary<string, object> NavigationParams { get; set; }
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public TaskCompletionSource<bool> OnCompletedTask { get; set; } = new TaskCompletionSource<bool>();
        public object Argument { get; set; }
    }

    public class NavigationPopInfo {
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public object CustomData { get; set; }
        public TaskCompletionSource<bool> OnCompletedTask { get; set; } = new TaskCompletionSource<bool>();
        public object Argument { get; set; }
    }
}
