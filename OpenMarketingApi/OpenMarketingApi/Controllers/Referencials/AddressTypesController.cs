using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Address types")]
    public class AddressTypesController : ControllerBase
    {
        private readonly ILogger<AddressTypesController> _logger;
        private readonly IAddressTypeService _addressTypeService;

        public AddressTypesController(IAddressTypeService addressTypeService, ILogger<AddressTypesController> logger)
        {
            _addressTypeService = addressTypeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all existing address types
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/address-types")]
        [SwaggerOperation("GetAddressTypes")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AddressType>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<AddressType>>> GetAddressTypes([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogTrace("Enter GetAddressTypes");

                var addressTypes = await _addressTypeService.GetAllAddressTypes();
                return Ok(addressTypes);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressTypes service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressTypes] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressTypes");
            }
        }

        /// <summary>
        /// Get existing address type by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/address-types/{id}")]
        [SwaggerOperation("GetAddressTypesById")]
        [SwaggerResponse(statusCode: 200, type: typeof(AddressType), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AddressType>> GetAddressTypesById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] int id)
        {
            try
            {
                _logger.LogDebug("Enter GetAddressTypesById : {id}", id);

                ActionResult<AddressType> addressType = await _addressTypeService.GetAddressTypeById(id);
                return Ok(addressType.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAddressTypesById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAddressTypesById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAddressTypesById");
            }
        }

        /// <summary>
        /// Create new address type
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
        [Route("/api/open-marketing/v{version:apiVersion}/address-types")]
        [SwaggerOperation("PostAddressType")]
        [SwaggerResponse(statusCode: 201, type: typeof(AddressType), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<AddressType>> PostAddressType(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody]AddressTypePOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostAddressType : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var addressTypes = await _addressTypeService.CreateAddressType(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetAddressTypesById), new { id = addressTypes?.Value?.Id }, addressTypes?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostAddressType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostAddressType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostAddressType");
            }
        }

        /// <summary>
        /// Update an address type
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="addressTypeId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/address-types/{addressTypeId}")]
        [SwaggerOperation("UpdateAddressType")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> UpdateAddressType(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")]int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody]AddressTypePUT body, 
            [FromRoute][Required]string addressTypeId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateAddressType : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _addressTypeService.EditAddressType(userId, correlationId, siteOrigin, addressTypeId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateAddressType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateAddressType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateAddressType");
            }
        }

        /// <summary>
        /// Delete an address type
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="addressTypeId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/address-types/{addressTypeId}")]
        [SwaggerOperation("DeleteAddressType")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> DeleteAddressType(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string addressTypeId)
        {
            try
            {
                _logger.LogDebug("Enter DeleteAddressType Id : {addressTypeId}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", addressTypeId, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);

                var _ = await _addressTypeService.DeleteAddressType(userId, correlationId, siteOrigin, addressTypeId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteAddressType service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteAddressType] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteAddressType");
            }
        }
    }
}
