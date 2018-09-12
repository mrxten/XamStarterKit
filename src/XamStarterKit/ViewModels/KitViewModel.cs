using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using XamStarterKit.Helpers;
using static XamStarterKit.KitMessages;

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

        #region Data

        public Dictionary<string, object> DataToPreload {
            get => Get<Dictionary<string, object>>();
            set => Set(value);
        }

        public Dictionary<string, object> DataToLoad {
            get => Get<Dictionary<string, object>>();
            set => Set(value);
        }

        public bool IsLoadDataStarted {
            get => Get<bool>();
            protected internal set => Set(value);
        }

        public bool IsPreloadDataStarted {
            get => Get<bool>();
            protected internal set => Set(value);
        }

        public void StartLoadingData() {
            if (IsLoadDataStarted) return;
            IsLoadDataStarted = true;

            Task.Run(LoadingDataAsync, CancellationToken);
        }

        public void StartPreloadingData() {
            if (IsPreloadDataStarted) return;
            IsPreloadDataStarted = true;

            PreloadingData();
        }

        protected virtual Task LoadingDataAsync() {
            return Task.FromResult(0);
        }

        protected virtual void PreloadingData() {

        }

        bool GetContainsValue<T>(Dictionary<string, object> dictionary, string key, out T value) {
            if (dictionary != null && dictionary.ContainsKey(key)) {
                value = (T)dictionary[key];
                return true;
            }
            value = default(T);
            return false;
        }

        #endregion

        #region MakeCommand

        readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();

        protected RelayCommand MakeCommand(Func<object, Task> task, bool canExecute = true, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as RelayCommand ??
                   SaveCommand(new RelayCommand(task, canExecute), propertyName) as
                       RelayCommand;
        }
        protected RelayCommand MakeCommand(Func<Task> task, bool canExecute = true, [CallerMemberName] string propertyName = null) {
            return GetCommand(propertyName) as RelayCommand ??
                   SaveCommand(new RelayCommand(task, canExecute), propertyName) as
                       RelayCommand;
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