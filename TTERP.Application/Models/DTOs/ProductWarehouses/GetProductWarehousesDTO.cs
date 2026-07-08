using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ProductWarehouses
{
    public class GetProductWarehousesDTO
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public double Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
