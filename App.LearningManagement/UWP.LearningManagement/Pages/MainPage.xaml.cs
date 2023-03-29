using System;
using UWP.LearningManagement.Pages;
using UWP.LearningManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP.LearningManagement
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
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
