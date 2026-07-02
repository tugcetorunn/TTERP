using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // tüm değişiklikleri db ye tek seferde yazar.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
