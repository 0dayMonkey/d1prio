using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Address level structure")]
    public class AddressLevelStructuresController : ControllerBase
    {
        private readonly ILogger<AddressLevelStructuresController> _logger;
        private readonly IAddressLevelStructureService _AddressLevelStructureService;

        public AddressLevelStructuresController(IAddressLevelStructureService AddressLevelStructureService, ILogger<AddressLevelStructuresController> logger)
        {
            _AddressLevelStructureService = AddressLevelStructureService;
            _logger = logger;
        }

        /// <summary>
        /// Get address level structure
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-level-structure")]
        [SwaggerOperation("Get specified addressLevels from Country")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AddressLevelStructureResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<AddressLevelStructureResponse>>> GetAddressLevelStructure(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string countryId)
        {
            try
            {
                _logger.LogTrace("Enter GetAddressLevelStructures from country id : {countryID}", countryId);

                var AddressLevelStructures = await _AddressLevelStructureService.GetAddressLevelStructureFromCountry(countryId);
                return Ok(AddressLevelStructures);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressLevelStructures service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressLevelStructures] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressLevelStructures");
            }
        }

        /// <summary>
        /// Post address level structure
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-level-structure")]
        [SwaggerOperation("Create a new addressLevel")]
        [SwaggerResponse(statusCode: 200, type: typeof(AddressLevelStructureResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AddressLevelStructureResponse>> CreateAddressLevelStructure(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string countryId,
            [FromBody] AddressLevelStructurePOST body)
        {
            try
            {
                _logger.LogDebug("Enter CreateAddressLevelStructure for countryId {countryId}", countryId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                ActionResult<AddressLevelStructureResponse> AddressLevelStructure = await _AddressLevelStructureService.CreateAddressLevelStructureForCountry(userId, correlationId, siteOrigin, body, countryId);
                return Ok(AddressLevelStructure.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "CreateAddressLevelStructure service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[CreateAddressLevelStructure] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit CreateAddressLevelStructure");
            }
        }

        /// <summary>
        /// Update an address level structure
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="countryId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/address-level-structure")]
        [SwaggerOperation("Update an address level structure")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateAddressLevelStructure(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] AddressLevelStructurePUT body,
            [FromRoute][Required] string countryId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateAddressLevelStructure : {countryId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", countryId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _AddressLevelStructureService.UpdateAddressLevelStructureForCountry(userId, correlationId, siteOrigin, body, countryId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateAddressLevelStructure service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateAddressLevelStructure] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateAddressLevelStructure");
            }
        }
    }
}
