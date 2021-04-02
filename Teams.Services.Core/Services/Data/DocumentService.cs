namespace Teams.Services.Core.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Services.Interfaces;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public class DocumentService<T> : IDocumentService<T> where T : class
    {
        private readonly DocumentClient _client;

        private string databaseId;

        public DocumentService(IConfigurationService configuration)
        {
#if DEBUG
            this._client = new DocumentClient(new Uri(configuration.DatastoreSettings().Endpoint), configuration.DatastoreSettings().Key);

#else
            this._client = new DocumentClient(
            new Uri(configuration.DatastoreSettings().Endpoint), 
            configuration.DatastoreSettings().Key, 
            new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                MaxConnectionLimit = 1000,
            });

#endif
            this.databaseId = configuration.DatastoreSettings().DatabaseId;
            this.CollectionId = configuration.DatastoreSettings().CollectionId;
        }

        private string CollectionId { get; set; }

        public async Task<T> GetItemAsync(Guid id)
        {
            try
            {
                Document document = await this._client
                    .ReadDocumentAsync(UriFactory.CreateDocumentUri(
                        this.databaseId, 
                        this.CollectionId, 
                        id.ToString()),
                        new RequestOptions { PartitionKey = new PartitionKey(typeof(T).Name)});
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecuteDirectQueryAsync(string filter)
        {
            var query = this._client
                .CreateDocumentQuery(
                    UriFactory.CreateDocumentCollectionUri(this.databaseId, this.CollectionId),
                    filter,
                    new FeedOptions()
                    {
                        MaxItemCount = 10000,
                        EnableCrossPartitionQuery = true
                    })
                .AsDocumentQuery();

            List<T> results = new List<T>();

            try
            {
                while (query.HasMoreResults)
                {
                    var result = await query.ExecuteNextAsync<T>();
                    results.AddRange(result);
                }
            }
            catch (Exception)
            {
            }

            return results;
        }

        public async Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, int maxItemCount = -1)
        {
            IDocumentQuery<T> query = this._client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(this.databaseId, this.CollectionId),
                new FeedOptions
                {
                    MaxItemCount = maxItemCount,
                    EnableCrossPartitionQuery = true
                })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();

            try
            {
                while (query.HasMoreResults)
                {
                    var result = await query.ExecuteNextAsync<T>();
                    results.AddRange(result);
                }
            }
            catch (Exception)
            {
            }

            return results.ToList();
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await this._client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.databaseId, this.CollectionId), item);
        }

        public async Task<Document> UpsertItemAsync(Guid id, T item)
        {
            return await this._client.UpsertDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.CollectionId, id.ToString()), item);
        }

        public async Task<Document> UpdateItemAsync(Guid id, T item)
        {
            return await this._client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.CollectionId, id.ToString()), item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await this._client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(this.databaseId, this.CollectionId, id.ToString()),
                new RequestOptions
                {
                    PartitionKey = new PartitionKey(typeof(T).Name)
                });
        }
    }
}
