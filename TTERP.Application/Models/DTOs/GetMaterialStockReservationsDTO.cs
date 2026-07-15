using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs
{
    public class GetMaterialStockReservationsDTO
    {
        public int ProductionId { get; set; }
        public int ProductionItemId { get; set; }
        public int MaterialId { get; set; }
        public int WarehouseId { get; set; }
        public double ReservedQuantity { get; set; }
        public double ConsumedQuantity { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReleasedDate { get; set; }
    }
}
