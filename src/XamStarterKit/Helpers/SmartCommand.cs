using System;
using System.Windows.Input;

namespace XamStarterKit.Helpers
{
	public class SmartCommand : ICommand
	{
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;

		//Not using in this implementation
		public event EventHandler CanExecuteChanged;
		
		public SmartCommand(Action execute, Func<bool> canExecute = null) : this(o => execute(), c => canExecute?.Invoke() ?? true)
		{
		}

		public SmartCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
				_execute?.Invoke(parameter);
		}

		public void Execute()
		{
			if (CanExecute(null))
				_execute?.Invoke(null);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke(parameter) ?? true;
		}
	}
}