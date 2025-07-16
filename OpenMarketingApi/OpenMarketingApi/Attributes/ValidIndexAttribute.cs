namespace OpenMarketingApi.Attributes
{
    public class ValidIndexAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int index;
            if (value != null && int.TryParse(value.ToString(), out index))
            {
                if (index != 0 && index != 1)
                {
                    return new ValidationResult("Index must be either 0 or 1.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
