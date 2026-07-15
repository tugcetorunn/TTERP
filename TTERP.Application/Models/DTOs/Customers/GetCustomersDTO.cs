using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Customers
{
    public class GetCustomersDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string TaxNumber { get; set; } // kuruluş ise vkn, bireysel ise tckn
        public int? CustomerType { get; set; }
        public string? CustomerTypeName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? CustomerBalance { get; set; }
        public string? AddressLine { get; set; }
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
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
