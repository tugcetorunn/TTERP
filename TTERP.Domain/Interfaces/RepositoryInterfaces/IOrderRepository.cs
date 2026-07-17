using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderWithOrderItemsAsync(int? orderId, CancellationToken cancellationToken);
        Task<Order?> GetOrderWithPaymentsAsync(int orderId, CancellationToken cancellationToken);
    }
}
