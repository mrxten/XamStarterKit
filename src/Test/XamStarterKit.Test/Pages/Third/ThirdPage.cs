using System;

using Xamarin.Forms;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Test.Pages.Third {
    public class ThirdPage : ContentPage, IBasePage {
        public ThirdPage() {
            Title = "3";
            Content = new StackLayout {
                Children = {
                    new Label { Text = "Hello Third page" }
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

