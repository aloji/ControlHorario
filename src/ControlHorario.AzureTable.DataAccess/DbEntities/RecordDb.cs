using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class RecordDb : TableEntity
    {
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
