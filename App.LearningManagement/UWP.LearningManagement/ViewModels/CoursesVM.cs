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
            dto = DTO;
        }
        public CoursesDTO dto { get; set; }
        public string Display
        {
            get
            {
                return $"[{dto.Code}] {dto.Name} - {dto.Description} - {dto.Room} - {dto.Semester} ]";
            }
        }
    }
}
