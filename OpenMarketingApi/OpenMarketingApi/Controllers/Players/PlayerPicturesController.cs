using OpenMarketingApi.Interfaces.Players;

namespace OpenMarketingApi.Controllers.Players
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / Pictures")]
    public class PlayerPicturesController : ControllerBase
    {
        private readonly ILogger<PlayerPicturesController> _logger;
        private readonly IPlayerPicturesService _playerPictureService;

        public PlayerPicturesController(IPlayerPicturesService playerPictureService, ILogger<PlayerPicturesController> logger)
        {
            _playerPictureService = playerPictureService;
            _logger = logger;
        }

        /// <summary>
        /// Get existing player picture by Type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pictureType"></param>
        /// <param name="playerId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/pictures/{pictureType}")]
        [SwaggerOperation("GetPictureById")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<byte[]>> GetPictureByType(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] PictureTypeEnum pictureType,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter GetPictureByType : {pictureType}, playerId {playerId}", pictureType, playerId);

                ActionResult<byte[]> playerPicture = await _playerPictureService.GetPictureByType(playerId, pictureType);
                return Ok(playerPicture.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPictureByType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPictureByType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPictureByType");
            }
        }

        /// <summary>
        /// Update a player picture 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="pictureType"></param>
        /// <param name="picture"></param>
        /// <param name="playerId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/pictures/{pictureType}")]
        [SwaggerOperation("UpdatePicture")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdatePicture(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] PictureTypeEnum pictureType,
            [FromBody] byte[] picture,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter UpdatePicture {pictureType}  : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}", pictureType, correlationId, siteOrigin, userId, playerId);

                var _ = await _playerPictureService.EditPicture(userId, correlationId, siteOrigin, picture, playerId, pictureType);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdatePicture service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdatePicture] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdatePicture");
            }
        }

        /// <summary>
        /// Delete an player picture
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="pictureType"></param>
        /// <param name="playerId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/pictures/{pictureType}")]
        [SwaggerOperation("DeletePicture")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeletePicture(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] PictureTypeEnum pictureType,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter DeletedPicture : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", correlationId, siteOrigin, userId, playerId, deleteAction.LastUpdatedTimestamp);

                var _ = await _playerPictureService.DeletePicture(userId, correlationId, siteOrigin, playerId, pictureType, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeletePicture service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeletePicture] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeletePicture");
            }
        }
    }
}
