using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ProductionItems
{
    public class PlanProductionItemDTO
    {
        public int MaterialId { get; set; }
        public int SourceWarehouseId { get; set; }
        public double PlannedQuantity { get; set; }
      
    }
}
