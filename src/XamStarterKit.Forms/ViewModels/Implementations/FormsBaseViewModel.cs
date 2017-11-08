using XamStarterKit.Forms.Localization;
using XamStarterKit.Forms.ViewModels.Abstractions;
using XamStarterKit.ViewModels.Implementations;

namespace XamStarterKit.Forms.ViewModels.Implementations
{
    public class FormsBaseViewModel : BaseViewModel, IFormsViewModel
    {
        public dynamic Localize { get; }

        public FormsBaseViewModel()
        {
            Localize = new LocalizeDynamicObject();
        }

        public virtual void FirstAppearing()
        {
        }
         
        public virtual void Appearing()
        {
        }

        public virtual void Disappering()
        {
        }

        public virtual bool BackButtonPressed()
        {
            CancellAll();
            return false;
        }

        public virtual void SizeAllocated(double width, double height)
        {
        }
    }
}