using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.TaskAssignments;

namespace TTERP.Application.Models.DTOs.Tasks
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int TaskType { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? MaterialId { get; set; }
        public DateTime DueDate { get; set; }
        public int? Priority { get; set; }
        public List<CreateTaskAssignmentDTO>? Assignments { get; set; }
    }
}
