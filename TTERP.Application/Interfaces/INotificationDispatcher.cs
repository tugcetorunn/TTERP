using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Notifications;

namespace TTERP.Application.Interfaces
{
    // NotificationHub handlerda kullanılmasın diye (clean arch a aykırı (application katmanında webapi katmanından referans almamak için)). bağımlılıklar dıştan içe doğru olmalı webapi -> application
    public interface INotificationDispatcher
    {
        Task SendToUserAsync(int employeeId, GetMyNotificationsDTO message, CancellationToken cancellationToken = default);
    }
}
