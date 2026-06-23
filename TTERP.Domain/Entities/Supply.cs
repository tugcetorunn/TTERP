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
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; } // tedarik anındaki fiyat, malzeme fiyatı değişebilir, tedarikteki fiyatı saklamak için
        public double TotalPrice { get; set; }
        public DateTime SupplyDate { get; set; }
        public int? SupplyStatus { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
