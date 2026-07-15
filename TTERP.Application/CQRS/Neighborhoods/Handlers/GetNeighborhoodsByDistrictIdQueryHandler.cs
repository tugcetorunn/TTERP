using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Neighborhoods.Queries;
using TTERP.Application.CQRS.Titles.Queries;
using TTERP.Application.Models.DTOs.Neighborhoods;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Neighborhoods.Handlers
{
    public class GetNeighborhoodsByDistrictIdQueryHandler : IRequestHandler<GetNeighborhoodsByDistrictIdQuery, Response<List<NeighborhoodLocationDTO>>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public GetNeighborhoodsByDistrictIdQueryHandler(INeighborhoodRepository neighborhoodRepository)
        {
            _neighborhoodRepository = neighborhoodRepository;
        }

        public async Task<Response<List<NeighborhoodLocationDTO>>> Handle(GetNeighborhoodsByDistrictIdQuery request, CancellationToken cancellationToken)
        {
            var neighborhoods = await _neighborhoodRepository.GetListWithFilterAsync(
                select: n => n.Adapt<NeighborhoodLocationDTO>(),
                where: n => n.IsDeleted == false && n.IsActive && n.DistrictId == request.DistrictId,
                include: n => n.Include(n => n.PostalCode)!,
                orderBy: n => n.OrderBy(n => n.Name)
                );

            return Response<List<NeighborhoodLocationDTO>>.Success(neighborhoods.ToList());
        }
    }
}
