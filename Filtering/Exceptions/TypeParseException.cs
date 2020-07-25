using System;

namespace Filtering.Exceptions
{
    public class TypeParseException : FilterException
    {
        public override string ErrorCode => "TYPA_DEX";

        public TypeParseException(string value, Type type)
            : base($"The value [{value}] is not parsable for type [{type?.Name}].")
        {
        }
    }
}