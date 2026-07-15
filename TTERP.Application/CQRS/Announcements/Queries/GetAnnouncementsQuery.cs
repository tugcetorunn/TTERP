using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Announcements;
using TTERP.Shared.Models;
using Response = TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Announcements.Queries
{
    public class GetAnnouncementsQuery : IRequest<Response<IReadOnlyList<GetAnnouncementsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetAnnouncementsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
