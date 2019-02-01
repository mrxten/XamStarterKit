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

        static readonly ConcurrentDictionary<string, object> CachedResources = new ConcurrentDictionary<string, object>();

        static object MakeResource(object resource, [CallerMemberName] string propertyName = null) {
            return GetResource(propertyName) ?? SetResource(resource, propertyName);
        }

        static object GetResource([CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (CachedResources.TryGetValue(propertyName, out var cachedResource))
                return cachedResource;

            return null;
        }

        static object SetResource(object resource, [CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!CachedResources.ContainsKey(propertyName))
                CachedResources.TryAdd(propertyName, resource);

            return resource;
        }

        #endregion
    }
}
