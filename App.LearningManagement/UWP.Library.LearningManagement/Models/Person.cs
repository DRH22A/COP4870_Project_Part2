using System;
using UWP.Library.LearningManagement.DTO;

namespace Library.LearningManagement.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Person()
        {
            Name = string.Empty;
            Id= 0;
            Password = string.Empty;
        }

        public Person(PeopleDTO dto)
        {
            Name = dto.Name;
            Id = dto.Id;
            Password = dto.Password;
        }
        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }
    }
}