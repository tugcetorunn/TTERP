using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Neighborhoods
{
    public class NeighborhoodLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int DistrictId { get; set; }
    }
}
