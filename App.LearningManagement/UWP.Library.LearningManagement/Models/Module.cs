using System.Collections.Generic;
using UWP.Library.LearningManagement.DTO;

namespace Library.LearningManagement.Models
{
    public class Module
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ContentItem> Content { get; set; }

        public Module() { 
            Name = string.Empty;
            Description = string.Empty;
        }

        public Module(ModuleDTO dto) 
        {
            Name = dto.Name;
            Description = dto.Description;
        }
        public override string ToString()
        {
            return Name + ": " + Description;
        }
    }
}
