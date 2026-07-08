using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Invoices
{
    public class GetInvoicesDTO
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int? OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountIncludingTax { get; set; }
        public decimal FinalAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
