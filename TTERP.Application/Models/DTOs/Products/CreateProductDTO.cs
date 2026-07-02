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
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Currency { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public int CategoryId { get; set; }
    }
}
