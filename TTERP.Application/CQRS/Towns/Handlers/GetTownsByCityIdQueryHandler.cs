using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Titles.Queries;
using TTERP.Application.CQRS.Towns.Queries;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Application.Models.DTOs.Towns;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Towns.Handlers
{
    public class GetTownsByCityIdQueryHandler : IRequestHandler<GetTownsByCityIdQuery, Response<List<TownLocationDTO>>>
    {
        private readonly ITownRepository _townRepository;

        public GetTownsByCityIdQueryHandler(ITownRepository townRepository)
        {
            _townRepository = townRepository;
        }

        public async Task<Response<List<TownLocationDTO>>> Handle(GetTownsByCityIdQuery request, CancellationToken cancellationToken)
        {
            var towns = await _townRepository.GetListWithFilterAsync(
                select: t => t.Adapt<TownLocationDTO>(),
                where: t => t.IsDeleted == false && t.IsActive && t.CityId == request.CityId,
                include: t => t.Include(t => t.Districts)!,
                orderBy: t => t.OrderBy(t => t.Name)
                );

            return Response<List<TownLocationDTO>>.Success(towns.ToList());
        }
    }
}
