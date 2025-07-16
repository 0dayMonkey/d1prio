using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.IdentityDocument;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Identity Documents")]
    public class IdentityDocumentTypesController : ControllerBase
    {
        private readonly ILogger<IdentityDocumentTypesController> _logger;
        private readonly IIdentityDocumentService _identityDocumentService;

        public IdentityDocumentTypesController(IIdentityDocumentService identityDocumentService, ILogger<IdentityDocumentTypesController> logger)
        {
            _identityDocumentService = identityDocumentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing ID document types
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/id-document-types")]
        [SwaggerOperation("Get all existing ID document types")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<IdDocumentType>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<IdDocumentType>>> GetIdDocumentTypes([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            _logger.LogDebug("Enter GetIdDocumentTypes");
            try
            {
                var idDocumentTypes = await _identityDocumentService.GetAllIdentityDocumentType();
                return Ok(idDocumentTypes);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetIdDocumentTypes service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetIdDocumentTypes] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetIdDocumentTypes");
            }
        }

        /// <summary>
        /// Get existing ID document types by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/id-document-types/{id}")]
        [SwaggerOperation("Get existing ID document types by ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(IdDocumentType), description: "Success")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<IdDocumentType>> GetIdDocumentTypesById(
            [FromHeader(Name = "API-User-ID"), Required] string userId, 
            [FromRoute][Required] string id)
        {
            _logger.LogDebug("Enter GetIdDocumentTypesById : {id}", id);
            try
            {
                ActionResult<IdDocumentType> idDocumentType = await _identityDocumentService.GetIdentityDocumentTypeAsync(id);
                return Ok(idDocumentType.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetIdDocumentTypesById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetIdDocumentTypesById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetIdDocumentTypesById");
            }
        }

        /// <summary>
        /// Create new document types
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/id-document-types")]
        [SwaggerOperation("Create new document types")]
        [SwaggerResponse(statusCode: 201, type: typeof(IdDocumentType), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<IdDocumentType>> PostDocumentTypes(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] IdDocumentTypePOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostDocumentTypes : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var idDocumentTypes = await _identityDocumentService.CreateIdentityDocumentTypeAsync(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetIdDocumentTypesById), new { id = idDocumentTypes?.Value?.Id }, idDocumentTypes?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostDocumentTypes service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostDocumentTypes] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostDocumentTypes");
            }
        }

        /// <summary>
        /// Update an ID Document Type
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="documentTypeId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/id-document-types/{documentTypeId}")]
        [SwaggerOperation("Update an ID Document Type")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateIdDocumentType(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] IdDocumentTypePUT body,
            [FromRoute][Required] string documentTypeId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateIdDocumentType : {documentTypeId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", documentTypeId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _identityDocumentService.EditIdentityDocumentTypeAsync(userId, correlationId, siteOrigin, documentTypeId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateIdDocumentType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateIdDocumentType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateIdDocumentType");
            }
        }

        /// <summary>
        /// Delete an id document type
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="documentTypeId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/id-document-types/{documentTypeId}")]
        [SwaggerOperation("Delete an id document type")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteIdDocumentType(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string documentTypeId)
        {
            _logger.LogDebug("Enter DeleteIdDocumentType id : {documentTypeId}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", documentTypeId, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);
            try
            {
                var _ = await _identityDocumentService.DeleteIdentityDocumentType(userId, correlationId, siteOrigin, documentTypeId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteIdDocumentType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteIdDocumentType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteIdDocumentType");
            }
        }
    }
}
