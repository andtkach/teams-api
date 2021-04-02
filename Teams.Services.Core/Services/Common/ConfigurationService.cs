namespace Teams.Services.Core.Services.Common
{
    using Core.Configuration;
    using Core.Services.Interfaces;

    using Microsoft.Extensions.Configuration;

    public class ConfigurationService : IConfigurationService
    {
        private readonly CoreConfiguration coreConfiguration;

        public ConfigurationService(IConfiguration configuration)
        {
            this.coreConfiguration = new CoreConfiguration(configuration);
        }

        public DatastoreConfiguration DatastoreSettings()
        {
            return this.coreConfiguration.DatastoreSettings;
        }
    }
}
