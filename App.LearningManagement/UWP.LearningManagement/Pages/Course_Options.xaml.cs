using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
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
                TextBlock errorTextBlock = (TextBlock)((StackPanel)addCourseDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");
                string name = nameTextBox.Text.Trim();
                string code = codeTextBox.Text.Trim();
                string description = descriptionTextBox.Text.Trim();

                if (string.IsNullOrEmpty(name))
                {
                    errorTextBlock.Text = "Please enter a course name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                Course newCourse;
                if (nameTextBox.SelectedText != null)
                {
                    newCourse = new Course { Code = code, Name = name, Description = description };
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
            // Show a dialog to select the course to update
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
                    // Show a dialog to edit the course information
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
                        TextBlock errorTextBlock = (TextBlock)((StackPanel)editCourseDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");
                        string name = nameTextBox.Text.Trim();
                        string code = codeTextBox.Text.Trim();
                        string description = descriptionTextBox.Text.Trim();

                        if (string.IsNullOrEmpty(name))
                        {
                            errorTextBlock.Text = "Please enter a course name.";
                            contentDialogArgs.Cancel = true;
                            return;
                        }

                        Course updatedCourse;
                        if (nameTextBox.SelectedText != null)
                        {
                            updatedCourse = new Course { Code = code, Name = name, Description = description };
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
                var dialog = new MessageDialog($"Name: {course.Name}\nCode: {course.Code}\nDescription: {course.Description}");
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
            // Set the items source of the peopleList control to the list of people in the StudentService
            coursesList.ItemsSource = CourseService.Current.Courses;

            if (!isListOpen)
            {
                coursesList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
            else
            {
                coursesList.Visibility = Visibility.Collapsed;
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

        void AddAssignment_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // GroupAssignment function
        void GroupAssignment_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // UpdateAssignment function
        void UpdateAssignment_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // RemoveAssignment function
        void RemoveAssignment_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // SearchCourses function
        void SearchCourses_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // CRUDAnnouncement function
        void CRUDAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // CRUDModule function
        void CRUDModule_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
