using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    // sipariş verilip malzeme depoya geldiğinde, o anki fiyattan, o anki miktardan db ye kayıt atar. bir transaction tablosudur.
    public class SupplyItem : BaseEntity<int>
    {
        public int SupplyId { get; set; }
        public Supply? Supply { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public int? SupplierMaterialId { get; set; }
        public SupplierMaterial? SupplierMaterial { get; set; } // bu tedarik kalemi hangi teklif üzerinden yapılmıştı sorusunun cevabı olacak (ileride)
        public double Quantity { get; set; }
        public int Currency { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; } // tedarik anındaki fiyat, malzeme fiyatı değişebilir, tedarikteki fiyatı saklamak için
        public decimal TaxRate { get; set; } 
        public decimal DiscountRate { get; set; } = 0;
        // hesaplanan alanlar (snapshot)
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
