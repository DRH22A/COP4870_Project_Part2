using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.ObjectModel;

namespace UWP.LearningManagement.ViewModels
{
    public class MainViewModel
    {
        private CourseService courseService;
        private StudentService studentService;
        public MainViewModel() {
            courseService = new CourseService();
            studentService = new StudentService();
        }

        public ObservableCollection<Course> Courses
        {
            get
            {
                return courseService.Courses;
            }
        }

        public ObservableCollection<Person> People => studentService.People;

    }
}
