using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public double GetWeightedAverage(List<AssignmentGroup> assignmentGroups)
        {
            double totalWeightedGrade = 0;
            double totalWeight = 0;
            int totalAssignments = 0;

            foreach (var assignmentGroup in assignmentGroups)
            {
                double groupWeight = assignmentGroup.weight;
                double groupTotalGrade = 0;
                double groupTotalWeight = 0;

                foreach (var assignment in assignmentGroup.assignments)
                {
                    if (Grades.TryGetValue(assignment.Id, out double grade))
                    {
                        groupTotalGrade += grade * assignmentGroup.weight;
                        groupTotalWeight += assignmentGroup.weight;
                        totalAssignments++;
                    }
                }

                if (groupTotalWeight > 0)
                {
                    double groupWeightedGrade = groupTotalGrade / groupTotalWeight;
                    totalWeightedGrade += groupWeightedGrade * groupWeight;
                    totalWeight += groupWeight;
                }
            }

            if (totalWeight > 0)
            {
                return totalWeightedGrade / totalWeight;
            }
            else
            {
                return 0;
            }
        }
    }

    public enum PersonClassification
    {
        Freshman, Sophomore, Junior, Senior
    }

}
