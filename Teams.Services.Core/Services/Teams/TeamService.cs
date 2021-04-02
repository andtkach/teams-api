using Teams.Services.Core.Models.Dto;

namespace Teams.Services.Core.Services.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Models.Domain;
    using Core.Models.MethodActions.Teams;
    using Core.Services.Interfaces;

    public class TeamService : ITeamService
    {
        private readonly IDocumentService<Project> _projects;
        private readonly IDocumentService<Team> _teams;
        private readonly IDocumentService<Member> _members;

        public TeamService(
            IDocumentService<Project> projects,
            IDocumentService<Team> teams,
            IDocumentService<Member> members)
        {
            this._projects = projects;
            this._teams = teams;
            this._members = members;
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync(Guid author)
            =>
                await this._projects.GetItemsAsync(_ => 
                    _.BaseType == nameof(Project)
                    && _.Author == author);

        public async Task<IEnumerable<Project>> GetProjectsAsync(Guid author, Guid id)
            =>
                await this._projects
                    .GetItemsAsync(_ => _.TeamId == id 
                                        && _.BaseType == nameof(Project)
                                        && _.Author == author);

        public async Task<Project> GetProjectAsync(Guid id) 
            => 
                await this._projects.GetItemAsync(id);

        public async Task DeleteProjectAsync(Guid id) 
            => 
                await this._projects.DeleteItemAsync(id);

        public async Task<Project> CreateProjectAsync(Guid author, ProjectCreation value)
        {
            Project result = new Project()
            {
                TeamId = value.TeamId,
                DisplayName = value.DisplayName,
                StartDate = value.StartDate,
                Location = value.Location,
                Priority = value.Priority,
                Author = author
            };

            result.ApplyDefaults();
            await this._projects.CreateItemAsync(result);
            return result;
        }

        public async Task UpdateProjectAsync(Guid author, ProjectUpdate value)
        {
            var result = await this.GetProjectAsync(value.Id);
            result.TeamId = value.TeamId;
            result.DisplayName = value.DisplayName;
            result.Location = value.Location;
            result.Priority = value.Priority;
            result.StartDate = value.StartDate;
            result.Author = author;

            result.ApplyDefaults(true);
            
            await this._projects.UpdateItemAsync(result.Id, result);
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync(Guid author)
            =>
                (await this._teams.GetItemsAsync(_ =>
                    _.BaseType == nameof(Team)
                    && _.Author == author))
                .OrderBy(o => o.DisplayName);

        public async Task<TeamDetails> GetTeamDetailsAsync(Guid author, Guid id)
        {
            var result = new TeamDetails
            {
                Team = await this._teams.GetItemAsync(id),
                Projects = (await this._projects.GetItemsAsync(_ =>
                        _.BaseType == nameof(Project)
                        && _.TeamId == id
                        && _.Author == author))
                    .OrderBy(o => o.DisplayName),
                Members = (await this._members.GetItemsAsync(_ =>
                        _.BaseType == nameof(Member)
                        && _.TeamId == id
                        && _.Author == author))
                    .OrderBy(o => o.FirstName)
            };

            return result;
        }

        public async Task<Team> GetTeamAsync(Guid id)
            =>
                await this._teams.GetItemAsync(id);

        public async Task DeleteTeamAsync(Guid id) 
            => 
                await this._teams.DeleteItemAsync(id);

        public async Task<Team> CreateTeamAsync(Guid author, TeamCreation value)
        {
            Team result = new Team()
            {
                DisplayName = value.DisplayName,
                Author = author
            };

            result.ApplyDefaults();
            await this._teams.CreateItemAsync(result);
            
            return result;
        }

        public async Task UpdateTeamAsync(Guid author, TeamUpdate value)
        {
            var result = await this.GetTeamAsync(value.Id);
            result.DisplayName = value.DisplayName;
            result.Author = author;
            result.ApplyDefaults(true);
            
            await this._teams.UpdateItemAsync(result.Id, result);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(Guid author) 
            => 
                await this._members.GetItemsAsync(_ => 
                    _.BaseType == nameof(Member)
                    && _.Author == author);

        public async Task<IEnumerable<Member>> GetMembersAsync(Guid author, Guid id)
            =>
                await this._members.GetItemsAsync(_ => 
                    _.TeamId == id 
                    && _.BaseType == nameof(Member)
                    && _.Author == author);

        public async Task<Member> GetMemberAsync(Guid id) 
            => 
                await this._members.GetItemAsync(id);

        public async Task DeleteMemberAsync(Guid id) 
            => 
                await this._members.DeleteItemAsync(id);

        public async Task<Member> CreateMemberAsync(Guid author, MemberCreation value)
        {
            Member result = new Member()
            {
                TeamId = value.TeamId,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Email = value.Email,
                Author = author,
            };
            result.ApplyDefaults();
            await this._members.CreateItemAsync(result);
            return result;
        }

        public async Task UpdateMemberAsync(Guid author, MemberUpdate value)
        {
            var result = await this.GetMemberAsync(value.Id);
            result.TeamId = value.TeamId;
            result.FirstName = value.FirstName;
            result.LastName = value.LastName;
            result.Email = value.Email;
            result.Author = author;
            result.ApplyDefaults(true);
            await this._members.UpdateItemAsync(result.Id, result);
        }
    }
}
