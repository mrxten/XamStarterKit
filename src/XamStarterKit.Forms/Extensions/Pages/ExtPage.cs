using System;
using Xamarin.Forms;
using XamStarterKit.Forms.ViewModels.Abstractions;

namespace XamStarterKit.Forms.Extensions.Pages
{
    public class ExtPage : ContentPage
    {
        public ExtPage()
        {
            Appearing += OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs eventArgs)
        {
            (BindingContext as IFormsViewModel)?.FirstAppearing();
            Appearing -= OnAppearing;
        }

        protected override bool OnBackButtonPressed()
        {
            return (BindingContext as IFormsViewModel)?.BackButtonPressed() ?? base.OnBackButtonPressed();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as IFormsViewModel)?.Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as IFormsViewModel)?.Disappering();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            (BindingContext as IFormsViewModel)?.SizeAllocated(width, height);
        }
    }
}