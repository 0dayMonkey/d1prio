namespace Framework.Exceptions
{
    public class DomainNotFoundException : DomainException
    {
        public string Key { get; }


        public DomainNotFoundException(string domainNotFoundKey, string message) : base(message)
        {
            Key = domainNotFoundKey;
        }
    }
}
