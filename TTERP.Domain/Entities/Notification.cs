using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Notification : BaseEntity<int>
    {
        public string Title { get; set; }
        public int? NotificationType { get; set; }
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string? ActionUrl { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
