using OpenMarketingApi.Interfaces.Loyalty;
using OpenMarketingApi.Models.Loyalty.Configuration;
using OpenMarketingApi.Models.Loyalty.Tier;

namespace OpenMarketingApi.Controllers.Loyalty;

[ApiController]
[ApiExplorerSettings(GroupName = "open-marketing")]
[Route("/api/open-marketing/v{version:apiVersion}/tiers")]
[SwaggerTag("Loyalty / Tiers")]
public class LoyaltyTiersController : ControllerBase
{
    private readonly ILoyaltyService _loyaltyService;
    private readonly ILogger<LoyaltyTiersController> _logger;

    public LoyaltyTiersController(ILoyaltyService loyaltyService, ILogger<LoyaltyTiersController> logger)
    {
        _loyaltyService = loyaltyService;
        _logger = logger;
    }

    /// <summary>
    /// Get available player tiers defined in the system
    /// </summary>
    /// <response code="200">ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal Error</response>
    [HttpGet]
    [ApiVersion("1")]
    [SwaggerOperation("Get available player tiers defined in the system")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<TierResponse>), description: "ok")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult<List<TierResponse>>> GetLoyaltyTiers(
        [FromHeader(Name = "API-User-ID"), Required] string userId)
    {
        try
        {
            
            _logger.LogDebug("Enter GetLoyaltyTiers");

            ActionResult<List<TierResponse>> tier = await _loyaltyService.GetLoyaltyTiers();
            return Ok(tier.Value);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetLoyaltyTiers service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetLoyaltyTiers] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetLoyaltyTiers");
        }
    }

    /// <summary>
    /// Get loyalty tier details
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tierId"></param>
    /// <response code="200">ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Error</response>
    [HttpGet("{tierId}")]
    [ApiVersion("1")]
    [SwaggerOperation("Get loyalty tier details")]
    [SwaggerResponse(statusCode: 200, type: typeof(TierResponse), description: "ok")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult<TierResponse>> GetLoyaltyTier(
        [FromHeader(Name = "API-User-ID"), Required] string userId,
        [FromRoute, SwaggerParameter("The Casino Management System Tier Identifier", Required = true)] string tierId)
    {
        try
        {
            
            _logger.LogDebug("Enter GetLoyaltyTier");

            ActionResult<TierResponse> tier = await _loyaltyService.GetLoyaltyTier(tierId);
            return Ok(tier.Value);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetLoyaltyTiers service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetLoyaltyTier] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetLoyaltyTier");
        }
    }

    /// <summary>
    /// Create a new loyalty tier to the club
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="siteOriginId"></param>
    /// <param name="userId"></param>
    /// <param name="body"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Error</response>
    [HttpPost]
    [ApiVersion("1")]
    [SwaggerOperation("Create a new loyalty tier to the club")]
    [SwaggerResponse(statusCode: 201, type: typeof(TierResponse), description: "Created")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult<TierResponse>> PostLoyaltyTier(
        [FromHeader(Name = "Correlation-ID")] string? correlationId,
        [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
        [FromHeader(Name = "API-User-ID"), Required] string userId,
        [FromBody, Required] TierPOST body)
    {
        try
        {
            _logger.LogDebug("Enter PostLoyaltyTier CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}", correlationId, siteOriginId, userId);
            _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
            ActionResult<TierResponse> tier = await _loyaltyService.CreateLoyaltyTier(userId, correlationId, siteOriginId, body);
            return CreatedAtAction(nameof(GetLoyaltyTier), new { tierId = tier?.Value?.Id }, tier?.Value);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "PostLoyaltyTier service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[PostLoyaltyTier] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit PostLoyaltyTier");
        }
    }

    /// <summary>
    /// Update loyalty tier details
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="siteOriginId"></param>
    /// <param name="userId"></param>
    /// <param name="body"></param>
    /// <param name="tierId"></param>
    /// <response code="204">Success - No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Error</response>
    [HttpPut("{tierId}")]
    [ApiVersion("1")]
    [SwaggerOperation("Update loyalty tier details")]
    [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden - the loyalty tier long label already exists.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult> PutLoyaltyTier(
        [FromHeader(Name = "Correlation-ID")] string? correlationId,
        [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
        [FromHeader(Name = "API-User-ID"), Required] string userId,
        [FromBody, Required] TierPUT body,
        [FromRoute, SwaggerParameter("The Casino Management System Tier Identifier", Required = true)] string tierId)
    {
        try
        {
            
            _logger.LogDebug("Enter PutLoyaltyTier id : {tierId} CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}", tierId, correlationId, siteOriginId, userId);

            ActionResult _ = await _loyaltyService.UpdateLoyaltyTier(userId, correlationId, siteOriginId, tierId, body);
            return NoContent();
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "PutLoyaltyTier service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[PutLoyaltyTier] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit PutLoyaltyTier");
        }
    }

    /// <summary>
    /// Get loyalty tier configuration
    /// </summary>
    /// <response code="200">ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal Error</response>
    [HttpGet("configuration")]
    [ApiVersion("1")]
    [SwaggerOperation("Get loyalty tier configuration")]
    [SwaggerResponse(statusCode: 200, type: typeof(ConfigurationResponse), description: "ok")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult<ConfigurationResponse>> GetLoyaltyTierConfiguration()
    {
        try
        {
            
            _logger.LogDebug("Enter GetLoyaltyTierConfiguration");

            ActionResult<ConfigurationResponse> tier = await _loyaltyService.GetLoyaltyTierConfiguration();
            return Ok(tier.Value);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetLoyaltyTierConfiguration service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetLoyaltyTierConfiguration] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetLoyaltyTierConfiguration");
        }
    }

    /// <summary>
    /// Update loyalty tier configuration
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="siteOriginId"></param>
    /// <param name="userId"></param>
    /// <param name="body"></param>
    /// <response code="204">Success - No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Error</response>
    [HttpPut("configuration")]
    [ApiVersion("1")]
    [SwaggerOperation("Update loyalty tier configuration")]
    [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
    [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not found")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    public async Task<ActionResult> PutLoyaltyTierConfiguration(
        [FromHeader(Name = "Correlation-ID")] string? correlationId,
        [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
        [FromHeader(Name = "API-User-ID"), Required] string userId,
        [FromBody, Required] ConfigurationPUT body)
    {
        try
        {
            
            _logger.LogDebug("Enter PutLoyaltyTierConfiguration : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}", correlationId, siteOriginId, userId);

            ActionResult _ = await _loyaltyService.UpdateLoyaltyTierConfiguration(userId, correlationId, siteOriginId, body);
            return NoContent();
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "PutLoyaltyTierConfiguration service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[PutLoyaltyTierConfiguration] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit PutLoyaltyTierConfiguration");
        }
    }
}