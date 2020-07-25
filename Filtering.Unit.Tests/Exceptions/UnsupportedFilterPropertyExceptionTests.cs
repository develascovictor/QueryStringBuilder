using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;
using System;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class UnsupportedFilterPropertyExceptionTests : BaseExceptionTests
    {
        [TestCase(null, typeof(string))]
        [TestCase("", typeof(int))]
        [TestCase(" ", typeof(bool))]
        [TestCase("foo", typeof(long?))]
        [TestCase("BaR", typeof(DateTime?))]
        public void ShouldInstantiateConstructorWithParameters(string propertyName, Type type)
        {
            var unsupportedFilterPropertyException = new UnsupportedFilterPropertyException(propertyName, type);
            ValidateException(unsupportedFilterPropertyException, "USFP_DEX", $"The property [{propertyName}] for type [{type.Name}] is invalid.");
        }
    }
}