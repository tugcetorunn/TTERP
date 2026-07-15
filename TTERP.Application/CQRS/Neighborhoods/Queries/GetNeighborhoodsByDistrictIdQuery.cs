using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Neighborhoods;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Neighborhoods.Queries
{
    public class GetNeighborhoodsByDistrictIdQuery : IRequest<Response<List<NeighborhoodLocationDTO>>>
    {
        public int DistrictId { get; set; }
        public GetNeighborhoodsByDistrictIdQuery(int districtId)
        {
            DistrictId = districtId;
        }
    }
}
