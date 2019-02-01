using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using XamStarterKit.Helpers;
using Xamarin.Forms;
using XamStarterKit.Navigation;

namespace XamStarterKit.ViewModels {
    public class KitViewModel : Bindable, IDisposable {
        CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public CancellationToken CancellationToken => _tokenSource?.Token ?? CancellationToken.None;

        public KitPageState State {
            get => Get(KitPageState.Clean);
            set => Set(value);
        }

        public virtual void Cancel() {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            Cancel();
        }

        ~KitViewModel() {
            Dispose(false);
        }

        protected virtual Task<bool> NavigateTo(
            object toIdentifier,
            NavigationMode mode = NavigationMode.Normal,
            Dictionary<string, object> navParams = null,
            object argument = null) {

            var completedTask = new TaskCompletionSource<bool>();
            MessageBus.SendMessage(
                KitMessage.NavigationPush,
                new NavigationPushInfo {
                    To = toIdentifier.ToString(),
                    Mode = mode,
                    NavigationParams = navParams,
                    Argument = argument,
                    OnCompletedTask = completedTask,
                });
            return completedTask.Task;
        }

        protected virtual Task<bool> NavigateBack(
            NavigationMode mode = NavigationMode.Normal,
            object argument = null) {

            var completedTask = new TaskCompletionSource<bool>();
            MessageBus.SendMessage(
                KitMessage.NavigationPop,
                new NavigationPopInfo {
                    Mode = mode,
                    Argument = argument,
                    OnCompletedTask = completedTask,
                });
            return completedTask.Task;
        }

        #region Data

        public Dictionary<string, object> NavigationParams {
            get => Get<Dictionary<string, object>>();
            set => Set(value);
        }

        public bool IsLoadDataStarted {
            get => Get<bool>();
            protected set => Set(value);
        }

        public void StartLoadData() {
            if (IsLoadDataStarted)
                return;
            IsLoadDataStarted = true;
            DataLoadAsync();
        }

        protected virtual Task DataLoadAsync() {
            return Task.FromResult(0);
        }

        protected bool GetContainsValue<T>(string key, out T value) {
            if (NavigationParams != null && NavigationParams.ContainsKey(key) && NavigationParams[key] is T generic) {
                value = generic;
                return true;
            }
            value = default(T);
            return false;
        }

        #endregion

        #region MakeCommand

        readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();


        protected RelayCommand MakeCommand(Func<Task> task, Func<bool> canExecute = null, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as RelayCommand ??
                   SaveCommand(new RelayCommand(task, canExecute), propertyName) as
                       RelayCommand;
        }

        protected RelayCommand MakeCommand(Func<object, Task> task, Func<object, bool> canExecute = null, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as RelayCommand ??
                   SaveCommand(new RelayCommand(task, canExecute), propertyName) as
                       RelayCommand;
        }

        protected Command MakeCommand(Action task, Func<bool> canExecute = null, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as Command ??
                   SaveCommand(new Command(task, canExecute), propertyName) as
                       Command;
        }

        protected Command MakeCommand(Action<object> task, Func<object, bool> canExecute = null, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as Command ??
                   SaveCommand(new Command(task, canExecute), propertyName) as
                       Command;
        }

        ICommand SaveCommand(ICommand command, string propertyName) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!_cachedCommands.ContainsKey(propertyName))
                _cachedCommands.TryAdd(propertyName, command);
            return command;
        }

        ICommand GetCommand(string propertyName) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            return _cachedCommands.TryGetValue(propertyName, out var cachedCommand)
                ? cachedCommand
                : null;
        }

        #endregion
    }
}