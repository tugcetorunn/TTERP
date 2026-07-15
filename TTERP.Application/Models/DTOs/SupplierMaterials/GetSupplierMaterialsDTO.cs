using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.SupplierMaterials
{
    public class GetSupplierMaterialsDTO
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialUnit { get; set; }
        public string MaterialUnitName { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public int ReasonForEntryOrExit { get; set; }
        public string ReasonForEntryOrExitName { get; set; }
        public int? LeadTimeDays { get; set; } 
        public double? MOQ { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
