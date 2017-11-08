using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamStarterKit.Extensions.Bindings;
using XamStarterKit.Forms.DependencyServices;
using XamStarterKit.Forms.Localization;
using XamStarterKit.Forms.ViewModels.Implementations;

namespace XamStarterKit.Sample.Pages.Localize
{
    public class LocalizeViewModel : FormsBaseViewModel
    {
        private CultureInfo _currentCulture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

        public ICommand ChangeLangCommand { get; }

        public LocalizeViewModel()
        {
            ChangeLangCommand = new Command(ChangeLang);
        }

        private void ChangeLang()
        {
            var newCult = _currentCulture.Equals(new CultureInfo("ru-RU"))
                ? new CultureInfo("en-US")
                : new CultureInfo("ru-RU");

            DynamicLocalize.UpdateCulture(newCult);
            _currentCulture = newCult;
        }
    }
}