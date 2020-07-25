using System;

namespace Filtering.Exceptions
{
    public class UnsupportedWhitelistPropertyException : FilterException
    {
        public override string ErrorCode => "USWP_DEX";

        public UnsupportedWhitelistPropertyException(string propertyName, Type type)
            : base($"The property [{propertyName}] is not supported by the whitelist for type [{type.Name}].")
        {
        }
    }
}