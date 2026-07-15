using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ProductWarehouses
{
    public class GetProductsStockDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }
        public double TotalQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
