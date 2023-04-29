using Library.LearningManagement.Models;
using Newtonsoft.Json;
using SupportTicketApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Library.LearningManagement.DTO;
using Windows.Data.Json;

namespace UWP.LearningManagement.ViewModels
{
    public class CoursesVM
    {
        public CoursesVM() { 
            Dto = new CoursesDTO();
        }
        public CoursesDTO Dto { get; set; }
        public CoursesVM(CoursesDTO DTO)
        {
            Dto = DTO;
        }

        public async Task<CoursesDTO> AddCourse()
        {
            var handler = new WebRequestHandler();
            var returnVal = await handler.Post("http://localhost:5140/Courses", Dto);
            var deserializedReturn = JsonConvert.DeserializeObject<CoursesDTO>(returnVal);
            return deserializedReturn;
        }
        public string DisplayCourses
        {
            get
            {
                return $"[{Dto.Code}] {Dto.DisplayCourse} - {Dto.Description} - Room:{Dto.Room} - Semester:{Dto.Semester} - {Dto.CreditHours} Credits";
            }
        }
    }
}
