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
        public ICollection<OrderItemWarehouse>? OrderItemWarehouses { get; set; } = new List<OrderItemWarehouse>(); // order item create edilirken aynı anda bunun da oluşması için ve order item id yi initialize etmesi için gerekiyor.
        public double Quantity { get; set; }
        public int Currency { get; set; }
        public decimal UnitPrice { get; set; } // sipariş anındaki ürün fiyatı, ürün fiyatı değişebilir, siparişteki fiyatı saklamak için
        public decimal Discount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
