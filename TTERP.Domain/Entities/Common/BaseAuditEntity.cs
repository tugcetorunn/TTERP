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
        public string CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedDate { get; private set; }
        public string? DeletedBy { get; private set; }

        protected BaseAuditEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public void SetCreated(string user)
        {
            CreatedBy = user;
        }

        public void SetUpdated(string user)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SoftDelete(string user)
        {
            IsDeleted = true;
            IsActive = false;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = user;
        }
    }
}
