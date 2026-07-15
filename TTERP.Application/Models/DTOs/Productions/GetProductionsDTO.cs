using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Application.Models.DTOs.ProductionProgresses;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Application.Models.DTOs.WorkflowHistories;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Productions
{
    public class GetProductionsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public double PlannedQuantity { get; set; }
        public double? ActualQuantity { get; set; }
        public int TargetWarehouseId { get; set; } // üretilen ürün hangi depoya girecek
        public string? TargetWarehouseName { get; set; }
        public string? TargetWarehouseCode { get; set; }
        public int ProductionStatus { get; set; }
        public string ProductionStatusName { get; set; }
        public string? ProductionStatusShortCode { get; set; }
        public string? ProductionStatusBadgeColor { get; set; }
        public string? ProductionStatusIcon { get; set; }
        public DateTime ProductionDate { get; set; }
        public ICollection<AllowedWorkflowTransitionDTO>? AllowedTransitions { get; set; } = new List<AllowedWorkflowTransitionDTO>();
        public WorkflowActionPermissionsDTO Actions { get; set; } = new WorkflowActionPermissionsDTO();
        public ICollection<GetProductionItemsDTO>? ProductionItems { get; set; }
        public ICollection<GetProductionProgressesDTO>? ProductionProgresses { get; set; } = new List<GetProductionProgressesDTO>();
        public ICollection<GetWorkflowHistoryDTO>? WorkflowHistories { get; set; } = new List<GetWorkflowHistoryDTO>();
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
