using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IFaceAppService
    {
        Task<Guid> CreateAsync(Domain.Entities.Person person);
        Task<Guid?> GetFacePersonId(byte[] data);
        Task AddFaceAsync(Guid facePersonId, byte[] data);
    }
}
