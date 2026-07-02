using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Warehouses
{
    public class CreateWarehouseDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
