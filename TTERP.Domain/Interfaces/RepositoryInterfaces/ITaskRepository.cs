using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = TTERP.Domain.Entities.Task;

namespace TTERP.Domain.Interfaces
{
    public interface ITaskRepository : IBaseRepository<Task>
    {
    }
}
