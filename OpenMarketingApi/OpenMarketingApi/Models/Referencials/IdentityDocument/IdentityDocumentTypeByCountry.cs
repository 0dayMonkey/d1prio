namespace OpenMarketingApi.Models.Referencials
{
    [DataContract]
    public class IdentityDocumentTypeByCountry
    {
        [Required]
        [DataMember(Name = "countryId")]
        public string CountryId { get; set; } = null!;

        [DataMember(Name = "yearsValidity")]
        public byte? YearsValidity { get; set; }

        [DataMember(Name = "official")]
        public bool? Official { get; set; }
    }
}
