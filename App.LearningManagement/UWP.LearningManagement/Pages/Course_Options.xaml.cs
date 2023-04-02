using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UWP.LearningManagement.ViewModels;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP.LearningManagement.Pages
{
    public sealed partial class Course_Options : Page
    {
        public Course_Options()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            ViewToggle.IsChecked = Toggle_State.IsChecked;
        }

        private void ViewToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            ViewToggle.Content = "Switch To Student Mode";
            addToCourse.Content = "Add A Student To A Course";
            removeFromCourse.Content = "Remove A Student From A Course";
            addAssignment.Visibility = Visibility.Visible;
            updateAssignment.Visibility = Visibility.Visible;
            removeAssignment.Visibility = Visibility.Visible;
            crudAnnouncement.Visibility = Visibility.Visible;
            crudModule.Visibility = Visibility.Visible;
            addCourse.Visibility = Visibility.Visible;
            updateCourse.Visibility = Visibility.Visible;
            deleteCourse.Visibility = Visibility.Visible;
        }

        private void ViewToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            ViewToggle.Content = "Switch To TA/Instructor Mode";
            addToCourse.Content = "Add Yourself To A Course";
            removeFromCourse.Content = "Remove Yourself From A Course";
            addAssignment.Visibility = Visibility.Collapsed;
            updateAssignment.Visibility = Visibility.Collapsed;
            removeAssignment.Visibility = Visibility.Collapsed;
            crudAnnouncement.Visibility= Visibility.Collapsed;
            crudModule.Visibility = Visibility.Collapsed;
            addCourse.Visibility = Visibility.Collapsed;
            updateCourse.Visibility = Visibility.Collapsed;
            deleteCourse.Visibility = Visibility.Collapsed;
        }

        private async void CreateCourseRecord_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog addCourseDialog = new ContentDialog
            {
                Title = "Add a New Course",
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBox { PlaceholderText = "Enter course name here", Name = "CourseNameTextBox" },
                        new TextBox { PlaceholderText = "Enter course code here", Name = "CourseCodeTextBox" },
                        new TextBox { PlaceholderText = "Enter course description here", Name = "CourseDescriptionTextBox" },
                        new TextBox { PlaceholderText = "Enter course room here", Name = "CourseRoomTextBox" },
                        new TextBox { PlaceholderText = "Enter credit hours here", Name = "CreditHoursTextBox" },
                        new ComboBox { PlaceholderText = "Select season", Name = "SeasonComboBox",
                        ItemsSource = Enum.GetValues(typeof(SeasonEnum)).Cast<SeasonEnum>() },
                        new ComboBox { PlaceholderText = "Select year", Name = "YearComboBox",
                        ItemsSource = Enum.GetValues(typeof(YearEnum)).Cast<YearEnum>() },
                        new TextBlock { Foreground = new SolidColorBrush(Colors.Red), Name = "ErrorTextBlock" }
                    }
                },
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel"
            };

            addCourseDialog.PrimaryButtonClick += async (contentDialog, contentDialogArgs) =>
            {
                TextBox nameTextBox = (TextBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseNameTextBox");
                TextBox codeTextBox = (TextBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseCodeTextBox");
                TextBox descriptionTextBox = (TextBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseDescriptionTextBox");
                TextBox roomTextBox = (TextBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseRoomTextBox");
                TextBox creditTextBox = (TextBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CreditHoursTextBox");
                ComboBox seasonComboBox = (ComboBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is ComboBox && ((ComboBox)c).Name == "SeasonComboBox");
                ComboBox yearComboBox = (ComboBox)((StackPanel)addCourseDialog.Content).Children.First(c => c is ComboBox && ((ComboBox)c).Name == "YearComboBox");
                TextBlock errorTextBlock = (TextBlock)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");

                string name = nameTextBox.Text.Trim();
                string code = codeTextBox.Text.Trim();
                string description = descriptionTextBox.Text.Trim();
                string room = roomTextBox.Text.Trim();
                string creditString = creditTextBox.Text.Trim();
                int credit = int.Parse(creditString);
                SeasonEnum season = (SeasonEnum)seasonComboBox.SelectedItem;
                YearEnum year = (YearEnum)yearComboBox.SelectedItem;

                if (string.IsNullOrEmpty(name))
                {
                    errorTextBlock.Text = "Please enter a course name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                Course newCourse;
                if (nameTextBox.SelectedText != null)
                {
                    newCourse = new Course { Code = code, Name = name, CreditHours = credit, Description = description, Room = room, Semester = new Semester { Season = SeasonEnum.Fall, Year = YearEnum.Year_2023 } };
                }
                else
                {
                    errorTextBlock.Text = "Please enter a course.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                if (CourseService.Current.Courses.Any(s => s.Code == newCourse.Code))
                {
                    errorTextBlock.Text = "A course with the same code already exists. Please enter a unique code.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                if (CourseService.Current.Courses.Any(s => s.Name == newCourse.Name))
                {
                    errorTextBlock.Text = "A course with the same name already exists. Please enter a unique name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }
                CourseService.Current.Add(newCourse);

                if (!string.IsNullOrWhiteSpace(course_searchBox.Text))
                {
                    coursesList.ItemsSource = StudentService.Current.Search(course_searchBox.Text);
                }
                else
                {
                    coursesList.ItemsSource = StudentService.Current.People;
                }
            };
            await addCourseDialog.ShowAsync();
        }

        private async void UpdateCourseRecord_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to update?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = courseComboBox.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    ContentDialog editCourseDialog = new ContentDialog
                    {
                        Title = "Edit Course Information",
                        Content = new StackPanel
                        {
                            Children =
                            {
                                new TextBox { PlaceholderText = "Enter course name here", Name = "CourseNameTextBox", Text = selectedCourse.Name },
                                new TextBox { PlaceholderText = "Enter course code here", Name = "CourseCodeTextBox", Text = selectedCourse.Code },
                                new TextBox { PlaceholderText = "Enter course description here", Name = "CourseDescriptionTextBox", Text = selectedCourse.Description },
                                new TextBox { PlaceholderText = "Enter course room here", Name = "CourseRoomTextBox", Text = selectedCourse.Room},
                                new ComboBox { PlaceholderText = "Select season", Name = "SeasonComboBox", SelectedItem = selectedCourse.Semester.Season,
                                ItemsSource = Enum.GetValues(typeof(SeasonEnum)).Cast<SeasonEnum>() },
                                new ComboBox { PlaceholderText = "Select year", Name = "YearComboBox", SelectedItem = selectedCourse.Semester.Year,
                                ItemsSource = Enum.GetValues(typeof(YearEnum)).Cast<YearEnum>() },
                                new TextBlock { Foreground = new SolidColorBrush(Colors.Red), Name = "ErrorTextBlock" }
                            }
                        },
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };

                    editCourseDialog.PrimaryButtonClick += async (contentDialog, contentDialogArgs) =>
                    {
                        TextBox nameTextBox = (TextBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseNameTextBox");
                        TextBox codeTextBox = (TextBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseCodeTextBox");
                        TextBox descriptionTextBox = (TextBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseDescriptionTextBox");
                        TextBox roomTextBox = (TextBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "CourseRoomTextBox");
                        ComboBox seasonComboBox = (ComboBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is ComboBox && ((ComboBox)c).Name == "SeasonComboBox");
                        ComboBox yearComboBox = (ComboBox)((StackPanel)editCourseDialog.Content).Children.First(c => c is ComboBox && ((ComboBox)c).Name == "YearComboBox");
                        TextBlock errorTextBlock = (TextBlock)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");

                        string name = nameTextBox.Text.Trim();
                        string code = codeTextBox.Text.Trim();
                        string description = descriptionTextBox.Text.Trim();
                        string room = roomTextBox.Text.Trim();
                        SeasonEnum season = (SeasonEnum)seasonComboBox.SelectedItem;
                        YearEnum year = (YearEnum)yearComboBox.SelectedItem;

                        if (string.IsNullOrEmpty(name))
                        {
                            errorTextBlock.Text = "Please enter a course name.";
                            contentDialogArgs.Cancel = true;
                            return;
                        }

                        Course updatedCourse;
                        if (nameTextBox.SelectedText != null)
                        {
                            updatedCourse = new Course { Code = code, Name = name, Description = description, Room = room, Semester = new Semester { Season = SeasonEnum.Fall, Year = YearEnum.Year_2023 } };
                        }
                        else
                        {
                            errorTextBlock.Text = "Please enter a course.";
                            contentDialogArgs.Cancel = true;
                            return;
                        }

                        if (CourseService.Current.Courses.Any(s => s.Code == updatedCourse.Code && s.Code != selectedCourse.Code))
                        {
                            errorTextBlock.Text = "A course with the same code already exists. Please enter a unique code.";
                            contentDialogArgs.Cancel = true;
                            return;
                        }

                        if (CourseService.Current.Courses.Any(s => s.Name == updatedCourse.Name && s.Name != selectedCourse.Name))
                        {
                            errorTextBlock.Text = "A course with the same name already exists. Please enter a unique name.";
                            contentDialogArgs.Cancel = true;
                            return;
                        }

                        selectedCourse.Code = codeTextBox.Text;
                        selectedCourse.Name= nameTextBox.Text;
                        selectedCourse.Description= descriptionTextBox.Text;
                        if (!string.IsNullOrWhiteSpace(course_searchBox.Text))
                        {
                            coursesList.ItemsSource = CourseService.Current.Search(course_searchBox.Text);
                        }
                        else
                        {
                            coursesList.ItemsSource = CourseService.Current.Courses;
                        }
                        await Task.CompletedTask;
                    };
                    await editCourseDialog.ShowAsync();
                }
            }
        }

        private async void Course_Info_Click(object sender, RoutedEventArgs e)
        {
            var course = ((FrameworkElement)sender).DataContext as Course;
            if (course != null)
            {
                var dialog = new MessageDialog($"Name: {course.Name}\nCode: {course.Code}\nDescription: {course.Description}\nRoom: {course.Room}\nSemester: {course.Semester}");
                await dialog.ShowAsync();
            }
            else
            {
                return;
            }
        }

        bool isListOpen = false;
        private void DisplayCourses_Click(object sender, RoutedEventArgs e)
        {
            coursesList.ItemsSource = CourseService.Current.Courses;
            if (!isListOpen)
            {
                coursesList.Visibility = Visibility.Visible;
                navigationList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
            else
            {
                coursesList.Visibility = Visibility.Collapsed;
                navigationList.Visibility = Visibility.Collapsed;
                isListOpen = false;
            }
        }


        private async void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var comboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var deleteCourseDialog = new ContentDialog
            {
                Title = "Delete a Course",
                Content = comboBox,
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            if (await deleteCourseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = comboBox.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    CourseService.Current.Remove(selectedCourse);
                }
            }
        }

        private void searchBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string searchQuery = course_searchBox.Text;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                coursesList.ItemsSource = CourseService.Current.Courses;
            }
            else
            {
                coursesList.ItemsSource = CourseService.Current.Search(searchQuery);
            }

            if (!isListOpen)
            {
                coursesList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
        }

        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var studentComboBox = new ComboBox { PlaceholderText = "Select a student", ItemsSource = StudentService.Current.People };

            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to add a student to?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            var studentDialog = new ContentDialog
            {
                Title = "Which student would you like to add to this course?",
                Content = studentComboBox,
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel"
            };

            if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = courseComboBox.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    if (await studentDialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var selectedStudent = studentComboBox.SelectedItem as Student;
                        if (selectedStudent != null)
                        {
                            selectedCourse.Roster.Add(selectedStudent);
                        }
                    }
                }
            }
        }

        private async void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to remove a student from?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = courseComboBox.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    var studentComboBox = new ComboBox { PlaceholderText = "Select a student", ItemsSource = selectedCourse.Roster };

                    var studentDialog = new ContentDialog
                    {
                        Title = "Which student would you like to remove from this course?",
                        Content = studentComboBox,
                        PrimaryButtonText = "Remove",
                        SecondaryButtonText = "Cancel"
                    };

                    if (await studentDialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var selectedStudent = studentComboBox.SelectedItem as Student;
                        if (selectedStudent != null)
                        {
                            selectedCourse.Roster.Remove(selectedStudent);
                        }
                    }
                }
            }
        }

        private bool TryParseDateTime(string input, out DateTime result)
        {
            string format = "MM/dd/yyyy";
            bool time = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return time;
        }

        int check = 0;
        private async void AddAssignment_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to add an assignment to?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            while (check == 0)
            {
                if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    var selectedCourse = courseComboBox.SelectedItem as Course;
                    if (selectedCourse != null)
                    {
                        var groupNames = selectedCourse.AssignmentGroups.Select(g => g.group_name).ToList();
                        var assignmentGroupComboBox = new ComboBox { PlaceholderText = "Select a group", ItemsSource = groupNames };
                        var assignmentGroupDialog = new ContentDialog
                        {
                            Title = "Which group would you like to add the assignment to?",
                            Content = assignmentGroupComboBox,
                            PrimaryButtonText = "Next",
                            SecondaryButtonText = "Create New Group"
                        };

                        if (await assignmentGroupDialog.ShowAsync() == ContentDialogResult.Primary)
                        {
                            var selectedAssignment = assignmentGroupComboBox.SelectedItem as Assignment;
                            if (selectedAssignment != null)
                            {
                                var assignmentNameTextBox = new TextBox { Text = selectedAssignment.Name, PlaceholderText = "Enter Assignment Name" };
                                var assignmentDescriptionTextBox = new TextBox { Text = selectedAssignment.Description, PlaceholderText = "Enter Assignment Description" };
                                var assignmentIdTextBox = new TextBox { Text = selectedAssignment.Id.ToString(), PlaceholderText = "Enter Assignment ID" };
                                var assignmentPointsTextBox = new TextBox { PlaceholderText = "Enter Assignment Points", Name = "Selected Course Assignment Points" };
                                var assignmentDueDateTextBox = new TextBox { Text = selectedAssignment.DueDate.ToString("MM/dd/yyyy"), PlaceholderText = "Enter Assignment Due Date (MM/DD/YYYY)" };

                                var editAssignmentDialog = new ContentDialog
                                {
                                    Title = "Edit Assignment",
                                    Content = new StackPanel
                                    {
                                        Children =
                                {
                                    new TextBlock { Text = "Name" },
                                    assignmentNameTextBox,
                                    new TextBlock { Text = "Description" },
                                    assignmentDescriptionTextBox,
                                    new TextBlock { Text = "ID" },
                                    assignmentIdTextBox,
                                    new TextBlock { Text = "Points" },
                                    assignmentPointsTextBox,
                                    new TextBlock { Text = "Due Date (MM/DD/YYYY)" },
                                    assignmentDueDateTextBox
                                }
                                    },
                                    PrimaryButtonText = "Save",
                                    SecondaryButtonText = "Cancel"
                                };

                                if (await editAssignmentDialog.ShowAsync() == ContentDialogResult.Primary)
                                {
                                    double points = double.Parse(assignmentPointsTextBox.Text);
                                    if (int.TryParse(assignmentIdTextBox.Text, out int id)
                                        && TryParseDateTime(assignmentDueDateTextBox.Text, out DateTime dueDate))
                                    {
                                        selectedAssignment.Name = assignmentNameTextBox.Text;
                                        selectedAssignment.Description = assignmentDescriptionTextBox.Text;
                                        selectedAssignment.Id = id;
                                        selectedAssignment.TotalAvailablePoints = points;
                                        selectedAssignment.DueDate = dueDate;
                                    }
                                }
                            }
                            else
                            {
                                // Create new assignment
                                var assignmentNameTextBox = new TextBox { PlaceholderText = "Enter Assignment Name" };
                                var assignmentDescriptionTextBox = new TextBox { PlaceholderText = "Enter Assignment Description" };
                                var assignmentIdTextBox = new TextBox { PlaceholderText = "Enter Assignment ID" };
                                var assignmentPointsTextBox = new TextBox { PlaceholderText = "Enter Assignment Points" };
                                var assignmentDueDateTextBox = new TextBox { PlaceholderText = "Enter Assignment Due Date (MM/DD/YYYY)" };
                                var createAssignmentDialog = new ContentDialog
                                {
                                    Title = "Create Assignment",
                                    Content = new StackPanel
                                    {
                                        Children =
                                        {
                                            new TextBlock { Text = "Name" },
                                            assignmentNameTextBox,
                                            new TextBlock { Text = "Description" },
                                            assignmentDescriptionTextBox,
                                            new TextBlock { Text = "ID" },
                                            assignmentIdTextBox,
                                            new TextBlock { Text = "Points" },
                                            assignmentPointsTextBox,
                                            new TextBlock { Text = "Due Date (MM/DD/YYYY)" },
                                            assignmentDueDateTextBox
                                        }
                                    },
                                    PrimaryButtonText = "Create",
                                    SecondaryButtonText = "Cancel"
                                };

                                if (await createAssignmentDialog.ShowAsync() == ContentDialogResult.Primary)
                                {
                                    double points = double.Parse(assignmentPointsTextBox.Text);
                                    if (int.TryParse(assignmentIdTextBox.Text, out int id)
                                        && TryParseDateTime(assignmentDueDateTextBox.Text, out DateTime dueDate))
                                    {
                                        var newAssignment = new Assignment
                                        {
                                            Name = assignmentNameTextBox.Text,
                                            Description = assignmentDescriptionTextBox.Text,
                                            Id = id,
                                            TotalAvailablePoints = points,
                                            DueDate = dueDate
                                        };

                                        selectedCourse.Assignments.Add(newAssignment);
                                    }
                                }
                            }

                            check = 1;
                        }
                        else
                        {
                            var assignmentGroupNameTextBox = new TextBox { PlaceholderText = "Enter Assignment Group Name", Name = "Group Assignment Name" };
                            var assignmentGroupWorthTextBox = new TextBox { PlaceholderText = "Enter Assignment Group Percentage", Name = "Group Assignment Worth" };
                            double weight;
                            double.TryParse(assignmentGroupWorthTextBox.Text, out weight);
                            var groupNameDialog = new ContentDialog
                            {
                                Title = "What is the name of the assignment group?",
                                Content = assignmentGroupNameTextBox,
                                PrimaryButtonText = "Next",
                                SecondaryButtonText = "Cancel"
                            };

                            var groupWorthDialog = new ContentDialog
                            {
                                Title = "What is the weight of this group?(20 = 20%)",
                                Content = assignmentGroupWorthTextBox,
                                PrimaryButtonText = "Done",
                                SecondaryButtonText = "Cancel"
                            };
                            if (await groupNameDialog.ShowAsync() == ContentDialogResult.Primary)
                            {
                                if (await groupWorthDialog.ShowAsync() == ContentDialogResult.Primary)
                                {
                                    AssignmentGroup assignmentGroup = new AssignmentGroup
                                    {
                                        group_name = assignmentGroupNameTextBox.Text,
                                        weight = weight
                                    };
                                    selectedCourse.AddAssignmentGroup(new AssignmentGroup { group_name = assignmentGroupNameTextBox.Text, weight = weight });
                                }
                            }
                        }
                    }
                }
                else
                {
                    check = 0;
                    return;
                }
            }
            check = 0;
        }

        // UpdateAssignment function
        private async void UpdateAssignment_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to update an assignment in?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = courseComboBox.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    var assignmentsComboBox = new ComboBox { PlaceholderText = "Select an assignment", ItemsSource = selectedCourse.Assignments };
                    var assignmentsDialog = new ContentDialog
                    {
                        Title = "Which assignment would you like to update?",
                        Content = assignmentsComboBox,
                        PrimaryButtonText = "Next",
                        SecondaryButtonText = "Cancel"
                    };

                    if (await assignmentsDialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var selectedAssignment = assignmentsComboBox.SelectedItem as Assignment;
                        if (selectedAssignment != null)
                        {
                            var assignmentNameTextBox = new TextBox { Text = selectedAssignment.Name, PlaceholderText = "Enter Assignment Name" };
                            var assignmentDescriptionTextBox = new TextBox { Text = selectedAssignment.Description, PlaceholderText = "Enter Assignment Description" };
                            var assignmentIdTextBox = new TextBox { Text = selectedAssignment.Id.ToString(), PlaceholderText = "Enter Assignment ID" };
                            var assignmentPointsTextBox = new TextBox { PlaceholderText = "Enter Assignment Points", Name = "Selected Course Assignment Points" };
                            var assignmentDueDateTextBox = new TextBox { Text = selectedAssignment.DueDate.ToString("MM/dd/yyyy"), PlaceholderText = "Enter Assignment Due Date (MM/DD/YYYY)" };

                            var editAssignmentDialog = new ContentDialog
                            {
                                Title = "Edit Assignment",
                                Content = new StackPanel
                                {
                                    Children =
                                    {
                                        new TextBlock { Text = "Name" },
                                        assignmentNameTextBox,
                                        new TextBlock { Text = "Description" },
                                        assignmentDescriptionTextBox,
                                        new TextBlock { Text = "ID" },
                                        assignmentIdTextBox,
                                        new TextBlock { Text = "Points" },
                                        assignmentPointsTextBox,
                                        new TextBlock { Text = "Due Date (MM/DD/YYYY)" },
                                        assignmentDueDateTextBox
                                    }
                                },
                                PrimaryButtonText = "Save",
                                SecondaryButtonText = "Cancel"
                            };

                            if (await editAssignmentDialog.ShowAsync() == ContentDialogResult.Primary)
                            {
                                double points = double.Parse(assignmentPointsTextBox.Text);
                                if (int.TryParse(assignmentIdTextBox.Text, out int id)
                                    && TryParseDateTime(assignmentDueDateTextBox.Text, out DateTime dueDate))
                                {
                                    selectedAssignment.Name = assignmentNameTextBox.Text;
                                    selectedAssignment.Description = assignmentDescriptionTextBox.Text;
                                    selectedAssignment.Id = id;
                                    selectedAssignment.TotalAvailablePoints = points;
                                    selectedAssignment.DueDate = dueDate;
                                }
                                else
                                {
                                    var errorDialog = new ContentDialog
                                    {
                                        Title = "Error",
                                        Content = "Invalid input. Please enter valid values for ID, Points, and Due Date (MM/DD/YYYY).",
                                        CloseButtonText = "Ok"
                                    };
                                    await errorDialog.ShowAsync();
                                }
                            }
                        }
                        else
                        {
                            var errorDialog = new ContentDialog
                            {
                                Title = "Error",
                                Content = "No assignment selected. Please select an assignment to update.",
                                CloseButtonText = "Ok"
                            };
                            await errorDialog.ShowAsync();
                        }
                    }
                }
                else
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "No course selected. Please select a course to update an assignment in.",
                        CloseButtonText = "Ok"
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }


        private async void RemoveAssignment_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var assignmentComboBox = new ComboBox { PlaceholderText = "Select an assignment" };
            assignmentComboBox.ItemsSource = CourseService.Current.Courses.SelectMany(c => c.Assignments);
            var chooseCourseDialog = new ContentDialog
            {
                Title = "Choose a course",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            var deleteAssignmentDialog = new ContentDialog
            {
                Title = "Delete an Assignment",
                Content = assignmentComboBox,
                PrimaryButtonText = "Remove",
                SecondaryButtonText = "Cancel"
            };

            if (await chooseCourseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                if (await deleteAssignmentDialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    var selectedAssignment = assignmentComboBox.SelectedItem as Assignment;
                    if (selectedAssignment != null)
                    {
                        CourseService.Current.RemoveAssignment(selectedAssignment);
                    }
                }
            }
        }

        private async void CRUDAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var chooseCourseDialog = new ContentDialog
            {
                Title = "Choose a course",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };
            var result = await chooseCourseDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var course = (Course)courseComboBox.SelectedItem;
                chooseCourseDialog.Hide();

                var createButton = new Button { Content = "Create" };
                var readButton = new Button { Content = "Read" };
                var updateButton = new Button { Content = "Update" };
                var deleteButton = new Button { Content = "Delete" };

                var announcementDialog = new ContentDialog
                {
                    Title = "Create, Read, Update, or Delete An Announcement",
                    Content = new StackPanel { Children = { createButton, readButton, updateButton, deleteButton } },
                    CloseButtonText = "Cancel"
                };

                createButton.Click += async (s, args) =>
                {
                    announcementDialog.Hide();
                    var announcement = new Announcement();
                    var nameBox = new TextBox { PlaceholderText = "Announcement Name" };
                    var descriptionBox = new TextBox { PlaceholderText = "Announcement Description" };
                    var content = new StackPanel { Children = { nameBox, descriptionBox } };
                    var dialog = new ContentDialog
                    {
                        Title = "Create Announcement",
                        Content = content,
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };
                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        announcement.announcement_name = nameBox.Text;
                        announcement.announcement_description = descriptionBox.Text;
                        course.Announcements.Add(announcement);
                    }
                };

                readButton.Click += (s, args) =>
                {
                    announcementDialog.Hide();
                    var announcements = string.Join("\n", course.Announcements.Select(a => a.ToString()).ToArray());
                    var dialog = new ContentDialog
                    {
                        Title = "Announcements",
                        Content = announcements,
                        PrimaryButtonText = "Close",
                    };
                    _ = dialog.ShowAsync();
                };

                updateButton.Click += async (s, args) =>
                {
                    announcementDialog.Hide();
                    var selectedAnnouncement = new ListBox { ItemsSource = course.Announcements };
                    var nameBox = new TextBox { PlaceholderText = "Announcement Name" };
                    var descriptionBox = new TextBox { PlaceholderText = "Announcement Description" };
                    var content = new StackPanel { Children = { selectedAnnouncement, nameBox, descriptionBox } };
                    var dialog = new ContentDialog
                    {
                        Title = "Update Announcement",
                        Content = content,
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };

                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        var announcement = selectedAnnouncement.SelectedItem as Announcement;
                        announcement.announcement_name = nameBox.Text;
                        announcement.announcement_description = descriptionBox.Text;
                    }
                };

                deleteButton.Click += async (s, args) =>
                {
                    announcementDialog.Hide();
                    var selectedAnnouncement = new ListBox { ItemsSource = course.Announcements };
                    var content = new StackPanel { Children = { new TextBlock { Text = "Are you sure you want to delete this announcement?" }, selectedAnnouncement } };
                    var dialog = new ContentDialog
                    {
                        Title = "Delete Announcement",
                        Content = content,
                        PrimaryButtonText = "Delete",
                        SecondaryButtonText = "Cancel"
                    };

                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        var announcement = selectedAnnouncement.SelectedItem as Announcement;
                        course.Announcements.Remove(announcement);
                    }
                };
                await announcementDialog.ShowAsync();
            }
        }



        private async void CRUDModule_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var chooseCourseDialog = new ContentDialog
            {
                Title = "Choose a course",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };
            var result = await chooseCourseDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var course = (Course)courseComboBox.SelectedItem;
                chooseCourseDialog.Hide();

                var createButton = new Button { Content = "Create" };
                var readButton = new Button { Content = "Read" };
                var updateButton = new Button { Content = "Update" };
                var deleteButton = new Button { Content = "Delete" };

                var moduleDialog = new ContentDialog
                {
                    Title = "Create, Read, Update, or Delete A Module",
                    Content = new StackPanel { Children = { createButton, readButton, updateButton, deleteButton } },
                    CloseButtonText = "Cancel"
                };

                createButton.Click += async (s, args) =>
                {
                    moduleDialog.Hide();
                    var module = new Module();
                    var nameBox = new TextBox { PlaceholderText = "Module Name" };
                    var descriptionBox = new TextBox { PlaceholderText = "Module Description" };
                    var content = new StackPanel { Children = { nameBox, descriptionBox } };
                    var dialog = new ContentDialog
                    {
                        Title = "Create Module",
                        Content = content,
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };
                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        module.Name = nameBox.Text;
                        module.Description = descriptionBox.Text;
                        course.Modules.Add(module);
                    }
                };

                readButton.Click += (s, args) =>
                {
                    moduleDialog.Hide();
                    var modules = string.Join("\n", course.Modules.Select(a => a.ToString()).ToArray());
                    var dialog = new ContentDialog
                    {
                        Title = "Modules",
                        Content = modules,
                        PrimaryButtonText = "Close",
                    };
                    _ = dialog.ShowAsync();
                };

                updateButton.Click += async (s, args) =>
                {
                    moduleDialog.Hide();
                    var selectedModule = new ListBox { ItemsSource = course.Modules };
                    var nameBox = new TextBox { PlaceholderText = "Module Name" };
                    var descriptionBox = new TextBox { PlaceholderText = "Module Description" };
                    var content = new StackPanel { Children = { selectedModule, nameBox, descriptionBox } };
                    var dialog = new ContentDialog
                    {
                        Title = "Update Module",
                        Content = content,
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };

                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        var module = selectedModule.SelectedItem as Module;
                        module.Name = nameBox.Text;
                        module.Description = descriptionBox.Text;
                    }
                };

                deleteButton.Click += async (s, args) =>
                {
                    moduleDialog.Hide();
                    var selectedModule = new ListBox { ItemsSource = course.Modules };
                    var content = new StackPanel { Children = { new TextBlock { Text = "Are you sure you want to delete this module?" }, selectedModule } };
                    var dialog = new ContentDialog
                    {
                        Title = "Delete Module",
                        Content = content,
                        PrimaryButtonText = "Delete",
                        SecondaryButtonText = "Cancel"
                    };

                    var dialogResult = await dialog.ShowAsync();
                    if (dialogResult == ContentDialogResult.Primary)
                    {
                        var module = selectedModule.SelectedItem as Module;
                        course.Modules.Remove(module);
                    }
                };
                await moduleDialog.ShowAsync();
            }
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
