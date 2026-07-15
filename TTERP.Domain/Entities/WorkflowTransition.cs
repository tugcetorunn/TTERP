using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class WorkflowTransition : BaseEntity<int>
    {
        public int WorkflowType { get; set; }
        public int FromStatusCode { get; set; }
        public int ToStatusCode { get; set; }
        public int ActionCode { get; set; }
        public string? RequiredRole { get; set; }
        public bool RequiresConfirmation { get; set; }
        public bool CreatesStockMovement { get; set; }
        public int DisplayOrder { get; set; }
    }
}
