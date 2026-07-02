using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Materials
{
    public class MaterialDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<MaterialStockLocationDTO>? MaterialStocks { get; set; }
        //public ICollection<SupplierMaterial>? SupplierMaterials { get; set; }
        //public ICollection<MaterialWarehouse>? MaterialWarehouses { get; set; }
        //public ICollection<SupplyItem>? SupplyItems { get; set; }
        //public ICollection<Task>? Tasks { get; set; }
        //public ICollection<ProductionItem>? ProductionItems { get; set; }
    }
}
