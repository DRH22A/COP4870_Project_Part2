using System.Collections.Generic;

namespace Library.LearningManagement.Models
{
    public class Module
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ContentItem> Content { get; set; }

        public Module() { 
            Content= new List<ContentItem>();
        }
        public override string ToString()
        {
            return Name + ": " + Description;
        }
    }
}
