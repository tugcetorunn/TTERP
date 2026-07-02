using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Categories.Queries;
using TTERP.Application.Models.DTOs.Categories;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Categories.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Response<IReadOnlyList<GetCategoriesDTO>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<IReadOnlyList<GetCategoriesDTO>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetListWithFilterAsync(
                select: c => c.Adapt<GetCategoriesDTO>(),
                where: c => c.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || c.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetCategoriesDTO>>.Success(categories.ToList());
        }
    }
}
