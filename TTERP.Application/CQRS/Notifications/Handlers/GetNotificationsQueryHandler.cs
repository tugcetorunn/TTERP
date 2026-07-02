using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Notifications.Queries;
using TTERP.Application.Models.DTOs.Notifications;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Notifications.Handlers
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Response<IReadOnlyList<GetMyNotificationsDTO>>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Response<IReadOnlyList<GetMyNotificationsDTO>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetListWithFilterAsync(
                select: n => n.Adapt<GetMyNotificationsDTO>(),
                where: n => n.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || n.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetMyNotificationsDTO>>.Success(notifications.ToList());
        }
    }
}
