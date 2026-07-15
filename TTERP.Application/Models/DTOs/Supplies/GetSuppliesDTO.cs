using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Supplies
{
    public class GetSuppliesDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SupplyDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? SupplyStatus { get; set; }
        public string? SupplyStatusName { get; set; }
        public string? SupplyStatusShortCode { get; set; }
        public string? SupplyStatusBadgeColor { get; set; }
        public string? SupplyStatusIcon { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public ICollection<GetSupplyItemsDTO> SupplyItems { get; set; } = new List<GetSupplyItemsDTO>();
        public ICollection<AllowedWorkflowTransitionDTO>? AllowedTransitions { get; set; } = new List<AllowedWorkflowTransitionDTO>();
        public WorkflowActionPermissionsDTO Actions { get; set; } = new WorkflowActionPermissionsDTO();
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
