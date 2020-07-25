using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Filtering.Helpers
{
    public static class PropertyCache<T> where T : class
    {
        private static readonly IDictionary<string, string> Cache;

        static PropertyCache()
        {
            //Ignore Case
            Cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var t = typeof(T);

            foreach (var propertyName in t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(property => property.Name))
            {
                Cache[propertyName] = propertyName;
            }
        }

        public static string Get(string propertyName)
        {
            return Cache.TryGetValue(propertyName ?? string.Empty, out var result) ? result : null;
        }
    }
}