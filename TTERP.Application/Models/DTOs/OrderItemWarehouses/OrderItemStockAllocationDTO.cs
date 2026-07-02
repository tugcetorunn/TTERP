using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.OrderItemWarehouses
{
    public class OrderItemStockAllocationDTO
    {
        public int WarehouseId { get; set; }
        public double QuantityFromWarehouse { get; set; }
    }
}
