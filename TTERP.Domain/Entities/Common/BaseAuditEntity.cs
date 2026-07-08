using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Entities.Common
{
    public abstract class BaseAuditEntity : IAuditableEntity
    {
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }
        public int? CreatedBy { get; private set; }
        public int? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedDate { get; private set; }
        public int? DeletedBy { get; private set; }

        protected BaseAuditEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

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
