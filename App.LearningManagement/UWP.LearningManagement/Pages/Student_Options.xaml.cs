using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UWP.LearningManagement.ViewModels;
using Windows.Devices.Enumeration;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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
            peopleList.ItemsSource = StudentService.Current.People;
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

        private void GradeAssignment_Click(object sender, RoutedEventArgs e)
        {

        }


        private void WeightedAverage_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Calculate a weighted average" button click
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

    }
}
