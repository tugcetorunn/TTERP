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
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, Response<int>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IProductWarehouseRepository productWarehouseRepository, IParameterValueRepository parameterValueRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderId <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş bilgisi geçersiz.");
            }

            if (request.ProductId <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Ürün seçilmelidir.");
            }

            if (request.Quantity <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş miktarı sıfırdan büyük olmalıdır.");
            }

            if (request.Discount < 0)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş kalemi indirimi negatif olamaz.");
            }

            if (request.StockAllocations == null ||
                !request.StockAllocations.Any())
            {
                return Response<int>.Fail(
                    400,
                    "En az bir depo dağılımı girilmelidir.");
            }

            var order = await _orderRepository.GetOrderWithOrderItemsAsync(
                request.OrderId,
                cancellationToken);

            if (order == null)
            {
                return Response<int>.Fail(
                    404,
                    "Sipariş bulunamadı.");
            }

            if (!order.OrderStatus.HasValue)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş durum bilgisi bulunamadı.");
            }

            var draftStatus = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "OrderStatus",
                    "draft",
                    1,
                    cancellationToken);

            if (draftStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Taslak sipariş durumu tanımlanmamış.");
            }

            if (order.OrderStatus.Value != draftStatus.ParamCode)
            {
                return Response<int>.Fail(
                    400,
                    "Yalnızca taslak durumundaki siparişlere kalem eklenebilir.");
            }

            var product = await _productRepository.FindAsync(request.ProductId);

            if (product == null)
            {
                return Response<int>.Fail(
                    404,
                    "Ürün bulunamadı.");
            }

            if (order.Currency != product.Currency)
            {
                return Response<int>.Fail(
                    400,
                    "Eklenmek istenen ürünün para birimi sipariş para birimiyle aynı olmalıdır.");
            }

            var duplicatedWarehouseIds = request.StockAllocations
                .GroupBy(x => x.WarehouseId)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicatedWarehouseIds.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Aynı depo bir sipariş kaleminde birden fazla kez seçilemez.");
            }

            if (request.StockAllocations.Any(x =>
                    x.WarehouseId <= 0 ||
                    x.QuantityFromWarehouse <= 0))
            {
                return Response<int>.Fail(
                    400,
                    "Depo dağılımı bilgileri geçersiz.");
            }

            var allocatedQuantity = request.StockAllocations.Sum(
                x => x.QuantityFromWarehouse);

            if (Math.Abs(allocatedQuantity - request.Quantity) > 0.000001)
            {
                return Response<int>.Fail(
                    400,
                    "Depolardan karşılanacak miktarların toplamı sipariş miktarına eşit olmalıdır.");
            }

            var unitPrice = request.UnitPrice.HasValue &&
                            request.UnitPrice.Value > 0
                ? request.UnitPrice.Value
                : product.Price;

            var grossAmount = unitPrice * (decimal)request.Quantity;

            if (request.Discount > grossAmount)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş kalemi indirimi brüt tutarı aşamaz.");
            }

            var netAmount = grossAmount - request.Discount;
            var taxAmount = netAmount * (product.TaxRate / 100m);
            var totalPrice = netAmount + taxAmount;

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = unitPrice,
                Discount = request.Discount,
                TaxRate = product.TaxRate,
                TotalPrice = totalPrice,
                Currency = product.Currency,
                OrderItemWarehouses = request.StockAllocations
                    .Select(allocation => new OrderItemWarehouse
                    {
                        WarehouseId = allocation.WarehouseId,
                        Quantity = allocation.QuantityFromWarehouse
                    })
                    .ToList()
            };

            order.OrderItems ??= new List<OrderItem>();
            order.OrderItems.Add(orderItem);

            order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);

            if (order.Discount < 0)
            {
                order.Discount = 0;
            }

            if (order.Discount > order.TotalAmount)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş indirimi yeni sipariş toplamını aşamaz.");
            }

            order.FinalAmount = order.TotalAmount - order.Discount;

            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(orderItem.Id, 201, "Sipariş kalemi başarıyla eklendi.");
        }
    }
}
