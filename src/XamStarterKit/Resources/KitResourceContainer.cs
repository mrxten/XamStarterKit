using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XamStarterKit.Resources {
    public class KitResourceContainer {
        #region internals resources

        protected static readonly ConcurrentDictionary<string, object> CachedResources = new ConcurrentDictionary<string, object>();

        protected static object MakeResource(object resource, [CallerMemberName] string propertyName = null) {
            return GetResource(propertyName) ?? SetResource(resource, propertyName);
        }

        protected static object GetResource([CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (CachedResources.TryGetValue(propertyName, out var cachedResource))
                return cachedResource;

            return null;
        }

        protected static object SetResource(object resource, [CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!CachedResources.ContainsKey(propertyName))
                CachedResources.TryAdd(propertyName, resource);

            return resource;
        }

        #endregion

        #region internals styles

        protected static readonly ConcurrentDictionary<string, Style> CachedStyles = new ConcurrentDictionary<string, Style>();

        protected static Style GetCachedStyle(Type targetType,
            StyleSetters setters,
            Style baseStyle = null,
            [CallerMemberName] string propertyName = null) {
            return GetCachedStyle(() => GetStyle(targetType, setters, baseStyle), propertyName);
        }

        protected static Style GetCachedStyle(Func<Style> styleFunc, [CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!CachedStyles.ContainsKey(propertyName))
                CachedStyles.TryAdd(propertyName, styleFunc.Invoke());

            if (CachedStyles.TryGetValue(propertyName, out var cachedStyle))
                return cachedStyle;

            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        protected static Style GetStyle(Type targetType, StyleSetters setters, Style baseStyle = null) {
            var style = new Style(targetType);
            if (baseStyle != null)
                style.BasedOn = baseStyle;
            setters.ApplyFor(style);
            return style;
        }

        protected class StyleSetters : IEnumerable {
            readonly List<Setter> _children = new List<Setter>();

            public IEnumerator GetEnumerator() {
                return _children?.GetEnumerator();
            }

            public void ApplyFor(Style style) {
                foreach (var setter in _children)
                    style.Setters.Add(setter);
            }

            public void Add(BindableProperty property, object value) {
                _children.Add(new Setter {
                    Property = property,
                    Value = value
                });
            }
        }

        #endregion
    }
}
