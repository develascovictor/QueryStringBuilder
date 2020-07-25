using Filtering.Exceptions;
using Filtering.Unit.Tests.Exceptions.Base;
using NUnit.Framework;
using System;

namespace Filtering.Unit.Tests.Exceptions
{
    [TestFixture]
    public class UnsupportedNullValueExceptionTests : BaseExceptionTests
    {
        [TestCase(null)]
        [TestCase(typeof(string))]
        [TestCase(typeof(int))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(long))]
        [TestCase(typeof(short))]
        [TestCase(typeof(double))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(char))]
        public void ShouldInstantiateConstructorWithParameters(Type type)
        {
            var unsupportedNullValueException = new UnsupportedNullValueException(type);
            ValidateException(unsupportedNullValueException, "USNV_DEX", $"The type [{type?.Name}] does not support null values.");
        }
    }
}