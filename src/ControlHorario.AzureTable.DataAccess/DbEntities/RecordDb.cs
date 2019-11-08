using ControlHorario.Domain.Entities;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class RecordDb : TableEntity, IPerson, ITime, IIdentity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsStart { get; set; }
    }
}
