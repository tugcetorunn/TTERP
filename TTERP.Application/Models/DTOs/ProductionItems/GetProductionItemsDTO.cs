using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ProductionItems
{
    public class GetProductionItemsDTO
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialUnit { get; set; }
        public int SourceWarehouseId { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? SourceWarehouseCode { get; set; }
        public double PlannedQuantity { get; set; }
        public double? ActualQuantity { get; set; }
        public double? ScrapQuantity { get; set; }
        public double ReservedQuantity { get; set; }
        public double ConsumedQuantity { get; set; }
        public bool ReservationReleased { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
      
    }
}
