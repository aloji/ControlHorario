using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class RecordDb : TableEntity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsStart { get; set; }
    }
}
