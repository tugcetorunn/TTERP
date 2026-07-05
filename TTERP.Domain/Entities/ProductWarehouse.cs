using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ProductWarehouse : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public double Quantity { get; set; }
        public int? ReasonForEntryOrExit { get; set; } // parametreden
    }
}