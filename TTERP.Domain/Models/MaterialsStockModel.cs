using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Models
{
    public class MaterialsStockModel
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string MaterialCode { get; set; } = null!;
        public string MaterialUnit { get; set; } = null!;
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public string WarehouseCode { get; set; } = null!;
        public double TotalQuantity { get; set; }
        public double ReservedQuantity { get; set; }
        public double AvailableQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
