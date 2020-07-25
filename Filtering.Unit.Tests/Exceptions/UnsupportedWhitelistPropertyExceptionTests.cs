using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;
using System;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class UnsupportedWhitelistPropertyExceptionTests : BaseExceptionTests
    {
        [TestCase(null, typeof(string))]
        [TestCase("", typeof(int))]
        [TestCase(" ", typeof(bool))]
        [TestCase("foo", typeof(long?))]
        [TestCase("BaR", typeof(DateTime?))]
        public void ShouldInstantiateConstructorWithParameters(string propertyName, Type type)
        {
            var unsupportedWhitelistPropertyException = new UnsupportedWhitelistPropertyException(propertyName, type);
            ValidateException(unsupportedWhitelistPropertyException, "USWP_DEX", $"The property [{propertyName}] is not supported by the whitelist for type [{type.Name}].");
        }
    }
}