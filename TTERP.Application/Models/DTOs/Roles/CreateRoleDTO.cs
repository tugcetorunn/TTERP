using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Roles
{
    public class CreateRoleDTO
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string NameForUI { get; set; }
    }
}
