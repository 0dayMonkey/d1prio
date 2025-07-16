namespace OpenMarketingApi.Models.Loyalty.Configuration;

public record ConfigurationPUT
{
    [Required]
    public string DefaultTier { get; init; }
}
