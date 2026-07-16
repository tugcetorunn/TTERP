using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.MaterialWarehouses
{
    public class GetMaterialsStockDTO
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public int MaterialUnit { get; set; }
        public string MaterialUnitName { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }
        public double TotalQuantity { get; set; }
        public double ReservedQuantity { get; set; }
        public double AvailableQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
