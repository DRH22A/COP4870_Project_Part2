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
            DisplayCourse = c.Name;
            Description = c.Description;
            Room = c.Room;
            Semester = c.Semester;
            CreditHours = c.CreditHours;
        }

        public string Code { get; set; }
        public string DisplayCourse { get; set; }
        public string Description { get; set; }
        public string Room { get; set; }
        public Semester Semester { get; set; }
        public List<Person> Roster { get; set; }
        public List<AssignmentDTO> Assignments { get; set; }
        public List<Module> Modules { get; set; }
        public List<AssignmentGroup> AssignmentGroups { get; set; }
        public List<Announcement> Announcements { get; set; }
        public double CreditHours { get; set; }
        public double TotalGPAPoints { get; set; }
    }
}
