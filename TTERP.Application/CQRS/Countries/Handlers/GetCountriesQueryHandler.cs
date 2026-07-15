using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Countries.Queries;
using TTERP.Application.Models.DTOs.Countries;
using TTERP.Application.Models.DTOs.Locations;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Countries.Handlers
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, Response<List<CountryLocationDTO>>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Response<List<CountryLocationDTO>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.GetListWithFilterAsync(
                select: c => c.Adapt<CountryLocationDTO>(),
                where: c => c.IsDeleted == false && c.IsActive,
                include: c => c.Include(c => c.Cities)!,
                orderBy: c => c.OrderBy(c => c.Name)
                );

            return Response<List<CountryLocationDTO>>.Success(countries.ToList());
        }
    }
}
