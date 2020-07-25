using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Filtering.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> ApplySortByExpressions<T>(this IQueryable<T> queryable, Dictionary<string, bool> sortByList) where T : class
        {
            var sortByString = sortByList != null ? string.Join(",", sortByList.Select(GetOrderStatement)) : string.Empty;
            var orderedQueryable = (IOrderedQueryable<T>) (string.IsNullOrEmpty(sortByString) ? queryable.OrderBy("id") : queryable.OrderBy(sortByString));

            return orderedQueryable;
        }

        private static string GetOrderStatement(KeyValuePair<string, bool> kv)
        {
            var orderBy = kv.Value ? "ASC" : "DESC";
            return $"{kv.Key} {orderBy}";
        }
    }
}