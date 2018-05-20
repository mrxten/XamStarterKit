using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XamStarterKit.Resources
{
    public class BaseConverterContainer
    {
		protected static readonly ConcurrentDictionary<string, IValueConverter> CachedConverters = new ConcurrentDictionary<string, IValueConverter>();

		protected static IValueConverter MakeConverter(Type converterType, [CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			if (!CachedConverters.ContainsKey(propertyName))
			{
				var converter = Activator.CreateInstance(converterType) as IValueConverter;
				if (converter == null) return null;
				CachedConverters.TryAdd(propertyName, converter);
				return converter;
			}

			IValueConverter cachedCommand;
			if (CachedConverters.TryGetValue(propertyName, out cachedCommand))
				return cachedCommand;

			throw new ArgumentOutOfRangeException(nameof(propertyName));
		}
	}
}
