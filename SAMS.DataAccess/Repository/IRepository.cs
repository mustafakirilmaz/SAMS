using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace SAMS.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //DbSet<TEntity> Table { get; }
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate = null);
        Task Add(TEntity entity, CancellationToken cancellationToken = default);

        Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Delete(TEntity entity, bool forceHardDelete = false);

        void DeleteById(long id, bool forceHardDelete = false);

        void DeleteAll(IEnumerable<TEntity> entities, bool forceHardDelete = false);

        void Update(TEntity entity);

        void UpdateAll(IEnumerable<TEntity> entities);

        int Count();

        int Count(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetById(long id);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool trackingEnabled = false);

        Task<IPaginate<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 20, bool trackingEnabled = false, CancellationToken cancellationToken = default);

        IQueryable<TEntity> AsQueryable();

        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null);
    }
}
