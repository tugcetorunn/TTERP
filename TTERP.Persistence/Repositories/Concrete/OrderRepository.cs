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
            return await context.Orders
                                .Include(order => order.Customer)
                                .Include(order => order.Employee)
                                .Include(order => order.OrderItems)!
                                    .ThenInclude(item => item.Product)
                                .Include(order => order.OrderItems)!
                                    .ThenInclude(item => item.OrderItemWarehouses)!
                                        .ThenInclude(allocation => allocation.Warehouse)
                                .FirstOrDefaultAsync(
                                    order => order.Id == orderId &&
                                             order.IsActive &&
                                             !order.IsDeleted, cancellationToken);
        }
    }
}
