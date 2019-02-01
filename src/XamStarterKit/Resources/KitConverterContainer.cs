using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using XamStarterKit.Helpers;

namespace XamStarterKit.Resources {
    public class KitConverterContainer {
        protected static readonly ConcurrentDictionary<string, IValueConverter> CachedConverters = new ConcurrentDictionary<string, IValueConverter>();

        protected static IValueConverter MakeConverter(Type converterType, [CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!CachedConverters.ContainsKey(propertyName)) {
                if (!(Activator.CreateInstance(converterType) is IValueConverter converter)) return null;
                CachedConverters.TryAdd(propertyName, converter);
                return converter;
            }

            if (CachedConverters.TryGetValue(propertyName, out var cachedConverter))
                return cachedConverter;

            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        protected static IValueConverter MakeConverter<From, To>(
            SimpleConvertMethod<From, To> simpleConvertMethod = null,
            SimpleConvertMethod<To, From> simpleConvertBackMethod = null,
            ConvertMethod<From, To> convertMethod = null,
            ConvertMethod<To, From> convertBackMethod = null,
            [CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!CachedConverters.ContainsKey(propertyName)) {
                var basicConverter = new BasicConverter<From, To>(simpleConvertMethod, simpleConvertBackMethod, convertMethod, convertBackMethod);
                CachedConverters.TryAdd(propertyName, basicConverter);
                return basicConverter;
            }

            if (CachedConverters.TryGetValue(propertyName, out var cachedConverter))
                return cachedConverter;

            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }
    }
}
