using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.SupplierMaterials
{
    // bir satınalmacının tedarikçi seçeneklerinden aldığı malzeme fiyatlarını girdiği alan
    public class CreateSupplierMaterialDTO
    {
        public int SupplierId { get; set; }
        public int MaterialId { get; set; }
        public int Currency { get; set; }
        public decimal UnitPrice { get; set; }
        public int? LeadTimeDays { get; set; } 
        public double? MOQ { get; set; }
    }
}
