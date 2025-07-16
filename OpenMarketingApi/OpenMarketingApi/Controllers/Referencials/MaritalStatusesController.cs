using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.MaritalStatus;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Marital Statuses")]
    public class MaritalStatusesController : ControllerBase
    {
        private readonly ILogger<MaritalStatusesController> _logger;
        private readonly IMaritalStatusService _maritalStatusService;

        public MaritalStatusesController(IMaritalStatusService maritalStatusService, ILogger<MaritalStatusesController> logger)
        {
            _maritalStatusService = maritalStatusService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing Marital Statuses
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/marital-statuses")]
        [SwaggerOperation("Get all existing Marital Statuses")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<MaritalStatus>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<MaritalStatus>>> GetAllMaritalStatuses([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogTrace("Enter GetAllMaritalStatuses");

                var maritalStatuses = await _maritalStatusService.GetAllMaritalStatuses();
                return Ok(maritalStatuses);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAllMaritalStatuses service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAllMaritalStatuses] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAllMaritalStatuses");
            }
        }

        /// <summary>
        /// Get existing marital status by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/marital-statuses/{id}")]
        [SwaggerOperation("Get existing marital status by ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(MaritalStatus), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<MaritalStatus>> GetMaritalStatus(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetMaritalStatusById : {id}", id);

                ActionResult<MaritalStatus> maritalStatus = await _maritalStatusService.GetMaritalStatus(id);
                return Ok(maritalStatus.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetMaritalStatus service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetMaritalStatus] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetMaritalStatus");
            }
        }

        /// <summary>
        /// Create new marital status
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
        [Route("/api/open-marketing/v{version:apiVersion}/marital-statuses")]
        [SwaggerOperation("Create new marital status")]
        [SwaggerResponse(statusCode: 201, type: typeof(MaritalStatus), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<MaritalStatus>> PostMaritalStatus(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] MaritalStatusPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostMaritalStatus :  CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var maritalStatus = await _maritalStatusService.CreateMaritalStatus(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetMaritalStatus), new { id = maritalStatus?.Value?.Id }, maritalStatus?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostMaritalStatus service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostMaritalStatus] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostMaritalStatus");
            }
        }

        /// <summary>
        /// Update a marital status
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/marital-statuses/{id}")]
        [SwaggerOperation("Update a marital status")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateMaritalStatus(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] MaritalStatusPUT body,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter UpdateMaritalStatus : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", id, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _maritalStatusService.EditMaritalStatus(userId, correlationId, siteOrigin, id, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateMaritalStatus service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateMaritalStatus] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateMaritalStatus");
            }
        }

        /// <summary>
        /// Delete a marital status
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/marital-statuses/{id}")]
        [SwaggerOperation("Delete a marital status")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeletedMaritalStatus(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter DeletedMaritalStatus id : {id},  CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}",id, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);

                var _ = await _maritalStatusService.DeleteMaritalStatus(userId, correlationId, siteOrigin, id, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeletedMaritalStatus service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeletedMaritalStatus] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeletedMaritalStatus");
            }
        }
    }
}
