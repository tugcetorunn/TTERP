using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    /// <summary>
    /// Tedarik işlemlerini temsil eder. Bir tedarik, belirli bir malzemenin belirli bir miktarının belirli bir fiyattan tedarik edilmesini içerir. Tedarik tarihi, tedarik anındaki fiyat ve toplam fiyat gibi bilgileri saklar. Malzeme fiyatları zamanla değişebilir, bu nedenle tedarik anındaki fiyatı saklamak önemlidir.
    /// </summary>
    public class Supply : BaseEntity<int>
    {
        public decimal TotalAmount { get; set; }
        public DateTime SupplyDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DocumentNumber { get; set; } // irsaliye no
        public int? SupplyStatus { get; set; } = 1; // planlandı
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public ICollection<SupplyItem>? SupplyItems { get; set; } = new List<SupplyItem>();
    }
}
