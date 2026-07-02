using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class OrderItemWarehouse : BaseEntity<int>
    {
        public int OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public double Quantity { get; set; }
    }
}