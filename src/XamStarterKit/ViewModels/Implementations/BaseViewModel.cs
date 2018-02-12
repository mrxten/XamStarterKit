using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using XamStarterKit.Helpers;
using XamStarterKit.Localization;
using XamStarterKit.ViewModels.Abstractions;

namespace XamStarterKit.ViewModels.Implementations
{
    public class BaseViewModel : Bindable, IBaseViewModel
    {
        public DynamicLocalize L { get; }

	    protected CancellationTokenSource BasicCancellation { get; set; } = new CancellationTokenSource();

		public BaseViewModel()
        {
            L = new DynamicLocalize();
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

	    public virtual void PageFirstOpened()
	    {
	    }

	    public virtual void PageLeaved()
	    {
	    }

	    public virtual void ReturnedToPage()
	    {
	    }

	    public void CancellAll()
	    {
		    BasicCancellation?.Cancel();
		    BasicCancellation = new CancellationTokenSource();
	    }

	    public virtual void Cancel(object obj)
	    {
	    }

	    public void Dispose()
	    {
			L?.Dispose();
		    BasicCancellation?.Cancel();
		    BasicCancellation?.Dispose();
	    }
	}
}