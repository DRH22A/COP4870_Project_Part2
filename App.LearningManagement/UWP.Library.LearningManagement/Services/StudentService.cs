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
    public class StudentService
    {
        private ObservableCollection<Person> personList;
        private ObservableCollection<Course> courseList;

        private static StudentService _instance;
        public ObservableCollection<Person> People
        {
            get { return personList; }
        }
        public StudentService()
        {
            personList = new ObservableCollection<Person>()
            {
                new Student{ Name = "Daniel Halterman", Id=1, Classification=PersonClassification.Freshman, Password="123"},
                new Instructor{ Name = "Brian Ranner", Id=2, Password="234"},
                new TeachingAssistant{ Name = "Scott Reynolds", Id=3},
                new Student{ Name = "Daniel Halterman2", Id=11, Classification=PersonClassification.Freshman},
                new Student{ Name = "Daniel Halterman3", Id=111, Classification=PersonClassification.Freshman},
                new Student{ Name = "Daniel Halterman4", Id=1111, Classification=PersonClassification.Freshman}

            };
            courseList = new ObservableCollection<Course>();    
        }

        public static StudentService Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StudentService();
                }

                return _instance;
            }
        }

        public void Add(Person student)
        {
            personList.Add(student);
        }
        public void Remove(Person student)
        {
            personList.Remove(student);
        }

        public IEnumerable<Person> Search(string query)
        {
            return personList.Where(s => s.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
