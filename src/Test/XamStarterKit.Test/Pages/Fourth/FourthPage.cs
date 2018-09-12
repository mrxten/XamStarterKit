using System;

using Xamarin.Forms;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Test.Pages.Fourth {
    public class FourthPage : ContentPage, IBasePage {
        public FourthPage() {
            Title = "4";
            Content = new StackLayout {
                Children = {
                    new Label { Text = "Hello Fourth page" }
                }
            };
        }

        public KitViewModel ViewModel => BindingContext as KitViewModel;

        public void Dispose() {
        }

        public void StartLoadingData() {
        }
    }
}

