using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UWP.LearningManagement.ViewModels;
using Windows.UI;
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
                TextBlock errorTextBlock = (TextBlock)((StackPanel)addPersonDialog.Content).Children.First(c => c is TextBlock && ((TextBlock)c).Name == "ErrorTextBlock");
                string name = nameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    errorTextBlock.Text = "Please enter a name.";
                    contentDialogArgs.Cancel = true;
                    return;
                }

                if (int.TryParse(idTextBox.Text.Trim(), out int id))
                {
                    if (StudentService.Current.Students.Any(s => s.Id == id))
                    {
                        errorTextBlock.Text = "A student with the same ID already exists. Please enter a unique ID.";
                        contentDialogArgs.Cancel = true;
                        return;
                    }

                    if (StudentService.Current.Students.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    {
                        errorTextBlock.Text = "A student with the same name already exists. Please enter a unique name.";
                        contentDialogArgs.Cancel = true;
                        return;
                    }

                    Person newPerson = new Person { Id = id, Name = name };
                    StudentService.Current.Add(newPerson);

                    if (!string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        peopleList.ItemsSource = StudentService.Current.Search(searchBox.Text);
                    }
                    else
                    {
                        peopleList.ItemsSource = StudentService.Current.Students;
                    }
                }
                else
                {
                    errorTextBlock.Text = "Please enter a valid integer ID.";
                    contentDialogArgs.Cancel = true;
                    return;
                }
            };

            await addPersonDialog.ShowAsync();
        }

        private async void UpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog updatePersonDialog = new ContentDialog
            {
                Title = "Update a Person",
                Content = new ComboBox { PlaceholderText = "Select a person", ItemsSource = StudentService.Current.Students },
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
                        // Display an error message if the user didn't enter a new name
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Invalid Name",
                            Content = "Please enter a valid name.",
                            CloseButtonText = "OK"
                        };
                        _ = errorDialog.ShowAsync();
                        return;
                    }

                    // Create a new ContentDialog to allow the user to edit the ID of the selected person
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
                            if (newId == selectedPerson.Id)
                            {
                                // Display an error message if the user didn't enter a new ID
                                ContentDialog errorDialog = new ContentDialog
                                {
                                    Title = "Invalid ID",
                                    Content = "Please enter a different ID.",
                                    CloseButtonText = "OK"
                                };
                                _ = errorDialog.ShowAsync();
                                return;
                            }
                            if (StudentService.Current.Students.Any(s => s.Id == newId))
                            {
                                // Display an error message if the new ID already exists
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
                                peopleList.ItemsSource = StudentService.Current.Students;
                            }
                        }
                        else
                        {
                            // Display an error message if the user entered an invalid ID
                            ContentDialog errorDialog = new ContentDialog
                            {
                                Title = "Invalid ID",
                                Content = "Please enter a valid integer ID.",
                                CloseButtonText = "OK"
                            };
                            _ = errorDialog
        .ShowAsync();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        peopleList.ItemsSource = StudentService.Current.Search(searchBox.Text);
                    }
                    else
                    {
                        peopleList.ItemsSource = StudentService.Current.Students;
                    }
                }
            }
        }





        bool isListOpen = false;
        private void ListPeople_Click(object sender, RoutedEventArgs e)
        {
            if (!isListOpen)
            {
                // Show the list of people
                peopleList.Visibility = Visibility.Visible; 
                isListOpen = true;
            }
            else
            {
                // Hide the list of people
                peopleList.Visibility = Visibility.Collapsed;
                isListOpen = false;
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = searchBox.Text;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                // Reset the items source if the search query is empty
                peopleList.ItemsSource = StudentService.Current.Students;
            }
            else
            {
                // Filter the items source based on the search query
                peopleList.ItemsSource = StudentService.Current.Search(searchQuery);
            }

            // Show the list of people if it's not already visible
            if (!isListOpen)
            {
                peopleList.Visibility = Visibility.Visible;
                isListOpen = true;
            }
        }


        private void GradeAssignment_Click(object sender, RoutedEventArgs e)
        {
            // Handle the "Provide a grade for a specific assignment" button click
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
