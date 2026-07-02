using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.SupplierMaterials
{
    public class GetMaterialSuppliersDTO
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public decimal UnitPrice { get; set; }
        public int? LeadTimeDays { get; set; } 
        public double? MOQ { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
