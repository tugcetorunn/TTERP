using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Titles.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Teams;
using TTERP.Application.Models.DTOs.Titles;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Titles.Handlers
{
    public class GetTitlesQueryHandler : IRequestHandler<GetTitlesQuery, Response<IReadOnlyList<GetTitlesDTO>>>
    {
        private readonly ITitleRepository _titleRepository;

        public GetTitlesQueryHandler(ITitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<Response<IReadOnlyList<GetTitlesDTO>>> Handle(GetTitlesQuery request, CancellationToken cancellationToken)
        {
            var titles = await _titleRepository.GetListWithFilterAsync(
                t => t.Adapt<GetTitlesDTO>(),
                t => t.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || t.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetTitlesDTO>>.Success(titles.ToList());
        }
    }
}
