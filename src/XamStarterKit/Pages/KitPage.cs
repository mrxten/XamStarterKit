using XamStarterKit.ViewModels;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace XamStarterKit.Pages {
    public class KitPage : ContentPage, IDisposable {
        protected KitViewModel ViewModel => BindingContext as KitViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();
            ViewModel?.StartLoadData();
        }

        protected override bool OnBackButtonPressed() {
            Task.Run(async () => {
                await Task.Delay(25);
                Dispose();
            });
            return base.OnBackButtonPressed();
        }

        protected override void OnParentSet() {
            base.OnParentSet();
            if (Parent != null) return;
            Dispose();
        }

        public virtual void Dispose() {
            ViewModel?.Dispose();
        }
    }
}