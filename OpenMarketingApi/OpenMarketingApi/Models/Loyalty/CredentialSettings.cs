namespace OpenMarketingApi.Models.Loyalty
{
    public class CredentialSettings
    {
        public int? ValidityDays { get; set; }
        public int? MaxNumber { get; set; }
        public bool? AllowExceedMaxNumber { get; set; }
        public bool? IsTemporaryCredential { get; set; }
        public bool? IsCredentialEncoded { get; set; }
        public bool? IsCredentialPrinted { get; set; }
    }
}
