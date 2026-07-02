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
    public class GetWarehousesDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<GetWarehouseMaterialsDTO>? MaterialWarehouses { get; set; }
        public ICollection<GetWarehouseProductsDTO>? ProductWarehouses { get; set; }
        //public ICollection<SupplyItem>? SupplyItems { get; set; }
        //public ICollection<OrderItemWarehouse>? OrderItemWarehouses { get; set; }
        //public ICollection<Production>? SourceProductions { get; set; }
        //public ICollection<Production>? TargetProductions { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
