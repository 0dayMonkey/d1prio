namespace AddressLocation.Domain.Models
{
    public class AddressLevelDescription
    {
        public decimal? Id { get; set; }
        public bool IsAbbreviationUsed { get; set; }
        public bool IsDescriptionUsed { get; set; }
        public string? Label { get; set; }
    }
}
