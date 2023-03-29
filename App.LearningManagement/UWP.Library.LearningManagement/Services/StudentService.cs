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
        private ObservableCollection<Person> studentList;
        private ObservableCollection<Course> courseList;

        private static StudentService _instance;
        public ObservableCollection<Person> Students
        {
            get { return studentList; }
        }
        public StudentService()
        {
            studentList = new ObservableCollection<Person>()
            {
                new Person{ Name = "Daniel Halterman", Id=1},
                new Person{ Name = "Brian Ranner", Id=2},
                new Person{ Name = "Scott Reynolds", Id=3}
            };
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
            studentList.Add(student);
        }

        public IEnumerable<Person> Search(string query)
        {
            return studentList.Where(s => s.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
