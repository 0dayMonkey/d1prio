using Framework.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using OpenPromoApi.Interfaces;
using OpenPromoApi.Models;

namespace OpenPromoApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-promo")]
    [SwaggerTag("Promotions")]
    public class PromotionsController : ControllerBase
    {
        private readonly ILogger<PromotionsController> _logger;
        private readonly IPromoService _promoService;
        public PromotionsController(IPromoService credentialService, ILogger<PromotionsController> logger)
        {
            _promoService = credentialService;
            _logger = logger;
        }

        /// <summary>
        /// Get current promotions of customer ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allowedSiteId"></param>
        /// <param name="customerId">The customer identifier</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-promo/v{version:apiVersion}/promotions")]
        [SwaggerOperation("Get existing promotions for customer allowed in specific site")]
        [SwaggerResponse(statusCode: 200, type: typeof(PromotionResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PromotionResponse>>> GetAllPromotionsForCustomer(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromQuery, Range(1, 9999, ErrorMessage = "Allowed site ID must be between 1 and 9999."), SwaggerParameter("Site ID", Required = true)] int allowedSiteId,
            [FromQuery, SwaggerParameter("Customer ID", Required = true)] string customerId)
        {
            try
            {
                _logger.LogDebug("Enter GetAllPromotionsForCustomer : {CustomerId}, userId :{userID}, siteId : {siteId}", customerId, userId, allowedSiteId);

                List<PromotionResponse> promos = await _promoService.GetAllPromotionsForCustomer(allowedSiteId, customerId, userId);
                return Ok(promos);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetAllPromotionsForCustomer service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetAllPromotionsForCustomer] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetAllPromotionsForCustomer");
            }
        }

        /// <summary>
        /// Get current promotion by ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="allowedSiteId"></param>
        /// <param name="id">The Promotion Identifier</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-promo/v{version:apiVersion}/promotions/{id}")]
        [SwaggerOperation("Get existing promotion by ID and allowed site id")]
        [SwaggerResponse(statusCode: 200, type: typeof(PromotionResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<PromotionResponse>> GetPromotionById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromQuery, Range(1, 9999, ErrorMessage = "Allowed site ID must be between 1 and 9999."),SwaggerParameter("Site ID", Required = true)] int allowedSiteId,
            [FromRoute, SwaggerParameter("Promotion ID", Required = true)] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetPromotionById : {promoId}, userId :{userID}, siteId : {siteId}", id, userId, allowedSiteId);

                PromotionResponse promo = await _promoService.GetPromotionById(allowedSiteId, id, userId);
                if (promo == null)
                {
                    _logger.LogError("Promotion with id {0} Not found", id);
                    return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status404NotFound, "Not found", $"Promotion with id {id} Not found for allowed site {allowedSiteId}", HttpContext.Request.Path);
                }
                return Ok(promo);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPromotionById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPromotionById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPromotionById");
            }
        }

        /// <summary>
        /// Patch promotion status
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="id"></param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Internal Error</response>
        [HttpPatch]
        [ApiVersion("1")]
        [Route("/api/open-promo/v{version:apiVersion}/promotions/{id}/status")]
        [SwaggerOperation("Patch promotion status")]
        [SwaggerResponse(statusCode: 204, description: "Success. The promotion has been successfully updated")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 422, type: typeof(ProblemDetails), description: "Unprocessable Entity")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<PromotionResponse>>> PatchPromotionStatus(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("Promotion ID", Required = true)] string id,
            [FromBody][Required] PromotionStatusPATCH body)
        {
            try
            {
                _logger.LogDebug("Enter PatchPromotionStatus :  CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var changeContext = new ChangeContext(correlationId, siteOrigin);
                await _promoService.PatchPromotionStatus(body, id, userId, changeContext);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PatchPromotionStatus service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PatchPromotionStatus] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PatchPromotionStatus");
            }
        }
    }
}
