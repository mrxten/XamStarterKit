using System.Threading;
using XamStarterKit.Extensions.Bindings;
using XamStarterKit.ViewModels.Abstractions;

namespace XamStarterKit.ViewModels.Implementations
{
    public class BaseViewModel : ObservableObject, IViewModel
    {
        protected CancellationTokenSource Cancellation { get; set; } = new CancellationTokenSource();

        public void CancellAll()
        {
            Cancellation?.Cancel();
            Cancellation = new CancellationTokenSource();
        }

        public virtual void Cancel(object obj)
        {

        }
    }
}