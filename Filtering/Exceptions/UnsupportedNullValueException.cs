using System;

namespace Filtering.Exceptions
{
    public class UnsupportedNullValueException : FilterException
    {
        public override string ErrorCode => "USNV_DEX";

        public UnsupportedNullValueException(Type type)
            : base($"The type [{type?.Name}] does not support null values.")
        {
        }
    }
}