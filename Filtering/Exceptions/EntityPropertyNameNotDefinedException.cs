namespace Filtering.Exceptions
{
    public class EntityPropertyNameNotDefinedException : FilterException
    {
        public override string ErrorCode => "EPNND_DEX";

        public EntityPropertyNameNotDefinedException(string filterProperty)
            : base($"An entity property name is not defined for the provided filter property [{filterProperty}].")
        {
        }
    }
}