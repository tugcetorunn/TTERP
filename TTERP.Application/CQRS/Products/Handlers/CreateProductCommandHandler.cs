using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Products.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Products.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(product.Id, 201, "Ürün başarıyla oluşturuldu.");
        }
    }
}
