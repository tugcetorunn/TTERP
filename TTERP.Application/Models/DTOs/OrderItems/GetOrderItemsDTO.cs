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
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int Currency { get; set; }
        public string? CurrencyName { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItemStockLocationDTO>? StockLocations { get; set; } = new List<OrderItemStockLocationDTO>();
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
