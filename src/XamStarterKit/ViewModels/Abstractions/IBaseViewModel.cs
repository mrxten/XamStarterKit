using System;
using XamStarterKit.Localization;

namespace XamStarterKit.ViewModels.Abstractions
{
    public interface IBaseViewModel : ICancellable, IDisposable
	{
        DynamicLocalize L { get; }

        void Appearing();

        void Disappering();

        bool BackButtonPressed();

	    void PageFirstOpened();

	    void PageLeaved();

	    void ReturnedToPage();
    }
}