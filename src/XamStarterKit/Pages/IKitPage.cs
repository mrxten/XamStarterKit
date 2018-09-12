using XamStarterKit.ViewModels;
using System;
namespace XamStarterKit.Pages.Abstractions {
    public interface IKitPage : IDisposable {
        KitViewModel ViewModel { get; }

        void StartLoadingData();
    }
}