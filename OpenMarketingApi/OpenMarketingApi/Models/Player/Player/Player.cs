using ApiTools.Converters;
using OpenMarketingApi.Models.Player.CustomField;
using OpenMarketingApi.Models.Referencials.Address;
using OpenMarketingApi.Models.Referencials.Occupation;
using OpenMarketingApi.Models.Referencials.SocioProfessional;
using OpenMarketingApi.Models.Referencials.Title;

namespace OpenMarketingApi.Models.Player.Player;
/// <summary>
/// 
/// </summary>
[DataContract]
public partial class Player
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>

    [DataMember(Name = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets FirstName
    /// </summary>

    [DataMember(Name = "firstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or Sets LastName
    /// </summary>

    [DataMember(Name = "lastName")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or Sets Gender
    /// </summary>

    [DataMember(Name = "gender")]
    public string Gender { get; set; }

    /// <summary>
    /// Gets or Sets MaidenName
    /// </summary>

    [DataMember(Name = "maidenName")]
    public string MaidenName { get; set; }

    /// <summary>
    /// Gets or Sets Alias
    /// </summary>

    [DataMember(Name = "alias")]
    public string Alias { get; set; }

    /// <summary>
    /// Gets or Sets SocialSecurityNumber
    /// </summary>

    [DataMember(Name = "socialSecurityNumber")]
    public string SocialSecurityNumber { get; set; }

    /// <summary>
    /// Gets or Sets BirthDate
    /// </summary>

    [DataMember(Name = "birthDate")]
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets or Sets BirthPlace
    /// </summary>

    [DataMember(Name = "birthPlace")]
    public AddressLocation BirthPlace { get; set; }

    /// <summary>
    /// Gets or Sets SocialSecurityCountry
    /// </summary>

    [DataMember(Name = "socialSecurityCountry")]
    public Country? SocialSecurityCountry { get; set; }

    /// <summary>
    /// Gets or Sets title
    /// </summary>

    [DataMember(Name = "Title")]
    public TitleResponse? Title { get; set; }

    /// <summary>
    /// Gets or Sets Occupation
    /// </summary>

    [DataMember(Name = "occupation")]
    public OccupationReference Occupation { get; set; }

    /// <summary>
    /// Gets or Sets SocioProfessionalCategory
    /// </summary>

    [DataMember(Name = "socioProfessionalCategory")]
    public SocioProfessionalCategoryReference SocioProfessionalCategory { get; set; }

    /// <summary>
    /// ISO 639-1 language code
    /// </summary>
    /// <value>ISO 639-1 language code</value>

    [DataMember(Name = "languageId")]
    public string LanguageId { get; set; }

    /// <summary>
    /// Gets or Sets MaritalStatus
    /// </summary>

    [DataMember(Name = "maritalStatus")]
    public PlayerMaritalStatus MaritalStatus { get; set; }

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
    /// Gets or Sets Nationalities
    /// </summary>

    [DataMember(Name = "nationalities")]
    public List<Nationality> Nationalities { get; set; }

    /// <summary>
    /// Gets or Sets CustomFields
    /// </summary>

    [DataMember(Name = "customFields")]
    public List<PlayerCustomField> CustomFields { get; set; }

    /// <summary>
    /// Gets or Sets CreationTimestamp
    /// </summary>

    [DataMember(Name = "creationTimestamp")]
    public DateTime CreationTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets LastUpdatedTimestamp
    /// </summary>

    [DataMember(Name = "lastUpdatedTimestamp")]
    public DateTime LastUpdatedTimestamp { get; set; }
}
