namespace Filtering.Exceptions
{
    public class MissingValueOnOperatorException : FilterException
    {
        public override string ErrorCode => "MVOO_DEX";

        public MissingValueOnOperatorException(string operation, string filter)
            : base($"The operator [{operation}] in the filter [{filter}] requires a value.")
        {
        }
    }
}