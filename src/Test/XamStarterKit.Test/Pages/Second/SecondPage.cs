using System;

using Xamarin.Forms;
using XamStarterKit.Pages;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Test.Pages.Second {
    public class SecondPage : KitPage {
        public SecondPage() {
            Title = "2";
            Content = new StackLayout {
                Children = {
                    new Label { Text = "Hello Second page" }
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

