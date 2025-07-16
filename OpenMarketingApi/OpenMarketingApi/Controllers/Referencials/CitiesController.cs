using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly ICityService _cityService;
        public CitiesController(ICityService cityService, ILogger<CitiesController> logger)
        {
            _cityService = cityService;
            _logger = logger;
        }

        /// <summary>
        /// Get referencial information for cities
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchModel">Search model object cities</param>
        /// <param name="countryId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/cities")]
        [SwaggerOperation("Get referencial information for cities for the specified country")]
        [SwaggerResponse(statusCode: 200, type: typeof(SearchResult<CityResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<SearchResult<CityResponse>>> GetCities(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("Country Identifier", Required = true)] string countryId,
            [FromQuery, Required, SwaggerParameter(@"<summary>Required Fields: First (0), Rows (min:1,max:1000).</summary>
            <summary>Fields allowed for sorting (Active): properties.</summary>
            <summary>Fields allowed for filtering (Field): properties.</summary>
            <summary>Direction allowed: asc, desc.</summary>
            <summary>MatchMode allowed: equal, in.</summary>
            <summary>Operator: and, or.</summary>", Required = true)] string searchModel = "{\"first\":0,\"rows\":10}")
        {
            try
            {
                _logger.LogTrace("Enter GetCities : CountryId : {countryId}", countryId);

                var cities = await _cityService.GetCities(searchModel, countryId);
                return Ok(cities.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCities service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCities] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCities");
            }
        }

        /// <summary>
        /// Get existing City by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/cities/{cityId}")]
        [SwaggerOperation("Get existing City by ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(CityResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<CityResponse>> GetCityById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("Country Identifier", Required = true)] string countryId,
            [FromRoute, SwaggerParameter("City Identifier", Required = true)] string cityId)
        {
            try
            {
                _logger.LogDebug("Enter GetCityById : {cityId}, countryId : {countryId}", cityId, countryId);

                ActionResult<CityResponse> country = await _cityService.GetCity(countryId, cityId);
                return Ok(country.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCityById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCityById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCityById");
            }
        }

        /// <summary>
        /// Add a city to the referencial
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="countryId"></param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/cities")]
        [SwaggerOperation("Add a city to the referencial")]
        [SwaggerResponse(statusCode: 201, type: typeof(CityResponse), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<CityResponse>> PostCity(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("Country Identifier", Required = true)] string countryId,
            [FromBody][Required] CityPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostCity : CountryId :{countryId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", countryId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var city = await _cityService.CreateCity(userId, correlationId, siteOrigin, countryId, body);
                return CreatedAtAction(nameof(GetCityById), new { countryId = countryId ,cityId = city?.Value?.Id }, city?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostCity service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostCity] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostCity");
            }
        }

        /// <summary>
        /// Update a city to the referencial
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="countryId"></param>
        /// <param name="cityId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/cities/{cityId}")]
        [SwaggerOperation("Update a city to the referencial")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> PutCity(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] CityPUT body, 
            [FromRoute, SwaggerParameter("Country Identifier", Required = true)] string countryId,
            [FromRoute, SwaggerParameter("City Identifier", Required = true)] string cityId)
        {
            try
            {
                _logger.LogDebug("Enter PutCity : {cityId}, CountryId :{countryId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", cityId, countryId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _cityService.UpdateCity(userId, correlationId, siteOrigin, countryId,cityId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PutCity service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PutCity] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PutCity");
            }
        }

        ///// <summary>
        ///// Delete a city
        ///// </summary>
        ///// <param name="correlationId"></param>
        ///// <param name="siteOrigin"></param>
        ///// <param name="countryId"></param>
        ///// <param name="cityId"></param>
        ///// <response code="204">Success - No Content</response>
        ///// <response code="400">Bad Request</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Error</response>
        //[HttpDelete]
        //[ApiVersion("1")]
        //[Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}/cities/{cityId}")]
        //[SwaggerOperation("DeleteCity")]
        //[SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        //[SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        //[SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        //[SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        //[SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        //public async Task<ActionResult> DeleteCity([FromHeader(Name = "Correlation-ID")] string? correlationId, [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin, [FromRoute][Required] string countryId, [FromRoute][Required] string cityId)
        //{
        //    _logger.LogDebug("DeletedCity {0}, correlationId: {correlationId}, siteOrigin: {siteOrigin}, countryId: {3}", cityId, correlationId, siteOrigin, countryId);
        //    try
        //    {
        //        var _ = await _cityService.DeleteCity(userId, correlationId, siteOrigin, countryId, cityId);
        //        return NoContent();
        //    }
        //    catch (ServiceException se)
        //    {
        //        _logger.LogError(se, "DeletedCity service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
        //        return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        //    }
        //}
    }
}
