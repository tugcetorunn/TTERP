using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>(); // bir order create edildiğinde order item da create edilecek, bu yüzden null olamaz. boş bir liste ile başlatıyoruz.
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Task>? Tasks { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
        public int? OrderStatus { get; set; }
        public int? PaymentStatus { get; set; }
        public int? ShippingStatus { get; set; }
        public int Currency { get; set; }
        public bool IsStockProcessed { get; set; }
        public DateTime? StockProcessedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal InvoicedAmount { get; set; }
        public bool CanTakePayment { get; set; }
        public bool CanCreateInvoice { get; set; }
        public bool CanChangeShipping { get; set; }
        public int? ConversationId { get; set; }
    }
}
