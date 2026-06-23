using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Entities.Common
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
