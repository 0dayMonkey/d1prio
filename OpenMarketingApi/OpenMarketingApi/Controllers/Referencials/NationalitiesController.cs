using OpenMarketingApi.Interfaces.Referencials;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Nationalities")]
    public class NationalitiesController : ControllerBase
    {
        private readonly ILogger<NationalitiesController> _logger;
        private readonly INationalityService _nationalityService;

        public NationalitiesController(INationalityService nationalityService, ILogger<NationalitiesController> logger)
        {
            _nationalityService = nationalityService;
            _logger = logger;
        }

        /// <summary>
        /// Get Nationalities
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/nationalities")]
        [SwaggerOperation("Get Nationalities")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Nationality>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<Nationality>>> GetNationalities([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogTrace("Enter GetNationalities");

                var nationalities = await _nationalityService.GetNationalities();
                return Ok(nationalities);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetNationalities service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetNationalities] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetNationalities");
            }
        }
    }
}
