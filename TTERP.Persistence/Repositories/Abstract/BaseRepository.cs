using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using TTERP.Domain.Entities.Common;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;

namespace TTERP.Persistence.Repositories.Abstract
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IAuditableEntity
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
            entity.SetCreated(entity.CreatedBy ?? 0); // 0 geçiyorsa System
            await context.AddAsync(entity);
            // UOW kullanıldığı için SaveChangesAsync çağrısı burada yapılmaz. UOW, tüm değişiklikleri tek bir işlem olarak kaydetmek için kullanılır. Transaction hatasız tamamlanması durumunda SaveChangesAsync çağrısı UOW tarafından yapılır.
        }

        public void Update(TEntity entity)
        {
            entity.SetUpdated(entity.UpdatedBy ?? 0);
            context.Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await table.Where(x => x.IsDeleted != true).Where(x => x.IsActive != false).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity != null)
            {
                entity.SoftDelete(entity.DeletedBy ?? 0);
                context.Update(entity);
            }
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

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }
    }
}
