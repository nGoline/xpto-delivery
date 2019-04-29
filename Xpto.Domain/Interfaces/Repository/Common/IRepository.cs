using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;

namespace Xpto.Domain.Interfaces.Repository.Common
{
    public interface IRepository<TEntity>
      where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteByAsync(Func<TEntity, bool> where);
        Task<TEntity> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<TEntity, object>> include = null);
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, Expression<Func<TEntity, object>> include = null);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, Expression<Func<TEntity, object>> include = null);
    }
}