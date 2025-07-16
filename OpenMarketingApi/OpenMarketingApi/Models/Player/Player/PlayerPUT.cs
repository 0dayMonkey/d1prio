using ApiTools.Converters;
using OpenMarketingApi.Interfaces.Common;
using OpenMarketingApi.Models.Player.CustomField;

namespace OpenMarketingApi.Models.Player.Player
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerPUT : IServiceAction
    {
        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [Required]
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [Required]
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Supported values: * &#x27;M&#x27; - Male * &#x27;F&#x27; - Female * &#x27;U&#x27; - Undefined 
        /// </summary>
        /// <value>Supported values: * &#x27;M&#x27; - Male * &#x27;F&#x27; - Female * &#x27;U&#x27; - Undefined </value>
        [Required]
        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or Sets MaidenName
        /// </summary>
        [DataMember(Name = "maidenName")]
        public string? MaidenName { get; set; }

        /// <summary>
        /// Gets or Sets Alias
        /// </summary>
        [DataMember(Name = "alias")]
        public string? Alias { get; set; }

        /// <summary>
        /// Gets or Sets BirthDate
        /// </summary>
        [DataMember(Name = "birthDate")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or Sets BirthCountryId
        /// </summary>
        [DataMember(Name = "birthCountryId")]
        public string? BirthCountryId { get; set; }

        /// <summary>
        /// Gets or Sets BirthCityId
        /// </summary>
        [DataMember(Name = "birthCityId")]
        public string? BirthCityId { get; set; }

        /// <summary>
        /// Gets or Sets OccupationId
        /// </summary>
        [DataMember(Name = "occupationId")]
        public string? OccupationId { get; set; }

        /// <summary>
        /// Gets or Sets SocioProfessionalCategoryId
        /// </summary>
        [DataMember(Name = "socioProfessionalCategoryId")]
        public string? SocioProfessionalCategoryId { get; set; }

        /// <summary>
        /// ISO 639-1 language code
        /// </summary>
        /// <value>ISO 639-1 language code</value>
        [DataMember(Name = "languageId")]
        public string? LanguageId { get; set; }

        /// <summary>
        /// Gets or Sets MaritalStatusId
        /// </summary>
        [DataMember(Name = "maritalStatusId")]
        public string? MaritalStatusId { get; set; }

        /// <summary>
        /// Gets or Sets WeddingDate
        /// </summary>
        [DataMember(Name = "weddingDate")]
        public DateOnly? WeddingDate { get; set; }

        /// <summary>
        /// Gets or Sets MainCasinoId
        /// </summary>
        [DataMember(Name = "mainCasinoId")]
        public int? MainCasinoId { get; set; }

        /// <summary>
        /// Gets or Sets SocialSecurityNumber
        /// </summary>
        [DataMember(Name = "socialSecurityNumber")]
        public string? SocialSecurityNumber { get; set; }

        /// <summary>
        /// Gets or Sets SocialSecurityCountryId
        /// </summary>
        [DataMember(Name = "socialSecurityCountryId")]
        public string? SocialSecurityCountryId { get; set; }

        /// <summary>
        /// Gets or Sets titleId
        /// </summary>
        [DataMember(Name = "TitleId")]
        public string? TitleId { get; set; }

        /// <summary>
        /// Gets or Sets NationalityIds
        /// </summary>
        [DataMember(Name = "nationalityIds")]
        public List<string>? NationalityIds { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets CustomFields
        /// </summary>
        [DataMember(Name = "customFields")]
        public List<PlayerCustomFieldRequest>? CustomFields { get; set; }
    }
}
