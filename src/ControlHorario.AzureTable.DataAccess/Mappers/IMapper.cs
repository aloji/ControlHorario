using Microsoft.Azure.Cosmos.Table;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IMapper<E,EDb>
        where EDb : TableEntity
    {
        EDb Convert(E source,
         string partitionKey,
         string rowKey);
        E Convert(EDb source);
    }
}
