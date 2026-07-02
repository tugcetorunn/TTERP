using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.OrderItemWarehouses;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.OrderItems
{
    public class GetOrderItemsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItemStockLocationDTO>? StockLocations { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
