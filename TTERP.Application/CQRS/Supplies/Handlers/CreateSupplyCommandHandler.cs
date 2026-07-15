using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Supplies.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Handlers
{
    public class CreateSupplyCommandHandler : IRequestHandler<CreateSupplyCommand, Response<int>>
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly ISupplierMaterialRepository _supplierMaterialRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplyCommandHandler(
            ISupplyRepository supplyRepository,
            ISupplierMaterialRepository supplierMaterialRepository,
            IUnitOfWork unitOfWork,
            IMaterialWarehouseRepository materialWarehouseRepository,
            IHttpContextAccessor httpContextAccessor,
            IParameterValueRepository parameterValueRepository)
        {
            _supplyRepository = supplyRepository;
            _supplierMaterialRepository = supplierMaterialRepository;
            _unitOfWork = unitOfWork;
            _materialWarehouseRepository = materialWarehouseRepository;
            _httpContextAccessor = httpContextAccessor;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(CreateSupplyCommand request, CancellationToken cancellationToken)
        {
            if (!request.SupplierId.HasValue)
            {
                return Response<int>.Fail(400, "Tedarikçi seçilmelidir.");
            }

            if (request.SupplyItems == null || !request.SupplyItems.Any())
            {
                return Response<int>.Fail(400, "En az bir tedarik kalemi eklenmelidir.");
            }

            var supply = request.Adapt<Supply>();
            supply.SupplyItems = new List<SupplyItem>();

            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                supply.EmployeeId = userId;
            }

            var deliveredParamCode = await _parameterValueRepository.ParamValueToParamCode(
                "SupplyStatus",
                "Delivered",
                cancellationToken);

            var supplyEntryParamCode = await _parameterValueRepository.ParamValueToParamCode(
                "ReasonForEntryOrExit",
                "SupplyEntry",
                cancellationToken);

            decimal totalAmount = 0;

            foreach (var item in request.SupplyItems)
            {
                if (item.Quantity <= 0)
                {
                    return Response<int>.Fail(400, "Tedarik miktarı sıfırdan büyük olmalıdır.");
                }

                if (item.DiscountRate < 0 || item.DiscountRate > 100)
                {
                    return Response<int>.Fail(400, "İskonto oranı 0 ile 100 arasında olmalıdır.");
                }

                var supplierMaterial = await _supplierMaterialRepository.GetByIdWithDetailsAsync(
                    item.SupplierMaterialId,
                    cancellationToken);

                if (supplierMaterial == null)
                {
                    return Response<int>.Fail(404, "İlgili tedarikçi malzeme kaydı bulunamadı.");
                }

                if (supplierMaterial.SupplierId != request.SupplierId.Value)
                {
                    return Response<int>.Fail(
                        400,
                        $"{supplierMaterial.Material?.Name ?? "Seçilen malzeme"} bu tedarikçiye ait değildir.");
                }

                var unitPrice = item.UnitPrice.HasValue && item.UnitPrice.Value > 0
                    ? item.UnitPrice.Value
                    : supplierMaterial.UnitPrice;

                var taxRate = supplierMaterial.Material?.TaxRate ?? 0;
                var grossAmount = unitPrice * (decimal)item.Quantity;
                var discountAmount = grossAmount * (item.DiscountRate / 100);
                var netAmount = grossAmount - discountAmount;
                var taxAmount = netAmount * (taxRate / 100);
                var totalPrice = netAmount + taxAmount;

                var supplyItem = new SupplyItem
                {
                    SupplierMaterialId = supplierMaterial.Id,
                    MaterialId = supplierMaterial.MaterialId,
                    WarehouseId = item.WarehouseId,
                    Quantity = item.Quantity,
                    Currency = supplierMaterial.Currency,
                    ListPrice = supplierMaterial.ListPrice,
                    UnitPrice = unitPrice,
                    DiscountRate = item.DiscountRate,
                    TaxRate = taxRate,
                    NetAmount = netAmount,
                    TaxAmount = taxAmount,
                    TotalPrice = totalPrice
                };

                supply.SupplyItems.Add(supplyItem);
                totalAmount += totalPrice;

                if (supply.SupplyStatus == deliveredParamCode)
                {
                    await _materialWarehouseRepository.IncreaseStockAsync(
                        warehouseId: item.WarehouseId,
                        materialId: supplierMaterial.MaterialId,
                        quantity: item.Quantity,
                        reason: supplyEntryParamCode,
                        cancellationToken: cancellationToken);
                }
            }

            supply.TotalAmount = totalAmount;

            if (supply.SupplyStatus == deliveredParamCode)
            {
                supply.DeliveryDate = DateTime.UtcNow;
            }

            await _supplyRepository.AddAsync(supply);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                supply.Id,
                201,
                "Tedarik kaydı başarıyla oluşturuldu.");
        }
    }
}
