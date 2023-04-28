using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using Newtonsoft.Json;
using SupportTicketApplication;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UWP.Library.LearningManagement.DTO;

namespace UWP.LearningManagement.ViewModels
{
    internal class MainViewModel
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
        public ObservableCollection<CoursesVM> CoursesDTOs
        {
            get
            {
                var payload = new WebRequestHandler().Get("http://localhost:5140/Courses").Result;
                var returnVal = JsonConvert.DeserializeObject<ObservableCollection<CoursesDTO>>(payload).Select(d => new CoursesVM(d));
                return (ObservableCollection<CoursesVM>)returnVal;
            }
        }

    }
}
