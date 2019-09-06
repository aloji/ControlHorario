using ControlHorario.Domain.Events;
using System;

namespace ControlHorario.Application.Events
{
    public class PersonCreatedEvent : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public PersonCreatedEvent(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
