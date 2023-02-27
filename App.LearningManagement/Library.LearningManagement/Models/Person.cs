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
    }
}