using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class PlanProductionCommandHandler : IRequestHandler<PlanProductionCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlanProductionCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(PlanProductionCommand request, CancellationToken cancellationToken)
        {
            var production = request.Adapt<Production>();

            if (request.ProductionItems != null && request.ProductionItems.Any())
            {
                production.ProductionItems = request.ProductionItems.Adapt<List<ProductionItem>>();
            }

            await _productionRepository.AddAsync(production);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(production.Id, 201, "Üretim planı başarıyla oluşturuldu.");
        }
    }
}
