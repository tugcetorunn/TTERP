using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<Response<int>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? NationalId { get; set; }
        public string? TaxNumber { get; set; }
        public int? CustomerType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? CustomerBalance { get; set; } = 0;
        public string? AddressLine { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }
    }
}
