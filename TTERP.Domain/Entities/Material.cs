using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Material : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; } // son alınan fiyat, tedariklerde geçmişe dönük fiyat var.
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public ICollection<SupplierMaterial>? SupplierMaterials { get; set; }
        public ICollection<MaterialWarehouse>? MaterialWarehouses { get; set; }
        public ICollection<SupplyItem>? SupplyItems { get; set; }
        public ICollection<Task>? Tasks { get; set; }
        public ICollection<ProductionItem>? ProductionItems { get; set; }
    }
}
