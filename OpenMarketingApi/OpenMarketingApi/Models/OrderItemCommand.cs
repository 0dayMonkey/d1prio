namespace OpenMarketingApi.Models;

public record OrderItemCommand
{
    /// <summary>
    /// The ItemId to order.
    /// </summary>
    [Required]
    [SwaggerSchema("The item Id")]
    public string ItemId { get; init; }

    /// <summary>
    /// The quantity of the order.
    /// </summary>
    [Required]
    [SwaggerSchema("The quantity of the order.")]
    public int Quantity { get; init; }
}