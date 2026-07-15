using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ProductionProgresses
{
    public class GetProductionProgressesDTO
    {
        public int ProductionId { get; set; }
        public double ProducedQuantity { get; set; }
        public string? Note { get; set; }
        public DateTime ProgressDate { get; set; } = DateTime.UtcNow;
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
    }
}
