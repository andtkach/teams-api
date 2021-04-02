namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using Extensions.Common;
    using FluentValidation;
    
    public class TeamUpdateValidator : AbstractValidator<TeamUpdate>
    {
        public TeamUpdateValidator()
        {
            this.Include(new TeamCreationValidator());
            RuleFor(x => x.Id).PopulatedGuid();
        }
    }
}