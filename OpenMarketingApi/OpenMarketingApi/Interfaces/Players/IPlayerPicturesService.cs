namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerPicturesService
    {
        Task<ActionResult<byte[]>> GetPictureByType(string playerId, PictureTypeEnum pictureType);
        Task<ActionResult> EditPicture(string userId, string? CorrelationId, int? SiteOriginId, byte[] picture, string playerId, PictureTypeEnum pictureType);
        Task<ActionResult> DeletePicture(string userId, string? CorrelationId, int? SiteOriginId, string playerId, PictureTypeEnum pictureType, ServiceAction body);
    }
}
