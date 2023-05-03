using System;
using System.Collections.Generic;
using System.Text;
using Library.LearningManagement.Models;
using Library.LearningManagement.DTO;


namespace UWP.Library.LearningManagement.DTO
{
    public class ModuleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ContentItem> Content { get; set; }

        public ModuleDTO() { }
        public ModuleDTO(Module module)
        {
            Name = module.Name;
            Description = module.Description;
        }
    }
}
