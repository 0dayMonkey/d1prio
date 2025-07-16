using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Address levels")]
    public class AddressLevelsController : ControllerBase
    {
        private readonly ILogger<AddressLevelsController> _logger;
        private readonly IAddressLevelService _AddressLevelService;

        public AddressLevelsController(IAddressLevelService AddressLevelService, ILogger<AddressLevelsController> logger)
        {
            _AddressLevelService = AddressLevelService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing address levels
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-levels/{level}")]
        [SwaggerOperation("Get specified addressLevels from Country")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AddressLevelResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<AddressLevelResponse>>> GetAddressLevel(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string countryId,
            [FromRoute][Required] int level)
        {
            try
            {
                _logger.LogTrace("Enter GetAddressLevel : {level} from country id : {countryID}", level, countryId);

                var AddressLevels = await _AddressLevelService.GetAddressLevelFromCountry(countryId, level);
                return Ok(AddressLevels);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressLevel service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressLevel] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressLevel");
            }
        }

        /// <summary>
        /// Post address level
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-levels/{level}")]
        [SwaggerOperation("Create a new addressLevel")]
        [SwaggerResponse(statusCode: 200, type: typeof(AddressLevelResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AddressLevelResponse>> CreateAddressLevel(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string countryId,
            [FromRoute][Required] int level,
            [FromBody] AddressLevelPOST body)
        {
            try
            {
                _logger.LogDebug("Enter CreateAddressLevel : {level} for countryId {countryId}", level, countryId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                ActionResult<AddressLevelResponse> AddressLevel = await _AddressLevelService.CreateAddressLevelForCountry(userId, correlationId, siteOrigin, body, countryId, level);
                return Ok(AddressLevel.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "CreateAddressLevel service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[CreateAddressLevel] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit CreateAddressLevel");
            }
        }

        /// <summary>
        /// Update a address level 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="countryId"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-levels/{level}/{id}")]
        [SwaggerOperation("UpdateAddressLevel")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateAddressLevel(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] AddressLevelPUT body,
            [FromRoute][Required] string countryId,
            [FromRoute][Required] int level,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter UpdateAddressLevel : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, countryId: {countryId}, level: {level}", id, correlationId, siteOrigin, userId, countryId, level);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _AddressLevelService.UpdateAddressLevelForCountry(userId, correlationId, siteOrigin, body, countryId, level, id);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateAddressLevel service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateAddressLevel] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateAddressLevel");
            }
        }

        /// <summary>
        /// Delete an address level
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="countryId"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-levels/{level}/{id}")]
        [SwaggerOperation("DeleteAddressLevel")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteAddressLevel(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string countryId,
            [FromRoute][Required] int level,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter DeleteAddressLevel id : {id}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, countryId: {countryId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}, level : {level}", id, correlationId, siteOrigin, userId, countryId, deleteAction.LastUpdatedTimestamp, level);

                var _ = await _AddressLevelService.DeleteAddressLevelForCountry(userId, correlationId, siteOrigin, deleteAction, countryId, level, id);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteAddressLevel service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteAddressLevel] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteAddressLevel");
            }
        }
    }
}
