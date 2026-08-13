using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Customer : BaseEntity<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string? CompanyName { get; set; }
        public string? NationalId { get; set; }
        public string? TaxNumber { get; set; } // kuruluş ise vkn, bireysel ise tckn
        public int? CustomerType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? CustomerBalance { get; set; } = 0;
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
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Task>? Tasks { get; set; }
    }
}
