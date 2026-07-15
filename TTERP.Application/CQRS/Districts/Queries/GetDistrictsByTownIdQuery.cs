using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Districts;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Districts.Queries
{
    public class GetDistrictsByTownIdQuery : IRequest<Response<List<DistrictLocationDTO>>>
    {
        public int TownId { get; set; }
        public GetDistrictsByTownIdQuery(int townId)
        {
            TownId = townId;
        }
    }
}
