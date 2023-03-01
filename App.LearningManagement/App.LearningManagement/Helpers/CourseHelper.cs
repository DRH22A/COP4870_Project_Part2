using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService;
        private StudentService studentService;
        private readonly ListNavigator<Course> courseNavigator;

        public CourseHelper()
        {
            studentService= StudentService.Current;
            courseService = CourseService.Current;
            courseNavigator = new ListNavigator<Course>(courseService.Courses, pageSize: 5);
        }

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            bool isNewCourse = false;
            if (selectedCourse == null)
            {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            var choice = "Y";
            if (!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course code?");
                choice = Console.ReadLine() ?? "N";
                if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("What is the code of the course?");
                    string newCode = Console.ReadLine() ?? string.Empty;

                    bool check = false;
                    while (!check)
                    {
                        if (courseService.Courses.Any(c => c.Code == newCode))
                        {
                            Console.WriteLine("A course with that code already exists.");
                            Console.WriteLine("Please enter a different code:");
                            newCode = Console.ReadLine() ?? string.Empty;
                        }
                        else
                        {
                            check = true;
                        }
                    }
                    selectedCourse.Code = newCode;
                }
            }


            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the code of the course?");
                string newCode = Console.ReadLine() ?? string.Empty;
                bool check = false;
                while (!check)
                {
                    if (courseService.Courses.Any(c => c.Code == newCode))
                    {
                        Console.WriteLine("A course with that code already exists.");
                        Console.WriteLine("Please enter a different code:");
                        newCode = Console.ReadLine() ?? string.Empty;
                    }
                    else
                    {
                        check = true;
                    }
                }
                selectedCourse.Code = newCode;
                Console.WriteLine("How many credit hours is this course worth?");
                string temp_credit = Console.ReadLine() ?? string.Empty;
                int int_credit = Int32.Parse(temp_credit);
                selectedCourse.CreditHours = int_credit;
            }

            if (!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course name?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the name of the course?");
                selectedCourse.Name = Console.ReadLine() ?? string.Empty;
            }

            if(!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course description?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the description of the course?");
                selectedCourse.Description = Console.ReadLine() ?? string.Empty;
            }

            if (!isNewCourse)
            {
                Console.WriteLine("Do you want to update the course credit hours?");
                choice = Console.ReadLine() ?? "N";
            }
            else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the new credit hours of the course?");
                string temp_hours_input = Console.ReadLine() ?? string.Empty;
                int int_hours_input = Int32.Parse(temp_hours_input);
                selectedCourse.CreditHours = int_hours_input;
            }

            if (isNewCourse)
            {

                SetupRoster(selectedCourse);
                SetupAssignments(selectedCourse);
            }
            
            
            if(isNewCourse)
            {
                courseService.Add(selectedCourse);
            }

        }
        public void DisplayCourses()
        {
            var courses = courseNavigator.GetCurrentPage();
            while (true)
            {
                foreach (var course in courses.Values)
                {
                    Console.WriteLine($"{course.Code} - {course.Name}");
                }
                Console.WriteLine("Use 'P' to go to the previous page, 'N' to go to the next page, 'F' to go to the first page, 'L' to go to the last page," +
                                  " or enter a page number to go directly to that page. Enter 'Q' to quit:");
                Console.WriteLine($"Page [{courseNavigator.ReturnCurrentPage()} / {courseNavigator.ReturnLastPage()}]");
                var input = Console.ReadLine();
                if (input.Equals("P", StringComparison.InvariantCultureIgnoreCase) && courseNavigator.HasPreviousPage)
                {
                    courses = courseNavigator.GoBackward();
                }
                else if (input.Equals("N", StringComparison.InvariantCultureIgnoreCase) && courseNavigator.HasNextPage)
                {
                    courses = courseNavigator.GoForward();
                }
                else if (input.Equals("F", StringComparison.InvariantCultureIgnoreCase))
                {
                    courses = courseNavigator.GoToFirstPage();
                }
                else if (input.Equals("L", StringComparison.InvariantCultureIgnoreCase))
                {
                    courses = courseNavigator.GoToLastPage();
                }
                else if (int.TryParse(input, out int pageNumber))
                {
                    courses = courseNavigator.GoToPage(pageNumber);
                }
                else if (input.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;  // prompt the user again
                }
            }
        }

        public void UpdateCourseRecord()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course to update:");
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                CreateCourseRecord(selectedCourse);
            }
        }

        public void SearchCourses(string? query = null)
        {
            if(string.IsNullOrEmpty(query))
            {
                DisplayCourses();
            }
            else
            {
                courseService.Search(query).ToList().ForEach(Console.WriteLine);
            }

            Console.WriteLine("Select a course:");
            var code = Console.ReadLine() ?? string.Empty;

            var selectedCourse = courseService
                .Courses
                .FirstOrDefault(c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            if(selectedCourse != null)
            {
                Console.WriteLine(selectedCourse.DetailDisplay);
            }
        }

        public void AddStudent()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course to add the student to:");
            var selection = Console.ReadLine();
            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                studentService.Students.Where(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);
                if (studentService.Students.Any(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if(selection != null)
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId) as Student;
                    if (selectedStudent != null)
                    {
                        selectedCourse.Roster.Add(selectedStudent);
                    }
                }
            }
        }
        public void RemoveStudent()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course to remove the student from:");
            var selection = Console.ReadLine();
            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                selectedCourse.Roster.ForEach(Console.WriteLine);
                if (selectedCourse.Roster.Any())
                {
                    selection = Console.ReadLine() ?? string.Empty;
                } else
                {
                    selection = null;
                }

                if (selection != null)
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);
                    if (selectedStudent != null)
                    {
                        selectedCourse.Roster.Remove(selectedStudent);
                    }
                }

            }
        }
        public void AddAssignment()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course to add the assignment to:");
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                selectedCourse.Assignments.Add(CreateAssignment());
            }
        }

        public void GroupAssignment()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course to add the assignment to:");
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to add to a group:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    Console.WriteLine("Enter the group name:");
                    var groupName = Console.ReadLine();
                    var assignmentGroup = selectedCourse.AssignmentGroups.FirstOrDefault(g => g.group_name.Equals(groupName, StringComparison.InvariantCultureIgnoreCase));
                    if (assignmentGroup == null)
                    {
                        Console.WriteLine("Enter the weight of the group (ie. 40 = 40%):");
                        var temp_weight = Console.ReadLine();
                        double new_weight = Double.Parse(temp_weight);
                        while(new_weight > 100)
                        {
                            Console.WriteLine("Invalid weight value");
                            Console.WriteLine("Enter the weight of the group (ie. 40 = 40%):");
                            temp_weight = Console.ReadLine();
                            new_weight = Double.Parse(temp_weight);

                        }
                        assignmentGroup = new AssignmentGroup
                        {
                            group_name = groupName ?? string.Empty,
                            assignments = new List<Assignment> { selectedAssignment },
                            weight = new_weight
                        };
                        selectedCourse.AssignmentGroups.Add(assignmentGroup);
                    }
                    else
                    {
                        if (!assignmentGroup.assignments.Contains(selectedAssignment))
                        {
                            assignmentGroup.assignments.Add(selectedAssignment);
                            Console.WriteLine($"Added assignment '{selectedAssignment.Name}' to group '{assignmentGroup.group_name}'");
                            Console.WriteLine("Assignments in the group:");
                        }
                        else
                        {
                            Console.WriteLine($"Assignment '{selectedAssignment.Name}' already exists in group '{assignmentGroup.group_name}'");
                        }
                    }
                    Console.WriteLine($"{assignmentGroup.group_name}:");
                    assignmentGroup.assignments.ForEach(Console.WriteLine);
                }
            }
        }



        public void UpdateAssignment()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course:");
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to update:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if(selectedAssignment != null)
                {
                    var index = selectedCourse.Assignments.IndexOf(selectedAssignment);
                    selectedCourse.Assignments.RemoveAt(index);
                    selectedCourse.Assignments.Insert(index, CreateAssignment());
                }
            }
        }

        public void RemoveAssignment()
        {
            DisplayCourses();
            Console.WriteLine("Enter the code for the course:");
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                Console.WriteLine("Choose an assignment to delete:");
                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    selectedCourse.Assignments.Remove(selectedAssignment);
                }
            }
        }

        private void SetupRoster(Course c)
        {
            Console.WriteLine("Which students should be enrolled in this course? ('Q' to quit)");
            bool continueAdding = true;
            while (continueAdding)
            {
                studentService.Students.Where(s => !c.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);
                var selection = "Q";
                if (studentService.Students.Any(s => !c.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    continueAdding = false;
                }
                else
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);

                    if (selectedStudent != null)
                    {
                        c.Roster.Add(selectedStudent);
                    }
                }
            }
        }

        private void SetupAssignments(Course c)
        {
            Console.WriteLine("Would you like to add assignments? (Y/N)");
            var assignResponse = Console.ReadLine() ?? "N";
            bool continueAdding;
            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                continueAdding = true;
                while (continueAdding)
                {
                    c.Assignments.Add(CreateAssignment());
                    Console.WriteLine("Add more assignments? (Y/N)");
                    assignResponse = Console.ReadLine() ?? "N";
                    if (assignResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = false;
                    }
                }
            }

        }

        private Assignment CreateAssignment()
        {
            //Name
            Console.WriteLine("Name:");
            var assignmentName = Console.ReadLine() ?? string.Empty;
            //Description
            Console.WriteLine("Description:");
            var assignmentDescription = Console.ReadLine() ?? string.Empty;
            //TotalPoints
            Console.WriteLine("TotalPoints:");
            var totalPoints = decimal.Parse(Console.ReadLine() ?? "100");
            //DueDate
            Console.WriteLine("DueDate:");
            var dueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

            return new Assignment
            {
                Name = assignmentName,
                Description = assignmentDescription,
                TotalAvailablePoints = totalPoints,
                DueDate = dueDate
            };
        }

        public void CRUDAnnouncement()
        {
            Console.WriteLine("Would you like to [1.]create, [2.]read, [3.]update, or [4.]delete an announcement for a course:");
            string choice_value = Console.ReadLine() ?? string.Empty;
            int int_choice_value = Int32.Parse(choice_value);
            Announcement CreateAnnouncement()
            {
                var announcement = new Announcement();
                Console.WriteLine("Enter announcement name:");
                announcement.announcement_name = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter announcement description:");
                announcement.announcement_description = Console.ReadLine() ?? string.Empty;
                return announcement;
            }
            if (int_choice_value == 1)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course to add the announcement to:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    var announcement = CreateAnnouncement();
                    selectedCourse.Announcements.Add(announcement);
                    Console.WriteLine($"Announcement '{announcement.announcement_name}' added to course '{selectedCourse.Name}'");
                }
            }
            else if (int_choice_value == 2)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine($"Announcements for course '{selectedCourse.Name}':");
                    for (int i = 0; i < selectedCourse.Announcements.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Announcements[j].announcement_name);
                        j++;
                    }
                }
            }
            else if (int_choice_value == 3)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine("Choose an announcement to update:");
                    for (int i = 0; i < selectedCourse.Announcements.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Announcements[j].announcement_name);
                        j++;
                    }
                    var selectionStr = Console.ReadLine() ?? string.Empty;
                    var selectionInt = int.Parse(selectionStr);
                    selectionInt -= 1;
                    var selectedAnnouncement = selectedCourse.Announcements.FirstOrDefault(a => a.announcement_id == selectionInt);
                    if (selectedAnnouncement != null)
                    {
                        var index = selectedCourse.Announcements.IndexOf(selectedAnnouncement);
                        selectedCourse.Announcements.RemoveAt(index);
                        selectedCourse.Announcements.Insert(index, CreateAnnouncement());
                        Console.WriteLine($"Announcement '{selectedAnnouncement.announcement_name}' updated");
                    }
                }
            }
            else if (int_choice_value == 4)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine("Choose an announcement to delete:");
                    for (int i = 0; i < selectedCourse.Announcements.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Announcements[j].announcement_name);
                        j++;
                    }
                    var selectionStr = Console.ReadLine() ?? string.Empty;
                    var selectionInt = int.Parse(selectionStr);
                    selectionInt -= 1;
                    var selectedAnnouncement = selectedCourse.Announcements.FirstOrDefault(a => a.announcement_id == selectionInt);
                    if (selectedAnnouncement != null)
                    {
                        selectedCourse.Announcements.Remove(selectedAnnouncement);
                        Console.WriteLine($"Announcement '{selectedAnnouncement.announcement_name}' deleted");
                    }
                }
            }
        }
        public void CRUDModule()
        {
            Console.WriteLine("Would you like to [1.]create, [2.]read, [3.]update, or [4.]delete a module for a course:");
            string choice_value = Console.ReadLine() ?? string.Empty;
            int int_choice_value = Int32.Parse(choice_value);

            Module CreateModule()
            {
                var module = new Module();
                Console.WriteLine("Enter module name:");
                module.Name = Console.ReadLine();
                Console.WriteLine("Enter module description:");
                module.Description = Console.ReadLine();
                return module;
            }

            if (int_choice_value == 1)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course to add the module to:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    var module = CreateModule();
                    selectedCourse.Modules.Add(module);
                    Console.WriteLine($"Module '{module.Name}' added to course '{selectedCourse.Name}'");
                }
            }
            else if (int_choice_value == 2)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine($"Modules for course '{selectedCourse.Name}':");
                    for (int i = 0; i < selectedCourse.Modules.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Modules[j].Name);
                        j++;
                    }
                }
            }
            else if (int_choice_value == 3)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine("Choose a module to update:");
                    for (int i = 0; i < selectedCourse.Modules.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Modules[j].Name);
                        j++;
                    }
                    var selectionStr = Console.ReadLine() ?? string.Empty;
                    var selectionInt = int.Parse(selectionStr);
                    selectionInt -= 1;
                    var selectedModule = selectedCourse.Modules.ElementAtOrDefault(selectionInt);
                    if (selectedModule != null)
                    {
                        var index = selectedCourse.Modules.IndexOf(selectedModule);
                        selectedCourse.Modules.RemoveAt(index);
                        selectedCourse.Modules.Insert(index, CreateModule());
                        Console.WriteLine($"Module '{selectedModule.Name}' updated");
                    }
                }
            }
            else if (int_choice_value == 4)
            {
                DisplayCourses();
                Console.WriteLine("Enter the code for the course:");
                var selection = Console.ReadLine();

                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                if (selectedCourse != null)
                {
                    Console.WriteLine("Choose a module to delete:");
                    for (int i = 0; i < selectedCourse.Modules.Count; i++)
                    {
                        int j = 0;
                        Console.WriteLine("[" + ++i + ".] " + selectedCourse.Modules[j].Name);
                        j++;
                    }
                    var selectionStr = Console.ReadLine();
                    int selectionIndex;
                    if (int.TryParse(selectionStr, out selectionIndex) && selectionIndex >= 1 && selectionIndex <= selectedCourse.Modules.Count)
                    {
                        selectedCourse.Modules.RemoveAt(selectionIndex - 1);
                        Console.WriteLine("Module deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
            }
        }
    }
}
