using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public int Currency { get; set; }
        public decimal Price { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Task>? Tasks { get; set; }
        public ICollection<ProductWarehouse>? ProductWarehouses { get; set; }
        public ICollection<Production>? Productions { get; set; }
    }
}
