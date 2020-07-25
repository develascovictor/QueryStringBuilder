using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;
using System;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class TypeParseExceptionTests : BaseExceptionTests
    {
        [TestCase(null, typeof(string))]
        [TestCase("", typeof(int))]
        [TestCase(" ", typeof(bool))]
        [TestCase("foo", typeof(long?))]
        [TestCase("BaR", typeof(DateTime?))]
        public void ShouldInstantiateConstructorWithParameters(string value, Type type)
        {
            var typeParseException = new TypeParseException(value, type);
            ValidateException(typeParseException, "TYPA_DEX", $"The value [{value}] is not parsable for type [{type?.Name}].");
        }
    }
}