using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    // x malzemesini kimlerden alabiliriz sorusunun cevabı. transaction değil mevcut durumu gösteren (state) tablodur.
    public class SupplierMaterial : BaseEntity<int>
    {
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public int Currency { get; set; }
        public decimal UnitPrice { get; set; }
        public int? LeadTimeDays { get; set; } // teslim süresi
        public double? MOQ { get; set; } // minimum sipariş miktarı - MinimumOrderQuantity
    }
}
