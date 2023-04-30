using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWP.LearningManagement.ViewModels;
using Windows.Security.Authentication.OnlineId;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using UWP.Library.LearningManagement.Database;
using UWP.LearningManagement.Dialogs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.LearningManagement.Pages
{

    public sealed partial class Database_Options : Page
    {
        public Database_Options()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
            ViewToggle.IsChecked = Toggle_State.IsChecked;
        }

        private void ViewToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = true;
            ViewToggle.Content = "Switch To Student Mode";

        }

        private void ViewToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggle_State.IsChecked = false;
            ViewToggle.Content = "Switch To TA/Instructor Mode";

        }

        private async void AddCourse_Database(object sender, RoutedEventArgs e)
        {
            var addDialog = new CourseDialog(DataContext as MainViewModel);
            await addDialog.ShowAsync();
        }

        private async void EditCourse_Database(object sender, RoutedEventArgs e)
        {
            var addDialog = new CourseDialog(DataContext as MainViewModel);
            await addDialog.ShowAsync();
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
