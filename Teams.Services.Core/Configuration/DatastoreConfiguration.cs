namespace Teams.Services.Core.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class DatastoreConfiguration
    {
        public DatastoreConfiguration(IConfiguration configuration)
        {
            this.Endpoint = configuration["CosmosDb:AccountEndpoint"];
            this.Key = configuration["CosmosDb:AccountKeys"];
            this.DatabaseId = configuration["CosmosDb:DatabaseId"];
            this.CollectionId = configuration["CosmosDb:CollectionId"];
        }

        public string Endpoint { get; }

        public string Key { get; }
        
        public string DatabaseId { get; }
        
        public string CollectionId { get; }
    }
}
