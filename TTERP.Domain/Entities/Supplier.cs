using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Supplier : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? TownId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }
        public Town? Town { get; set; }
        public District? District { get; set; }
        public Neighborhood? Neighborhood { get; set; }
        public ICollection<SupplierMaterial>? SupplierMaterials { get; set; }
        public ICollection<Supply>? Supplies { get; set; }
    }
}
