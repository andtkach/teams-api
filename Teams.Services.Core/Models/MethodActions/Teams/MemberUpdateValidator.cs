namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using Extensions.Common;
    using FluentValidation;

    public class MemberUpdateValidator : AbstractValidator<MemberUpdate>
    {
        public MemberUpdateValidator()
        {
            this.Include(new MemberCreationValidator());
            RuleFor(x => x.Id).PopulatedGuid();
        }
    }
}