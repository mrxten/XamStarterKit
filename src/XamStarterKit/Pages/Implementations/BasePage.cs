using System;
using Xamarin.Forms;
using XamStarterKit.ViewModels.Abstractions;

namespace XamStarterKit.Pages.Implementations
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            Appearing += OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs eventArgs)
        {
            (BindingContext as IBaseViewModel)?.PageFirstOpened();
            Appearing -= OnAppearing;
        }

        protected override bool OnBackButtonPressed()
        {
            return (BindingContext as IBaseViewModel)?.BackButtonPressed() ?? base.OnBackButtonPressed();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as IBaseViewModel)?.Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as IBaseViewModel)?.Disappering();
        }
    }
}