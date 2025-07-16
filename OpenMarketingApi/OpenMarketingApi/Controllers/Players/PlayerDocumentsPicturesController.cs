using OpenMarketingApi.Attributes;
using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Document;

namespace OpenMarketingApi.Controllers.Players
{
    [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents/{id}/pictures/{index}")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / Identity Document Picture")]
    public class PlayerDocumentsPicturesController : ControllerBase
    {
        private readonly ILogger<PlayerDocumentsPicturesController> _logger;
        private readonly IPlayerDocumentsPicturesService _idDocPictureService;

        public PlayerDocumentsPicturesController(IPlayerDocumentsPicturesService idDocPictureService, ILogger<PlayerDocumentsPicturesController> logger)
        {
            _idDocPictureService = idDocPictureService;
            _logger = logger;
        }

        /// <summary>
        /// Get existing document picture id
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <param name="index">Index of image side: O => Recto: 1 => Verso</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [SwaggerOperation("GetPlayerDocumentsPicture")]
        [SwaggerResponse(statusCode: 200, type: typeof(PlayerDocumentsPicturesResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PlayerDocumentsPicturesResponse>>> GetPlayerDocumentsPicture(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromRoute] string id,
            [FromRoute][Required][ValidIndex] int index)
        {
            try
            {
                _logger.LogTrace("Enter GetPlayerDocumentsPicture");

                var playerDocument = await _idDocPictureService.GetPlayerDocumentsPicture(playerId, id, index);
                return Ok(playerDocument.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPlayerDocumentsPicture service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPlayerDocumentsPicture] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPlayerDocumentsPicture");
            }
        }

        /// <summary>
        /// Create a new player id document
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <param name="index">Index of image side: O => Recto: 1 => Verso</param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [SwaggerOperation("CreatePlayerDocumentsPicture")]
        [SwaggerResponse(statusCode: 201, type: typeof(PlayerDocumentResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerDocumentsPicturesResponse>> CreatePlayerDocumentsPicture(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID"), Required] int siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id,
            [FromRoute][Required] int index,
            [FromBody] PlayerDocumentsPicturesPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostDocument : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}, playerId: {playerId}", correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var documentPicture = await _idDocPictureService.CreatePlayerDocumentsPicture(userId, correlationId, siteOrigin, body, playerId, id, index);
                return CreatedAtAction(nameof(GetPlayerDocumentsPicture), new { playerId, id, index }, documentPicture.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostDocument service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostDocument] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostDocument");
            }
        }

        /// <summary>
        /// Update a player id document 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <param name="index">Index of image side: O => Recto: 1 => Verso</param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [SwaggerOperation("UpdatePlayerDocumentsPicture")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdatePlayerDocumentsPicture(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID"), Required] int siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] PlayerDocumentsPicturesPUT body,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id,
            [FromRoute][Required] string index)
        {
            try
            {
                _logger.LogDebug("Enter UpdateDocument id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}", id, correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _idDocPictureService.EditPlayerDocumentsPicture(userId, correlationId, siteOrigin, id, body, playerId, index);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateDocument service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateDocument] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateDocument");
            }
        }

        /// <summary>
        /// Delete an player comment
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="index">Index of image side: O => Recto: 1 => Verso</param>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [SwaggerOperation("DeletePlayerDocumentsPicture")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeletePlayerDocumentsPicture(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id,
            [FromRoute][Required] string index)
        {
            try
            {
                _logger.LogDebug("Enter DeletePlayerDocumentsPicture id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", id, correlationId, siteOrigin, userId, playerId);

                var _ = await _idDocPictureService.DeletePlayerDocumentsPicture(userId, correlationId, siteOrigin, id, playerId, index);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeletePlayerDocumentsPicture service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeletePlayerDocumentsPicture] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeletePlayerDocumentsPicture");
            }
        }
    }
}
