namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using Extensions.Common;
    using FluentValidation;

    public class MemberCreationValidator : AbstractValidator<MemberCreation>
    {
        public MemberCreationValidator()
        {
            RuleFor(x => x.TeamId).PopulatedGuid();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.Email).NotNull();
        }
    }
}