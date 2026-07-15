using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ProductionProgress : BaseEntity<int>
    {
        public int ProductionId { get; set; }
        public Production? Production { get; set; }
        public double ProducedQuantity { get; set; }
        public string? Note { get; set; }
        public DateTime ProgressDate { get; set; } = DateTime.UtcNow;
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
