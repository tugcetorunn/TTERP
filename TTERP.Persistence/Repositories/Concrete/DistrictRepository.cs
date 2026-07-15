using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class DistrictRepository : BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
