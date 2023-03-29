using System;

namespace Library.LearningManagement.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Person()
        {
            Name = string.Empty;
            Id= 0;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }

        public static object Where(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}