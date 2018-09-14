using System;

using Xamarin.Forms;
using XamStarterKit.Pages;
using XamStarterKit.ViewModels;

namespace XamStarterKit.Test.Pages.Fourth {
    public class FourthPage : KitPage {
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

