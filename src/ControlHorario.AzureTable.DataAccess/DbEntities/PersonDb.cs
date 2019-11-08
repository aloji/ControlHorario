using Microsoft.Azure.Cosmos.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class PersonDb : TableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? FacePersonId { get; set; }
    }
}
