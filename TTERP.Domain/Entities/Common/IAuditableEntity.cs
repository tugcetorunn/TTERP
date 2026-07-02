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
        int? CreatedBy { get; }
        int? UpdatedBy { get; }
        int? DeletedBy { get; }
        bool IsActive { get; }
        bool IsDeleted { get; }

        void SetCreated(int? user);
        void SetUpdated(int? user);
        void SoftDelete(int? user);
    }
}
