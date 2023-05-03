using Library.LearningManagement.Models;
using Library.LearningManagement.DTO;
using System;

namespace Library.LearningManagement.DTO
{
    public class AssignmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double TotalAvailablePoints { get; set; }
        public DateTime DueDate { get; set; }

        public AssignmentDTO() { }
        public AssignmentDTO(Assignment assignment)
        {
            Id = assignment.Id;
            Name = assignment.Name;
            Description = assignment.Description;
            TotalAvailablePoints = assignment.TotalAvailablePoints;
            DueDate = assignment.DueDate;
        }
    }
}
