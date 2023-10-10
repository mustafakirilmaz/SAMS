using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SAMS.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        //public DbSet<TEntity> Table => _dbSet;
        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null) query = query.Where(predicate);

            return await query.AnyAsync();
        }

        public Task Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddAsync(entity, cancellationToken).AsTask();
        }

        public Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(TEntity entity, bool forceHardDelete = false)
        {
            var dbEntityEntry = _dbContext.Entry(entity);

            var property = entity.GetType().GetProperty("IsDeleted");
            if (!forceHardDelete && property != null)
            {
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    throw new ArgumentNullException("Entity tracking pasif durumda. Sorguya 'disableTracking: false' ekleyerek tekrar deneyiniz.");
                }
                else
                {
                    property.SetValue(entity, true);
                    dbEntityEntry.State = EntityState.Modified;
                    Update(entity);
                }
            }
            else
            {
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }

        public void DeleteById(long id, bool forceHardDelete = false)
        {
            var entity = _dbSet.Find(id);
            Delete(entity, forceHardDelete);
        }

        public void DeleteAll(IEnumerable<TEntity> entities, bool forceHardDelete = false)
        {
            foreach (var entity in entities)
            {
                Delete(entity, forceHardDelete);
            }
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateAll(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.Where(filter).Count();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IPaginate<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 20, bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
            {
                return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
            }

            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable<TEntity>();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null) predicate = x => true;
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
