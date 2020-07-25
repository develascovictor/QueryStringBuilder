namespace Filtering.Exceptions
{
    public class UnsupportedOperatorException : FilterException
    {
        public override string ErrorCode => "USOP_DEX";

        public UnsupportedOperatorException(string filter)
            : base($"The filter operators in the filter [{filter}] are invalid.")
        {
        }
    }
}