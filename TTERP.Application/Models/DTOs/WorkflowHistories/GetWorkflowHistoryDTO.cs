using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.WorkflowHistories
{
    public class GetWorkflowHistoryDTO
    {
        public int Id { get; set; }
        public int WorkflowType { get; set; }
        public int RecordId { get; set; }
        public int? FromStatusCode { get; set; }
        public string? FromStatusName { get; set; }
        public int ToStatusCode { get; set; }
        public string? ToStatusName { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Note { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
