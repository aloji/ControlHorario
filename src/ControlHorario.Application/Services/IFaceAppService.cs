using System;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IFaceAppService
    {
        Task<Guid> CreateAsync(Domain.Entities.Person person);
        Task<Guid?> GetFacePersonId(byte[] data);
        Task<Guid?> GetFacePersonId(string url);
        Task AddFaceAsync(Guid facePersonId, byte[] data);
        Task AddFaceAsync(Guid facePersonId, string url);
        Task<bool> TrainAndWaitAsync();
    }
}
