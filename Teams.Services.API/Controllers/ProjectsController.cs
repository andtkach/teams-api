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
    [Route("api/teams/projects")]
    [Route("api/v{version:apiVersion}/teams/projects")]
    [Produces("application/json")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ITeamService _teams;

        public ProjectsController(ITeamService teams)
        {
            this._teams = teams;
        }

        /// <summary>
        /// Returns a collection of all projects.
        /// </summary>
        /// <returns>A collection of all projects.</returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<ActionResult<List<Project>>> GetAll()
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var results = await this._teams.GetProjectsAsync(authorization.SubscriptionKey);
            return this.Ok(results);
        }

        /// <summary>
        /// Returns a collection of projects for a team.
        /// </summary>
        /// <returns>A collection of projects for a team.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<ActionResult<List<Project>>> GetProjects(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var results = await this._teams.GetProjectsAsync(authorization.SubscriptionKey, id);
            return this.Ok(results);
        }

        /// <summary>
        /// Returns an individual project.
        /// </summary>
        /// <param name="id">The associated project identifier</param>
        /// <returns>A individual project.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Project), 200)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<IActionResult> Get(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            var result = await this._teams.GetProjectAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="value">A populated ProjectCreation object.</param>
        /// <returns>General information for a newly created project.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Project), 201)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<IActionResult> Post([FromBody] ProjectCreation value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            string errorMessage;
            try
            {
                var result = await this._teams.CreateProjectAsync(authorization.SubscriptionKey, value);
                return this.CreatedAtAction(nameof(this.Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return this.BadRequest(errorMessage.AsServiceError());
        }

        /// <summary>
        /// Updates individual project details.
        /// </summary>
        /// <param name="value">A populated ProjectUpdate object.</param>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ValidateModelState]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<IActionResult> Put([FromBody] ProjectUpdate value)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.UpdateProjectAsync(authorization.SubscriptionKey, value);
            return this.NoContent();
        }

        /// <summary>
        /// Deletes an individual project.
        /// </summary>
        /// <param name="id">A single project identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ServiceErrorInformation), 400)]
        [ProducesResponseType(404)]
        [ApiExplorerSettings(GroupName = "Projects")]
        public async Task<IActionResult> Projects(Guid id)
        {
            var authorization = Request.Headers.Validate();
            if (authorization.SubscriptionKey.IsEmpty())
            {
                return this.BadRequest(ServiceErrorType.SubscriptionKeyRequired.AsErrorMessage());
            }

            await this._teams.DeleteProjectAsync(id);
            return this.NoContent();
        }
    }
}