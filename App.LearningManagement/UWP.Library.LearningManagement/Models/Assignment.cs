using Library.LearningManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Library.LearningManagement.DTO;

namespace Library.LearningManagement.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double TotalAvailablePoints { get; set; }
        public DateTime DueDate { get; set; }
        public Assignment()
        {
            Name = string.Empty;
            Description = string.Empty;
            Id = 0;
            TotalAvailablePoints = 0;
            DueDate = DateTime.MinValue;
        }

        public Assignment(AssignmentDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            TotalAvailablePoints = dto.TotalAvailablePoints;
            DueDate = dto.DueDate;
        }
        public override string ToString()
        {
            return $"{Id}. ({DueDate}) {Name} {Description}";
        }
    }
}
