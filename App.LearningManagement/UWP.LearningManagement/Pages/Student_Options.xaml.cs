﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UWP.LearningManagement.ViewModels;
using Windows.Security.Authentication.OnlineId;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP.LearningManagement.Pages
{
    public sealed partial class Student_Options : Page
    {
        public Student_Options()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            ViewToggle.IsChecked = Toggle_State.IsChecked;
        }

        private void ViewToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            ViewToggle.Content = "Switch To Student Mode";
            addPerson.Visibility = Visibility.Visible;
            updatePerson.Visibility = Visibility.Visible;
            deletePerson.Visibility = Visibility.Visible;
            searchBox.Visibility = Visibility.Visible;
            listingStudents.Visibility = Visibility.Visible;
            gradeAssignment.Visibility = Visibility.Visible;
        }

        private void ViewToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            ViewToggle.Content = "Switch To TA/Instructor Mode";
            addPerson.Visibility = Visibility.Collapsed;
            updatePerson.Visibility = Visibility.Collapsed;
            deletePerson.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Collapsed;
            listingStudents.Visibility = Visibility.Collapsed;
            gradeAssignment.Visibility =(Visibility) Visibility.Collapsed;
        }

        private async void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog addPersonDialog = new ContentDialog
            {
                Title = "Add a New Person",
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBox { PlaceholderText = "Enter name here", Name = "NameTextBox" },
                        new TextBox { PlaceholderText = "Enter ID here", Name = "IdTextBox" },
                        new ComboBox { Header = "Classification", Name = "ClassificationComboBox", ItemsSource = Enum.GetValues(typeof(PersonClassification)) },
                        new TextBlock { Foreground = new SolidColorBrush(Colors.Red), Name = "ErrorTextBlock" }
                    }
                },

                PrimaryButtonText = "Add",
                SecondaryButtonText = "Cancel"
            };

            addPersonDialog.PrimaryButtonClick += async (contentDialog, contentDialogArgs) =>
            {
                TextBox nameTextBox = (TextBox)((StackPanel)addPersonDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "NameTextBox");
                TextBox idTextBox = (TextBox)((StackPanel)addPersonDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "IdTextBox");
                ComboBox classificationComboBox = (ComboBox)((StackPanel)addPersonDialog.Content).Children.First(c => c is ComboBox && ((ComboBox)c).Name == "ClassificationComboBox");
                TextBlock errorTextBlock = (TextBlock)((StackPanel)addPersonDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");
                string name = nameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    errorTextBlock.Text = "Please enter a name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                Person newPerson;
                if (classificationComboBox.SelectedIndex == 0)
                {
                    newPerson = new Student { Id = int.Parse(idTextBox.Text.Trim()), Name = name, Classification=PersonClassification.Freshman };
                }
                else if (classificationComboBox.SelectedIndex == 1)
                {
                    newPerson = new Student { Id = int.Parse(idTextBox.Text.Trim()), Name = name, Classification = PersonClassification.Sophomore };
                }
                else if (classificationComboBox.SelectedIndex == 2)
                {
                    newPerson = new Student { Id = int.Parse(idTextBox.Text.Trim()), Name = name, Classification = PersonClassification.Junior };
                }
                else if (classificationComboBox.SelectedIndex == 3)
                {
                    newPerson = new Student { Id = int.Parse(idTextBox.Text.Trim()), Name = name, Classification = PersonClassification.Freshman };
                }
                else if (classificationComboBox.SelectedIndex == 4)
                {
                    newPerson = new Instructor { Id = int.Parse(idTextBox.Text.Trim()), Name = name };
                }
                else if (classificationComboBox.SelectedIndex == 5)
                {
                    newPerson = new TeachingAssistant { Id = int.Parse(idTextBox.Text.Trim()), Name = name };
                }
                else
                {
                    errorTextBlock.Text = "Please select a classification.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                if (StudentService.Current.People.Any(s => s.Id == newPerson.Id))
                {
                    errorTextBlock.Text = "A person with the same ID already exists. Please enter a unique ID.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                if (StudentService.Current.People.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    errorTextBlock.Text = "A person with the same name already exists. Please enter a unique name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                StudentService.Current.Add(newPerson);

                if (!string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    peopleList.ItemsSource = StudentService.Current.Search(searchBox.Text);
                }
                else
                {
                    peopleList.ItemsSource = StudentService.Current.People;
                }
            };

            await addPersonDialog.ShowAsync();
        }

        private async void UpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog updatePersonDialog = new ContentDialog
            {
                Title = "Update a Person",
                Content = new ComboBox { PlaceholderText = "Select a person", ItemsSource = StudentService.Current.People },
                PrimaryButtonText = "Update",
                SecondaryButtonText = "Cancel"
            };

            ContentDialogResult result = await updatePersonDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ComboBox personComboBox = (ComboBox)updatePersonDialog.Content;
                Person selectedPerson = (Person)personComboBox.SelectedItem;

                ContentDialog editNameDialog = new ContentDialog
                {
                    Title = "Edit Name",
                    Content = new TextBox { PlaceholderText = "Enter new name here", Text = selectedPerson.Name },
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Cancel"
                };

                ContentDialogResult editNameDialogResult = await editNameDialog.ShowAsync();
                if (editNameDialogResult == ContentDialogResult.Primary)
                {
                    TextBox nameTextBox = (TextBox)editNameDialog.Content;
                    string newName = nameTextBox.Text.Trim();
                    if (string.IsNullOrEmpty(newName))
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Invalid Name",
                            Content = "Please enter a valid name.",
                            CloseButtonText = "OK"
                        };
                        _ = errorDialog.ShowAsync();
                        return;
                    }

                    ContentDialog editIdDialog = new ContentDialog
                    {
                        Title = "Edit ID",
                        Content = new TextBox { PlaceholderText = "Enter new ID here", Text = selectedPerson.Id.ToString() },
                        PrimaryButtonText = "Save",
                        SecondaryButtonText = "Cancel"
                    };

                    ContentDialogResult editIdDialogResult = await editIdDialog.ShowAsync();
                    if (editIdDialogResult == ContentDialogResult.Primary)
                    {
                        TextBox idTextBox = (TextBox)editIdDialog.Content;
                        if (int.TryParse(idTextBox.Text.Trim(), out int newId))
                        {
                            if (newId == selectedPerson.Id && newId != selectedPerson.Id)
                            {
                                ContentDialog errorDialog = new ContentDialog
                                {
                                    Title = "Invalid ID",
                                    Content = "Please enter a different ID.",
                                    CloseButtonText = "OK"
                                };
                                _ = errorDialog.ShowAsync();
                                return;
                            }
                            if (StudentService.Current.People.Any(s => s.Id == newId) && newId != selectedPerson.Id)
                            {
                                ContentDialog errorDialog = new ContentDialog
                                {
                                    Title = "Duplicate ID",
                                    Content = "A student with the same ID already exists. Please enter a unique ID.",
                                    CloseButtonText = "OK"
                                };
                                _ = errorDialog.ShowAsync();
                                return;
                            }

                            selectedPerson.Id = newId;
                            selectedPerson.Name = newName;

                            if (!string.IsNullOrWhiteSpace(searchBox.Text))
                            {
                                peopleList.ItemsSource = StudentService.Current.Search(searchBox.Text);
                            }
                            else
                            {
                                peopleList.ItemsSource = StudentService.Current.People;
                            }
                        }
                        else
                        {
                            ContentDialog errorDialog = new ContentDialog
                            {
                                Title = "Invalid ID",
                                Content = "Please enter a valid integer ID.",
                                CloseButtonText = "OK"
                            };
                            _ = errorDialog.ShowAsync();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        peopleList.ItemsSource = StudentService.Current.Search(searchBox.Text);
                    }
                    else
                    {
                        peopleList.ItemsSource = StudentService.Current.People;
                    }
                }
            }
        }

        private async void Delete_Person_Click(object sender, RoutedEventArgs e)
        {
            var comboBox = new ComboBox { PlaceholderText = "Select a person", ItemsSource = StudentService.Current.People };
            var deletePersonDialog = new ContentDialog
            {
                Title = "Delete a Person",
                Content = comboBox,
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            if (await deletePersonDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedPerson = comboBox.SelectedItem as Person;
                if (selectedPerson != null)
                {
                    StudentService.Current.Remove(selectedPerson);
                }
            }
        }

        private async void Person_Info_Click(object sender, RoutedEventArgs e)
        {
            var student = ((FrameworkElement)sender).DataContext as Student;
            if (student != null)
            {
                var dialog = new MessageDialog($"Name: {student.Name}\nId: {student.Id}\nClassification: {student.Classification}");
                await dialog.ShowAsync();
            }
            else
            {
                var instructor = ((FrameworkElement)sender).DataContext as Instructor;
                if (instructor != null)
                {
                    var dialog = new MessageDialog($"Name: {instructor.Name}\nId: {instructor.Id}");
                    await dialog.ShowAsync();
                }
                else
                {
                    var teachingAssistant = ((FrameworkElement)sender).DataContext as TeachingAssistant;
                    if (teachingAssistant != null)
                    {
                        var dialog = new MessageDialog($"Name: {teachingAssistant.Name}\nId: {teachingAssistant.Id}");
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        bool isListOpen = false;
        private void ListPeople_Click(object sender, RoutedEventArgs e)
        {
            peopleList.ItemsSource = StudentService.Current.People.Take(5);
            if (!isListOpen)
            {
                peopleList.Visibility = Visibility.Visible;
                studentNavigationList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
            else
            {
                peopleList.Visibility = Visibility.Collapsed;
                studentNavigationList.Visibility = Visibility.Collapsed;
                isListOpen = false;
            }
        }

        private int currentPage = 1;
        private const int PageSize = 5;
        private int totalPages = 100;

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            UpdatePeopleList();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
            UpdatePeopleList();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
            }
            UpdatePeopleList();
        }


        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = totalPages;
            UpdatePeopleList();
        }

        private void UpdatePeopleList()
        {
            var people = StudentService.Current.People.Skip((currentPage - 1) * PageSize).Take(PageSize);
            peopleList.ItemsSource = people;

            totalPages = (int)Math.Ceiling((double)StudentService.Current.People.Count / PageSize);

            // Update the current page text block
            currentPageTextBlock.Text = currentPage.ToString();
        }


        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = searchBox.Text;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                peopleList.ItemsSource = StudentService.Current.People;
            }
            else
            {
                peopleList.ItemsSource = StudentService.Current.Search(searchQuery);
            }

            if (!isListOpen)
            {
                peopleList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
        }

        private async void ViewCurrentCourses_Click(object sender, RoutedEventArgs e)
        {
            var student = StudentService.Current.People.OfType<Person>().FirstOrDefault(s => s.Id == StudentService.Current.LoggedInUserId);

            string message = $"Current courses for {student.Name}:\n";
            var courses = CourseService.Current.Courses.Where(c => c.Roster.Contains(student));
            foreach (var course in courses)
            {
                message += $"{course.Code} - {course.Name}\n";
            }

            var resultDialog = new ContentDialog
            {
                Title = "Current Courses",
                Content = message,
                CloseButtonText = "OK"
            };

            await resultDialog.ShowAsync();
        }

        private async void ViewPreviousCourses_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Select Semester",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel",
                Content = new StackPanel
                {
                    Children =
                    {
                        new ComboBox
                        {
                            PlaceholderText = "Select a season",
                            ItemsSource = Enum.GetValues(typeof(SeasonEnum)),
                            SelectedItem = SeasonEnum.Fall
                        },
                        new ComboBox
                        {
                            PlaceholderText = "Select a year",
                            ItemsSource = Enum.GetValues(typeof(YearEnum)).Cast<YearEnum>().Where(y => y <= YearEnum.Year_2023).ToList(),
                            SelectedItem = YearEnum.Year_2023
                        }
                    }
                }
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Get the current student's ID
                var currentStudentId = StudentService.Current.LoggedInUserId;

                // Get the selected semester
                var stackPanel = dialog.Content as StackPanel;
                var seasonCombo = stackPanel.Children[0] as ComboBox;
                var yearCombo = stackPanel.Children[1] as ComboBox;
                var semester = new Semester { Season = (SeasonEnum)seasonCombo.SelectedItem, Year = (YearEnum)yearCombo.SelectedItem };

                // Get a list of previous courses for the selected semester and the current student
                var previousCourses = CourseService.Current.Courses.Where(c => c.Semester.Season == semester.Season &&
                                c.Semester.Year == semester.Year &&
                                c.Roster.Any(p => p.Id == currentStudentId)).ToList();


                // Display the previous courses
                var message = previousCourses.Any() ? string.Join("\n", previousCourses) : "No previous courses found for the selected semester.";
                var dialog2 = new ContentDialog
                {
                    Title = "Previous Courses",
                    PrimaryButtonText = "OK",
                    Content = message
                };
                await dialog2.ShowAsync();
            }
        }
        int id_checker=-1;
        private async void GradeAssignment_Click(object sender, RoutedEventArgs e)
        {
            var courseComboBox = new ComboBox { PlaceholderText = "Select a course", ItemsSource = CourseService.Current.Courses };
            var courseDialog = new ContentDialog
            {
                Title = "Which course would you like to add an assignment grade to?",
                Content = courseComboBox,
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel"
            };

            if (await courseDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var selectedCourse = courseComboBox.SelectedItem as Course;
                var studentComboBox = new ComboBox { PlaceholderText = "Select a student", ItemsSource = selectedCourse.Roster };
                var studentDialog = new ContentDialog
                {
                    Title = "Which student would you like to add an assignment grade to?",
                    Content = studentComboBox,
                    PrimaryButtonText = "Next",
                    SecondaryButtonText = "Cancel"
                };
                if (await studentDialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    var assignmentComboBox = new ComboBox { PlaceholderText = "Select an assignment", ItemsSource = selectedCourse.Assignments };
                    var assignmentDialog = new ContentDialog
                    {
                        Title = "Which assignment would you like to grade for this student?",
                        Content = assignmentComboBox,
                        PrimaryButtonText = "Next",
                        SecondaryButtonText = "Cancel"
                    };

                    if (await assignmentDialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var selectedAssignment = assignmentComboBox.SelectedItem as Assignment;
                        var selectedStudent = studentComboBox.SelectedItem as Student;
                        ConsoleDialog gradeDialog = new ConsoleDialog("Enter the grade for the student (0-100):");
                        if (await gradeDialog.ShowAsync() == ContentDialogResult.Primary)
                        {
                            if (double.TryParse(gradeDialog.Text, out double grade) && grade >= 0)
                            {
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
                                    grade_letter = "F";
                                }
                                if(selectedCourse.Assignments[selectedCourse.Assignments.Count - 1].Id == id_checker)
                                {
                                    selectedCourse.Assignments[selectedCourse.Assignments.Count - 1].Id++;
                                }
                                while (selectedCourse.Assignments[selectedCourse.Assignments.Count - 1].Id != id_checker)
                                {
                                    selectedStudent.SetGrade(selectedCourse.Assignments[selectedCourse.Assignments.Count - 1].Id, grade);
                                    MessageDialog messageDialog = new MessageDialog($"Grade '{grade}' {grade_letter} added for student '{selectedStudent.Name}' in course '{selectedCourse.Name}' and assignment `{selectedAssignment.Name}`.");
                                    id_checker = selectedCourse.Assignments[selectedCourse.Assignments.Count - 1].Id;
                                    await messageDialog.ShowAsync();
                                }
                            }
                            else
                            {
                                MessageDialog messageDialog = new MessageDialog("Invalid grade value.");
                                await messageDialog.ShowAsync();
                            }
                        }
                    }
                }
            }
        }


        class ConsoleDialog : ContentDialog
        {
            public TextBox TextBox { get; }
            public string Text => TextBox.Text;
            public ConsoleDialog(string message)
            {
                TextBox = new TextBox();
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = message },
                        TextBox
                    }
                };
                PrimaryButtonText = "OK";
            }
        }

        private async void WeightedAverage_Click(object sender, RoutedEventArgs e)
        {
            var currentStudentId = StudentService.Current.LoggedInUserId;
            var currentStudent = StudentService.Current.People.FirstOrDefault(p => p.Id == currentStudentId) as Student;
            var currentStudentName = currentStudent?.Name;
            var studentService = StudentService.Current;
            var courseService = CourseService.Current;
            if (currentStudentId != null)
            {
                var studentCourses = courseService.Courses.Where(c => c.Roster.Any(p => p.Id == currentStudentId)).ToList();
                var courseListBox = new ListBox { ItemsSource = studentCourses, SelectionMode = SelectionMode.Multiple };
                var courseDialog = new ContentDialog
                {
                    Title = "Select a Course for a weighted average, or select multiple to calculate your GPA",
                    Content = courseListBox,
                    PrimaryButtonText = "Calculate Weighted Average",
                    SecondaryButtonText = "Calculate GPA"
                };
                var courseResult = await courseDialog.ShowAsync();
                if (courseResult == ContentDialogResult.Primary)
                {
                    var selectedCourse = courseListBox.SelectedItem as Course;
                    if (selectedCourse != null)
                    {
                        double totalWeightedAverage = 0;
                        double totalWeight = 0;

                        foreach (var assignmentGroup in selectedCourse.AssignmentGroups)
                        {
                            double weightedAverage = currentStudent.GetWeightedAverage(new List<AssignmentGroup> { assignmentGroup });
                            totalWeightedAverage += weightedAverage * assignmentGroup.weight;
                            totalWeight += assignmentGroup.weight;
                        }

                        string letterGrade = "";
                        double finalGrade = totalWeightedAverage / totalWeight;

                        if (finalGrade >= 90)
                        {
                            letterGrade = "A";
                        }
                        else if (finalGrade >= 80 && finalGrade < 90)
                        {
                            letterGrade = "B";
                        }
                        else if (finalGrade >= 70 && finalGrade < 80)
                        {
                            letterGrade = "C";
                        }
                        else if (finalGrade >= 60 && finalGrade < 70)
                        {
                            letterGrade = "D";
                        }
                        else
                        {
                            letterGrade = "F";
                        }

                        var messageDialog = new ContentDialog
                        {
                            Title = $"Weighted Average for {currentStudentName} in {selectedCourse.Name}",
                            Content = $"{Math.Round(finalGrade, 2)}% ({letterGrade})",
                            PrimaryButtonText = "Ok"
                        };
                        await messageDialog.ShowAsync();
                    }
                }
                else
                {
                    double totalWeightedAverage = 0;
                    double totalWeight = 0;
                    double totalCreditHours = 0;
                    foreach (Course selectedCourse in courseListBox.SelectedItems)
                    {
                        foreach (var assignmentGroup in selectedCourse.AssignmentGroups)
                        {
                            double weightedAverage = currentStudent.GetWeightedAverage(new List<AssignmentGroup> { assignmentGroup });
                            totalWeightedAverage += weightedAverage * assignmentGroup.weight;
                            totalWeight += assignmentGroup.weight;
                        }
                        totalCreditHours += selectedCourse.CreditHours;
                    }
                    double finalGrade = totalWeightedAverage / totalWeight;
                    double gpa_storage = 0;
                    int coursesGraded = 0;

                    if (finalGrade >= 90)
                    {
                        gpa_storage += 4 * totalCreditHours;
                        coursesGraded = courseListBox.SelectedItems.Count;
                    }
                    else if (finalGrade >= 80 && finalGrade < 90)
                    {
                        gpa_storage += 3 * totalCreditHours;
                        coursesGraded = courseListBox.SelectedItems.Count;
                    }
                    else if (finalGrade >= 70 && finalGrade < 80)
                    {
                        gpa_storage += 2 * totalCreditHours;
                        coursesGraded = courseListBox.SelectedItems.Count;
                    }
                    else if (finalGrade >= 60 && finalGrade < 70)
                    {
                        gpa_storage += 1 * totalCreditHours;
                        coursesGraded = courseListBox.SelectedItems.Count;
                    }
                    else
                    {
                        coursesGraded = 0;
                    }

                    double gpa = gpa_storage / totalCreditHours;
                    var gpaDialog = new ContentDialog
                    {
                        Title = $"{currentStudentName}'s GPA",
                        Content = $"Your GPA is {Math.Round(gpa, 2)}",
                        CloseButtonText = "Ok"
                    };
                    await gpaDialog.ShowAsync();
                }
            }
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

    }
}
