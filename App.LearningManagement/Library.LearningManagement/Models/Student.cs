using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public class Student : Person
    {
        public Dictionary<int, double> Grades { get; set; }

        public PersonClassification Classification { get; set; }

        public Student() {
            Grades = new Dictionary<int, double>();
        }

        public void SetGrade(int assignmentId, double grade)
        {
            if (Grades.ContainsKey(assignmentId))
            {
                Grades[assignmentId] = grade;
            }
            else
            {
                Grades.Add(assignmentId, grade);
            }
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Classification}";
        }
    }

    public enum PersonClassification
    {
        Freshman, Sophomore, Junior, Senior
    }
}
