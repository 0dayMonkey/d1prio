using OpenMarketingApi.Interfaces.Loyalty;
using OpenMarketingApi.Models.Loyalty.Account;

namespace OpenMarketingApi.Controllers.Loyalty
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [Route("/api/open-marketing/v{version:apiVersion}/accounts")]
    [SwaggerTag("Loyalty / Accounts")]
    public class LoyaltyAccountsController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<LoyaltyAccountsController> _logger;

        public LoyaltyAccountsController(ILoyaltyService loyaltyService, ILogger<LoyaltyAccountsController> logger)
        {
            _loyaltyService = loyaltyService;
            _logger = logger;
        }

        /// <summary>
        /// Get loyalty data for a player
        /// </summary>
        /// <remarks>Get loyalty data for a player</remarks>
        /// <param name="userId"></param>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("{playerId}")]
        [SwaggerOperation("Get loyalty data for a player")]
        [SwaggerResponse(statusCode: 200, type: typeof(AccountResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AccountResponse>> GetLoyaltyAccount(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true), StringLength(21, MinimumLength = 1)] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter GetLoyaltyAccount -> playerId : {playerId}, userId: {userId}", playerId, userId);
                ActionResult<AccountResponse> account = await _loyaltyService.GetLoyaltyAccount(playerId);
                return Ok(account.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetLoyaltyAccount service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetLoyaltyAccount] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetLoyaltyAccount");
            }
        }

        /// <summary>
        /// Create club account for a player
        /// </summary>
        /// <param name="body"></param>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="clientId"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [SwaggerOperation("Create the player club account")]
        [SwaggerResponse(statusCode: 201, type: typeof(AccountResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AccountResponse>> PostLoyaltyAccount(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required()] string userId,
            [FromHeader(Name = "API-Client-ID"), Required()] string clientId,
            [FromBody] AccountPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostLoyaltyAccount CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {body.PlayerId}, clientId: {clientId}", correlationId, siteOriginId, userId, body.PlayerId, clientId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                ActionResult<AccountResponse> account = await _loyaltyService.CreateLoyaltyAccount(correlationId, siteOriginId, userId, clientId, body);
                return CreatedAtAction(nameof(GetLoyaltyAccount), new { playerId = body.PlayerId }, account?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, $"PostLoyaltyAccount service exception: {se.StatusCode} - {se.Message} - {se.StackTrace}");
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostLoyaltyAccount] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostLoyaltyAccount");
            }
        }

        /// <summary>
        /// Update player information
        /// </summary>
        /// <param name="body"></param>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="clientId"></param>
        /// <param name="playerId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPatch]
        [ApiVersion("1")]
        [Route("{playerId}")]
        [SwaggerOperation("Update player loyalty tiers and/or points balance")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> PatchLoyaltyAccount(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID"), Required] int siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required()] string userId,
            [FromHeader(Name = "API-Client-ID")] string? clientId,
            [FromBody] AccountPATCH body,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true), StringLength(21, MinimumLength = 1)] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter PatchLoyaltyAccount CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {playerId}, clientId: {clientId}", correlationId, siteOriginId, userId, playerId, clientId);
                ActionResult _ = await _loyaltyService.UpdateLoyaltyAccount(correlationId, siteOriginId, userId, clientId, playerId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PatchLoyaltyAccount service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PatchLoyaltyAccount] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PatchLoyaltyAccount");
            }
        }

        /// <summary>
        /// Delete the player club account
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [Route("{playerId}")]
        [ApiVersion("1")]
        [SwaggerOperation("Delete  the player club account")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteLoyaltyAccount(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID")][Required()] string userId,
            [FromBody] ServiceAction body,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true), StringLength(21, MinimumLength = 1)] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter DeleteLoyaltyAccount CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {playerId}", correlationId, siteOriginId, userId, playerId);

                return await _loyaltyService.DeleteLoyaltyAccount(correlationId, siteOriginId, userId, playerId, body);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteLoyaltyAccount service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteLoyaltyAccount] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteLoyaltyAccount");
            }
        }
    }
}
