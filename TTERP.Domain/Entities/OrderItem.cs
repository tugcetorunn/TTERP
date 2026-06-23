using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class OrderItem : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; } // sipariş anındaki ürün fiyatı, ürün fiyatı değişebilir, siparişteki fiyatı saklamak için
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double TotalPrice { get; set; }
    }
}
