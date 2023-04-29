using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UWP.Library.LearningManagement.DTO;

namespace UWP.Library.LearningManagement.Database
{
    public static class DatabaseContext
    {
        public static ObservableCollection<Course> courses = new ObservableCollection<Course>
        {
            new Course { Code = "COP1212", Name = "COMPUTING", Description = "COOL COMPUTERS", Room = "HCB103", Semester = { Season = SeasonEnum.Spring, Year = YearEnum.Year_2023}}
        };

        public static ObservableCollection<Person> people = new ObservableCollection<Person>
        {
            new Student {Name = "DANNY", Id = 123, Classification = PersonClassification.Freshman}
        };
    }
}
