using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductionItems;

namespace TTERP.Application.Models.DTOs.Productions
{
    public class CompleteProductionDTO
    {
        public int ProductionId { get; set; }
        public double ActualQuantity { get; set; }
        public List<CompleteProductionItemDTO> CompleteProductionItems { get; set; }
    }
}
