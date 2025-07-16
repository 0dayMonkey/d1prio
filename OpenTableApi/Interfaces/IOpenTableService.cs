using OpenTableApi.Models;

namespace OpenTableApi.Interfaces
{
    public interface IOpenTableService
    {
        Task<PlayerSessionResponse> GetPlayerSessionAsync(long id);
        Task<PlayerSessionResponse> CreatePlayerSessionAsync(PlayerSessionCreationRequest playerSession);
        Task<PlayerSessionResponse> UpdatePlayerSessionAsync(PlayerSessionUpdateRequest playerSession);
        Task DeletePlayerSessionAsync(long id);
    }
}
