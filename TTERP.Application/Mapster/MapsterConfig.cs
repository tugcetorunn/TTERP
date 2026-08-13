using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Commands;
using TTERP.Application.CQRS.ParameterValues.Commands;
using TTERP.Application.CQRS.ProductionItems.Commands;
using TTERP.Application.Models.DTOs.Customers;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Application.Models.DTOs.OrderItemWarehouses;
using TTERP.Application.Models.DTOs.Orders;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Application.Models.DTOs.ProductionProgresses;
using TTERP.Application.Models.DTOs.Productions;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Application.Models.DTOs.Titles;
using TTERP.Application.Models.DTOs.WorkflowHistories;
using TTERP.Domain.Entities;
using TTERP.Domain.Models;

namespace TTERP.Application.Mapster
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<ParameterValue, GetParameterValuesDTO>
                .NewConfig()
                .Map(dest => dest.ParamType, src => src.ParameterDefinition!.ParamType);

            TypeAdapterConfig<MaterialWarehouse, GetMaterialWarehousesDTO>
                .NewConfig()
                .Map(dest => dest.MaterialName,
                     src => src.Material != null ? src.Material.Name : string.Empty)
                .Map(dest => dest.MaterialCode,
                     src => src.Material != null ? src.Material.Code : string.Empty)
                .Map(dest => dest.MaterialUnit,
                     src => src.Material != null ? src.Material.Unit : 1)
                .Map(dest => dest.WarehouseName,
                     src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
                .Map(dest => dest.WarehouseCode,
                     src => src.Warehouse != null ? src.Warehouse.Code : string.Empty);

            TypeAdapterConfig<MaterialWarehouse, GetMaterialsStockDTO>
                .NewConfig()
                .Map(dest => dest.MaterialName,
                     src => src.Material != null ? src.Material.Name : string.Empty)
                .Map(dest => dest.MaterialCode,
                     src => src.Material != null ? src.Material.Code : string.Empty)
                .Map(dest => dest.MaterialUnit,
                     src => src.Material != null ? src.Material.Unit : 1)
                .Map(dest => dest.WarehouseName,
                     src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
                .Map(dest => dest.WarehouseCode,
                     src => src.Warehouse != null ? src.Warehouse.Code : string.Empty);

            TypeAdapterConfig<ProductWarehouse, GetProductWarehousesDTO>
                .NewConfig()
                .Map(dest => dest.ProductName,
                     src => src.Product != null ? src.Product.Name : string.Empty)
                .Map(dest => dest.ProductCode,
                     src => src.Product != null ? src.Product.Code : string.Empty)
                .Map(dest => dest.WarehouseName,
                     src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
                .Map(dest => dest.WarehouseCode,
                     src => src.Warehouse != null ? src.Warehouse.Code : string.Empty);

            TypeAdapterConfig<ProductWarehouse, GetProductsStockDTO>
                .NewConfig()
                .Map(dest => dest.ProductName,
                     src => src.Product != null ? src.Product.Name : string.Empty)
                .Map(dest => dest.ProductCode,
                     src => src.Product != null ? src.Product.Code : string.Empty)
                .Map(dest => dest.WarehouseName,
                     src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
                .Map(dest => dest.WarehouseCode,
                     src => src.Warehouse != null ? src.Warehouse.Code : string.Empty);

            TypeAdapterConfig<ProductWarehouse, GetProductToWarehousesDTO>
                .NewConfig()
                .Map(dest => dest.WarehouseName,
                     src => src.Warehouse != null ? src.Warehouse.Name : string.Empty)
                .Map(dest => dest.WarehouseCode,
                     src => src.Warehouse != null ? src.Warehouse.Code : string.Empty);

            TypeAdapterConfig<ProductWarehouse, GetWarehouseToProductsDTO>
                .NewConfig()
                .Map(dest => dest.ProductName,
                     src => src.Product != null ? src.Product.Name : string.Empty)
                .Map(dest => dest.ProductCode,
                     src => src.Product != null ? src.Product.Code : string.Empty);

            TypeAdapterConfig<ProductionItem, GetProductionItemsDTO>
                .NewConfig()
                .Map(dest => dest.PlannedQuantity, src => src.Quantity)
                .Map(dest => dest.MaterialName, src => src.Material != null ? src.Material.Name : null)
                .Map(dest => dest.MaterialCode, src => src.Material != null ? src.Material.Code : null)
                .Map(dest => dest.MaterialUnit, src => src.Material != null ? src.Material.Unit : null)
                .Map(dest => dest.SourceWarehouseName, src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : null)
                .Map(dest => dest.SourceWarehouseCode, src => src.SourceWarehouse != null ? src.SourceWarehouse.Code : null);

            TypeAdapterConfig<Production, GetProductionsDTO>
                .NewConfig()
                .Map(dest => dest.ActualQuantity, src => src.ActualQuantity)
                .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : null)
                .Map(dest => dest.ProductCode, src => src.Product != null ? src.Product.Code : null)
                .Map(dest => dest.TargetWarehouseName, src => src.TargetWarehouse != null ? src.TargetWarehouse.Name : null)
                .Map(dest => dest.TargetWarehouseCode, src => src.TargetWarehouse != null ? src.TargetWarehouse.Code : null)
                .Map(dest => dest.ProductionProgresses, src => src.ProductionProgresses)
                .Map(dest => dest.ProductionItems, src => src.ProductionItems);

            TypeAdapterConfig<PlanProductionItemCommand, ProductionItem>
                .NewConfig()
                .Map(dest => dest.Quantity, src => src.PlannedQuantity);

            TypeAdapterConfig<ProductionProgress, GetProductionProgressesDTO>
                .NewConfig()
                .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.FullName : null);

            TypeAdapterConfig<ProductionItem, GetProductionItemsDTO>
                .NewConfig()
                .Map(dest => dest.PlannedQuantity, src => src.Quantity)
                .Map(dest => dest.MaterialName, src => src.Material != null
                        ? src.Material.Name
                        : null)
                .Map(dest => dest.MaterialCode, src => src.Material != null
                        ? src.Material.Code
                        : null)
                .Map(dest => dest.MaterialUnit, src => src.Material != null
                        ? src.Material.Unit
                        : 0)
                .Map(dest => dest.SourceWarehouseName, src => src.SourceWarehouse != null
                        ? src.SourceWarehouse.Name
                        : null)
                .Map(dest => dest.SourceWarehouseCode, src => src.SourceWarehouse != null
                        ? src.SourceWarehouse.Code
                        : null)
                .Map(dest => dest.ReservedQuantity, src => src.MaterialStockReservations != null
                        ? src.MaterialStockReservations.Where(reservation => reservation.IsActive && !reservation.IsDeleted)
                                                       .Sum(reservation => reservation.ReservedQuantity)
                        : 0)
                .Map(dest => dest.ConsumedQuantity, src => src.MaterialStockReservations != null
                        ? src.MaterialStockReservations.Where(reservation => reservation.IsActive && !reservation.IsDeleted)
                                                       .Sum(reservation =>
                                                           reservation.ConsumedQuantity)
                        : 0)
                .Map(dest => dest.ReservationReleased, src => src.MaterialStockReservations != null &&
                           src.MaterialStockReservations.Any() &&
                           src.MaterialStockReservations.Where(reservation => reservation.IsActive && !reservation.IsDeleted)
                                                        .All(reservation => reservation.IsReleased));

            TypeAdapterConfig<WorkflowHistory, GetWorkflowHistoryDTO>
                .NewConfig()
                .Map(dest => dest.EmployeeName, src => src.Employee != null ? src.Employee.FullName : null);

            TypeAdapterConfig<SupplierMaterial, GetSupplierMaterialsDTO>
                .NewConfig()
                .Map(dest => dest.MaterialName,
                     src => src.Material != null ? src.Material.Name : string.Empty)
                .Map(dest => dest.MaterialCode,
                     src => src.Material != null ? src.Material.Code : string.Empty)
                .Map(dest => dest.MaterialUnit,
                     src => src.Material != null ? src.Material.Unit : 1)
                .Map(dest => dest.TaxRate, 
                     src => src.Material != null ? src.Material.TaxRate : 0)
                .Map(dest => dest.SupplierName,
                     src => src.Supplier != null ? src.Supplier.Name : string.Empty);

            TypeAdapterConfig<Supply, GetSuppliesDTO>
                .NewConfig()
                .Map(d => d.SupplierName, s => s.Supplier != null ? s.Supplier.Name : null)
                .Map(d => d.EmployeeName, s => s.Employee != null ? s.Employee.FullName : null)
                .Map(d => d.SupplyStatusName, s => s.SupplyStatus != null ? s.SupplyStatus : null)
                .Map(d => d.SupplyItems, s => s.SupplyItems);

            TypeAdapterConfig<SupplyItem, GetSupplyItemsDTO>
                .NewConfig()
                .Map(dest => dest.MaterialName,
                    src => src.Material != null ? src.Material.Name : null)
                .Map(dest => dest.MaterialCode,
                    src => src.Material != null ? src.Material.Code : null)
                .Map(dest => dest.MaterialUnit,
                    src => src.Material != null ? src.Material.Unit : null)
                .Map(dest => dest.WarehouseName,
                    src => src.Warehouse != null ? src.Warehouse.Name : null)
                .Map(dest => dest.WarehouseCode,
                    src => src.Warehouse != null ? src.Warehouse.Code : null);

            TypeAdapterConfig<Customer, GetCustomersDTO>
                .NewConfig()
                .Map(d => d.CompanyName, s => s.CompanyName != null ? s.CompanyName : s.FullName);

            TypeAdapterConfig<Order, GetOrdersDTO>
                .NewConfig()
                .Map(d => d.EmployeeName, s => s.Employee != null ? s.Employee.FullName : null)
                .Map(d => d.CustomerName, s => s.Customer != null ? s.Customer.CompanyName : s.Customer!.FullName);

            TypeAdapterConfig<OrderItem, GetOrderItemsDTO>
                .NewConfig()
                .Map(destination => destination.ProductName, source => source.Product != null
                        ? source.Product.Name
                        : null)
                .Map(destination => destination.ProductCode, source => source.Product != null
                        ? source.Product.Code
                        : null)
                .Map(destination => destination.StockLocations, source => source.OrderItemWarehouses);

            TypeAdapterConfig<OrderItemWarehouse, OrderItemStockLocationDTO>
                .NewConfig()
                .Map(destination => destination.OrderItemId, source => source.OrderItemId)
                .Map(destination => destination.WarehouseId, source => source.WarehouseId)
                .Map(destination => destination.WarehouseName, source => source.Warehouse != null
                        ? source.Warehouse.Name
                        : null)
                .Map(destination => destination.WarehouseCode, source => source.Warehouse != null
                        ? source.Warehouse.Code
                        : null)
                .Map(destination => destination.Quantity, source => source.Quantity);

            TypeAdapterConfig<OrderItem, AddOrderItemCommand>
                .NewConfig()
                .Map(d => d.StockAllocations, s => s.OrderItemWarehouses != null ? s.OrderItemWarehouses : null);

            TypeAdapterConfig<Title, GetTitlesDTO>
                .NewConfig()
                .Map(d => d.EmployeeCount, s => s.Employees != null ? s.Employees.Count(employee => employee.IsActive && !employee.IsDeleted) : 0); // işe yaramadı. sql çözemiyor.
        }
    }
}
