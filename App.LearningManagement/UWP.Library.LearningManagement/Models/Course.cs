using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.LearningManagement.Models.Assignment;
using static Library.LearningManagement.Models.Module;


namespace Library.LearningManagement.Models
{
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Room { get; set; }
        public Semester Semester { get; set; }
        public List<Person> Roster { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Module> Modules { get; set; }
        public List<AssignmentGroup> AssignmentGroups { get; set; }
        public List<Announcement> Announcements { get; set; }
        public double CreditHours { get; set; }
        public double TotalGPAPoints { get; set; }
        public virtual string Display => $"{Code} - {Name}";

        public Course() { 
            Code = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Room = string.Empty;
            Semester = new Semester { Season = SeasonEnum.Fall, Year = YearEnum.Year_2023 };
            Roster = new List<Person>();
            Assignments= new List<Assignment>();
            Modules= new List<Module>();
            AssignmentGroups = new List<AssignmentGroup>();
            Announcements = new List<Announcement>();
            CreditHours = 0;
            TotalGPAPoints = 0;
        }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }

        public string DetailDisplay
        {
            get
            {
                return $"{ToString()}\n{Description}\n\n" +
                    $"Roster:\n{string.Join("\n", Roster.Select(s => s.ToString()).ToArray())}\n\n" +
                    $"Assignments:\n{string.Join("\n", Assignments.Select(a => a.ToString()).ToArray())}";
            }
        }
        public void AddAssignment(Assignment assignment, string groupName, double Weight)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                Assignments.Add(assignment);
            }
        }

        public void AddAssignmentGroup(AssignmentGroup assignmentGroup)
        {
            var group = AssignmentGroups.FirstOrDefault(g => g.group_name == assignmentGroup.group_name);
            if (group == null)
            {
                AssignmentGroups.Add(assignmentGroup);
            }
        }

        public void RemoveAssignment(Assignment assignment)
        {
            Assignments.Remove(assignment);
        }
    }
    public class AssignmentGroup
    {
        public string group_name { get; set; }
        public List<Assignment> assignments { get; set; }
        public double weight { get; set; }
    }
    public class Announcement 
    {
        public Announcement()
        {
            announcement_name = string.Empty;
            announcement_description = string.Empty;
            announcement_id = 0;
        }
        public string announcement_name { get; set; }
        public string announcement_description { get; set; }
        public int announcement_id { get; set; }
        public override string ToString()
        {
            return announcement_name + ": " + announcement_description;
        }
    }
}
