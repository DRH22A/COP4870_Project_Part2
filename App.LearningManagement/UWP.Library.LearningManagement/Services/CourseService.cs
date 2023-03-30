using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services
{
    public class CourseService
    {
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
                new Course {Code = "COP4870", Name = "Full-Stack Application Development with C#", Description = "Fun Programming 1"},
                new Course { Code = "COP4530", Name = "DATA STRUCTURES, ALGORITHMS, AND GENERIC PROGRAMMING", Description = "Fun Programming 2"},
                new Course { Code = "COP4342", Name = "UNIX TOOLS", Description = "Fun Programming 3"}
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

        public IEnumerable<Course> Search(string query)
        {
            return Courses.Where(s => s.Name.ToUpper().Contains(query.ToUpper())
                || s.Description.ToUpper().Contains(query.ToUpper())
                || s.Code.ToUpper().Contains(query.ToUpper()));
        }
    }
}
