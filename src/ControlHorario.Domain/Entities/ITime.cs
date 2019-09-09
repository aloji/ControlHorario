using System;

namespace ControlHorario.Domain.Entities
{
    public interface ITime
    {
        DateTime DateTimeUtc { get; set; }
    }
}