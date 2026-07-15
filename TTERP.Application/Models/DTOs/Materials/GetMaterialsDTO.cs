using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Materials
{
    public class GetMaterialsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public int Unit { get; set; }
        public string? UnitName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public int SupplierCount { get; set; }
        public int WarehouseCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<GetSupplierMaterialsDTO>? SupplierMaterials { get; set; }
        public ICollection<GetMaterialWarehousesDTO>? MaterialWarehouses { get; set; }
    }
}
