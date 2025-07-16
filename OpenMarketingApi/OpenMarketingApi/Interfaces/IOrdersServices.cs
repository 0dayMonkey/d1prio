namespace OpenMarketingApi.Interfaces;

public interface IOrdersServices
{
    Task<Order> CreateOrderAsync(OrderCommand command);
    Task<SearchResult<Order>> GetOrdersAsync(string searchModel);
}
