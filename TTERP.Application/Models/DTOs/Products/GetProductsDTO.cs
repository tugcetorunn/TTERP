using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Productions;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Products
{
    public class GetProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<GetProductWarehousesDTO>? ProductWarehouses { get; set; }
        public ICollection<GetProductionsDTO>? Productions { get; set; }
    }
}
