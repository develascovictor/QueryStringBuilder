using System;

namespace Filtering.Exceptions
{
    public class UnsupportedFilterPropertyException : FilterException
    {
        public override string ErrorCode => "USFP_DEX";

        public UnsupportedFilterPropertyException(string propertyName, Type type)
            : base($"The property [{propertyName}] for type [{type.Name}] is invalid.")
        {
        }
    }
}