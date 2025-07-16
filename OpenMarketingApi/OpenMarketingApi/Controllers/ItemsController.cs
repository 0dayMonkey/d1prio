namespace OpenMarketingApi.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "open-marketing")]
[SwaggerTag("Retail / Items")]
[Route("api/open-marketing/v{version:apiVersion}/store")]
public class ItemsController : ControllerBase
{
    private readonly IItemsService _itemsService;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IItemsService itemsService, ILogger<ItemsController> logger)
    {
        _itemsService = itemsService;
        _logger = logger;
    }

    [HttpGet]
    [ApiVersion("1")]
    [SwaggerOperation(
        Summary = "Get the picture",
        Description = "Given a CSM Item Identifier, retrieves the item's picture."
    )]
    [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Success")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request (the item identifier is required)")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized (need authentification)")]
    [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found (the item identifier or the picture does not exist)")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    [Route("items/{itemId}/picture")]
    public async Task<ActionResult<byte[]>> GetPictureAsync([SwaggerParameter("The CMS Item Identifier", Required = true)]string itemId)
    {
        try
        {
            _logger.LogDebug("Enter GetPictureAsync {itemId}", itemId);

            byte[] picture = await _itemsService.GetPictureAsync(itemId).ConfigureAwait(false);
            if (picture == null)
            {
                string err = $"Picture not found: itemId {itemId} not found";
                _logger.LogError(err);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status404NotFound, new Exception(err), HttpContext.Request.Path);
            }
            return Ok(picture);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetPictureAsync service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetPictureAsync] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetPictureAsync");
        }
    }

    [HttpGet]
    [ApiVersion("1")]
    [SwaggerOperation(
        Summary = "Get the store stock",
        Description = "Given a CMS store identifier, retrieves the paginated list of items."
    )]
    [SwaggerResponse(statusCode: 200, type: typeof(SearchResult<StoreRetailItem>), description: "Success")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request (the store identifier is missing or the searchModel is invalid)")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized (need authentification)")]
    [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found (the store identifier is unknown)")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
    [Route("{storeId}/items")]
    public async Task<ActionResult<SearchResult<StoreRetailItem>>> GetStoreStockAsync(
        [SwaggerParameter("The CMS Store Identifier", Required = true)]string storeId,
        [FromQuery, SwaggerParameter(@"<summary>Required Fields: First (0), Rows (min:1,max:1000).</summary>
<summary>Fields allowed for sorting (Active): id, currentPriceInPoints, quantityLeft. By default sorting on id asc.</summary>
<summary>Fields allowed for filtering (Field): id, longLabel, inStock, currentPriceInPoints, quantityLeft.</summary>
<summary>Direction allowed: asc, desc.</summary>
<summary>MatchMode allowed: equal, in.</summary>
<summary>Operator: and, or.</summary>", Required = true)]string searchModel = "{first:0,rows:10}")
    {
        try
        {
            _logger.LogDebug("Enter GetStoreStockAsync {storeId}", storeId);

            SearchResult<StoreRetailItem> storeRetailItem = await _itemsService.GetStoreStockAsync(storeId, searchModel).ConfigureAwait(false);
            return Ok(storeRetailItem);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetStoreStockAsync service exception: {statusCode} - {message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetStoreStockAsync] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetStoreStockAsync");
        }
    }
}
