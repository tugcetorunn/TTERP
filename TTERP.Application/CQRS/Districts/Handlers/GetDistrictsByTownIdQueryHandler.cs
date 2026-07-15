using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Districts.Queries;
using TTERP.Application.CQRS.Titles.Queries;
using TTERP.Application.Models.DTOs.Districts;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Districts.Handlers
{
    public class GetDistrictsByTownIdQueryHandler : IRequestHandler<GetDistrictsByTownIdQuery, Response<List<DistrictLocationDTO>>>
    {
        private readonly IDistrictRepository _districtRepository;

        public GetDistrictsByTownIdQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<Response<List<DistrictLocationDTO>>> Handle(GetDistrictsByTownIdQuery request, CancellationToken cancellationToken)
        {
            var districts = await _districtRepository.GetListWithFilterAsync(
                select: d => d.Adapt<DistrictLocationDTO>(),
                where: d => d.IsDeleted == false && d.IsActive && d.TownId == request.TownId,
                include: d => d.Include(d => d.Neighborhoods)!,
                orderBy: d => d.OrderBy(d => d.Name)
                );

            return Response<List<DistrictLocationDTO>>.Success(districts.ToList());
        }
    }
}
