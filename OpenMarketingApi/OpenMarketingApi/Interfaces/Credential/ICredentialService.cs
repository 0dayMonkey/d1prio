using OpenMarketingApi.Models.Credentials;

namespace OpenMarketingApi.Interfaces.Credentials

{
    public interface ICredentialService
    {
        //Task<ActionResult<SearchResult<AnonymousCredentialResponse>>> GetCredentials(string searchModel);
        Task<ActionResult<CredentialResponse>> GetCredentialById(string credentialId);
        Task<ActionResult<List<CredentialResponse>>> CreateAnonymousCredential(string userId, string? correlationId, int? siteOriginId, CredentialPOST credential);
        //Task<ActionResult> UpdateCredential(string userId, string? correlationId, int? siteOriginId, string credentialId, AnonymousCredentialPUT credential);
        Task<ActionResult> DeleteAnonymousCredential(string userId, string? correlationId, int? siteOriginId, string credentialId, ServiceAction body);
        Task<ActionResult> PatchCredential(string userId, string? correlationId, int? siteOriginId, string credentialId, CredentialPATCH credential);
    }
}
