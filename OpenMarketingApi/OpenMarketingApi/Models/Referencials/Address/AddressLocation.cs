namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class AddressLocation
    {
        /// <summary>
        /// Gets or Sets Country
        /// </summary>

        [Required]
        [DataMember(Name = "country")]
        public CountryReference Country { get; set; }

        /// <summary>
        /// Gets or Sets City
        /// </summary>

        [DataMember(Name = "city")]
        public CityReference City { get; set; }
    }
}
