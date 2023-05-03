using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWP.LearningManagement.Pages;
using UWP.LearningManagement.ViewModels;
using Windows.Security.Authentication.OnlineId;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP.LearningManagement
{
    public sealed partial class MainPage : Page
    {
        private int loggedInUserId = -1;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            ViewToggle.IsChecked = Toggle_State.IsChecked;
            Semester.CurrentSemester.Season = SeasonEnum.Fall;
            Semester.CurrentSemester.Year = YearEnum.Year_2023;
        }

        private void ViewToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            ViewToggle.Content = "Switch To Student Mode";
            studentInfo.Content = "View All Student Information";
            courseInfo.Content = "Manage All Courses";
            databaseInfo.Content = "Look At Database Content";
        }

        private void ViewToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            ViewToggle.Content = "Switch To TA/Instructor Mode";
            studentInfo.Content = "Check Your Student Information";
            courseInfo.Content = "Manage Your Courses";
            databaseInfo.Content = "Look At Database Content";
        }

        private void StudentOptions_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Student_Options));
        }

        private void MaintainCourses_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Course_Options));
        }

        private void DatabaseInfo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Database_Options));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("LoggedInUserId", out object loggedInUserIdObject))
            {
                loggedInUserId = (int)loggedInUserIdObject;
                Login.Visibility = Visibility.Collapsed;
                studentInfo.Visibility = Visibility.Visible;
                courseInfo.Visibility = Visibility.Visible;
                databaseInfo.Visibility = Visibility.Visible;
                SignOut.Visibility = Visibility.Visible;
                ViewToggle.Visibility = StudentService.Current.People.FirstOrDefault(p => p.Id == loggedInUserId) is Instructor || StudentService.Current.People.FirstOrDefault(p => p.Id == loggedInUserId) is TeachingAssistant ? Visibility.Visible : Visibility.Collapsed;
            }
        }

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

                Person person = StudentService.Current.People.FirstOrDefault(p => p.Id == id && p.Password == password);
                if (person != null)
                {
                    loggedInUserId = person.Id;
                    ApplicationData.Current.LocalSettings.Values["LoggedInUserId"] = loggedInUserId;
                    StudentService.Current.LoggedInUserId = person.Id;
                    CourseService.Current.LoggedInUserId = person.Id;

                    var dialog = new ContentDialog()
                    {
                        Title = "Success!",
                        Content = "Welcome " + person.Name,
                        CloseButtonText = "OK"
                    };
                    studentInfo.Visibility = Visibility.Visible;
                    courseInfo.Visibility = Visibility.Visible;
                    Login.Visibility = Visibility.Collapsed;
                    SignOut.Visibility = Visibility.Visible;
                    databaseInfo.Visibility = Visibility.Visible;
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
        }
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            loggedInUserId = -1;
            StudentService.Current.LoggedInUserId = -1;
            CourseService.Current.LoggedInUserId = -1;

            studentInfo.Visibility = Visibility.Collapsed;
            databaseInfo.Visibility= Visibility.Collapsed;
            courseInfo.Visibility = Visibility.Collapsed;
            ViewToggle.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Visible;
            SignOut.Visibility = Visibility.Collapsed;

            var dialog = new ContentDialog()
            {
                Title = "Success!",
                Content = "You have been logged out.",
                CloseButtonText = "OK"
            };
            ApplicationData.Current.LocalSettings.Values.Remove("LoggedInUserId");
            await dialog.ShowAsync();
        }
    }
}