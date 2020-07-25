using Filtering.Exceptions;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Exceptions.Base
{
    public abstract class BaseExceptionTests
    {
        protected void ValidateException(FilterException domainException, string errorCode, string message)
        {
            Assert.IsEmpty(domainException.Data);
            Assert.IsNull(domainException.Source);
            Assert.IsNull(domainException.StackTrace);
            Assert.IsNull(domainException.InnerException);
            Assert.AreEqual(domainException.ErrorCode, errorCode);
            Assert.AreEqual(domainException.Message, message);
        }
    }
}