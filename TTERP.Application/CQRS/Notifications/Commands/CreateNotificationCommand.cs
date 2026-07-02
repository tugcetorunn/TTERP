using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<Response<int>>
    {
        public int EmployeeId { get; set; } // okundu yapmak için gerek
        public string Title { get; set; }
        public string Message { get; set; }
        public int NotificationType { get; set; }
        // kullanıcı bildirime tıkladığında onu ilgili sayfaya yönlendirmek için.
        public string? ActionUrl { get; set; } // /tasks/detail/45
    }
}
