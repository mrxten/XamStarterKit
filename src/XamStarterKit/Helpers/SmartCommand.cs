using System;
using System.Windows.Input;

namespace XamStarterKit.Helpers
{
	public class SmartCommand : ICommand
	{
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;

		public event EventHandler CanExecuteChanged;
		
		private bool _isCanExecute;

		public SmartCommand(Action execute, Func<bool> canExecute = null, bool defaultCanExecute = true) : this(o => execute(), canExecute == null ? (Func<object, bool>)null : c => canExecute(), defaultCanExecute)
		{
		}

		public SmartCommand(Action<object> execute, Func<object, bool> canExecute = null, bool defaultCanExecute = true)
		{
			_execute = execute;
			_canExecute = canExecute;
			_isCanExecute = defaultCanExecute;
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

		public bool IsCanExecute
		{
			get => _isCanExecute;
			set
			{
				_isCanExecute = value;
				ChangeCanExecute();
			}
		}

		public void ChangeCanExecute()
		{
			var changed = CanExecuteChanged;
			changed?.Invoke(this, EventArgs.Empty);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke(parameter) ?? IsCanExecute;
		}
	}
}