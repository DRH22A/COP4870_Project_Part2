using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.LearningManagement.ViewModels;
using UWP.Library.LearningManagement.DTO;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWP.Library.LearningManagement.Database;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.LearningManagement.Dialogs
{
    public sealed partial class CourseRemoveDialog : ContentDialog
    {
        private int courseId = 0;

        public CourseRemoveDialog(MainViewModel mvm)
        {
            this.InitializeComponent();
            DataContext = new CoursesVM(mvm);
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            courseId = DatabaseContext.courses.FirstOrDefault(c => c.Code != string.Empty)?.Id ?? 0;
            var test = await (DataContext as CoursesVM).RemoveCourse(courseId);
            Console.WriteLine(test.DisplayCourse);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }

}
