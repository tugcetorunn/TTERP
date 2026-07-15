using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Cities.Queries;
using TTERP.Application.Models.DTOs.Cities;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Cities.Handlers
{
    public class GetCitiesByCountryIdQueryHandler : IRequestHandler<GetCitiesByCountryIdQuery, Response<List<CityLocationDTO>>>
    {
        private readonly ICityRepository _cityRepository;

        public GetCitiesByCountryIdQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<Response<List<CityLocationDTO>>> Handle(GetCitiesByCountryIdQuery request, CancellationToken cancellationToken)
        {
            var cities = await _cityRepository.GetListWithFilterAsync(
                select: c => c.Adapt<CityLocationDTO>(),
                where: c => c.IsDeleted == false && c.IsActive && c.CountryId == request.CountryId,
                include: c => c.Include(c => c.Towns)!,
                orderBy: c => c.OrderBy(c => c.Name)
                );

            return Response<List<CityLocationDTO>>.Success(cities.ToList());
        }
    }
}
