using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Task : BaseEntity<int>
    {
        public int TaskType { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? CreatedByEmployeeId { get; set; }
        public Employee? CreatedByEmployee { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? MaterialId { get; set; }
        public Material? Material { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; } = new List<TaskAssignment>();
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public bool HasConversation { get; set; } = false;
        public int? ConversationId { get; set; }
    }
}
