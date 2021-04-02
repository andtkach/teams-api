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
    [Route("api/teams/members")]
    [Route("api/v{version:apiVersion}/teams/members")]
    [Produces("application/json")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ITeamService _teams;

        public MembersController(ITeamService teams)
        {
            this._teams = teams;
        }

        /// <summary>
        /// Returns a collection of all team members.
        /// </summary>
        /// <returns>A collection of all team members.</returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Member>), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<ActionResult<List<Member>>> GetAll()
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var results = await this._teams.GetMembersAsync(authorization.SubscriptionKey);
            return this.Ok(results);
        }

        /// <summary>
        /// Returns a collection of members for a team.
        /// </summary>
        /// <returns>A collection of members for a team.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Member>), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<ActionResult<List<Member>>> GetMembers(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var results = await this._teams.GetMembersAsync(authorization.SubscriptionKey, id);
            return this.Ok(results);
        }

        /// <summary>
        /// Returns an individual team member.
        /// </summary>
        /// <param name="id">The associated team member identifier</param>
        /// <returns>A individual team member.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Member), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<IActionResult> Get(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var result = await this._teams.GetMemberAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new team member.
        /// </summary>
        /// <param name="value">A populated MemberCreation object.</param>
        /// <returns>General information for a newly created team member.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Member), 201)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<IActionResult> Post([FromBody] MemberCreation value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            string errorMessage;
            try
            {
                var result = await this._teams.CreateMemberAsync(authorization.SubscriptionKey, value);
                return this.CreatedAtAction(nameof(this.Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return this.BadRequest(errorMessage.AsServiceError());
        }

        /// <summary>
        /// Updates individual team member details.
        /// </summary>
        /// <param name="value">A populated MemberUpdate object.</param>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<IActionResult> Put([FromBody] MemberUpdate value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.UpdateMemberAsync(authorization.SubscriptionKey, value);
            return this.NoContent();
        }

        /// <summary>
        /// Deletes an individual team member.
        /// </summary>
        /// <param name="id">A single team member identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Members")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.DeleteMemberAsync(id);
            return this.NoContent();
        }
    }
}