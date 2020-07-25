using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class MissingValueOnOperatorExceptionTests : BaseExceptionTests
    {
        [TestCase(null, null)]
        [TestCase("", "foo")]
        [TestCase(" ", "BaR")]
        [TestCase("foo", " ")]
        [TestCase("BaR", "")]
        public void ShouldInstantiateConstructorWithParameters(string operation, string filter)
        {
            var missingValueOnOperatorException = new MissingValueOnOperatorException(operation, filter);
            ValidateException(missingValueOnOperatorException, "MVOO_DEX", $"The operator [{operation}] in the filter [{filter}] requires a value.");
        }
    }
}