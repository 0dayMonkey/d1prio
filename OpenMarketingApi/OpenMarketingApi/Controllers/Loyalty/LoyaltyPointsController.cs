using OpenMarketingApi.Interfaces.Loyalty;
using OpenMarketingApi.Models.Loyalty.Point;

namespace OpenMarketingApi.Controllers.Loyalty
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [Route("/api/open-marketing/v{version:apiVersion}/accounts")]
    [SwaggerTag("Loyalty / Points")]
    public class LoyaltyPointsController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<LoyaltyPointsController> _logger;

        public LoyaltyPointsController(ILoyaltyService loyaltyService,ILogger<LoyaltyPointsController> logger)
        {
            _loyaltyService = loyaltyService;
            _logger = logger;
        }

        /// <summary>
        /// Search points transaction history
        /// </summary>
        /// <remarks>Get points transaction history for a player</remarks>
        /// <param name="userId"></param>
        /// <param name="playerId"></param>
        /// <param name="searchModel"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("{playerId}/points-transactions")]
        [SwaggerOperation("Search points transaction history")]
        [SwaggerResponse(statusCode: 200, type: typeof(SearchResult<PointTransaction>), description: "ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "InternalError")]
        public async Task<ActionResult<SearchResult<PointTransaction>>> GetPointsTransactionHistory(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true)] string playerId, 
            [FromQuery][Required] string searchModel)
        {
            try
            {
                _logger.LogDebug("Enter GetPointsTransactionHistory : {playerId}", playerId);

                ActionResult<SearchResult<PointTransaction>> pointTransactions = await _loyaltyService.GetPointTransactions(playerId, searchModel);
                return Ok(pointTransactions.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPointsTransactionHistory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPointsTransactionHistory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPointsTransactionHistory");
            }
        }

        /// <summary>
        /// Get Point Conversion
        /// </summary>
        /// <remarks>Get point conversion info for a player</remarks>
        /// <param name="playerId"></param>
        /// <param name="userId"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("{playerId}/conversion-info")]
        [SwaggerOperation("Get Point Conversion")]
        [SwaggerResponse(statusCode: 200, type: typeof(PointConversion), description: "ok")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "InternalError")]
        public async Task<ActionResult<PointConversion>> GetPointsConversion(
            [FromRoute, SwaggerParameter("The Casino Management System Player Identifier", Required = true)] string playerId,
            [FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                
                _logger.LogDebug("Enter GetPointsConversion : {playerId}", playerId);

                ActionResult<PointConversion> pointConversion = await _loyaltyService.GetPointsConversion(playerId);
                return Ok(pointConversion.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetPointsConversion service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetPointsConversion] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetPointsConversion");
            }
        }

        /// <summary>
        /// Create loyalty point transactions for the specified player account
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="playerId"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/accounts/{playerId}/points-transactions")]
        [SwaggerOperation("PostPointTransaction")]
        [SwaggerResponse(statusCode: 201, type: typeof(PointTransaction), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public virtual async Task<ActionResult<PointTransaction>> PostPointTransactionAsync(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] PointTransactionBasePOST body, 
            [FromRoute][Required] string playerId)
        {
            try
            {
                _logger.LogDebug("Enter PostPointTransactionAsync : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, playerId: {playerId}", correlationId, siteOriginId, userId, playerId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                PointTransaction pointConversion = await _loyaltyService.CreatePointTransaction(userId, correlationId, siteOriginId, playerId, body);
                return Created("", pointConversion);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostPointTransactionAsync service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostPointTransactionAsync] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostPointTransactionAsync");
            }
        }
    }
}
