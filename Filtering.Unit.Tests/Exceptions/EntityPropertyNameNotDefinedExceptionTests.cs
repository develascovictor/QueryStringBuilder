using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class EntityPropertyNameNotDefinedExceptionTests : BaseExceptionTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("foo")]
        [TestCase("BaR")]
        public void ShouldInstantiateConstructorWithParameters(string filterProperty)
        {
            var entityPropertyNameNotDefinedException = new EntityPropertyNameNotDefinedException(filterProperty);
            ValidateException(entityPropertyNameNotDefinedException, "EPNND_DEX", $"An entity property name is not defined for the provided filter property [{filterProperty}].");
        }
    }
}