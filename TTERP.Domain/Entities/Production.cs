using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    // a deposundan hammadde alınıp, b deposuna örn 100 tane product koyulacak senaryoda target ve source warehouse kullanılacak
    public class Production : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public double PlannedQuantity { get; set; } // planlanan
        public double? ActualQuantity { get; set; } // gerçekleşen
        public int TargetWarehouseId { get; set; } // üretilen ürün hangi depoya girecek
        public Warehouse? TargetWarehouse { get; set; }
        public int ProductionStatus { get; set; } = 1; // default : planlandı (1)
        public ICollection<ProductionItem>? ProductionItems { get; set; }
    }
}
