using System;
using XamStarterKit.Localization;

namespace XamStarterKit.ViewModels.Abstractions
{
    public interface IBaseViewModel : ICancellable, IDisposable
	{
        DynamicLocalize L { get; }

		void Init();

		void Appearing();

        void Disappering();

        bool BackButtonPressed();

	    void FirstAppearing();

	    void Leaved();

	    void ReturnedBack();
    }
}