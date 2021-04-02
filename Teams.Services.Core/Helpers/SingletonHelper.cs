namespace Teams.Services.Core.Helpers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Services.Core.Models.Domain;
    using Teams.Services.Core.Services.Common;
    using Teams.Services.Core.Services.Data;
    using Teams.Services.Core.Services.Interfaces;
    using Teams.Services.Core.Services.Teams;

    public static class SingletonHelper
    {
        public static void ApplySingletons(IServiceCollection services, IConfiguration configuration)
        {
            InitializeRepositories(services, configuration);
            InitializeServices(services);
        }

        private static void InitializeRepositories(IServiceCollection services, IConfiguration configuration)
        {
            new DatastoreService(new ConfigurationService(configuration));

            services.AddSingleton<IDocumentService<Project>, DocumentService<Project>>();
            services.AddSingleton<IDocumentService<Team>, DocumentService<Team>>();
            services.AddSingleton<IDocumentService<Member>, DocumentService<Member>>();
        }

        private static void InitializeServices(IServiceCollection services)
        {
            services.AddSingleton<IDatastoreService, DatastoreService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<ITeamService, TeamService>();
        }
    }
}
