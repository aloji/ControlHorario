using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public interface IAzureTable<T> where T : TableEntity, new()
    {
        Task DeleteAsync(T item);
        Task<T> GetAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetAsync(string partitionKey);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
    }
}
