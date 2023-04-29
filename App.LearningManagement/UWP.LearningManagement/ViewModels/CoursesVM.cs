using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Library.LearningManagement.DTO;

namespace UWP.LearningManagement.ViewModels
{
    public class CoursesVM
    {
        public CoursesVM() { }
        public CoursesVM(CoursesDTO DTO)
        {
            Dto = DTO;
        }
        public CoursesDTO Dto { get; set; }
        public string DisplayCourses
        {
            get
            {
                return $"[{Dto.Code}] {Dto.Name} - {Dto.Description} - {Dto.Room} - {Dto.Semester} - {Dto.CreditHours}";
            }
        }
    }
}
