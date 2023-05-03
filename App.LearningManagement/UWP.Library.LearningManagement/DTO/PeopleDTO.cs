using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UWP.Library.LearningManagement.DTO
{
    public class PeopleDTO
    { 
        public PeopleDTO() { }
        public PeopleDTO(Person p) 
        {
            Id = p.Id;
            Name = p.Name;
            Password = p.Password;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string DisplayPeople { get; set; }

    }
}
