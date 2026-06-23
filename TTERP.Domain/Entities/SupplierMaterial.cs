using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class SupplierMaterial : BaseAuditEntity
    {
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public double UnitPrice { get; set; }
        public int? LeadTimeDays { get; set; } // teslim süresi
        public double? MOQ { get; set; } // minimum sipariş miktarı - MinimumOrderQuantity
    }
}
