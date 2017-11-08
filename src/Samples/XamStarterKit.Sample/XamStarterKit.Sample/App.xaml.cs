using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XamStarterKit.Forms.Localization;
using XamStarterKit.Sample.Pages.Localize;
using Xamarin.Forms;
using XamStarterKit.Forms.DependencyServices;

namespace XamStarterKit.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DynamicLocalize.Init(LocalizableResources.Localization.ResourceManager, DependencyService.Get<ILocalize>().GetCurrentCultureInfo());
            MainPage = new LocalizePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
