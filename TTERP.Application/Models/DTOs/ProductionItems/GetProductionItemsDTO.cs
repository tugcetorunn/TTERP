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
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public int SourceWarehouseId { get; set; }
        public string? SourceWarehouseName { get; set; }
        public double Quantity { get; set; }
        public double ScrapQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
      
    }
}
