using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class PersonDb : TableEntity
    {
        public PersonDb() { }
        public PersonDb(Guid id, string partitionKey)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = partitionKey;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
