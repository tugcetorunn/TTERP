using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Announcements.Commands
{
    public class UpdateAnnouncementCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TargetAudience { get; set; }
        public string? ImagePath { get; set; }
    }
}
