using Framework.Repositories;
using OpenPromoApi.Models;

namespace OpenPromoApi.Interfaces

{
    public interface IPromoService
    {
        Task<List<PromotionResponse>> GetAllPromotionsForCustomer(int siteId, string playerId, string userId);
        Task<PromotionResponse> GetPromotionById(int siteId, string id, string userId);
        Task PatchPromotionStatus(PromotionStatusPATCH promotionAction , string id, string userId, ChangeContext changeContext);
    }
}
