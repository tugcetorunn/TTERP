using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Roles
{
    public class GetRolesDTO
    {
        public int Id { get; set; }
        public string NameForUI { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
