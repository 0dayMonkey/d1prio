using OpenMarketingApi.Interfaces.Players;
using OpenMarketingApi.Models.Player.Document;
using OpenMarketingApi.Models.ProblemsDetails;

namespace OpenMarketingApi.Controllers.Players
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Player / ID Documents")]
    public class PlayerDocumentsController : ControllerBase
    {
        private readonly ILogger<PlayerDocumentsController> _logger;
        private readonly IPlayerDocumentsService _playerDocumentService;

        public PlayerDocumentsController(IPlayerDocumentsService playerDocumentService, ILogger<PlayerDocumentsController> logger)
        {
            _playerDocumentService = playerDocumentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing player id documents
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents")]
        [SwaggerOperation("GetDocuments")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PlayerDocumentResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PlayerDocumentResponse>>> GetDocuments(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogTrace("Enter GetDocuments");

                var playerDocument = await _playerDocumentService.GetAllDocument(playerId);
                return Ok(playerDocument);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetDocuments service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetDocuments] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetDocuments");
            }
        }

        /// <summary>
        /// Get existing player id document by ID
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents/{id}")]
        [SwaggerOperation("GetDocumentById")]
        [SwaggerResponse(statusCode: 200, type: typeof(PlayerDocumentResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerDocumentResponse>> GetDocumentById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetDocumentById : {id}, playerId {playerId}", id, playerId);

                ActionResult<PlayerDocumentResponse> playerDocument = await _playerDocumentService.GetDocumentById(id, playerId);
                return Ok(playerDocument.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetDocumentById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetDocumentById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetDocumentById");
            }
        }

        /// <summary>
        /// Create a new player id document
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents")]
        [SwaggerOperation("PostDocument")]
        [SwaggerResponse(statusCode: 201, type: typeof(PlayerDocumentResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 403, type: typeof(I18NProblemDetails), description: $"Forbidden - {ProblemDetailsType.IdDocI18N}")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PlayerDocumentResponse>> PostDocument(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string playerId,
            [FromBody] PlayerDocumentPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostDocument : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}, playerId: {playerId}", correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var playerDocument = await _playerDocumentService.CreateDocument(userId, correlationId, siteOrigin, body, playerId);
                return CreatedAtAction(nameof(GetDocumentById), new { playerId, id = playerDocument?.Value?.Id }, playerDocument?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostDocument service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (I18NViolationException exc)
            {
                _logger.LogError(exc, "I18NViolationException PostDocument service exception");
                var problemDetails = new I18NProblemDetails(StatusCodes.Status403Forbidden, HttpContext.Request.Path, exc);

                return ErrorHelper.ResponseExcRfc7807(problemDetails);
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
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents/{id}")]
        [SwaggerOperation("UpdateDocument")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 403, type: typeof(I18NProblemDetails), description: $"Forbidden - {ProblemDetailsType.IdDocI18N}")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateDocument(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] PlayerDocumentPUT body,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter UpdateDocument id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}", id, correlationId, siteOrigin, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _playerDocumentService.EditDocument(userId, correlationId, siteOrigin, id, body, playerId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateDocument service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (I18NViolationException exc)
            {
                _logger.LogError(exc, "I18NViolationException UpdateDocument service exception");
                var problemDetails = new I18NProblemDetails(StatusCodes.Status403Forbidden, HttpContext.Request.Path, exc);

                return ErrorHelper.ResponseExcRfc7807(problemDetails);
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
        /// Delete an player id document
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
        [Route("/api/open-marketing/v{version:apiVersion}/players/{playerId}/id-documents/{id}")]
        [SwaggerOperation("DeleteDocument")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteDocument(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string playerId,
            [FromRoute][Required] string id)
        {
            _logger.LogDebug("Enter DeleteDocument id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, playerId: {playerId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}",id, correlationId, siteOrigin, userId, playerId, deleteAction.LastUpdatedTimestamp);
            try
            {
                var _ = await _playerDocumentService.DeleteDocument(userId, correlationId, siteOrigin, id, playerId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteDocument service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteDocument] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteDocument");
            }
        }
    }
}
