namespace OpenMarketingApi.Models;

public record StoreRetailItem(string Id, string ShortLabel, string LongLabel, int? SiteId, string StoreId, bool InStock, double CurrentPriceInPoints, int? QuantityLeft);
