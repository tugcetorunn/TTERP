using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.SupplyItems
{
    public class GetSupplyItemsDTO
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public int SupplierMaterialId { get; set; }

        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialUnit { get; set; }

        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }

        public double Quantity { get; set; }

        public int Currency { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencyShortCode { get; set; }
        public string? CurrencySymbol { get; set; }

        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal TaxRate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
