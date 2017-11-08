using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace XamStarterKit.Forms.Localization
{
    public class LocalizeDynamicObject : DynamicObject, INotifyPropertyChanged, IDisposable
    {
        private static ResourceManager _resourceManager;

        private static CultureInfo _ci;

        private static Dictionary<LocalizeDynamicObject, HashSet<string>> _members;

        public event PropertyChangedEventHandler PropertyChanged;

        public static void Init(ResourceManager manager, CultureInfo cultureInfo)
        {
            _resourceManager = manager;
            _ci = cultureInfo;
            _members = new Dictionary<LocalizeDynamicObject, HashSet<string>>();
        }

        public static void UpdateCulture(CultureInfo cultureInfo)
        {
            _ci = cultureInfo;
            NotifyChanged();
        }

        public static void NotifyChanged()
        {
            foreach (var member in _members)
            {
                foreach (var prop in member.Value)
                {
                    member.Key.OnPropertyChanged(prop);
                }
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_resourceManager == null)
                throw new NullReferenceException("Please call Init before this.");

            var str = _resourceManager.GetString(binder.Name, _ci);
            result = str ?? binder.Name;

            if (str != null)
                if (_members.ContainsKey(this))
                    _members[this].Add(binder.Name);
                else
                    _members.Add(this, new HashSet<string>(new[] { binder.Name }));

            return true;
        }

        public void Dispose()
        {
            if (_members.ContainsKey(this))
                _members.Remove(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}