using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Materials
{
    public class GetMaterialsDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
