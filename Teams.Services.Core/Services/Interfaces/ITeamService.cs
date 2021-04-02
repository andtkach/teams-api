using Teams.Services.Core.Models.Dto;

namespace Teams.Services.Core.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models.Domain;
    using Core.Models.MethodActions.Teams;

    public interface ITeamService
    {
        Task<IEnumerable<Project>> GetProjectsAsync(Guid author);

        Task<IEnumerable<Project>> GetProjectsAsync(Guid author, Guid id);

        Task<Project> GetProjectAsync(Guid id);

        Task DeleteProjectAsync(Guid id);

        Task<Project> CreateProjectAsync(Guid author, ProjectCreation value);

        Task UpdateProjectAsync(Guid author, ProjectUpdate value);

        Task<Team> GetTeamAsync(Guid id);

        Task<IEnumerable<Team>> GetTeamsAsync(Guid author);
        
        Task<TeamDetails> GetTeamDetailsAsync(Guid author, Guid id);

        Task DeleteTeamAsync(Guid id);

        Task<Team> CreateTeamAsync(Guid author, TeamCreation value);

        Task UpdateTeamAsync(Guid author, TeamUpdate value);

        Task<IEnumerable<Member>> GetMembersAsync(Guid author);
        
        Task<IEnumerable<Member>> GetMembersAsync(Guid author, Guid id);
        
        Task DeleteMemberAsync(Guid id);
        
        Task<Member> GetMemberAsync(Guid id);
        
        Task<Member> CreateMemberAsync(Guid author, MemberCreation value);
        
        Task UpdateMemberAsync(Guid author, MemberUpdate value);
    }
}
