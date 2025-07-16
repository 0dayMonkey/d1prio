namespace Framework.Exceptions
{
    public class ApplicationNotFoundException : ApplicationException
    {
        public string Key { get; }


        public ApplicationNotFoundException(string applicationNotFoundKey, string message) : base(message)
        {
            Key = applicationNotFoundKey;
        }
    }
}
