using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Cities;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Cities.Queries
{
    public class GetCitiesByCountryIdQuery : IRequest<Response<List<CityLocationDTO>>>
    {
        public int CountryId { get; set; }
        public GetCitiesByCountryIdQuery(int countryId)
        {
            CountryId = countryId;
        }
    }
}
