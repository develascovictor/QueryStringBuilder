using ExpressionBuilder.Interfaces;
using System;

namespace Filtering.Exceptions
{
    public class UnsupportedTypeOnOperationException : FilterException
    {
        public override string ErrorCode => "USTO_DEX";

        public UnsupportedTypeOnOperationException(Type type, IOperation operation)
            : base($"Unsupported DataType [{type?.Name}] on Operation [{operation}].")
        {
        }
    }
}