namespace OpenMarketingApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "open-marketing")]
[Route("api/open-marketing/v{version:apiVersion}/store")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersServices _ordersServices;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrdersServices ordersServices, ILogger<OrdersController> logger)
    {
        _ordersServices = ordersServices;
        _logger = logger;
    }

    
    [HttpGet]
    [ApiVersion("1")]
    [SwaggerResponse(statusCode: 200, "Success", typeof(SearchResult<Order>))]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request (Check your criteria)")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized (need authentification)")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal error")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [SwaggerOperation(Summary = "Get Orders data for a player", Description= "search criteria in json string format.")]
    [Route("orders")]
    public async Task<IActionResult> GetOrdersAsync(
        [FromQuery, SwaggerParameter(description: @"<summary>Required Fields: First (0), Rows (min:1,max:1000) </summary>
                                        <summary>Fields allowed for sorting (Active): CreationTimestamp, status. By default sorting on CreationTimestamp desc </summary>
                                        <summary>Fields allowed for filtering (Field): StoreId, PlayerId, Status, ItemId, OrderId, CreationTimestam.</summary>
                                        <summary>Direction allowed: asc, desc.</summary>
                                        <summary>MatchMode allowed: equal, in. For the date: dateAfter, dateBefore.</summary>
                                        <summary>Operator: and, or.</summary>",
                        Required =true
        )]
        string searchModel = "{first:0,rows:10}")
    {
        try
        {
            _logger.LogDebug("Enter GetOrdersAsync ");

            var orders = await _ordersServices.GetOrdersAsync(searchModel).ConfigureAwait(false);

            return Ok(orders);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "GetOrdersAsync service ServiceException: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[GetOrdersAsync] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit GetOrdersAsync");
        }
    }

    [HttpPost]
    [ApiVersion("1")]
    [SwaggerOperation("Create an order")]
    [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(Order))]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request (the order creation request is not correct)")]
    [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized (need authentification)")]
    [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden (another order with the same playerId or storeId already exists)")]
    [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal error")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Route("orders")]
    public async Task<IActionResult> CreateOrderAsync([FromBody][Required] OrderCommand body)
    {
        try
        {
            _logger.LogDebug("Enter CreateOrderAsync called with {body}", JsonSerializer.Serialize(body));
            _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
            if (!ModelState.IsValid)
            {
                _logger.LogError("Create order bad request: model is invalid");
                return BadRequest(ModelState);
            }
            var orders = await _ordersServices.CreateOrderAsync(body).ConfigureAwait(false);

            return Created("", orders);
        }
        catch (ServiceException se)
        {
            _logger.LogError(se, "CreateOrderAsync service ServiceException: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
            return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[CreateOrderAsync] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
            return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
        }
        finally
        {
            _logger.LogTrace("Exit CreateOrderAsync");
        }
    }
}