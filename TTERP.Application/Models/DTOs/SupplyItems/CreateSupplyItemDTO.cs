using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.SupplyItems
{
    public class CreateSupplyItemDTO
    {
        public int MaterialId { get; set; }
        public int WarehouseId { get; set; }
        public double Quantity { get; set; }
        public int Currency { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxRate { get; set; }
    }
}
