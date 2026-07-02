using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ProductWarehouses
{
    public class CreateStockTransactionDTO
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public double Quantity { get; set; } // girilecek veya çıkılacak miktar (negatif değer de olabilir çıkış için)
        public int ReasonForEntryOrExit { get; set; }
    }
}
