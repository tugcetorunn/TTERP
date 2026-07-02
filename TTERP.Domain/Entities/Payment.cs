using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Payment : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? PaymentType { get; set; }
        public int? PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string? Note { get; set; }
    }
}
