using OpenMarketingApi.Models.Referencials.Address;
using OpenMarketingApi.Models.Referencials.IdentityDocument;

namespace OpenMarketingApi.Models.Player.Document
{
    public partial class PlayerDocumentResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = null!;
        [DataMember(Name = "documentNumber")]
        public string DocumentNumber { get; set; }
        [DataMember(Name = "documentType")]
        public IdDocTypeReference DocumentType { get; set; }
        [DataMember(Name = "issuingCountry")]
        public CountryReference IssuingCountry { get; set; }
        [DataMember(Name = "issuingCountry")]
        public CityReference? IssuingCity { get; set; }
        [DataMember(Name = "issueDate")]
        public DateTime? IssueDate { get; set; }
        [DataMember(Name = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }
        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
