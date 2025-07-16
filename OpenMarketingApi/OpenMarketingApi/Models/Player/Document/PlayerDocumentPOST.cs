using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Player.Document
{
    public partial class PlayerDocumentPOST : IServiceAction
    {
        [DataMember(Name = "id")]
        public string? Id { get; set; }
        [Required]
        [DataMember(Name = "documentNumber")]
        public string DocumentNumber { get; set; }
        [DataMember(Name = "documentTypeId")]
        public string DocumentTypeId { get; set; }
        [DataMember(Name = "issuingCountry")]
        public string IssuingCountryId { get; set; }
        [DataMember(Name = "issuingCity")]
        public string? IssuingCityId { get; set; }
        [DataMember(Name = "issueDate")]
        public DateTime? IssueDate { get; set; }
        [DataMember(Name = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }
        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }




    }
}
