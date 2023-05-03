using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using Newtonsoft.Json;
using SupportTicketApplication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UWP.LearningManagement.Dialogs;
using UWP.Library.LearningManagement.DTO;

namespace UWP.LearningManagement.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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
        public IEnumerable<CoursesVM> CoursesDTOs
        {
            get
            {
                var payload = new WebRequestHandler().Get("http://localhost:5140/Courses").Result;
                var returnVal = JsonConvert.DeserializeObject<ObservableCollection<CoursesDTO>>(payload).Select(c => new CoursesVM(c));
                return returnVal.OrderBy(c => c.Dto.Id);
            }
        }

        public IEnumerable<PeopleVM> PeopleDTOs
        {
            get
            {
                var payload = new WebRequestHandler().Get("http://localhost:5140/People").Result;
                var returnVal = JsonConvert.DeserializeObject<ObservableCollection<PeopleDTO>>(payload).Select(p => new PeopleVM(p));
                return returnVal.OrderBy(p => p.Dto.Id);
            }
        }
        public PeopleVM SelectedPerson { get; set; }

        public CoursesVM SelectedCourse { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshList()
        {
            NotifyPropertyChanged(nameof(CoursesDTOs));
            NotifyPropertyChanged(nameof(PeopleDTOs));
        }
    }
}
