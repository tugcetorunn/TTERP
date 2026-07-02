using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class MaterialWarehouse : BaseEntity<int>
    {
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public double Quantity { get; set; } // a deposunda şuan kaç tane b malzemesi var sorusunun cevabı
        public int ReasonForEntryOrExit { get; set; } // parametreden
    }
}
