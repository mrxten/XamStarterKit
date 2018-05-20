using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XamStarterKit.Resources
{
    public class BaseResourceContainer
    {
		static ResourceDictionary _resources;

		public static ResourceDictionary Resources => _resources;

		protected static readonly ConcurrentDictionary<string, object> CachedResources = new ConcurrentDictionary<string, object>();

		public virtual void Init(ResourceDictionary resourceDictionary)
		{
			_resources = resourceDictionary;
		}

		protected static object MakeResource(object resource, [CallerMemberName] string propertyName = null)
		{
			return GetResource(propertyName) ?? SetResource(resource, propertyName);
		}

		protected static void MakeGlobalStyle(Style style)
		{
			if (Resources == null)
				throw new ArgumentNullException(nameof(Resources));

			Resources.Add(style);
		}

		static object GetResource([CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			if (CachedResources.TryGetValue(propertyName, out var cachedResource))
				return cachedResource;

			return null;
		}

		static object SetResource(object resource, [CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			if (!CachedResources.ContainsKey(propertyName))
				CachedResources.TryAdd(propertyName, resource);

			return resource;
		}
	}
}
