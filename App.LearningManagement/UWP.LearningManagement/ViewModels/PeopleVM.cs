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
using UWP.Library.LearningManagement.Database;
using UWP.Library.LearningManagement.DTO;
using Windows.Data.Json;

namespace UWP.LearningManagement.ViewModels
{
    public class PeopleVM : INotifyPropertyChanged
    {
        private MainViewModel parentViewModel;

        public PeopleVM() 
        {
            Dto = new PeopleDTO();
        }
        public PeopleVM(MainViewModel mvm)
        {
            parentViewModel = mvm;
            if (parentViewModel?.SelectedPerson?.Dto==null)
            {
                Dto = new PeopleDTO();
            }
            else
            {
                Dto = parentViewModel.SelectedPerson.Dto;
            }
        }
        public PeopleDTO Dto { get; set; }
        public PeopleVM(PeopleDTO DTO)
        {
            Dto = DTO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<PeopleDTO> AddPerson()
        {
            var handler = new WebRequestHandler();
            var returnVal = await handler.Post("http://localhost:5140/People", Dto);
            var deserializedReturn = JsonConvert.DeserializeObject<PeopleDTO>(returnVal);
            parentViewModel.RefreshList();
            return deserializedReturn;
        }

        public async Task<PeopleDTO> RemovePerson(int personId)
        {
            var handler = new WebRequestHandler();
            var returnVal = await handler.Delete($"http://localhost:5140/People/{personId}", null);
            var deserializedReturn = JsonConvert.DeserializeObject<PeopleDTO>(returnVal);
            DatabaseContext.people.Remove(DatabaseContext.people.FirstOrDefault(p => p.Id == personId));
            parentViewModel.RefreshList();
            return deserializedReturn;
        }
        public string DisplayPeople
        {
            get
            {
                return $"[{Dto.Id}] - {Dto.Name}";
            }
        }
    }
}
