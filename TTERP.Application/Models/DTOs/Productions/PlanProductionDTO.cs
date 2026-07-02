using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Productions
{
    public class PlanProductionDTO
    {
        public int ProductId { get; set; }
        public double PlannedQuantity { get; set; }
        public int TargetWarehouseId { get; set; }
        public List<PlanProductionItemDTO>? ProductionItems { get; set; }
    }
}
