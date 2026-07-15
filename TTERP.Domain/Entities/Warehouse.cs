using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Warehouse : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? TownId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }
        public Town? Town { get; set; }
        public District? District { get; set; }
        public Neighborhood? Neighborhood { get; set; } // en dıştaki nav prop diğerleriyle ilişkili olduğu için yeterli
        public string AddressLine { get; set; }
        public ICollection<SupplyItem>? SupplyItems { get; set; }
        public ICollection<MaterialWarehouse>? MaterialWarehouses { get; set; }
        public ICollection<ProductWarehouse>? ProductWarehouses { get; set; }
        public ICollection<OrderItemWarehouse>? OrderItemWarehouses { get; set; }
        public ICollection<Production>? SourceProductions { get; set; }
        public ICollection<Production>? TargetProductions { get; set; }
    }
}
