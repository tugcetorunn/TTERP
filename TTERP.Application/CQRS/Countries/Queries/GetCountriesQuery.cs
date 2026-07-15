using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Countries;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Countries.Queries
{
    public class GetCountriesQuery : IRequest<Response<List<CountryLocationDTO>>>
    {
    }
}
