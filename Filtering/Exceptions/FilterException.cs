using System;

namespace Filtering.Exceptions
{
    public abstract class FilterException : Exception
    {
        public abstract string ErrorCode { get; }

        public FilterException(string message) 
            : base(message)
        {

        }

        public FilterException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
