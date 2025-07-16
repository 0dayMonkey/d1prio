using OpenMarketingApi.Interfaces.Referencials;
using OpenMarketingApi.Models.Referencials.Occupation;
using OpenMarketingApi.Models.Referencials.SocioProfessional;

namespace OpenMarketingApi.Controllers.Referencials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Referencial / Socio Professional Categories")]
    public class SocioProfessionalCategoriesController : ControllerBase
    {
        private readonly ILogger<SocioProfessionalCategoriesController> _logger;
        private readonly ISocioProfessionalCategoryAndOccupationService _socioProfessionalCategoryAndOccupationService;

        public SocioProfessionalCategoriesController(ISocioProfessionalCategoryAndOccupationService socioProfessionalCategoryAndOccupationService, ILogger<SocioProfessionalCategoriesController> logger)
        {
            _logger = logger;
            _socioProfessionalCategoryAndOccupationService = socioProfessionalCategoryAndOccupationService;
        }

        /// <summary>
        /// Get all existing Occupations in this Socio Professional Category
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}/occupations")]
        [SwaggerOperation("Get all existing Occupations in this Socio Professional Category")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Occupation>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<Occupation>>> GetOccupationsBySocioProfessionalCategory(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string socioProfessionalCategoryId)
        {
            try
            {
                _logger.LogDebug("Enter GetOccupationsBySocioProfessionalCategory id : {socioProfessionalCategoryId}", socioProfessionalCategoryId);

                var idDocumentType = await _socioProfessionalCategoryAndOccupationService.GetOccupationsBySocioProfessionalCategory(socioProfessionalCategoryId);
                return Ok(idDocumentType);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetOccupationsBySocioProfessionalCategory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetOccupationsBySocioProfessionalCategory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetOccupationsBySocioProfessionalCategory");
            }
        }

        /// <summary>
        /// Get all existing Occupations in this Socio Professional Category
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}/occupations/{id}")]
        [SwaggerOperation("Get all existing Occupations in this Socio Professional Category")]
        [SwaggerResponse(statusCode: 200, type: typeof(Occupation), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Occupation>> GetOccupationBySocioProfessionalCategory(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string socioProfessionalCategoryId,
            [FromRoute][Required] string id)
        {
            try
            {
                _logger.LogDebug("Enter GetOccupationBySocioProfessionalCategory id : {socioProfessionalCategoryId}", socioProfessionalCategoryId);

                var idDocumentType = await _socioProfessionalCategoryAndOccupationService.GetOccupationBySocioProfessionalCategory(socioProfessionalCategoryId, id);
                return Ok(idDocumentType);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetOccupationsBySocioProfessionalCategory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetOccupationBySocioProfessionalCategory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetOccupationBySocioProfessionalCategory");
            }
        }

        /// <summary>
        /// Get all existing Socio Professional Categories
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories")]
        [SwaggerOperation("Get all existing Socio Professional Categories")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SocioProfessionalCategory>), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<SocioProfessionalCategory>>> GetSocioProfessionalCategories([FromHeader(Name = "API-User-ID"), Required] string userId)
        {
            try
            {
                _logger.LogTrace("Enter GetSocioProfessionalCategories");

                var idDocumentTypes = await _socioProfessionalCategoryAndOccupationService.GetSocioProfessionalCategories();
                return Ok(idDocumentTypes);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetSocioProfessionalCategoriesAsync service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetSocioProfessionalCategories] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetSocioProfessionalCategories");
            }
        }

        /// <summary>
        /// Get existing Socio Professional Category by Id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}")]
        [SwaggerOperation("Get existing Socio Professional Category by Id")]
        [SwaggerResponse(statusCode: 200, type: typeof(SocioProfessionalCategory), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<SocioProfessionalCategory>> GetSocioProfessionalCategoryById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute][Required] string socioProfessionalCategoryId)
        {
            try
            {
                _logger.LogTrace("Enter GetSocioProfessionalCategoryById");

                var idDocumentType = await _socioProfessionalCategoryAndOccupationService.GetSocioProfessionalCategoryById(socioProfessionalCategoryId);
                return Ok(idDocumentType);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetSocioProfessionalCategoryById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetSocioProfessionalCategoryById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetSocioProfessionalCategoryById");
            }
        }

        /// <summary>
        /// Add an Occupation in the specified Socio Professional Category
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPost]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}/occupations")]
        [SwaggerOperation("Add an Occupation in the specified Socio Professional Category")]
        [SwaggerResponse(statusCode: 201, type: typeof(Occupation), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<Occupation>> PostOccupationInSocioProfessionalCategory(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] OccupationPOST body,
            [FromRoute][Required] string socioProfessionalCategoryId)
        {
            try
            {
                _logger.LogDebug("Enter PostOccupationInSocioProfessionalCategory : socioProfessionalCategoryId {socioProfessionalCategoryId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", socioProfessionalCategoryId ,correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var occupation = await _socioProfessionalCategoryAndOccupationService.CreateOccupationInSocioProfessionalCategory(userId, correlationId, siteOrigin, body, socioProfessionalCategoryId);
                return CreatedAtAction(nameof(GetOccupationBySocioProfessionalCategory), new { socioProfessionalCategoryId, id = occupation?.Value?.Id }, occupation?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostOccupationInSocioProfessionalCategory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostOccupationInSocioProfessionalCategory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostOccupationInSocioProfessionalCategory");
            }
        }

        /// <summary>
        /// Create a Socio Professional Category
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
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories")]
        [SwaggerOperation("Create a Socio Professional Category")]
        [SwaggerResponse(statusCode: 201, type: typeof(SocioProfessionalCategory), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<SocioProfessionalCategory>> PostSocioProfessionalCategories(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] SocioProfessionalCategoryPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostSocioProfessionalCategories : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var socioProfessionalCategory = await _socioProfessionalCategoryAndOccupationService.CreateSocioProfessionalCategories(userId, correlationId, siteOrigin, body);
                return CreatedAtAction(nameof(GetSocioProfessionalCategoryById), new { socioProfessionalCategoryId = socioProfessionalCategory?.Value?.Id }, socioProfessionalCategory?.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostSocioProfessionalCategories service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostSocioProfessionalCategories] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostSocioProfessionalCategories");
            }
        }

        /// <summary>
        /// Update an occupation
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <param name="occupationId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}/occupations/{occupationId}")]
        [SwaggerOperation("Update an occupation")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> UpdateOccupation(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] OccupationPUT body,
            [FromRoute][Required] string socioProfessionalCategoryId,
            [FromRoute][Required] string occupationId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateOccupation id : {occupationId},  socioProfessionalCategoryId {socioProfessionalCategoryId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", occupationId, socioProfessionalCategoryId, correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _socioProfessionalCategoryAndOccupationService.UpdateOccupation(userId, correlationId, siteOrigin, body, socioProfessionalCategoryId, occupationId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateOccupation service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateOccupation] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateOccupation");
            }
        }

        /// <summary>
        /// Update a Socio Professional Category
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}")]
        [SwaggerOperation("Update a Socio Professional Category")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> UpdateSocioProfessionalCategory(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody, Required] SocioProfessionalCategoryPUT body,
            [FromRoute][Required] string socioProfessionalCategoryId)
        {
            try
            {
                _logger.LogDebug("Enter UpdateSocioProfessionalCategory : CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var _ = await _socioProfessionalCategoryAndOccupationService.UpdateSocioProfessionalCategory(userId, correlationId, siteOrigin, body, socioProfessionalCategoryId);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdateSocioProfessionalCategory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[UpdateSocioProfessionalCategory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit UpdateSocioProfessionalCategory");
            }
        }

        /// <summary>
        /// Delete an occupation
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <param name="occupationId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}/occupations/{occupationId}")]
        [SwaggerOperation("Delete an occupation")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> DeleteOccupation(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string socioProfessionalCategoryId, 
            [FromRoute][Required] string occupationId)
        {
            try
            {
                _logger.LogDebug("Enter DeleteOccupation id : {occupationId}, socioProfessionalCategoryId {socioProfessionalCategoryId}, CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", occupationId, socioProfessionalCategoryId, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);

                var _ = await _socioProfessionalCategoryAndOccupationService.DeleteOccupation(userId, correlationId, siteOrigin, socioProfessionalCategoryId, occupationId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteOccupation service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteOccupation] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteOccupation");
            }
        }

        /// <summary>
        /// Delete a Socio Professional Category
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOrigin"></param>
        /// <param name="userId"></param>
        /// <param name="deleteAction"></param>
        /// <param name="socioProfessionalCategoryId"></param>
        /// <response code="204">Success - No Content</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/socio-professional-categories/{socioProfessionalCategoryId}")]
        [SwaggerOperation("Delete a Socio Professional Category")]
        [SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden, can't be deleted when occupations use it")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult> DeleteSocioProfessionalCategory(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] ServiceAction deleteAction,
            [FromRoute][Required] string socioProfessionalCategoryId)
        {
            try
            {
                _logger.LogDebug("Enter DeleteSocioProfessionalCategory id : {socioProfessionalCategoryId} CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId :{userId}, lastupdatedTimestamp : {body.LastUpdatedTimestamp}", socioProfessionalCategoryId, correlationId, siteOrigin, userId, deleteAction.LastUpdatedTimestamp);

                var _ = await _socioProfessionalCategoryAndOccupationService.DeleteSocioProfessionalCategory(userId, correlationId, siteOrigin, socioProfessionalCategoryId, deleteAction);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeleteSocioProfessionalCategory service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[DeleteSocioProfessionalCategory] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit DeleteSocioProfessionalCategory");
            }
        }
    }
}
