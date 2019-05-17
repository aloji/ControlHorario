using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public class AzureTable<T> : IAzureTable<T> 
        where T: TableEntity, new()
    {

        CloudTable table;
        readonly string connectionString;
        readonly string tableName;

        public AzureTable(string connectionString, string tableName)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        async Task<CloudTable> GetTableAsync()
        {
            if (table == null)
            {
                var storageAccount = CloudStorageAccount.Parse(this.connectionString);
                var tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference(this.tableName);

                await table.CreateIfNotExistsAsync();
            }
            return table;
        }

        public async Task CreateAsync(T item)
        {
            var table = await GetTableAsync();
            var operation = TableOperation.Insert(item);
            await table.ExecuteAsync(operation);
        }

        public async Task DeleteAsync(T item)
        {
            var table = await GetTableAsync();
            var operation = TableOperation.Delete(item);
            await table.ExecuteAsync(operation);
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            var table = await GetTableAsync();
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(operation);
            return (T)(dynamic)result.Result;
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey)
        {
            var table = await GetTableAsync();

            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            }
            while (continuationToken != null);

            return results;
        }

        public async Task UpdateAsync(T item)
        {
            var table = await GetTableAsync();
            var operation = TableOperation.InsertOrReplace(item);
            await table.ExecuteAsync(operation);
        }
    }
}
