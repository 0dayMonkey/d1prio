using Microsoft.AspNetCore.Http.Extensions;
using OpenMarketingApi.Interfaces.Credentials;
using OpenMarketingApi.Models.Credentials;

namespace OpenMarketingApi.Controllers.Credentials
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "open-marketing")]
    [SwaggerTag("Credentials")]
    public class CredentialsController : ControllerBase
    {
        private readonly ILogger<CredentialsController> _logger;
        private readonly ICredentialService _credentialService;
        public CredentialsController(ICredentialService credentialService, ILogger<CredentialsController> logger)
        {
            _credentialService = credentialService;
            _logger = logger;
        }

        /// <summary>
        /// Get existing Credential by ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="credentialId">The Credential Identifier</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/credentials/{credentialId}")]
        [SwaggerOperation("Get existing Credential by ID")]
        [SwaggerResponse(statusCode: 200, type: typeof(CredentialResponse), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<CredentialResponse>> GetCredentialById(
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromRoute, SwaggerParameter("Credential Identifier", Required = true)] string credentialId)
        {
            try
            {
                _logger.LogDebug("Enter GetCredentialById : {CredentialId}, userId :{userID}", credentialId, userId);

                ActionResult<CredentialResponse> country = await _credentialService.GetCredentialById(credentialId);
                return Ok(country.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "GetCredentialById service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[GetCredentialById] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit GetCredentialById");
            }
        }

        /// <summary>
        /// Update a credential, and all linked credentials (same support)
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="siteOriginId"></param>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <param name="credentialId">The Credential Identifier</param>
        /// <response code="204">Success - No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPatch]
        [ApiVersion("1")]
        [Route("/api/open-marketing/v{version:apiVersion}/credentials/{credentialId}")]
        [SwaggerOperation("PatchCredentials")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<IActionResult> PatchCredentials(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOriginId,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody] CredentialPATCH body,
            [FromRoute][Required] string credentialId)
        {
            try
            {
                _logger.LogDebug("Enter PatchCredentials : CorrelationId: {correlationId}, siteOriginId: {siteOriginId}, userId: {userId}, credentialId: {credentialId}", correlationId, siteOriginId, userId, credentialId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));

                await _credentialService.PatchCredential(userId, correlationId, siteOriginId, credentialId, body);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PatchCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PatchCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PatchCredentials");
            }
        }

        /// <summary>
        /// Add a Credential for an anonymous
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
        [Route("/api/open-marketing/v{version:apiVersion}/credentials")]
        [SwaggerOperation("Add a Credential for an anonymous")]
        [SwaggerResponse(statusCode: 201, type: typeof(List<CredentialResponse>), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        public async Task<ActionResult<List<CredentialResponse>>> PostAnonymousCredentials(
            [FromHeader(Name = "Correlation-ID")] string? correlationId,
            [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
            [FromHeader(Name = "API-User-ID"), Required] string userId,
            [FromBody][Required] CredentialPOST body)
        {
            try
            {
                _logger.LogDebug("Enter PostAnonymousCredentials :  CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", correlationId, siteOrigin, userId);
                _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
                var Credential = await _credentialService.CreateAnonymousCredential(userId, correlationId, siteOrigin, body);
                return Created(Request.GetEncodedUrl(), Credential.Value);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "PostAnonymousCredentials service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[PostAnonymousCredentials] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
                return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
            }
            finally
            {
                _logger.LogTrace("Exit PostAnonymousCredentials");
            }
        }

        ///// <summary>
        ///// Update a Credential for the anonymous
        ///// </summary>
        ///// <param name="correlationId"></param>
        ///// <param name="siteOrigin"></param>
        ///// <param name="userId"></param>
        ///// <param name="body"></param>
        ///// <param name="credentialId"></param>
        ///// <response code="204">Success - No Content</response>
        ///// <response code="400">Bad Request</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Error</response>
        //[HttpPut]
        //[ApiVersion("1")]
        //[Route("/api/open-marketing/v{version:apiVersion}/credentials/{CredentialId}")]
        //[SwaggerOperation("Update a Credential for the anonymous")]
        //[SwaggerResponse(statusCode: 204, description: "Success - No Content")]
        //[SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
        //[SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
        //[SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
        //[SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
        //[SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
        //public async Task<ActionResult> PutCredential(
        //    [FromHeader(Name = "Correlation-ID")] string? correlationId,
        //    [FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
        //    [FromHeader(Name = "API-User-ID"), Required] string userId,
        //    [FromBody][Required] AnonymousCredentialPUT body, 
        //    [FromRoute, SwaggerParameter("Credential Identifier", Required = true)] string credentialId)
        //{
        //    try
        //    {
        //        _logger.LogDebug("Enter PutCredential : credentialId :{credentialId}, CorrelationId: {correlationId}, siteOriginId: {siteOrigin}, userId: {userId}", credentialId, correlationId, siteOrigin, userId);
        //        _logger.LogTrace("Body content : {body}", JsonSerializer.Serialize(body));
        //        var _ = await _credentialService.UpdateCredential(userId, correlationId, siteOrigin, credentialId, body);
        //        return NoContent();
        //    }
        //    catch (ServiceException se)
        //    {
        //        _logger.LogError(se, "PutCredential service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
        //        return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "[PutCredential] Unhandled exception - status code : {statusCode}", StatusCodes.Status500InternalServerError);
        //        return ErrorHelper.InternalErrorProblemsDetails(StatusCodes.Status500InternalServerError, HttpContext.Request.Path);
        //    }
        //    finally
        //    {
        //        _logger.LogTrace("Exit PutCredential");
        //    }
        //}

		///// <summary>
		///// Delete a Credential of an anonymous
		///// </summary>
		///// <param name="correlationId"></param>
		///// <param name="siteOrigin"></param>
		///// <param name="userId"></param>
		///// <param name="deleteAction"></param>
		///// <param name="credentialId"></param>
		///// <response code="204">Success - No Content</response>
		///// <response code="400">Bad Request</response>
		///// <response code="401">Unauthorized</response>
		///// <response code="403">Forbidden</response>
		///// <response code="404">Not Found</response>
		///// <response code="500">Internal Error</response>
		//[HttpDelete]
  //      [ApiVersion("1")]
  //      [Route("/api/open-marketing/v{version:apiVersion}/credentials/{credentialId}")]
  //      [SwaggerOperation("Delete Credential for anonymous")]
  //      [SwaggerResponse(statusCode: 400, type: typeof(ValidationProblemDetails), description: "Bad Request")]
  //      [SwaggerResponse(statusCode: 401, type: typeof(ProblemDetails), description: "Unauthorized")]
  //      [SwaggerResponse(statusCode: 403, type: typeof(ProblemDetails), description: "Forbidden")]
  //      [SwaggerResponse(statusCode: 404, type: typeof(ProblemDetails), description: "Not Found")]
  //      [SwaggerResponse(statusCode: 500, type: typeof(ProblemDetails), description: "Internal Error")]
  //      public async Task<ActionResult> DeleteAnonymousCredential(
		//	[FromHeader(Name = "Correlation-ID")] string? correlationId,
		//	[FromHeader(Name = "Site-Origin-ID")] int? siteOrigin,
		//	[FromHeader(Name = "API-User-ID"), Required] string userId,
		//	[FromBody][Required] ServiceAction deleteAction,
		//	[FromRoute][Required] string credentialId)
  //      {
  //         _logger.LogDebug("DeleteAnonymousCredential {0}, correlationId: {correlationId}, siteOrigin: {siteOrigin}, credentialId: {3}", credentialId, correlationId, siteOrigin, credentialId);
  //         try
  //         {
  //             var _ = await _credentialService.DeleteAnonymousCredential(userId, correlationId, siteOrigin, credentialId, deleteAction);
  //             return NoContent();
  //         }
  //         catch (ServiceException se)
  //         {
  //             _logger.LogError(se, "DeleteAnonymousCredential service exception: {se.StatusCode} - {se.Message}", se.StatusCode, se.Message);
  //             return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
  //         }
  //         catch (Exception e)
  //         {
  //             return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
  //         }
  //      }
    }
}
