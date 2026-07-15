using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Products.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.Products;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Products.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Response<IReadOnlyList<GetProductsDTO>>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductsDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetListWithFilterAsync(
                    select: product => product,
                    where: product =>
                        product.IsDeleted == (request.IsDeleted ?? false) &&
                        (!request.IsActive.HasValue ||
                         product.IsActive == request.IsActive.Value),
                    include: query => query
                        .Include(product => product.Category)!);

                            var result = products
                                .Select(product => product.Adapt<GetProductsDTO>())
                                .ToList();

            return Response<IReadOnlyList<GetProductsDTO>>.Success(result);
        }
    }
}
