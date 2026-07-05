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
            var order = request.Adapt<Order>();

            var userIdClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(userIdClaims, out int userId))
            {
                order.EmployeeId = userId;
            }

            decimal totalAmount = 0m;
            int orderCurrencyId = 0;

            if(order.OrderItems != null && order.OrderItems.Any())
            {
                orderCurrencyId = await _productRepository.GetCurrencyByProductId(order.OrderItems.First().ProductId, cancellationToken); // tüm ürünlerde aynı para birimi olmasını bekliyoruz.
                order.Currency = orderCurrencyId;

                foreach (var item in order.OrderItems)
                {
                    var taxRate = await _productRepository.GetTaxRateByProductId(item.ProductId);
                    
                    decimal subTotal = (item.UnitPrice * (decimal)item.Quantity) - item.Discount;
                    decimal taxAmount = subTotal * (taxRate / 100m);
                    decimal totalPrice = subTotal + taxAmount;

                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        TaxRate = taxRate,
                        TotalPrice = totalPrice,
                        Currency = orderCurrencyId,
                        OrderItemWarehouses = new List<OrderItemWarehouse>()
                    };

                    foreach (var allocation in item.OrderItemWarehouses!)
                    {
                        orderItem.OrderItemWarehouses.Add(new OrderItemWarehouse
                        {
                            WarehouseId = allocation.WarehouseId,
                            Quantity = allocation.Quantity
                        });

                        await _productWarehouseRepository.DecreaseStockAsync(
                            item.ProductId, 
                            allocation.WarehouseId, 
                            allocation.Quantity,  
                            await _parameterValueRepository.ParamValueToParamCode("ProductStockTransactionType", "Order", cancellationToken),
                            cancellationToken);
                    }

                    order.OrderItems.Add(orderItem); // sipariş kalemi siparişe eklenir

                    totalAmount += totalPrice; // sipariş genel toplamına sipariş kalemi toplam fiyatı eklenir 
                }
            }

            order.TotalAmount = totalAmount;
            order.FinalAmount = totalAmount - order.Discount;

            await _orderRepository.AddAsync(order); // db ye tek bir transaction ile ekliyoruz. OrderItem ve OrderItemWarehouse ekleme işlemleri burada yapılır. ayrı yapılırsa birden fazla saveChangesAsync çağrısı olur. ve stok kontrolü yanlış olur. çünkü stok kontrolü ve sipariş ekleme işlemleri aynı transaction içinde olmalı. aksi takdirde stok kontrolü geçerli olsa bile sipariş eklenemezse stok yanlış olur.
            await _unitOfWork.SaveChangesAsync(cancellationToken); // ef core; order, orderItem ve orderItemWarehouse tablolarının
                                                                   // tamamına otomatik id bağlamalarını yapar

            return Response<int>.Success(order.Id, 201, "Sipariş ve kalemleri başarıyla oluşturuldu.");
        }
    }
}
