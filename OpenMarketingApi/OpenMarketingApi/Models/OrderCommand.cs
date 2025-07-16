namespace OpenMarketingApi.Models;

public record OrderCommand
{
    /// <summary>
    /// The PlayerId.
    /// </summary>
    [SwaggerSchema("The PlayerId should not be null or empty")]
    [Required]
    public string PlayerId { get; init; }

    /// <summary>
    /// The StoreId.
    /// </summary>
    [SwaggerSchema("The StoreId should not be null or empty")]
    [Required]
    public string StoreId { get; init; }

    /// <summary>
    /// The OrderItems.
    /// </summary>
    [SwaggerSchema("The List of order item")]
    [Required]
    public List<OrderItemCommand> OrderItems { get; init; }
}