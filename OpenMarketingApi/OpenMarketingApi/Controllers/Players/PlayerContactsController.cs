using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Contact;

namespace OpenMarketingApi.Controllers.Players
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / Contact Details")]
    public class PlayerContactsController : ControllerBase
    {
        private readonly ILogger<PlayerContactsController> _logger;
        private readonly IPlayerContactsService _playerContactsService;

        public PlayerContactsController(IPlayerContactsService playerContactsService, ILogger<PlayerContactsController> logger)
        {
            _playerContactsService = playerContactsService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Contact Details relating to specified player
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/contact-details")]
        [SwaggerOperation("GetContactDetails")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PlayerContactsResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerContactsResponse>> GetContactDetails(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogTrace("Enter GetContactDetails");
                var contacts = await _playerContactsService.GetContactDetails(playerId);
                return Ok(contacts);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetContactDetails service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetContactDetails] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetContactDetails");
            }
        }

        /// <summary>
        /// Update a player contact detail
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="body"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/contact-details")]
        [SwaggerOperation("UpdateContactDetail")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateContactDetail(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromBody] PlayerContactsPUT body)
        {
            try
            {
                _logger.LogDebug($"Enter UpdateContactDetail -> : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}, playerId: {playerId}", correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _playerContactsService.EditContactDetail(userId, correlationId, siteOrigin, playerId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, $"UpdateContactDetail service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateContactDetail] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateContactDetail");
            }
        }
    }
}
