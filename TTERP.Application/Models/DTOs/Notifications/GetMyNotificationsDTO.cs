using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Notifications
{
    // employeeid olmadan kimin bildirimi olduğu nasıl önyüze gönderilecek...
    public class GetMyNotificationsDTO
    {
        public int Id { get; set; } // okundu yapmak için gerek
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
