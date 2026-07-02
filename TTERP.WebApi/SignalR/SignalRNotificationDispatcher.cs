using Microsoft.AspNetCore.SignalR;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Notifications;
using TTERP.WebApi.Hubs;

namespace TTERP.WebApi.SignalR
{
    // HubContexti inject edeceğimiz yer. bağımlılıkları bozmamak için eklendi. app katmanından miras alıyor bu yüzden
    public class SignalRNotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(int employeeId, GetMyNotificationsDTO message, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.User(employeeId.ToString()).SendAsync("ReceiveNotification", message, cancellationToken);
        }
    }
}
