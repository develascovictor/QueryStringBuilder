using Filtering.Constants;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Constants
{
    [TestFixture]
    public class FilterOperatorsTests
    {
        [TestCase(FilterOperators.EqualsOperator, "==")]
        [TestCase(FilterOperators.ContainsOperator, "=@")]
        [TestCase(FilterOperators.NotEqualToOperator, "!=")]
        [TestCase(FilterOperators.NotContainsOperator, "!@")]
        [TestCase(FilterOperators.GreaterThanOperator, ">")]
        [TestCase(FilterOperators.LessThanOperator, "<")]
        [TestCase(FilterOperators.GreaterThanOrEqualToOperator, ">=")]
        [TestCase(FilterOperators.LessThanOrEqualToOperator, "<=")]
        public void ShouldValidateConstantStrings(string constant, string value)
        {
            Assert.AreEqual(constant, value);
        }
    }
}