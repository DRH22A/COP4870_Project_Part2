using Library.LearningManagement.Models;
using Library.LearningManagement.DTO;
using System;

namespace UWP.Library.LearningManagement.DTO
{
    public class AnnouncementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public AnnouncementDTO() { }
        public AnnouncementDTO(Announcement announcement)
        {
            Id = announcement.announcement_id;
            Name = announcement.announcement_name;
            Description = announcement.announcement_description;
        }
    }
}
