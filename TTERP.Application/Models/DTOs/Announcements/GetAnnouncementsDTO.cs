using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Announcements
{
    public class GetAnnouncementsDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
