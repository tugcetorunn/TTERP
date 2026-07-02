using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class, IAuditableEntity
    {
        Task AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity); // sadece ef core da state değiştirdiği için void
        Task DeleteAsync(int id);
        Task<TEntity> FindAsync(int id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetListAsync();
        Task<IEnumerable<TResult>> GetListWithFilterAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> where, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
    }
}
