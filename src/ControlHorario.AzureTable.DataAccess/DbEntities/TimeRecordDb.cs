using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class TimeRecordDb : TableEntity, IRecordDb
    {
        const string partitionKeyFormat = "yyyyMMdd";

        public TimeRecordDb() { }
        public TimeRecordDb(Guid personId, DateTime dateTimeUtc)
        {
            this.RowKey = personId.ToString();
            this.PartitionKey = dateTimeUtc.ToString(partitionKeyFormat);

            this.PersonId = personId;
            this.DateTimeUtc = dateTimeUtc;
        }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
