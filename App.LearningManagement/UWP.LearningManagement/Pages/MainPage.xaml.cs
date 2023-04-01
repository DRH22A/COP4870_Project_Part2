using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Linq;
using UWP.LearningManagement.Pages;
using UWP.LearningManagement.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace UWP.LearningManagement
{
    public sealed partial class MainPage : Page
    {
        private string _authToken;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            ViewToggle.IsChecked = Toggle_State.IsChecked;
        }

        private void ViewToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            ViewToggle.Content = "Switch To Student Mode";
            studentInfo.Content = "View All Student Information";
            courseInfo.Content = "Manage All Courses";
        }

        private void ViewToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            ViewToggle.Content = "Switch To TA/Instructor Mode";
            studentInfo.Content = "Check Your Student Information";
            courseInfo.Content = "Manage Your Courses";
        }

        private void StudentOptions_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Student_Options));
        }

        private void MaintainCourses_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Course_Options));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if a user is already logged in
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("LoggedInUserId", out object loggedInUserIdObject))
            {
                loggedInUserId = (int)loggedInUserIdObject;
                Login.Visibility = Visibility.Collapsed;
                studentInfo.Visibility = Visibility.Visible;
                courseInfo.Visibility = Visibility.Visible;
                ViewToggle.Visibility = StudentService.Current.People.FirstOrDefault(p => p.Id == loggedInUserId) is Instructor || StudentService.Current.People.FirstOrDefault(p => p.Id == loggedInUserId) is TeachingAssistant ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private int loggedInUserId = -1;
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog loginDialog = new ContentDialog()
            {
                Title = "Login",
                Content = new StackPanel()
                {
                    Children =
                    {
                        new TextBox { PlaceholderText = "Enter your ID", Name = "IdTextBox" },
                        new PasswordBox { PlaceholderText = "Enter your Password", Name = "PasswordBox" },
                        new Button { Content = "Sign Up", Name = "SignUpButton" }
                    }
                },
                PrimaryButtonText = "Sign In",
                SecondaryButtonText = "Cancel"
            };
            var result = await loginDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                TextBox idTextBox = (TextBox)((StackPanel)loginDialog.Content).Children.First(c => c is TextBox && ((TextBox)c).Name == "IdTextBox");
                PasswordBox passwordBox = (PasswordBox)((StackPanel)loginDialog.Content).Children.First(c => c is PasswordBox && ((PasswordBox)c).Name == "PasswordBox");

                int id = int.Parse(idTextBox.Text);
                string password = passwordBox.Password;

                // Find the person with the given ID and password
                Person person = StudentService.Current.People.FirstOrDefault(p => p.Id == id && p.Password == password);
                if (person != null)
                {
                    // Store the logged-in user's ID
                    loggedInUserId = person.Id;

                    var dialog = new ContentDialog()
                    {
                        Title = "Success!",
                        Content = "Welcome!",
                        CloseButtonText = "OK"
                    };
                    studentInfo.Visibility = Visibility.Visible;
                    courseInfo.Visibility = Visibility.Visible;
                    Login.Visibility = Visibility.Collapsed;
                    if (person is Instructor || person is TeachingAssistant)
                    {
                        studentInfo.Content = "View All Student Information";
                        courseInfo.Content = "Manage All Courses";
                        ViewToggle.Visibility = Visibility.Visible;
                    }
                    await dialog.ShowAsync();
                }
                else
                {
                    var dialog = new ContentDialog()
                    {
                        Title = "Invalid Credentials",
                        Content = "The ID or password you entered is incorrect.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
                return;
            }
            else
            {
                var signUpButton = (Button)loginDialog.FindName("SignUpButton");
                signUpButton.Click += SignUpButton_Click;
            }
        }
    }
}