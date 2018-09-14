using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamStarterKit.Navigation;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamStarterKit.Test {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            var nav = new LinearNavigationService<App>();

            MainPage = nav.SetRoot(new NavigationPushInfo {
                To = AppPages.First
            });

            Task.Run(async () => {
                await Task.Delay(1000);
                await nav.Push(new NavigationPushInfo {
                    To = AppPages.Second,
                });
                await Task.Delay(1000);
                await nav.Push(new NavigationPushInfo {
                    To = AppPages.Third,
                    Mode = NavigationMode.Root
                });
                await Task.Delay(1000);
                await nav.Push(new NavigationPushInfo {
                    To = AppPages.Fourth,
                });
                await Task.Delay(1000);
                await nav.Pop(new NavigationPopInfo {
                    Mode = NavigationMode.Root
                });
                await Task.Delay(1000);
                await nav.Push(new NavigationPushInfo {
                    To = AppPages.Third,
                    Mode = NavigationMode.Modal,
                    Argument = true
                });
                await Task.Delay(1000);
                await nav.Push(new NavigationPushInfo {
                    To = AppPages.First,
                    Mode = NavigationMode.Modal
                });
                await Task.Delay(1000);
                await nav.Pop(new NavigationPopInfo {
                    Mode = NavigationMode.Root
                });
            });
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }

    public enum AppPages {
        First,
        Second,
        Third,
        Fourth
    }
}
