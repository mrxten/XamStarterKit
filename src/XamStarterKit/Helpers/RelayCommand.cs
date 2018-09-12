using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace XamStarterKit.Helpers {
    public class RelayCommand : ICommand {
        readonly Func<object, Task> _tskParam;
        readonly Func<Task> _tsk;
        bool _canExecute;

        public RelayCommand(Func<object, Task> task, bool canExecute = true) {
            _canExecute = canExecute;
            _tskParam = task;
        }
        public RelayCommand(Func<Task> task, bool canExecute = true) {
            _canExecute = canExecute;
            _tsk = task;
        }

        public bool IsCanExecute {
            get => _canExecute;
            set {
                if (_canExecute == value) return;
                _canExecute = value;
                CanExecuteChanged?.Invoke(this, new EventArg<bool>(_canExecute));
            }
        }

        public bool CanExecute(object parameter) {
            return IsCanExecute;
        }

        public async void Execute(object parameter) {
            if (!CanExecute(parameter))
                return;

            IsCanExecute = false;
            if (_tskParam != null)
                await _tskParam?.Invoke(parameter);
            else if (_tsk != null)
                await _tsk?.Invoke();
            IsCanExecute = true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
