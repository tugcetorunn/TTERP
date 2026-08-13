using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Permission : BaseEntity<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Module { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
