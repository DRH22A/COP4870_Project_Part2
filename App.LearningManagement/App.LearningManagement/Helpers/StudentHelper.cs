using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    internal class StudentHelper
    {
        private StudentService studentService;
        private CourseService courseService;

        public StudentHelper()
        {
            studentService = StudentService.Current;
            courseService = CourseService.Current;
        }

        public void CreateStudentRecord(Person? selectedPerson = null)
        {
            bool isCreate = false;
            if (selectedPerson == null)
            {
                isCreate = true;
                Console.WriteLine("What type of person would you like to add?");
                Console.WriteLine("(S)tudent");
                Console.WriteLine("(T)eachingAssistant");
                Console.WriteLine("(I)nstructor");
                var choice = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(choice))
                {
                    return;
                }
                if (choice.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedPerson = new Student();
                }
                else if (choice.Equals("T", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedPerson = new TeachingAssistant();
                }
                else if (choice.Equals("I", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedPerson = new Instructor();
                }
            }

            Console.WriteLine("What is the name of the person?");
            var name = Console.ReadLine();
            Console.WriteLine("What is the id of the person?");
            string id = Console.ReadLine() ?? string.Empty;
            int temp_code = Int32.Parse(id);
            bool check = false;
            while (!check)
            {
                if (studentService.Students.Any(s => s.Id == temp_code))
                {
                    Console.WriteLine("A person with that id already exists.");
                    Console.WriteLine("Please enter a different id:");
                    id = Console.ReadLine() ?? string.Empty;
                    temp_code = Int32.Parse(id);
                }
                else
                {
                    check = true;
                }
            }
            selectedPerson.Id = temp_code;


            if (selectedPerson is Student)
            {
                Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
                var classification = Console.ReadLine() ?? string.Empty;
                PersonClassification classEnum = PersonClassification.Freshman;

                if (classification.Equals("O", StringComparison.InvariantCultureIgnoreCase))
                {
                    classEnum = PersonClassification.Sophomore;
                }
                else if (classification.Equals("J", StringComparison.InvariantCultureIgnoreCase))
                {
                    classEnum = PersonClassification.Junior;
                }
                else if (classification.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    classEnum = PersonClassification.Senior;
                }
                var studentRecord = selectedPerson as Student;
                if (studentRecord != null)
                {
                    studentRecord.Classification = classEnum;
                    studentRecord.Id = int.Parse(id ?? "0");
                    studentRecord.Name = name ?? string.Empty;

                    if (isCreate)
                    {
                        studentService.Add(selectedPerson);
                    }
                }
            }
            else
            {
                if (selectedPerson != null)
                {
                    selectedPerson.Id = int.Parse(id ?? "0");
                    selectedPerson.Name = name ?? string.Empty;
                    if (isCreate)
                    {
                        studentService.Add(selectedPerson);
                    }
                }
            }
        }

        public void UpdateStudentRecord()
        {
            Console.WriteLine("Select a person to update:");
            studentService.Students.ForEach(Console.WriteLine);

            var selectionStr = Console.ReadLine();

            if (int.TryParse(selectionStr, out int selectionInt))
            {
                var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectionInt);
                if (selectedStudent != null)
                {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        public void ListStudents()
        {
            studentService.Students.ForEach(Console.WriteLine);

            Console.WriteLine("Select a student:");
            var selectionStr = Console.ReadLine();
            var selectionInt = int.Parse(selectionStr ?? "0");

            Console.WriteLine("Student Course List:");
            courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
        }

        public void SearchStudents()
        {
            Console.WriteLine("Enter a query:");
            var query = Console.ReadLine() ?? string.Empty;

            studentService.Search(query).ToList().ForEach(Console.WriteLine);
            var selectionStr = Console.ReadLine();
            var selectionInt = int.Parse(selectionStr ?? "0");

            Console.WriteLine("Student Course List:");
            courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
        }

        public void GradeAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse == null)
            {
                Console.WriteLine("Invalid course code.");
                return;
            }

            studentService.Students.ForEach(Console.WriteLine);
            Console.WriteLine("Enter the Id of the student:");
            string studentIdString = Console.ReadLine();
            if (!int.TryParse(studentIdString, out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }
            var selectedStudent = StudentService.Current.Students.FirstOrDefault(s => s.Id == studentId) as Student;
            if (selectedStudent == null)
            {
                Console.WriteLine("Invalid student ID for the selected course.");
                return;
            }


            Console.WriteLine("Enter the Id of the assignment:");
            selectedCourse.Assignments.ForEach(Console.WriteLine);
            string assignmentIdString = Console.ReadLine();
            if (!int.TryParse(assignmentIdString, out int assignmentId))
            {
                Console.WriteLine("Invalid assignment ID.");
                return;
            }

            var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (selectedAssignment == null)
            {
                Console.WriteLine("Invalid assignment ID for the selected course.");
                return;
            }

            Console.WriteLine($"Enter the grade for the assignment '{selectedAssignment.Name}' for student '{selectedStudent.Name}' in course '{selectedCourse.Name}':");
            string gradeString = Console.ReadLine();
            if (!double.TryParse(gradeString, out double grade))
            {
                Console.WriteLine("Invalid grade value.");
                return;
            }
            string grade_letter = "";
            if (grade >= 90)
            {
                grade_letter = "A";
            }
            else if (grade >= 80 && grade < 90)
            {
                grade_letter = "B";
            }
            else if (grade >= 70 && grade < 80)
            {
                grade_letter = "C";
            }
            else if (grade >= 60 && grade < 70)
            {
                grade_letter = "D";
            }
            else if (grade < 60 && grade >= 0)
            {
                grade_letter = "E";
            }
            selectedStudent.SetGrade(selectedAssignment.Id, grade);
            Console.WriteLine($"Grade '{grade}' {grade_letter} added for assignment '{selectedAssignment.Name}' for student '{selectedStudent.Name}' in course '{selectedCourse.Name}'.");


        }


        public void GetWeightedAverage()
        {
            double gpa_storage = 0;
            int coursesGraded = 0;
            studentService.Students.ForEach(Console.WriteLine);
            Console.WriteLine("Enter the ID of the student:");
            var studentIdStr = Console.ReadLine();
            if (int.TryParse(studentIdStr, out int studentId))
            {
                var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == studentId && s is Student) as Student;
                bool addMoreCourses = true;
                while (selectedStudent != null && addMoreCourses)
                {
                    courseService.Courses.ForEach(Console.WriteLine);
                    Console.WriteLine("Enter the code of the course:");
                    var courseCode = Console.ReadLine();
                    var selectedCourse = courseService.Courses.FirstOrDefault(c => c.Code.Equals(courseCode, StringComparison.InvariantCultureIgnoreCase));

                    if (selectedCourse != null)
                    {
                        selectedCourse.TotalGPAPoints = 0; // Reset TotalGPAPoints to 0 before calculating the weighted average for the selected course
                        double totalWeightedAverage = 0;
                        double totalWeight = 0;

                        foreach (var assignmentGroup in selectedCourse.AssignmentGroups)
                        {
                            double weightedAverage = selectedStudent.GetWeightedAverage(new List<AssignmentGroup> { assignmentGroup });
                            totalWeightedAverage += weightedAverage * assignmentGroup.weight;
                            totalWeight += assignmentGroup.weight;
                        }

                        string letterGrade = "";
                        double finalGrade = totalWeightedAverage / totalWeight;

                        if (finalGrade >= 90)
                        {
                            letterGrade = "A";
                            selectedCourse.TotalGPAPoints += 4 * selectedCourse.CreditHours;
                            coursesGraded++;
                        }
                        else if (finalGrade >= 80 && finalGrade < 90)
                        {
                            letterGrade = "B";
                            selectedCourse.TotalGPAPoints += 3 * selectedCourse.CreditHours;
                            coursesGraded++;
                        }
                        else if (finalGrade >= 70 && finalGrade < 80)
                        {
                            letterGrade = "C";
                            selectedCourse.TotalGPAPoints += 2 * selectedCourse.CreditHours;
                            coursesGraded++;
                        }
                        else if (finalGrade >= 60 && finalGrade < 70)
                        {
                            letterGrade = "D";
                            selectedCourse.TotalGPAPoints += selectedCourse.CreditHours;
                            coursesGraded++;
                        }
                        else
                        {
                            letterGrade = "F";
                            coursesGraded++;
                        }

                        gpa_storage += (selectedCourse.TotalGPAPoints / selectedCourse.CreditHours); // use the counter variable in the calculation
                        Console.WriteLine(gpa_storage);
                        Console.WriteLine($"Weighted Average for {selectedStudent.Name} in {selectedCourse.Name}: {Math.Round(finalGrade, 2)}% ({letterGrade})");
                        Console.WriteLine("Would you like to continue adding courses for your GPA or print the GPA (C/P)");
                        string decision = Console.ReadLine();
                        if (decision.Equals("P", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (selectedCourse.CreditHours == 0)
                            {
                                Console.WriteLine("No courses were added.");
                            }
                            else
                            {
                                double totalGPA = gpa_storage / coursesGraded;
                                Console.WriteLine($"Total GPA for {selectedStudent.Name}: {Math.Round(totalGPA, 2)}");
                            }
                            break;
                        }
                        else if (decision.Equals("C", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // continue adding courses
                        }
                        else
                        {
                            // print GPA
                            addMoreCourses = false;
                            // rest of the code to print GPA
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid course code. Please try again.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid student ID. Please try again.");
            }
        }

        public void CalculateGPA()
        {

        }
    }
}
