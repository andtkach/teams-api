namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using FluentValidation;

    public class TeamCreationValidator : AbstractValidator<TeamCreation>
    {
        public TeamCreationValidator()
        {
            RuleFor(x => x.DisplayName).NotNull();
        }
    }
}