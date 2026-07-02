using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Application.Models.DTOs.Payments;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Orders
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public ICollection<GetOrderItemsDTO>? OrderItems { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public ICollection<GetPaymentsDTO>? Payments { get; set; }
        public int? OrderStatus { get; set; }
        public string? OrderStatusName { get; set; }
        public int? PaymentStatus { get; set; }
        public string? PaymentStatusName { get; set; }
        public int? ShippingStatus { get; set; }
        public string? ShippingStatusName { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public int? ConversationId { get; set; }
    }
}
