using ApiTools;
using ApiTools.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTableApi.Interfaces;
using OpenTableApi.Models;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenTableApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "table")]
    [Route("api/table/v{version:apiVersion}/[controller]")]
    public class PlayerSessionsController : ControllerBase
    {
        private readonly IOpenTableService _tableService;
        private readonly ILogger<PlayerSessionsController> _logger;
        public PlayerSessionsController(IOpenTableService tableService, ILogger<PlayerSessionsController> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new Player session
        /// </summary>
        /// <param name="request">The Player session creation request payload</param>
        /// <returns>A Player session creation response containing the new Player session's Id</returns>
        /// <response code="201">Created - the Player session was successfully created, returned payload contains the Player session identifier</response>
        /// <response code="400">Bad request - the Player session creation request is not correct</response>
        /// <response code="403">Forbiden - the player is unknown or barred</response>
        /// <response code="404">Not Found - no table session found for the period</response>
        /// <response code="500">Internal server error - see returned ProblemDetails for details</response>
        [HttpPost]
        [ApiVersion("1")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PlayerSessionResponse>> CreatePlayerSession([FromBody] PlayerSessionCreationRequest request)
        {
            _logger.LogDebug("CreatePlayerSession called with {0}", JsonSerializer.Serialize(request));
            if (!ModelState.IsValid)
            {
                _logger.LogError("CreatePlayerSession bad request: model is invalid");
                return BadRequest(ModelState);
            }

            try
            {
                var PlayerSession = await _tableService.CreatePlayerSessionAsync(request);
                return CreatedAtAction(null, new { id = PlayerSession.SessionId }, new PlayerSessionResponse
                {
                    SessionId = PlayerSession.SessionId,
                    SessionDate = PlayerSession.SessionDate,
                    AverageBet = PlayerSession.AverageBet,
                    CurrencyCode = PlayerSession.CurrencyCode,
                    Drop = PlayerSession.Drop,
                    EarnedPoints = PlayerSession.EarnedPoints,
                    FloatVariation = PlayerSession.FloatVariation,
                    Handle = PlayerSession.Handle,
                    NumberOfGames = PlayerSession.NumberOfGames,
                    PauseTimeSeconds = PlayerSession.PauseTimeSeconds,
                    PlayerID = PlayerSession.PlayerID,
                    SessionEndTimeUTC = PlayerSession.SessionEndTimeUTC,
                    SessionStartTimeUTC = PlayerSession.SessionStartTimeUTC,
                    SiteID = PlayerSession.SiteID,
                    TableCode = PlayerSession.TableCode,
                });
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "CreatePlayerSession service exception: {0} - {1}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CreatePlayerSession exception: {0}", e.Message);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }

        }

        /// <summary>
        /// Update a specific player session by its Id
        /// </summary>
        /// <param name="id">The player session's identifier</param>
        /// <param name="request">The player session modification request payload</param>
        /// <returns>No return</returns>
        /// <response code="204">No content - the player session was successfully updated</response>
        /// <response code="400">Bad request - the player session modification request is not correct</response>
        /// <response code="404">Not found - the corresponding player session was not found</response>
        /// <response code="500">Internal server error - see returned ProblemDetails for details</response>
        [HttpPut("{id}")]
        [ApiVersion("1")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePlayerSession(long id, [FromBody] PlayerSessionUpdateRequest request)
        {
            _logger.LogDebug("UpdatePlayerSession {0} called with {1}", id, JsonSerializer.Serialize(request));
            if (!ModelState.IsValid)
            {
                _logger.LogError("UpdatePlayerSession bad request: model is invalid");
                return BadRequest(ModelState);
            }

            if (!id.Equals(request.SessionId))
            {
                _logger.LogError("UpdatePlayerSession bad request: route id {0} doesn't match request. SessionId {1}", id, request.SessionId);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status400BadRequest, new Exception("Route id doesn't match the request.SessionId"), HttpContext.Request.Path);
            }

            try
            {
                await _tableService.UpdatePlayerSessionAsync(request);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "UpdatePlayerSession service exception: {0} - {1}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdatePlayerSession exception: {0}", e.Message);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
        }

        /// <summary>
        /// Delete a specific player session by its Id
        /// </summary>
        /// <param name="id">The player session's identifier</param>
        /// <returns>No return</returns>
        /// <response code="204">No content - the player session was successfully deleted</response>
        /// <response code="400">Bad request - the player session deletion request is not correct</response>
        /// <response code="404">Not found - the corresponding player session was not found</response>
        /// <response code="500">Internal server error - see returned ProblemDetails for details</response>
        [HttpDelete("{id}")]
        [ApiVersion("1")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeletePlayerSession(long id)
        {
            _logger.LogDebug("DeletePlayerSession id: {0}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogError("DeletePlayerSession bad request: model is invalid");
                return BadRequest(ModelState);
            }

            try
            {
                await _tableService.DeletePlayerSessionAsync(id);
                return NoContent();
            }
            catch (ServiceException se)
            {
                _logger.LogError(se, "DeletePlayerSession service exception: {0} - {1}", se.StatusCode, se.Message);
                return ErrorHelper.ResponseExcRfc7807((int)se.StatusCode, se, HttpContext.Request.Path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeletePlayerSession exception: {0}", e.Message);
                return ErrorHelper.ResponseExcRfc7807(StatusCodes.Status500InternalServerError, e, HttpContext.Request.Path);
            }
        }
    }
}
