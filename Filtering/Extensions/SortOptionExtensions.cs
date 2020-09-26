using Filtering.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Filtering.Extensions
{
    public static class SortOptionExtensions
    {
        public static Dictionary<string, bool> BuildSortByExpression<T>(string sortOptions, IReadOnlyDictionary<string, string> whitelist = null) where T : class
        {
            var dictionary = new Dictionary<string, bool>();

            if (string.IsNullOrWhiteSpace(sortOptions))
            {
                return dictionary;
            }

            whitelist = whitelist ?? new Dictionary<string, string>();
            var sortList = sortOptions.Split(',').Where(x => !string.IsNullOrWhiteSpace(x) && x.NullTrim() != "-");

            foreach (var sort in sortList)
            {
                var sortValue = sort;
                var sortBy = true;

                if (sortValue.StartsWith("-", StringComparison.Ordinal))
                {
                    sortBy = false;
                    sortValue = sort.Remove(0, 1);
                }

                if (whitelist.Any())
                {
                    if (!whitelist.TryGetValue(sortValue, out var value))
                    {
                        throw new UnsupportedWhitelistPropertyException(sortValue, typeof(T));
                    }

                    sortValue = value;
                }

                var propertyType = sortValue.GetPropertyType<T>();

                if (propertyType == null)
                {
                    throw new EntityPropertyNameNotDefinedException(sortValue);
                }

                dictionary.Add(sortValue, sortBy);
            }

            return dictionary;
        }
    }
}