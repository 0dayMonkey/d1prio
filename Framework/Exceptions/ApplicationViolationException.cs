namespace Framework.Exceptions
{
    public class ApplicationViolationException : ApplicationException
    {
        public string Key { get; }


        public ApplicationViolationException(string applicationViolationKey, string message) : base(message)
        {
            Key = applicationViolationKey;
        }
    }
}
