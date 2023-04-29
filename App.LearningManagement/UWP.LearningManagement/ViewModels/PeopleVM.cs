using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Library.LearningManagement.DTO;

namespace UWP.LearningManagement.ViewModels
{
    public class PeopleVM
    {
        public PeopleVM() { }
        public PeopleVM(PeopleDTO DTO)
        {
            Dto = DTO;
        }
        public PeopleDTO Dto { get; set; }
        public string DisplayPeople
        {
            get
            {
                return $"[{Dto.Id}] - {Dto.Name}";
            }
        }
    }
}
