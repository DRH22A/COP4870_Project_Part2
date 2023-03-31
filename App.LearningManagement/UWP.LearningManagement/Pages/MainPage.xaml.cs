using System;
using UWP.LearningManagement.Pages;
using UWP.LearningManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace UWP.LearningManagement
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            MyToggleButton.IsChecked = Toggle_State.IsChecked;
        }

        private void MyToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            MyToggleButton.Content = "Switch to Student Mode";
        }

        private void MyToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            MyToggleButton.Content = "Switch to TA/Instructor Mode";
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
    }
}
