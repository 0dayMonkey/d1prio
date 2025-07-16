namespace OpenMarketingApi.Interfaces;

public interface IItemsService
{
    Task<byte[]> GetPictureAsync(string itemId);
    Task<SearchResult<StoreRetailItem>> GetStoreStockAsync(string storeId, string searchModel);
}