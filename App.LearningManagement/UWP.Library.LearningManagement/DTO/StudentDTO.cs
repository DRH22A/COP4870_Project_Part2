using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UWP.Library.LearningManagement.DTO
{
    internal class StudentDTO : PeopleDTO
    {
        public StudentDTO(Person p) : base(p)
        {
            Grades = new Dictionary<int, double>();
        }

        public Dictionary<int, double> Grades { get; set; }

        public PersonClassification Classification { get; set; }

        public enum PersonClassification
        {
            Freshman, Sophomore, Junior, Senior, Instructor, TeachingAssistant
        }
    }
}
