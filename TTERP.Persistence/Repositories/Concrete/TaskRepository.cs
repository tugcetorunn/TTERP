using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Repositories.Abstract;
using TTERP.Persistence.Contexts;
using TTERP.Domain.Entities;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
