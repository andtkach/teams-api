namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using Extensions.Common;
    using FluentValidation;

    public class ProjectCreationValidator : AbstractValidator<ProjectCreation>
    {
        public ProjectCreationValidator()
        {
            RuleFor(x => x.TeamId).PopulatedGuid();
            RuleFor(x => x.DisplayName).NotNull();
        }
    }
}