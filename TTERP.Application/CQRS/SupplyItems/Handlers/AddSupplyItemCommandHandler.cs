using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.SupplyItems.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplyItems.Handlers
{
    public class AddSupplyItemCommandHandler : IRequestHandler<AddSupplyItemCommand, Response<int>>
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly ISupplierMaterialRepository _supplierMaterialRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddSupplyItemCommandHandler(ISupplyRepository supplyRepository, ISupplierMaterialRepository supplierMaterialRepository, IParameterValueRepository parameterValueRepository, IUnitOfWork unitOfWork)
        {
            _supplyRepository = supplyRepository;
            _supplierMaterialRepository = supplierMaterialRepository;
            _parameterValueRepository = parameterValueRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(AddSupplyItemCommand request, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetSupplyWithItemsAsync(request.SupplyId, cancellationToken);

            if (supply == null)
            {
                return Response<int>.Fail(
                    404,
                    "Tedarik kaydı bulunamadı.");
            }

            var deliveredParamCode = await _parameterValueRepository.ParamValueToParamCode("SupplyStatus", "Delivered", cancellationToken);

            if (supply.SupplyStatus == deliveredParamCode)
            {
                return Response<int>.Fail(
                    400,
                    "Teslim alınmış bir tedarik kaydına yeni kalem eklenemez.");
            }

            if (request.Quantity <= 0)
            {
                return Response<int>.Fail(400, "Tedarik miktarı sıfırdan büyük olmalıdır.");
            }

            if (request.DiscountRate < 0 || request.DiscountRate > 100)
            {
                return Response<int>.Fail(400, "İskonto oranı 0 ile 100 arasında olmalıdır.");
            }

            var supplierMaterial = await _supplierMaterialRepository.GetByIdWithDetailsAsync(request.SupplierMaterialId, cancellationToken);

            if (supplierMaterial == null)
            {
                return Response<int>.Fail(404, "Tedarikçi malzeme kaydı bulunamadı.");
            }

            if (supplierMaterial.SupplierId != supply.SupplierId)
            {
                return Response<int>.Fail(400, "Seçilen malzeme fiyat kaydı bu tedarikçiye ait değildir.");
            }

            var unitPrice = request.UnitPrice.HasValue && request.UnitPrice.Value > 0
                    ? request.UnitPrice.Value
                    : supplierMaterial.UnitPrice;

            var taxRate = supplierMaterial.Material?.TaxRate ?? 0;

            var grossAmount = unitPrice * (decimal)request.Quantity;

            var discountAmount = grossAmount * (request.DiscountRate / 100);

            var netAmount = grossAmount - discountAmount;

            var taxAmount = netAmount * (taxRate / 100);

            var totalPrice = netAmount + taxAmount;

            var supplyItem = new SupplyItem
            {
                SupplyId = supply.Id,
                SupplierMaterialId = supplierMaterial.Id,
                MaterialId = supplierMaterial.MaterialId,
                WarehouseId = request.WarehouseId,
                Quantity = request.Quantity,
                Currency = supplierMaterial.Currency,
                ListPrice = supplierMaterial.ListPrice,
                UnitPrice = unitPrice,
                DiscountRate = request.DiscountRate,
                TaxRate = taxRate,
                NetAmount = netAmount,
                TaxAmount = taxAmount,
                TotalPrice = totalPrice
            };

            supply.SupplyItems ??= new List<SupplyItem>();

            supply.SupplyItems.Add(supplyItem);

            supply.TotalAmount = supply.SupplyItems.Sum(item => item.TotalPrice);

            _supplyRepository.Update(supply);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(supplyItem.Id, 201, "Tedarik kalemi başarıyla eklendi.");
        }
    }
}
