using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Commands
{
    public class CreateWarehouseCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }
        public string AddressLine { get; set; }
    }
}
