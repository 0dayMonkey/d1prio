using Microsoft.AspNetCore.Http.Extensions;
using OpenMarketingApi.Interfaces.Loyalty;
using OpenMarketingApi.Models.Loyalty.Credential;

namespace OpenMarketingApi.Controllers.Loyalty
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [Route("/api/open-marketing/v{version:apiVersion}/accounts/")]
    [SwaggerTag("Loyalty / Credentials")]
    public class LoyaltyCredentialsController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<LoyaltyCredentialsController> _logger;

        public LoyaltyCredentialsController(ILoyaltyService loyaltyService, ILogger<LoyaltyCredentialsController> logger)
        {
            _loyaltyService = loyaltyService;
            _logger = logger;
        }

        /// <summary>
        /// Delete a credential, and all linked credentials (same support) for the specified player account
        /// </summary>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <param name="credentialId">The Credential Identifier</param>
        /// <param name="correlationId"></param>
        /// <param name="userId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="body"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/accounts/{playerId}/credentials/{credentialId}")]
        [SwaggerOperation("DeleteAccountCredentials")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]

        public async Task<IActionResult> DeleteAccountCredentials(
            [FromRoute][Required] string playerId, 
            [FromRoute][Required] decimal credentialId, 
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromBody][Required] ServiceAction body
            )
        {
            try
            {
                _logger.LogDebug("Enter DeleteAccountCredentials id : {credentialId}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", credentialId, correlationId, siteOriginId, userId, playerId, body.LastUpdatedTimestamp);

                LoyaltyCredentialDELETE delete = new()
                {
                    UserId = userId,
                    LastUpdatedTimestamp = body.LastUpdatedTimestamp,
                    CredentialId = credentialId.ToString(),
                    PlayerId = playerId
                };

                ActionResult<List<Credential>> credentials = await _loyaltyService.DeleteCredential(correlationId, siteOriginId, delete);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteAccountCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteAccountCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteAccountCredentials");
            }
        }

        /// <summary>
        /// Get all credentials associated to the specified player account
        /// </summary>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <param name="userId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("{playerId}/credentials")]
        [SwaggerOperation("Get all credentials associated to the specified player account")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Credential>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<CredentialResponse>>> GetAccountCredentials(
            [FromRoute][SwaggerParameter("The Casino Management System Player Identifier", Required = true)] string playerId,
            [FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogDebug("Enter GetAccountCredentials : playerId {playerId}", playerId);

                ActionResult<List<Credential>> credentials = await _loyaltyService.GetAccountCredentials(playerId);
                return Ok(credentials.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAccountCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAccountCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAccountCredentials");
            }
        }

        /// <summary>
        /// Update a credential, and all linked credentials (same support), for the specified player account
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <param name="credentialId">The Credential Identifier</param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPatch]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/accounts/{playerId}/credentials/{credentialId}")]
        [SwaggerOperation("PatchAccountCredentials")]

        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> PatchAccountCredentials(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] CredentialPatch body, 
            [FromRoute][Required] string playerId, 
            [FromRoute][Required] decimal credentialId)
        {
            try
            {
                _logger.LogDebug("Enter PatchAccountCredentials : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {playerId}", correlationId, siteOriginId, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var patch = new LoyaltyCredentialPATCH 
                { 
                    PlayerId = playerId,
                    CredentialId = credentialId.ToString(),
                    IsManualLocked = body.IsManualLocked,
                    PinCode = body.PinCode,
                    PinCodeHash = body.PinCodeHash,
                    UserId = userId,
                    FailedAttempt = body.FailedAttempt
                };
                await _loyaltyService.PatchCredential(correlationId, siteOriginId, patch);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PatchAccountCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PatchAccountCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PatchAccountCredentials");
            }
        }

        /// <summary>
        /// Create credential for the specified player account
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/accounts/{playerId}/credentials")]
        [SwaggerOperation("PostAccountCredentials")]
        [SwaggerResponse(statusCode: 201, type: typeof(List<Credential>), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<CredentialResponse>>> PostAccountCredentials(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] CredentialPost body, 
            [FromRoute][Required] string playerId)
        {
            try
            { 
                _logger.LogDebug("Enter PostAccountCredentials : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {playerId}", correlationId, siteOriginId, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                List<LoyaltyCredentialData> credentials = new List<LoyaltyCredentialData>();
                foreach (var c in body.Credentials)
                {
                    credentials.Add(new LoyaltyCredentialData { CredentialCode = c.CredentialCode, CredentialType = c.CredentialType });
                }

                var loyaltyCredentialPOST = new LoyaltyCredentialPOST
                {
                    PlayerId = playerId,
                    PlayerCredentials = credentials.ToArray(),
                    IssuingSiteId = (int)body.IssuingSiteId,
                    BeginValidityTimestamp = body.BeginValidityTimestamp,
                    EndValidityTimestamp = body.EndValidityTimestamp,
                    SupportType = body.SupportType,
                    SupportId = body.SupportId,
                    UserId = userId,
                    LastUpdatedTimestamp = body.LastUpdatedTimestamp
                };

                var result = await _loyaltyService.CreateCredentials(correlationId, siteOriginId, loyaltyCredentialPOST);
                return Created(Request.GetEncodedUrl(), result.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostAccountCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostAccountCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostAccountCredentials");
            }
        }
    }
}