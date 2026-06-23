using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Invoice : BaseEntity<int>
    {
        public string InvoiceNumber { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountIncludingTax { get; set; }
        public double FinalAmount { get; set; }
    }
}
