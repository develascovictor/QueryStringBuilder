using Filtering.Constants;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Constants
{
    [TestFixture]
    public class LogicalOperatorsTests
    {
        [TestCase(LogicalOperators.AndOperator, "&")]
        [TestCase(LogicalOperators.OrOperator, "|")]
        public void ShouldValidateConstantStrings(string constant, string value)
        {
            Assert.AreEqual(constant, value);
        }
    }
}