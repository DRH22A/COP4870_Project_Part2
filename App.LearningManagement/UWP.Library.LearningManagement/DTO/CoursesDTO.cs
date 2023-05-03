using Library.LearningManagement.DTO;
using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UWP.Library.LearningManagement.DTO
{
    public class CoursesDTO
    {
        public CoursesDTO() { }
        public CoursesDTO(Course c)
        {
            Code = c.Code;
            Name = c.Name;
            Description = c.Description;
            Room = c.Room;
            Semester = c.Semester;
            CreditHours = c.CreditHours;
            Id = c.Id;
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DisplayCourse { get; set; }
        public string Description { get; set; }
        public string Room { get; set; }
        public Semester Semester { get; set; }
        public List<Person> Roster { get; set; }
        public List<AnnouncementDTO> Announcements { get; set; }
        public List<AssignmentDTO> Assignments { get; set; }
        public List<ModuleDTO> Modules { get; set; }
        public double CreditHours { get; set; }
        public int Id { get; set; }
        public double TotalGPAPoints { get; set; }
    }
}
