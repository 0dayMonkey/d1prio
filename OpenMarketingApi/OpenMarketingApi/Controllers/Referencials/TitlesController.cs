using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Title;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [Route("/api/open-marketing/v{version:apiVersion}/titles")]
    [SwaggerTag("Referencial / Titles")]
    public class TitlesController : ControllerBase
    {
        private readonly ILogger<TitlesController> _logger;
        private readonly ITitleService _titleService;

        public TitlesController(ITitleService titleService, ILogger<TitlesController> logger)
        {
            _titleService = titleService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing titles
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [SwaggerOperation("GetTitles")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<TitleResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<TitleResponse>>> GetTitles([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogTrace("Enter GetTitles");

                var titles = await _titleService.GetAllTitles();
                return Ok(titles);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetTitles service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetTitles] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetTitles");
            }
        }

        /// <summary>
        /// Get existing title by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("{id}")]
        [SwaggerOperation("GetTitlesById")]
        [SwaggerResponse(statusCode: 200, type: typeof(TitleResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<TitleResponse>> GetTitlesById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetTitlesById : {id}", id);

                ActionResult<TitleResponse> title = await _titleService.GetTitleById(id);
                return Ok(title.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetTitlesById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetTitlesById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetTitlesById");
            }
        }

        /// <summary>
        /// Create new title
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
        [SwaggerOperation("PostTitle")]
        [SwaggerResponse(statusCode: 201, type: typeof(TitleResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<TitleResponse>> PostTitle(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody]TitlePOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostTitle : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var title = await _titleService.CreateTitle(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetTitlesById), new { id = title?.Value?.Id }, title?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostTitle service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostTitle] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostTitle");
            }
        }

        /// <summary>
        /// Update an title
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="titleId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("{titleId}")]
        [SwaggerOperation("UpdateTitle")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateTitle(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")]int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody]TitlePUT body, 
            [FromRoute][Required]string titleId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateTitle : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _titleService.EditTitle(userId, correlationId, siteOrigin, titleId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateTitle service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateTitle] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateTitle");
            }
        }

        /// <summary>
        /// Delete an title
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="titleId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("{titleId}")]
        [SwaggerOperation("DeleteTitle")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteTitle(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string titleId)
        {
            try
            {
                _logger.LogDebug("Enter DeleteTitle Id : {titleId}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", titleId, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);

                var _ = await _titleService.DeleteTitle(userId, correlationId, siteOrigin, titleId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteTitle service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteTitle] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteTitle");
            }
        }
    }
}
