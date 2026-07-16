using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Announcements.Commands;
using TTERP.Application.CQRS.Orders.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<int>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IProductWarehouseRepository productWarehouseRepository, IHttpContextAccessor httpContextAccessor, IParameterValueRepository parameterValueRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _httpContextAccessor = httpContextAccessor;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.CustomerId <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Müşteri seçilmelidir.");
            }

            if (request.OrderItems == null ||
                !request.OrderItems.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş için en az bir ürün kalemi eklenmelidir.");
            }

            if (request.Discount < 0)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş indirimi negatif olamaz.");
            }

            var draftStatus = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "OrderStatus",
                    "draft",
                    1,
                    cancellationToken);

            var unpaidStatus = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "PaymentStatus",
                    "unpaid",
                    1,
                    cancellationToken);

            var pendingShippingStatus = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "ShippingStatus",
                    "pending",
                    1,
                    cancellationToken);

            if (draftStatus == null ||
                unpaidStatus == null ||
                pendingShippingStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Sipariş başlangıç parametreleri tanımlanmamış.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int? employeeId = null;

            if (int.TryParse(userIdClaim, out var parsedEmployeeId))
            {
                employeeId = parsedEmployeeId;
            }

            var order = new Order
            {
                OrderDate = request.OrderDate,
                CustomerId = request.CustomerId,
                EmployeeId = employeeId,
                Discount = request.Discount,
                OrderStatus = draftStatus.ParamCode,
                PaymentStatus = unpaidStatus.ParamCode,
                ShippingStatus = pendingShippingStatus.ParamCode,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0m;
            int? orderCurrency = null;

            foreach (var itemRequest in request.OrderItems)
            {
                if (itemRequest.ProductId <= 0)
                {
                    return Response<int>.Fail(
                        400,
                        "Sipariş kaleminde ürün seçilmelidir.");
                }

                if (itemRequest.Quantity <= 0)
                {
                    return Response<int>.Fail(
                        400,
                        "Sipariş miktarı sıfırdan büyük olmalıdır.");
                }

                if (itemRequest.Discount < 0)
                {
                    return Response<int>.Fail(
                        400,
                        "Sipariş kalemi indirimi negatif olamaz.");
                }

                var product = await _productRepository.FindAsync(itemRequest.ProductId);

                if (product == null)
                {
                    return Response<int>.Fail(
                        404,
                        $"Ürün bulunamadı. Ürün ID: {itemRequest.ProductId}");
                }

                if (orderCurrency.HasValue &&
                    orderCurrency.Value != product.Currency)
                {
                    return Response<int>.Fail(
                        400,
                        "Aynı siparişte farklı para birimindeki ürünler kullanılamaz.");
                }

                orderCurrency ??= product.Currency;

                var unitPrice =
                    itemRequest.UnitPrice > 0
                        ? itemRequest.UnitPrice
                        : product.Price;

                var grossAmount =
                    unitPrice * (decimal)itemRequest.Quantity;

                if (itemRequest.Discount > grossAmount)
                {
                    return Response<int>.Fail(
                        400,
                        $"{product.Name} ürünü için indirim, brüt tutarı aşamaz.");
                }

                var netAmount =
                    grossAmount - itemRequest.Discount;

                var taxAmount =
                    netAmount * (product.TaxRate / 100m);

                var totalPrice =
                    netAmount + taxAmount;

                if (itemRequest.StockAllocations == null ||
                    !itemRequest.StockAllocations.Any())
                {
                    return Response<int>.Fail(
                        400,
                        $"{product.Name} ürünü için en az bir depo dağılımı girilmelidir.");
                }

                var allocatedQuantity =
                    itemRequest.StockAllocations.Sum(
                        allocation =>
                            allocation.QuantityFromWarehouse);

                if (Math.Abs(
                        allocatedQuantity -
                        itemRequest.Quantity) >
                    0.000001)
                {
                    return Response<int>.Fail(
                        400,
                        $"{product.Name} ürünü için depo dağılımlarının toplamı sipariş miktarına eşit olmalıdır.");
                }

                var duplicatedWarehouseIds =
                    itemRequest.StockAllocations
                        .GroupBy(allocation =>
                            allocation.WarehouseId)
                        .Where(group =>
                            group.Count() > 1)
                        .Select(group =>
                            group.Key)
                        .ToList();

                if (duplicatedWarehouseIds.Any())
                {
                    return Response<int>.Fail(
                        400,
                        $"{product.Name} ürünü için aynı depo birden fazla kez seçilemez.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = unitPrice,
                    Discount = itemRequest.Discount,
                    TaxRate = product.TaxRate,
                    TotalPrice = totalPrice,
                    Currency = product.Currency,
                    OrderItemWarehouses =
                        new List<OrderItemWarehouse>()
                };

                foreach (var allocation in itemRequest.StockAllocations)
                {
                    if (allocation.WarehouseId <= 0)
                    {
                        return Response<int>.Fail(
                            400,
                            $"{product.Name} ürünü için depo seçimi geçersiz.");
                    }

                    if (allocation.QuantityFromWarehouse <= 0)
                    {
                        return Response<int>.Fail(
                            400,
                            $"{product.Name} ürünü için depodan karşılanacak miktar sıfırdan büyük olmalıdır.");
                    }

                    orderItem.OrderItemWarehouses.Add(
                        new OrderItemWarehouse
                        {
                            WarehouseId =
                                allocation.WarehouseId,
                            Quantity =
                                allocation.QuantityFromWarehouse
                        });
                }

                order.OrderItems.Add(orderItem);
                totalAmount += totalPrice;
            }

            if (!orderCurrency.HasValue)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş para birimi belirlenemedi.");
            }

            if (request.Discount > totalAmount)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş indirimi toplam sipariş tutarını aşamaz.");
            }

            order.Currency = orderCurrency.Value;
            order.TotalAmount = totalAmount;
            order.FinalAmount =
                totalAmount - request.Discount;

            await _orderRepository.AddAsync(order); // db ye tek bir transaction ile ekliyoruz. OrderItem ve OrderItemWarehouse ekleme işlemleri burada yapılır. ayrı yapılırsa birden fazla saveChangesAsync çağrısı olur. ve stok kontrolü yanlış olur. çünkü stok kontrolü ve sipariş ekleme işlemleri aynı transaction içinde olmalı. aksi takdirde stok kontrolü geçerli olsa bile sipariş eklenemezse stok yanlış olur.
            await _unitOfWork.SaveChangesAsync(cancellationToken); // ef core; order, orderItem ve orderItemWarehouse tablolarının
                                                                   // tamamına otomatik id bağlamalarını yapar

            return Response<int>.Success(order.Id, 201, "Sipariş ve kalemleri başarıyla oluşturuldu.");
        }
    }
}
