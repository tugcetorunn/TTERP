using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ProductionItems
{
    public class CompleteProductionItemDTO
    {
        public int ProductionItemId { get; set; }
        public double ActualQuantity { get; set; }
        public double ScrapQuantity { get; set; }
    }
}
