using Microsoft.EntityFrameworkCore;
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
    public class SupplyRepository : BaseRepository<Supply>, ISupplyRepository
    {
        public SupplyRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<Supply?> GetSupplyWithItemsAsync(int supplyId, CancellationToken cancellationToken)
        {
            return await context.Supplies
                .Include(s => s.SupplyItems)
                .FirstOrDefaultAsync(s => s.Id == supplyId && !s.IsDeleted && s.IsActive, cancellationToken);
        }
    }
}
