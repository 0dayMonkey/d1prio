namespace Framework.Exceptions
{
    public class DomainViolationException : DomainException
    {
        public string Key { get; }


        public DomainViolationException(string domainViolationKey, string message) : base(message)
        {
            Key = domainViolationKey;
        }
    }
}
