using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.MaterialWarehouses.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Handlers
{
    public class GetMaterialStockTimelineQueryHandler : IRequestHandler<GetMaterialStockTimelineQuery, Response<IReadOnlyList<GetMaterialStockTimelineDTO>>>
    {
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IMaterialStockReservationRepository _materialStockReservationRepository;

        public GetMaterialStockTimelineQueryHandler(IParameterValueRepository parameterValueRepository, IMaterialWarehouseRepository materialWarehouseRepository, IMaterialStockReservationRepository materialStockReservationRepository)
        {
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _materialStockReservationRepository = materialStockReservationRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialStockTimelineDTO>>> Handle(GetMaterialStockTimelineQuery request, CancellationToken cancellationToken)
        {
            var unitValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "MaterialUnit",
                                            1,
                                            cancellationToken);

            var unitDictionary = unitValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var movements = await _materialWarehouseRepository.GetListWithFilterAsync(
                 select: x => new
                 {
                     x.Id,
                     x.MaterialId,
                     MaterialName = x.Material != null
                        ? x.Material.Name
                        : null,
                     MaterialCode = x.Material != null
                        ? x.Material.Code
                        : null,
                     x.WarehouseId,
                     WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,
                     WarehouseCode = x.Warehouse != null
                        ? x.Warehouse.Code
                        : null,
                     x.Quantity,
                     x.Material!.Unit,
                     ReasonCode = x.ReasonForEntryOrExit,
                     TransactionDate = x.CreatedDate
                 },
                 where: x => x.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || x.IsActive == request.IsActive.Value) && (!request.MaterialId.HasValue ||
                         x.MaterialId == request.MaterialId.Value) && (!request.WarehouseId.HasValue || x.WarehouseId == request.WarehouseId.Value),
                 include: x => x.Include(x => x.Material)
                                .Include(x => x.Warehouse)!);

            var reservations = await _materialStockReservationRepository.GetListWithFilterAsync(
                select: x => new GetMaterialStockTimelineDTO
                {
                    RecordType = "Reservation",
                    RecordId = x.Id,
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material != null
                        ? x.Material.Name
                        : null,
                    MaterialCode = x.Material != null
                        ? x.Material.Code
                        : null,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse != null
                        ? x.Warehouse.Name
                        : null,
                    WarehouseCode = x.Warehouse != null
                        ? x.Warehouse.Code
                        : null,
                    Quantity = x.ReservedQuantity - x.ConsumedQuantity,
                    ReasonCode = null,
                    ReasonName = "Rezervasyon",
                    ProductionId = x.ProductionId,
                    TransactionDate = x.ReservationDate,
                    IsReservation = true,
                    IsReleased = x.IsReleased
                },
                where: x => x.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || x.IsActive == request.IsActive.Value) && (!request.MaterialId.HasValue ||
                         x.MaterialId == request.MaterialId.Value) && (!request.WarehouseId.HasValue || x.WarehouseId == request.WarehouseId.Value),
                include: x => x.Include(x => x.Material)
                               .Include(x => x.Warehouse)!);

            var reasonValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "ReasonForEntryOrExit",
                                            1,
                                            cancellationToken);

            var reasonDictionary = reasonValues.Where(x => x != null)
                                               .ToDictionary(
                                                   x => x!.ParamCode,
                                                   x => x!.ParamValue);

            var movementDtos = movements.Select(x => new GetMaterialStockTimelineDTO
                {
                    RecordType = "Movement",
                    RecordId = x.Id,
                    MaterialId = x.MaterialId,
                    MaterialName = x.MaterialName,
                    MaterialCode = x.MaterialCode,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.WarehouseName,
                    WarehouseCode = x.WarehouseCode,
                    Quantity = x.Quantity,
                    MaterialUnit = x.Unit,
                    MaterialUnitName = x.Unit.HasValue && unitDictionary.TryGetValue(x.Unit.Value, out var materialUnitName) ? materialUnitName : null,
                    ReasonCode = x.ReasonCode,
                    ReasonName =
                        x.ReasonCode.HasValue &&
                        reasonDictionary.TryGetValue(
                            x.ReasonCode.Value,
                            out var reasonName)
                                ? reasonName
                                : null,
                    ProductionId = null,
                    TransactionDate = x.TransactionDate,
                    IsReservation = false,
                    IsReleased = false
                })
                .ToList();

            var result = movementDtos.Concat(reservations)
                                     .OrderByDescending(x => x.TransactionDate)
                                     .ToList();

            return Response<IReadOnlyList<GetMaterialStockTimelineDTO>>.Success(result);
        }
    }
}
