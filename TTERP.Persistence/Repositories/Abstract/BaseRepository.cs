using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using TTERP.Domain.Entities.Common;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;

namespace TTERP.Persistence.Repositories.Abstract
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity<int>
    {
        protected readonly AppDbContext context;
        protected readonly DbSet<TEntity> table;
        public BaseRepository(AppDbContext _context)
        {
            context = _context;
            table = context.Set<TEntity>();
        }
        public async Task<TEntity> FindAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.SetCreated(entity.CreatedBy);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.SetUpdated(entity.UpdatedBy!);
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await table.Where(x => x.IsDeleted != true).Where(x => x.IsActive != false).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            entity.SoftDelete(entity.DeletedBy!);
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TResult>> GetListWithFilterAsync<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = table.AsNoTracking();

            if (where != null)
                query = query.Where(where);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }
    }
}
