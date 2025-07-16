using Framework.Messages;
using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Comment;

namespace OpenMarketingApi.Controllers.Players
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / Comments")]
    public class PlayerCommentsController : ControllerBase
    {
        private readonly ILogger<PlayerCommentsController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IPlayerCommentsService _playerCommentService;

        public PlayerCommentsController(IPlayerCommentsService playerCommentService, ILogger<PlayerCommentsController> logger)
        {
            _playerCommentService = playerCommentService;
            _logger = logger;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
        }

        /// <summary>
        /// Get all existing player comments
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="userId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/comments")]
        [SwaggerOperation("GetComments")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PlayerCommentResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PlayerCommentResponse>>> GetComments([FromRoute][Required] string playerId,
            [FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogDebug("Enter GetComments");

                var playerComment = await _playerCommentService.GetAllComment(playerId);
                return Ok(playerComment);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetComments service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetComments] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetComments");
            }
        }

        /// <summary>
        /// Get existing player comment by ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/comments/{id}")]
        [SwaggerOperation("GetCommentById")]
        [SwaggerResponse(statusCode: 200, type: typeof(PlayerCommentResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerCommentResponse>> GetCommentById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetCommentById : {id}, playerId {playerId}", id, playerId);

                ActionResult<PlayerCommentResponse> playerComment = await _playerCommentService.GetCommentById(id, playerId);
                return Ok(playerComment.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCommentById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCommentById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCommentById");
            }
        }

        /// <summary>
        /// Create a new player comment
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/comments")]
        [SwaggerOperation("PostComment")]
        [SwaggerResponse(statusCode: 201, type: typeof(PlayerCommentResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerCommentResponse>> PostComment(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromBody] PlayerCommentPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostComment : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}, playerId: {playerId}", correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body, _jsonOptions));
                var playerComment = await _playerCommentService.CreateComment(userId, correlationId, siteOrigin, body, playerId);
                return CreatedAtAction(nameof(GetCommentById), new { playerId, id = playerComment?.Value?.Id }, playerComment?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostComment service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostComment] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostComment");
            }
        }

        /// <summary>
        /// Update a player comment 
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/comments/{id}")]
        [SwaggerOperation("UpdateComment")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateComment(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] PlayerCommentPUT body,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter UpdateComment : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}", id, correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body, _jsonOptions));
                var _ = await _playerCommentService.EditComment(userId, correlationId, siteOrigin, id, body, playerId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateComment service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateComment] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateComment");
            }
        }

        /// <summary>
        /// Delete a player comment
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/comments/{id}")]
        [SwaggerOperation("DeleteComment")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteComment(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter DeletedComment id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", id, correlationId, siteOrigin, userId, playerId, deleteAction.LastUpdatedTimestamp);

                var _ = await _playerCommentService.DeleteComment(userId, correlationId, siteOrigin, id, playerId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteComment service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteComment] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteComment");
            }
        }
    }
}
