using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Roles
{
    public class GetRoleDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameForUI { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int UserCount { get; set; }
        public List<RolePermissionDTO> Permissions { get; set; } = new();
    }
}
