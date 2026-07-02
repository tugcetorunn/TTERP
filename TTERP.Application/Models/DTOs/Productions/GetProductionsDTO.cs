using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Productions
{
    public class GetProductionsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double PlannedQuantity { get; set; }
        public int SourceWarehouseId { get; set; } // hammaddeler hangi depodan düşülecek
        public string? SourceWarehouseName { get; set; }
        public int TargetWarehouseId { get; set; } // üretilen ürün hangi depoya girecek
        public string? TargetWarehouseName { get; set; }
        public int ProductionStatus { get; set; }
        public string ProductionStatusName { get; set; }
        public ICollection<GetProductionItemsDTO>? ProductionItems { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
