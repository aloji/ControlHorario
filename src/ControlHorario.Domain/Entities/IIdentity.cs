using System;

namespace ControlHorario.Domain.Entities
{
    public interface IIdentity
    {
        Guid Id { get; set; }
    }
}
