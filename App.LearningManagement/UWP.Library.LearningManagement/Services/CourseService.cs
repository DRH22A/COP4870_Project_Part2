using Library.LearningManagement.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library.LearningManagement.Services
{
    public class CourseService
    {
        private int loggedInUserId = -1;
        private ObservableCollection<Course> courseList;
        private static CourseService _instance;

        public ObservableCollection<Course> Courses
        {
            get { return courseList; }
        }

        public CourseService()
        {
            courseList = new ObservableCollection<Course>()
            {
                new Course {Code = "COP4870", Name = "Full-Stack Application Development with C#", Description = "Fun Programming 1", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },
                new Course { Code = "COP4530", Name = "DATA STRUCTURES, ALGORITHMS, AND GENERIC PROGRAMMING", Description = "Fun Programming 2", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },
                new Course { Code = "COP4342", Name = "UNIX TOOLS4", Description = "Fun Programming 3", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },
                new Course { Code = "COP43422", Name = "UNIX TOOLS3", Description = "Fun Programming 3", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },
                new Course { Code = "COP434233", Name = "UNIX TOOLS2", Description = "Fun Programming 3", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },
                new Course { Code = "COP434254", Name = "UNIX TOOLS1", Description = "Fun Programming 3", Room = "102C", Semester = {Season = SeasonEnum.Spring, Year = YearEnum.Year_2024} },

            };
        }

        public static CourseService Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CourseService();
                }

                return _instance;
            }
        }

        public void Add(Course course)
        {
            courseList.Add(course);
        }

        public void Remove(Course course)
        {
            courseList.Remove(course);
        }
        public void RemoveAssignment(Assignment assignment)
        {
            foreach (var course in courseList)
            {
                if (course.Assignments.Contains(assignment))
                {
                    course.Assignments.Remove(assignment);
                    break;
                }
            }
        }

        public IEnumerable<Course> Search(string query)
        {
            return Courses.Where(s => s.Name.ToUpper().Contains(query.ToUpper())
                || s.Description.ToUpper().Contains(query.ToUpper())
                || s.Code.ToUpper().Contains(query.ToUpper()));
        }
        public int LoggedInUserId
        {
            get { return loggedInUserId; }
            set { loggedInUserId = value; }
        }
    }
}
