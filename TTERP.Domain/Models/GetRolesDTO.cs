using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Models
{
    public class GetRolesDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameForUI { get; set; }
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
