using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class UnsupportedOperatorExceptionTests : BaseExceptionTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("foo")]
        [TestCase("BaR")]
        public void ShouldInstantiateConstructorWithParameters(string filter)
        {
            var unsupportedOperatorException = new UnsupportedOperatorException(filter);
            ValidateException(unsupportedOperatorException, "USOP_DEX", $"The filter operators in the filter [{filter}] are invalid.");
        }
    }
}