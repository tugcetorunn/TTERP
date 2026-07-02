using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Materials.Queries;
using TTERP.Application.Models.DTOs.Materials;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Materials.Handlers
{
    public class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, Response<IReadOnlyList<GetMaterialsDTO>>>
    {
        private readonly IMaterialRepository _materialRepository;

        public GetMaterialsQueryHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialsDTO>>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetListWithFilterAsync(
                select: m => m.Adapt<GetMaterialsDTO>(),
                where: m => m.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || m.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetMaterialsDTO>>.Success(materials.ToList());
        }
    }
}
