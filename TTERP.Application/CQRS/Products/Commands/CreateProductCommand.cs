using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Products.Commands
{
    public class CreateProductCommand : IRequest<Response<int>>
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
