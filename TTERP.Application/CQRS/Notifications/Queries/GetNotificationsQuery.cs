using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Notifications;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<Response<IReadOnlyList<GetMyNotificationsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetNotificationsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
