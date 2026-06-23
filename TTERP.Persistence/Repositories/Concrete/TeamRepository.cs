using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
