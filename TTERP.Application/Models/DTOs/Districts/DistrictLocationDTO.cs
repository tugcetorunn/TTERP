using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Districts
{
    public class DistrictLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TownId { get; set; }
    }
}
