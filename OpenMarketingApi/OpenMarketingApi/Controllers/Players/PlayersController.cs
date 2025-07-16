using Framework.Messages;
using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Player;

namespace OpenMarketingApi.Controllers.Players
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [Route("/api/open-marketing/v{version:apiVersion}/players")]
    [SwaggerTag("Players")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
        {
            _playerService = playerService;
            _logger = logger;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
        }
        /// <summary>
        /// Get player information
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
        [Route("{playerId}")]
        [SwaggerOperation("Get player information")]
        [SwaggerResponse(statusCode: 200, type: typeof(Player), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Player>> GetPlayer(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true)] string playerId)
        {
            _logger.LogDebug("Enter GePlayer : {playerId}", playerId);
            try
            {
                ActionResult<Player> player = await _playerService.GetPlayer(playerId);
                return Ok(player.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPlayer service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPlayer] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPlayer");
            }
        }

        /// <summary>
        /// Create new player
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [SwaggerOperation("Create new player")]
        [SwaggerResponse(statusCode: 201, type: typeof(Player), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Player>> PostPlayer(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")][Required] int siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] PlayerPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostPlayer : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOriginId, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body, _jsonOptions));
                var player = await _playerService.CreatePlayer(userId, correlationId, siteOriginId, body);
                return CreatedAtAction(nameof(GetPlayer), new { playerId = player?.Value?.Id }, player?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostPlayer service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostPlayer] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostPlayer");
            }
        }

        /// <summary>
        /// Update player information
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId">The Casino Management System Player Identifier</param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("{playerId}")]
        [SwaggerOperation("Update player information")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> PutPlayer(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] PlayerPUT body,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true)] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter PutPlayer : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOriginId, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body, _jsonOptions));
                var _ = await _playerService.UpdatePlayer(userId, correlationId, siteOriginId, playerId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PutPlayer service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PutPlayer] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PutPlayer");
            }
        }
    }
}
