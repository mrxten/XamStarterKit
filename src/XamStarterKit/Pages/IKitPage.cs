using XamStarterKit.ViewModels;
using System;
namespace XamStarterKit.Pages {
    public interface IKitPage : IDisposable {
        KitViewModel ViewModel { get; }

        void StartLoadingData();
    }
}