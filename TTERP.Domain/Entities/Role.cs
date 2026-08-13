using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Role : IdentityRole<int>, IAuditableEntity
    {
        public string NameForUI { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; private set; }
        public DateTime? DeletedDate { get; private set; }
        public int? CreatedBy { get; private set; }
        public int? UpdatedBy { get; private set; }
        public int? DeletedBy { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public void SetCreated(int? user)
        {
            CreatedBy = user;
        }

        public void SetUpdated(int? user)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SoftDelete(int? user)
        {
            IsDeleted = true;
            IsActive = false;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = user;
        }
    }
}
