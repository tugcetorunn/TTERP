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
        public ICollection<OrderItem>? OrderItems { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Task>? Tasks { get; set; }
        public int? OrderStatus { get; set; }
        public int? PaymentStatus { get; set; }
        public int? ShippingStatus { get; set; }
        public int Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public int? ConversationId { get; set; }
    }
}
