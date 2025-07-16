
namespace FloorServer.Client.Tools.Validators
{
    public class LiveMessageTextValidator
    {
        public string ErrorMessage { get; set; }

        public bool Validate(string entry)
        {
            if (entry.Length >= 256)
            {
                ErrorMessage = "Live message text cannot exceed 255 caracteres";
                return false;
            }

            return true;
        }
    }
}
