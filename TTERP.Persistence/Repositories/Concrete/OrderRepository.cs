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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<Order?> GetOrderWithOrderItemsAsync(int? orderId, CancellationToken cancellationToken)
        {
            return await context.Set<Order>()
                                .Include(o => o.OrderItems)
                                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted && o.IsActive, cancellationToken);
        }
    }
}
