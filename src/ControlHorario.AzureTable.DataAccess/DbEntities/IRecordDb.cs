using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public interface IRecordDb
    {
        Guid PersonId { get; set; }
        DateTime DateTimeUtc { get; set; }
    }
}
