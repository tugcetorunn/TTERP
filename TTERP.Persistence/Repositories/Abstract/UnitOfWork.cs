using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;

namespace TTERP.Persistence.Repositories.Abstract
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // burada ef core kendi içinde bir transaction açar ve tüm add, update, delete işlemlerini tek seferde db ye yollar.
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
