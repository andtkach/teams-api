namespace Teams.Services.Core.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;

    public interface IDocumentService<T> where T : class
    {
        Task<Document> CreateItemAsync(T item);

        Task DeleteItemAsync(Guid id);

        Task<T> GetItemAsync(Guid id);

        Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, int maxItemCount = -1);

        Task<Document> UpdateItemAsync(Guid id, T item);

        Task<IEnumerable<T>> ExecuteDirectQueryAsync(string filter);
    }
}
