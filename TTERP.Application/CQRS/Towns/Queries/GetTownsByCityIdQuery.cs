using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Application.Models.DTOs.Towns;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Towns.Queries
{
    public class GetTownsByCityIdQuery : IRequest<Response<List<TownLocationDTO>>>
    {
        public int CityId { get; set; }
        public GetTownsByCityIdQuery(int cityId)
        {
            CityId = cityId;            
        }
    }
}
