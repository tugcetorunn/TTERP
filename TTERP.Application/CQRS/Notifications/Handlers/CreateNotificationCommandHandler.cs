using Mapster;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Notifications.Commands;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Notifications;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Notifications.Handlers
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Response<int>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationDispatcher _notificationDispatcher; // HubContext'i içeriye enjekte edecektik, araya bu yapıyı koyduk (bağımlılıktan dolayı)

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, INotificationDispatcher notificationDispatcher)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _notificationDispatcher = notificationDispatcher;
        }

        public async Task<Response<int>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = request.Adapt<Notification>();
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var message = new GetMyNotificationsDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = false,
                CreatedDate = DateTime.Now
            };

            await _notificationDispatcher.SendToUserAsync(request.EmployeeId, message, cancellationToken);

            return Response<int>.Success(notification.Id, 201, "Bildirim başarıyla gönderildi.");
        }
    }
}
