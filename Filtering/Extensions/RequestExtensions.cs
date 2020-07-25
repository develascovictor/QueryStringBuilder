using Filtering.Constants;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Filtering.Extensions
{
    public static class RequestExtensions
    {
        private const string DefaultParamName = "filter";
        private const string FilterPattern = "(?:\\b" + DefaultParamName + "=\\b)([^&]*)";
        private const string PatternJom = @"==([ \w]*)(?:\.([ \w]*))?(?:\.([ \w]*))?,?";
        private const string PatternJomToRemove = DefaultParamName + @"=(?:" + PatternJom + ")+&?";

        public static string GetFilterRequest(string queryString, string paramName = null)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return string.Empty;
            }

            var decodedQs = HttpUtility.UrlDecode(queryString, Encoding.UTF8);
            var withoutDslQs = GetRegex(PatternJomToRemove, paramName, RegexOptions.IgnoreCase).Replace(decodedQs, string.Empty);
            var match = GetRegex(FilterPattern, paramName, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(withoutDslQs);
            var filter = new StringBuilder();

            while (match.Success)
            {
                if (filter.Length > 0)
                {
                    filter.Append(LogicalOperators.AndOperator);
                }

                var statement = match.Groups[1].Value;
                var array = statement.Split(',');

                filter.Append(string.Join(LogicalOperators.OrOperator, array));
                match = match.NextMatch();
            }

            return filter.ToString();
        }

        private static Regex GetRegex(string pattern, string paramName, RegexOptions options)
        {
            var rgx = new Regex(pattern.Replace(DefaultParamName, string.IsNullOrWhiteSpace(paramName) ? DefaultParamName : paramName), options);
            return rgx;
        }
    }
}