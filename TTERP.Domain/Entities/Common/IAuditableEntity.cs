using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Entities.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; }
        DateTime? UpdatedDate { get; }
        DateTime? DeletedDate { get; }
        string CreatedBy { get; }
        string? UpdatedBy { get; }
        string? DeletedBy { get; }
        bool IsActive { get; }
        bool IsDeleted { get; }

        void SetCreated(string user);
        void SetUpdated(string user);
        void SoftDelete(string user);
    }
}
