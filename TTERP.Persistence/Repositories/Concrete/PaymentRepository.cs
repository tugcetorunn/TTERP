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
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<decimal> GetTotalPaidAmountByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await context.Payments
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .SumAsync(p => p.Amount, cancellationToken);
        }
    }
}
