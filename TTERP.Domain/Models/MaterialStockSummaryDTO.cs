using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Models
{
    public class MaterialStockSummaryDTO
    {
        public double PhysicalStock { get; set; }
        public double ReservedStock { get; set; }
        public double AvailableStock => PhysicalStock - ReservedStock;
    }
}
