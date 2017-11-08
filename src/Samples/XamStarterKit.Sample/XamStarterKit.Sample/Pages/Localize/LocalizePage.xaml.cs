using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamStarterKit.Sample.Pages.Localize
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocalizePage : ContentPage
    {
        public LocalizePage()
        {
            this.BindingContext = new LocalizeViewModel();
            InitializeComponent();
        }
    }
}