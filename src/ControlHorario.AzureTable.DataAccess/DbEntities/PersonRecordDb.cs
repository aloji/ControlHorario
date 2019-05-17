using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class PersonRecordDb : TableEntity, IRecordDb
    {
        public PersonRecordDb() { }
        public PersonRecordDb(Guid personId, DateTime dateTimeUtc)
        {
            this.PartitionKey = personId.ToString();
            this.RowKey = dateTimeUtc.Ticks.ToString();

            this.PersonId = personId;
            this.DateTimeUtc = dateTimeUtc;
        }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
