namespace Teams.Services.Core.Services.Data
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Services.Interfaces;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    
    public class DatastoreService : IDatastoreService
    {
        private readonly DocumentClient client;

        private string databaseId;
        
        private string collectionId;
        
        public DatastoreService(IConfigurationService configuration)
        {
            var datastoreSettings = configuration.DatastoreSettings();
            this.client = new DocumentClient(new Uri(datastoreSettings.Endpoint), datastoreSettings.Key);

            this.databaseId = datastoreSettings.DatabaseId;
            this.collectionId = datastoreSettings.CollectionId;

            this.CreateDatabaseIfNotExistsAsync().Wait();
            this.CreateCollectionIfNotExistsAsync().Wait();
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this.databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = this.databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this.databaseId),
                        new DocumentCollection { Id = this.collectionId },
                        new RequestOptions { OfferThroughput = this.DetermineThroughput(this.collectionId) });
                }
                else
                {
                    throw;
                }
            }
        }

        private int DetermineThroughput(string collectionId)
        {
            return collectionId.ToLower().StartsWith("production") ? 1000 : 400;
        }
    }
}
