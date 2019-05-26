using System;

namespace ControlHorario.Domain.Entities
{
    public class Person
    {
        public Person()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? FacePersonId { get; set; }
    }
}
