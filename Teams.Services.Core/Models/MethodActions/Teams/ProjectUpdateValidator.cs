namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using Extensions.Common;
    using FluentValidation;

    public class ProjectUpdateValidator : AbstractValidator<ProjectUpdate>
    {
        public ProjectUpdateValidator()
        {
            this.Include(new ProjectCreationValidator());
            RuleFor(x => x.Id).PopulatedGuid();
        }
    }
}