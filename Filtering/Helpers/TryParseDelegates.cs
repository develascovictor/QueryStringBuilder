using System;

namespace Filtering.Helpers
{
    public class TryParseDelegates
    {
        public delegate bool TryParse<TSource>(string s, out TSource source);

        public static class Delegates
        {
            public static TryParse<bool> Bool = bool.TryParse;
            public static TryParse<short> Short = short.TryParse;
            public static TryParse<int> Int = int.TryParse;
            public static TryParse<long> Long = long.TryParse;
            public static TryParse<decimal> Decimal = decimal.TryParse;
            public static TryParse<DateTime> DateTime = System.DateTime.TryParse;
        }
    }
}