using System;

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
        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }
    }
}