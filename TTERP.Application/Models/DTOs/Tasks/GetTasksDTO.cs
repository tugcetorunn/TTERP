using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.TaskAssignments;

namespace TTERP.Application.Models.DTOs.Tasks
{
    public class GetTasksDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TaskType { get; set; }
        public string TaskTypeName { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderNumber { get; set; }
        public string? ProductName { get; set; }
        public string? MaterialName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public string? StatusName { get; set; }
        public bool HasConversation { get; set; }
        public List<GetTaskAssignmentsDTO>? TaskAssignments { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
