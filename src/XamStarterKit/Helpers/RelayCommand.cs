
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;
using Xamarin.Forms;

namespace XamStarterKit.Helpers {
    public class RelayCommand : ICommand {
        bool _processing;
        readonly Func<object, Task> _tskParam;
        readonly Func<Task> _tsk;

        private readonly Func<bool> _canExecute;
        private readonly Func<object, bool> _canExecuteParam;

        public RelayCommand(Func<Task> func, Func<bool> canExecute = null) {
            _tsk = func ?? throw new ArgumentNullException(nameof(func));

            if (canExecute != null) {
                _canExecute = canExecute;
            }
        }

        public RelayCommand(Func<object, Task> funcParam, Func<object, bool> canExecuteParam = null) {
            _tskParam = funcParam ?? throw new ArgumentNullException(nameof(funcParam));
            if (canExecuteParam != null) {
                _canExecuteParam = canExecuteParam;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) {
            if (_canExecute != null)
                return _canExecute.Invoke();
            if (_canExecuteParam != null)
                return _canExecuteParam.Invoke(parameter);
            return true;
        }

        public virtual async void Execute(object parameter) {
            if (_processing) return;
            _processing = true;

            if (CanExecute(parameter)) {
                if (_tsk != null) {
                    await _tsk.Invoke();
                }
                else {
                    await _tskParam?.Invoke(parameter);
                }
            }
            _processing = false;
        }
    }
}
