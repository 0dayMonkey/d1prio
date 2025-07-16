using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Address;

namespace OpenMarketingApi.Controllers.Players
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / Addresses")]
    public class PlayerAddressesController : ControllerBase
    {
        private readonly ILogger<PlayerAddressesController> _logger;
        private readonly IPlayerAddressesService _playerAddressService;

        public PlayerAddressesController(IPlayerAddressesService playerAddressService, ILogger<PlayerAddressesController> logger)
        {
            _playerAddressService = playerAddressService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing player addresses
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/addresses")]
        [SwaggerOperation("GetAddresses")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PlayerAddressResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PlayerAddressResponse>>> GetAddressees(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter GetAddresses");

                var playerAddress = await _playerAddressService.GetAllAddress(playerId);
                return Ok(playerAddress);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressees service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressees] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressees");
            }
        }

        /// <summary>
        /// Get existing player address by ID
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/addresses/{id}")]
        [SwaggerOperation("GetAddressById")]
        [SwaggerResponse(statusCode: 200, type: typeof(PlayerAddressResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerAddressResponse>> GetAddressById(
            [FromRoute][Required] string playerId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string id)
        {
            try
            {
                
                _logger.LogDebug("Enter GetAddressById : {id}, playerId {playerId}", id, playerId);

                ActionResult<PlayerAddressResponse> playerAddress = await _playerAddressService.GetAddressById(id, playerId);
                return Ok(playerAddress.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressById");
            }
        }

        /// <summary>
        /// Create a new player address
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/addresses")]
        [SwaggerOperation("PostAddress")]
        [SwaggerResponse(statusCode: 201, type: typeof(PlayerAddressResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerAddressResponse>> PostAddress(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromBody] PlayerAddressPOST body)
        {
            try
            {              
                _logger.LogDebug("Enter PostAddress : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}, playerId: {playerId}", correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var playerAddresss = await _playerAddressService.CreateAddress(userId, correlationId, siteOrigin, body, playerId);
                return CreatedAtAction(nameof(GetAddressById), new { playerId, id = playerAddresss?.Value?.AddressId }, playerAddresss?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostAddress service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostAddress] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostAddress");
            }
        }

        /// <summary>
        /// Update a player address 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/addresses/{id}")]
        [SwaggerOperation("UpdateAddress")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateAddress(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] PlayerAddressPUT body,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {      
                _logger.LogDebug("Enter UpdateAddress : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}", id, correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _playerAddressService.EditAddress(userId, correlationId, siteOrigin, id, body, playerId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateAddress service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateAddress] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateAddress");
            }
        }

        /// <summary>
        /// Delete an player address
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/addresses/{id}")]
        [SwaggerOperation("DeleteAddress")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteAddress(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string id)
        {
            try
            {
               
                _logger.LogDebug("Enter DeleteAddress id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", id, correlationId, siteOriginId, userId, playerId, deleteAction.LastUpdatedTimestamp);

                var _ = await _playerAddressService.DeleteAddress(userId, correlationId, siteOriginId, id, playerId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "Enter DeleteAddress service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteAddress] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteAddress");
            }
        }
    }
}
