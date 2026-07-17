using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Application.Models.DTOs.Payments;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Application.Models.DTOs.WorkflowHistories;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Orders
{
    public class GetOrdersDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int OrderStatus { get; set; }
        public string? OrderStatusName { get; set; }
        public string? OrderStatusShortCode { get; set; }
        public string? OrderStatusBadgeColor { get; set; }
        public string? OrderStatusIcon { get; set; }
        public int PaymentStatus { get; set; }
        public string? PaymentStatusName { get; set; }
        public int ShippingStatus { get; set; }
        public string? ShippingStatusName { get; set; }
        public int Currency { get; set; }
        public string? CurrencyName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal InvoicedAmount { get; set; }

        public bool CanTakePayment { get; set; }
        public bool CanCreateInvoice { get; set; }
        public bool CanChangeShipping { get; set; }

        public ICollection<AllowedWorkflowTransitionDTO> AllowedTransitions { get; set; } = new List<AllowedWorkflowTransitionDTO>();
        public ICollection<AllowedWorkflowTransitionDTO> AllowedShippingTransitions { get; set; } = new List<AllowedWorkflowTransitionDTO>();
        public WorkflowActionPermissionsDTO Actions { get; set; } = new();
        public ICollection<GetOrderItemsDTO> OrderItems { get; set; } = new List<GetOrderItemsDTO>();
        public ICollection<GetWorkflowHistoryDTO> WorkflowHistories { get; set; } = new List<GetWorkflowHistoryDTO>();
        public ICollection<GetPaymentsDTO> Payments { get; set; } = new List<GetPaymentsDTO>();
        public ICollection<GetInvoicesDTO> Invoices { get; set; } = new List<GetInvoicesDTO>();
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
