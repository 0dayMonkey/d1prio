using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly ICountryService _countryService;
        public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        /// <summary>
        /// Get referencial information for countries
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchModel">Search model object countries</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries")]
        [SwaggerOperation("Get referencial information for countries")]
        [SwaggerResponse(statusCode: 200, type: typeof(SearchResult<Country>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<SearchResult<Country>>> GetCountries(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromQuery, Required, SwaggerParameter(@"<summary>Required Fields: First (0), Rows (min:1,max:1000).</summary>
            <summary>Fields allowed for sorting (Active): properties.</summary>
            <summary>Fields allowed for filtering (Field): properties.</summary>
            <summary>Direction allowed: asc, desc.</summary>
            <summary>MatchMode allowed: equal, in.</summary>
            <summary>Operator: and, or.</summary>", Required = true)] string searchModel = "{\"first\":0,\"rows\":10}")
        {
            _logger.LogDebug("Enter GetCountries");
            try
            {
                var countries = await _countryService.GetAllCountries(searchModel);
                return Ok(countries);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCountries service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCountries] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCountries");
            }
        }

        /// <summary>
        /// Get existing Country by ID
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{id}")]
        [SwaggerOperation("Get existing Country by ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(Country), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Country>> GetCountryById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string id)
        {
            _logger.LogDebug("Enter GetCountryById : {id}", id);
            try
            {
                ActionResult<Country> country = await _countryService.GetCountryById(id);
                return Ok(country.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCountryById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCountryById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCountryById");
            }
        }

        /// <summary>
        /// Add a country to the referencial
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries")]
        [SwaggerOperation("Add a country to the referencial")]
        [SwaggerResponse(statusCode: 201, type: typeof(Country), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Country>> PostCountry(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] CountryPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostCountry : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var country = await _countryService.CreateCountry(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetCountryById), new { id = country?.Value?.Id }, country?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostCountry service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostCountry] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostCountry");
            }
        }

        /// <summary>
        /// Update a country to the referencial
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="countryId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}")]
        [SwaggerOperation("Update a country to the referencial")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> PutCountry(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] CountryPUT body, 
            [FromRoute][Required] string countryId)
        {
            try
            {
                _logger.LogDebug("Enter PutCountry countryId {id} , CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", countryId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _countryService.EditCountry(userId, correlationId, siteOrigin, countryId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PutCountry service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PutCountry] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PutCountry");
            }
        }

        ///// <summary>
        ///// Delete a country
        ///// </summary>
        ///// <param name="correlationId"></param>
        ///// <param name="siteOrigin"></param>
        ///// <param name="countryId"></param>
        ///// <response code="204">Success - No Content</response>
        ///// <response code="400">Bad Request</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Error</response>
        //[HttpDelete]
        //[ApiVersion("1")]
        //[Route("/api/open-marketing/v{version:apiVersion}/countries/{countryId}")]
        //[SwaggerOperation("DeleteCountry")]
        //[SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        //[SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        //[SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        //[SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        //[SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        //public async Task<ActionResult> DeleteCountry([FromHeader(Name = "Correlation-ID")] string? correlationId, [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin, [FromRoute][Required] string countryId)
        //{
        //    _logger.LogDebug("DeletedCountry {0}, correlationId: {correlationId}, siteOrigin: {siteOrigin}", countryId, correlationId, siteOrigin);
        //    try
        //    {
        //        var _ = await _countryService.DeleteCountry(userId, correlationId, siteOrigin, countryId);
        //        return NoContent();
        //    }
        //    catch (ServiceException se)
        //    {
        //        _logger.LogError(se, "DeletedCountry service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
        //        return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
        //    }
        //}
    }
}
