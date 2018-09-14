using System;

using Xamarin.Forms;
using XamStarterKit.ViewModels;
using XamStarterKit.Pages;

namespace XamStarterKit.Test.Pages.First {
    public class FirstPage : KitPage {
        public FirstPage() {
            Title = "1";
            Content = new StackLayout {
                Children = {
                    new Label { Text = "Hello first page" }
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

