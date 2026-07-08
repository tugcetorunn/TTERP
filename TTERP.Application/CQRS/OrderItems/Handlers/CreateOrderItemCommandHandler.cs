using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Handlers
{
    // mevcut bir sipaişe sonradan kalem eklemek için kullanılacak.
    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, Response<int>>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderItemCommandHandler(IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IProductWarehouseRepository productWarehouseRepository, IParameterValueRepository parameterValueRepository)
        {
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var orderItem = request.Adapt<OrderItem>();

            orderItem.TaxRate = await _productRepository.GetTaxRateByProductId(orderItem.ProductId, cancellationToken);
            orderItem.Currency = await _productRepository.GetCurrencyByProductId(orderItem.ProductId, cancellationToken);

            decimal subTotal = (orderItem.UnitPrice * (decimal)orderItem.Quantity) - orderItem.Discount;

            decimal taxAmount = subTotal * (orderItem.TaxRate / 100m);

            orderItem.TotalPrice = subTotal + taxAmount;

            foreach (var allocation in request.StockAllocations)
            {
                orderItem.OrderItemWarehouses!.Add(new OrderItemWarehouse
                {
                    WarehouseId = allocation.WarehouseId,
                    Quantity = allocation.QuantityFromWarehouse,
                });

                await _productWarehouseRepository.DecreaseStockAsync(
                    allocation.WarehouseId,
                    request.ProductId,
                    allocation.QuantityFromWarehouse,
                    await _parameterValueRepository.ParamValueToParamCode("ProductStockTransactionType", "Order", cancellationToken),
                    cancellationToken
                );
            }

            await _orderItemRepository.AddAsync(orderItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(orderItem.Id, 201, "Sipariş kalemi başarıyla oluşturuldu.");
        }
    }
}
