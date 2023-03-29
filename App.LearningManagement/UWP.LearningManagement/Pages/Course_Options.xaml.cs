using System;
using UWP.LearningManagement.Pages;
using UWP.LearningManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.LearningManagement.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Course_Options : Page
    {
        public Course_Options()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        // CreateCourseRecord function
        void CreateCourseRecord_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // UpdateCourseRecord function
        void UpdateCourseRecord_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // AddStudent function
        void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // RemoveStudent function
        void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            // implementation
        }

        // AddAssignment function
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
