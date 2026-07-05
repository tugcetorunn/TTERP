using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class CompleteProductionCommandHandler : IRequestHandler<CompleteProductionCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteProductionCommandHandler(IProductionRepository productionRepository, IProductWarehouseRepository productWarehouseRepository, IUnitOfWork unitOfWork, IParameterValueRepository parameterValueRepository, IMaterialWarehouseRepository materialWarehouseRepository)
        {
            _productionRepository = productionRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _unitOfWork = unitOfWork;
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
        }

        public async Task<Response<int>> Handle(CompleteProductionCommand request, CancellationToken cancellationToken)
        {
            var production = await _productionRepository.GetProductionWithItemsAsync(request.ProductionId, cancellationToken);

            if (production == null)
            {
                return Response<int>.Fail(404, "Üretim emri bulunamadı.");
            }

            if (production.ProductionStatus == await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Completed", cancellationToken))
            {
                return Response<int>.Fail(400, "Üretim emri zaten tamamlanmış.");
            }

            if (production.ProductionStatus != await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "InProgress", cancellationToken))
            {
                return Response<int>.Fail(400, "Sadece 'Üretimde' durumundaki işler tamamlanabilir.");
            }

            production.ActualQuantity = request.ActualQuantity;
            production.ProductionStatus = await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Completed", cancellationToken);

            if (request.CompleteProductionItems != null && request.CompleteProductionItems.Any())
            {
                foreach (var item in request.CompleteProductionItems)
                {
                    var productionItem = production.ProductionItems?.FirstOrDefault(pi => pi.Id == item.ProductionItemId);
                    if (productionItem != null)
                    {
                        productionItem.ActualQuantity = item.ActualQuantity;
                        productionItem.ScrapQuantity = item.ScrapQuantity;

                        double totalDecreaseQuantity = (productionItem.ActualQuantity ?? 0) + (productionItem.ScrapQuantity ?? 0);

                        await _materialWarehouseRepository.DecreaseStockAsync(
                            productionItem.SourceWarehouseId,
                            productionItem.MaterialId,
                            totalDecreaseQuantity, 
                            await _parameterValueRepository.ParamValueToParamCode("MaterialStockTransactionType", "ProductionConsumption"), // üretim tüketimi için stok hareketi türü
                            cancellationToken);
                    }
                }
            }

            await _productWarehouseRepository.IncreaseStockAsync(
                production.TargetWarehouseId, 
                production.ProductId, 
                request.ActualQuantity, 
                await _parameterValueRepository.ParamValueToParamCode("MaterialStockTransactionType", "ProductionOutput"), // üretim çıktısı için stok hareketi türü
                cancellationToken);

            _productionRepository.Update(production);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(production.Id, 200, "Üretim emri başarıyla tamamlandı.");
        }
    }
}
