using Library.LearningManagement.Models;
using Newtonsoft.Json;
using SupportTicketApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UWP.Library.LearningManagement.DTO;
using Windows.Data.Json;

namespace UWP.LearningManagement.ViewModels
{
    public class CoursesVM : INotifyPropertyChanged
    {
        private MainViewModel parentViewModel;
        public CoursesVM() { 
            Dto = new CoursesDTO();
        }
        public CoursesVM(MainViewModel mvm)
        {
            parentViewModel = mvm;
            if (parentViewModel?.SelectedCourse?.Dto==null) {
                Dto = new CoursesDTO();
            }
            else
            {
                Dto = parentViewModel.SelectedCourse.Dto;
            }
        }
        public CoursesDTO Dto { get; set; }
        public CoursesVM(CoursesDTO DTO)
        {
            Dto = DTO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<CoursesDTO> AddCourse()
        {
            var handler = new WebRequestHandler();
            var returnVal = await handler.Post("http://localhost:5140/Courses", Dto);
            var deserializedReturn = JsonConvert.DeserializeObject<CoursesDTO>(returnVal);
            parentViewModel.RefreshList();
            return deserializedReturn;
        }
        public string DisplayCourses
        {
            get
            {
                return $"[{Dto.Code}] - {Dto.Name} - {Dto.Description} - Room:{Dto.Room} - {Dto.CreditHours} Credits";
            }
        }
    }
}
