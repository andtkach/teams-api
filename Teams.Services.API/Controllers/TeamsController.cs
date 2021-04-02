using Teams.Services.Core.Models.Dto;

namespace Teams.Services.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Teams.Services.API.Extensions;
    using Teams.Services.Core.Common.Enums;
    using Teams.Services.Core.Extensions.Common;
    using Teams.Services.Core.Filters;
    using Teams.Services.Core.Models.Domain;
    using Teams.Services.Core.Models.Errors;
    using Teams.Services.Core.Models.MethodActions.Teams;
    using Teams.Services.Core.Services.Interfaces;

    [ApiVersion("1.0")]
    [Route("api/teams")]
    [Route("api/v{version:apiVersion}/teams")]
    [Produces("application/json")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teams;

        public TeamsController(ITeamService teams)
        {
            this._teams = teams;
        }

        /// <summary>
        /// Returns a collection of all teams.
        /// </summary>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Team>), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<ActionResult<List<Team>>> GetAll()
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var results = await this._teams.GetTeamsAsync(authorization.SubscriptionKey);
            return this.Ok(results);
        }

        /// <summary>
        /// Returns an individual team.
        /// </summary>
        /// <param name="id">The associated team identifier</param>
        /// <returns>A individual team.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDetails), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<IActionResult> Get(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var result = await this._teams.GetTeamAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Returns an individual team with projects and team members.
        /// </summary>
        /// <param name="id">The associated team identifier</param>
        /// <returns>A individual team with details.</returns>
        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(Team), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<IActionResult> GetWithDetails(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var result = await this._teams.GetTeamDetailsAsync(authorization.SubscriptionKey, id);
            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="value">A populated TeamCreation object.</param>
        /// <returns>General information for a newly created team.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Team), 201)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<IActionResult> Post([FromBody] TeamCreation value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            string errorMessage;
            try
            {
                var result = await this._teams.CreateTeamAsync(authorization.SubscriptionKey, value);
                return this.CreatedAtAction(nameof(this.Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return this.BadRequest(errorMessage.AsServiceError());
        }

        /// <summary>
        /// Updates individual team details.
        /// </summary>
        /// <param name="value">A populated ContactUpdate object.</param>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<IActionResult> Put([FromBody] TeamUpdate value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.UpdateTeamAsync(authorization.SubscriptionKey, value);
            return this.NoContent();
        }

        /// <summary>
        /// Deletes an individual team.
        /// </summary>
        /// <param name="id">A single team identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Teams")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.DeleteTeamAsync(id);
            return this.NoContent();
        }
    }
}