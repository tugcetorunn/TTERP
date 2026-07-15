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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? TownId { get; set; }
        public string? TownName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? NeighborhoodId { get; set; }
        public string? NeighborhoodName { get; set; }
        public string AddressLine { get; set; }
        public ICollection<GetMaterialWarehousesDTO>? MaterialWarehouses { get; set; }
        public ICollection<GetWarehouseToProductsDTO>? ProductWarehouses { get; set; }
        //public ICollection<SupplyItem>? SupplyItems { get; set; }
        //public ICollection<OrderItemWarehouse>? OrderItemWarehouses { get; set; }
        //public ICollection<Production>? SourceProductions { get; set; }
        //public ICollection<Production>? TargetProductions { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
