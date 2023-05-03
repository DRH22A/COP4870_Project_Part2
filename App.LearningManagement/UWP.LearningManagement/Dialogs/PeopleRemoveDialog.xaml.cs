using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.LearningManagement.ViewModels;
using UWP.Library.LearningManagement.Database;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.LearningManagement.Dialogs
{
    public sealed partial class PeopleRemoveDialog : ContentDialog
    {
        private int personId = 0;

        public PeopleRemoveDialog(MainViewModel mvm)
        {
            this.InitializeComponent();
            DataContext = new PeopleVM(mvm);
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            personId = DatabaseContext.people.FirstOrDefault(s => s.Name != string.Empty)?.Id ?? 0;
            var test = await (DataContext as PeopleVM).RemovePerson(personId);
            Console.WriteLine(test.DisplayPeople);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}