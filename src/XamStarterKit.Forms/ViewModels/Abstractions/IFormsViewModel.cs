using XamStarterKit.Forms.Localization;
using XamStarterKit.ViewModels.Abstractions;

namespace XamStarterKit.Forms.ViewModels.Abstractions
{
    public interface IFormsViewModel : IViewModel
    {
        DynamicLocalize Localize { get; }

        void FirstAppearing();

        void Appearing();

        void Disappering();

        bool BackButtonPressed();

        void SizeAllocated(double width, double height);
    }
}