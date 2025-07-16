namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelDescriptionPUT
    {
        [Required]
        public bool IsAbbreviationUsed { get; set; }
        [Required]
        public bool IsDescriptionUsed { get; set; }
        public string? Label { get; set; }
    }
}
