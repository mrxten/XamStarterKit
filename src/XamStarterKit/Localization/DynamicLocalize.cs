using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace XamStarterKit.Localization {
    public class DynamicLocalize : INotifyPropertyChanged, IDisposable {
        static ResourceManager _resourceManager;
        static CultureInfo _ci;
        static Dictionary<DynamicLocalize, HashSet<string>> _members;

        public event PropertyChangedEventHandler PropertyChanged;

        [IndexerName("Index")]
        public string this[string name] {
            get {
                var str = _resourceManager.GetString(name, _ci);
                if (str == null) return name;

                if (_members.ContainsKey(this))
                    _members[this].Add($"Index[{name}]");
                else
                    _members.Add(this, new HashSet<string>(new[] { $"Index[{name}]" }));
                return str;
            }
        }

        public static void Init(ResourceManager manager, CultureInfo cultureInfo) {
            _resourceManager = manager;
            _ci = cultureInfo;
            _members = new Dictionary<DynamicLocalize, HashSet<string>>();
        }

        public static void UpdateCulture(CultureInfo cultureInfo) {
            _ci = cultureInfo;
            NotifyChanged();
        }

        public static void NotifyChanged() {
            foreach (var member in _members) {
                foreach (var prop in member.Value) {
                    member.Key.OnPropertyChanged(prop);
                }
            }
        }

        public void Dispose() {
            if (_members.ContainsKey(this))
                _members.Remove(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}