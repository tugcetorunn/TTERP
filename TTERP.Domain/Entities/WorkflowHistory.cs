using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class WorkflowHistory : BaseEntity<int>
    {
        public int WorkflowType { get; set; }
        public int RecordId { get; set; }
        public int? FromStatusCode { get; set; }
        public int ToStatusCode { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string? Note { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
