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

        public CreateSupplyCommandHandler(ISupplyRepository supplyRepository, ISupplierMaterialRepository supplierMaterialRepository, IUnitOfWork unitOfWork, IMaterialWarehouseRepository materialWarehouseRepository, IHttpContextAccessor httpContextAccessor, IParameterValueRepository parameterValueRepository)
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
            var supply = request.Adapt<Supply>();

            var userIdClaims = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(userIdClaims, out int userId))
            {
                supply.EmployeeId = userId;
            }

            decimal totalAmount = 0m;

            var supplyItems = request.SupplyItems?.Adapt<List<SupplyItem>>();

            if (supplyItems != null && supplyItems.Any())
            {
                foreach (var item in supplyItems)
                {
                    var supplierMaterial = await _supplierMaterialRepository.GetBySupplierAndMaterialAsync(supply.SupplierId, item.MaterialId, cancellationToken);
                    if (supplierMaterial == null)
                    {
                        return Response<int>.Fail(404, $"İlgili tedarikçi malzeme kaydı bulunamadı.");
                    }

                    var materialWarehouse = await _materialWarehouseRepository.GetByMaterialAndWarehouseAsync(item.MaterialId, item.WarehouseId, cancellationToken);
                    if (materialWarehouse == null)
                    {
                        return Response<int>.Fail(404, $"{item.MaterialId} nolu malzeme, {item.WarehouseId} nolu depoda bulunmamaktadır.");
                    }

                    item.Currency = supplierMaterial.Currency;
                    item.ListPrice = supplierMaterial.ListPrice;
                    item.UnitPrice = supplierMaterial.UnitPrice;
                    item.TaxRate = supplierMaterial.Material!.TaxRate;
                    decimal taxAmount = (item.UnitPrice * (decimal)item.Quantity) * (item.TaxRate / 100);
                    item.TotalPrice = (item.UnitPrice * (decimal)item.Quantity) + taxAmount;

                    supply.SupplyItems!.Add(item);

                    totalAmount += item.TotalPrice;

                    if(supply.SupplyStatus == await _parameterValueRepository.ParamValueToParamCode("SupplyStatus", "Delivered", cancellationToken))
                    {
                        await _materialWarehouseRepository.IncreaseStockAsync(
                            item.MaterialId, 
                            item.WarehouseId, 
                            item.Quantity, 
                            await _parameterValueRepository.ParamValueToParamCode("MaterialStockTransactionType", "SupplyEntry"),
                            cancellationToken);
                    }
                }
            }

            supply.TotalAmount = totalAmount;

            await _supplyRepository.AddAsync(supply);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(supply.Id, 201, "Tedarik kaydı başarıyla oluşturuldu.");
        }
    }
}
