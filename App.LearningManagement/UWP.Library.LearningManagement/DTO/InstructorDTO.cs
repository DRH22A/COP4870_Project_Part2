using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UWP.Library.LearningManagement.DTO
{
    internal class InstructorDTO : PeopleDTO
    {
        public InstructorDTO(Person p) : base(p)
        {
        }
    }
}
