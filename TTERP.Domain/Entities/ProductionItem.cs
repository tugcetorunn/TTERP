using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ProductionItem : BaseEntity<int>
    {
        public int ProductionId { get; set; }
        public Production? Production { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public int SourceWarehouseId { get; set; } // hammaddeler hangi depodan düşülecek
        public Warehouse? SourceWarehouse { get; set; }
        public double Quantity { get; set; } // planlanan miktar
        public double? ActualQuantity { get; set; } // gerçekleşen miktar
        public double? ScrapQuantity { get; set; } = 0; // fire miktarı
    }
}
