namespace Teams.Services.Core.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class CoreConfiguration
    {
        public CoreConfiguration(IConfiguration configuration)
        {
            this.DatastoreSettings = new DatastoreConfiguration(configuration);
        }

        public DatastoreConfiguration DatastoreSettings { get; }
    }
}
