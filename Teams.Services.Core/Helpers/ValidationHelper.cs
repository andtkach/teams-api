namespace Teams.Services.Core.Helpers
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Services.Core.Models.MethodActions.Teams;

    public static class ValidationHelper
    {
        public static void ApplyValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<ProjectCreation>, ProjectCreationValidator>();
            services.AddTransient<IValidator<ProjectUpdate>, ProjectUpdateValidator>();
            services.AddTransient<IValidator<TeamCreation>, TeamCreationValidator>();
            services.AddTransient<IValidator<TeamUpdate>, TeamUpdateValidator>();
            services.AddTransient<IValidator<MemberCreation>, MemberCreationValidator>();
            services.AddTransient<IValidator<MemberUpdate>, MemberUpdateValidator>();
        }
    }
}
