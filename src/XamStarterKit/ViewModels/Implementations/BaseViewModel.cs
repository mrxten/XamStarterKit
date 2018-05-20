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

		public virtual void Init()
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

	    public virtual void FirstAppearing()
	    {
	    }

	    public virtual void Leaved()
	    {
	    }

	    public virtual void ReturnedBack()
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

		#region MakeCommand

		readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();

		public ConcurrentDictionary<string, ICommand> CachedCommands => _cachedCommands;

		protected SmartCommand MakeCommand(Action commandAction, Func<bool> canExecute = null, [CallerMemberName] string propertyName = null)
		{
			return GetCommand(propertyName) as SmartCommand ?? SaveCommand(new SmartCommand(commandAction, canExecute), propertyName) as SmartCommand;
		}
		protected SmartCommand MakeCommand(Action<object> commandAction, Func<object, bool> canExecute = null, [CallerMemberName] string propertyName = null)
		{
			return GetCommand(propertyName) as SmartCommand ?? SaveCommand(new SmartCommand(commandAction, canExecute), propertyName) as SmartCommand;
		}

		ICommand SaveCommand(ICommand command, string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			if (!CachedCommands.ContainsKey(propertyName))
			{
				CachedCommands.TryAdd(propertyName, command);
			}

			return command;
		}

		ICommand GetCommand(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			ICommand cachedCommand;
			if (CachedCommands.TryGetValue(propertyName, out cachedCommand))
				return cachedCommand;

			return null;
		}

		#endregion
	}
}