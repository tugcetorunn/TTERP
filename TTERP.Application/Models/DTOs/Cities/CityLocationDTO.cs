using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Cities
{
    public class CityLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
