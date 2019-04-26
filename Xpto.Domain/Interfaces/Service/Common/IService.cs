using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Interfaces.Service.Common
{
    public interface IService<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ValidationResult> AddAsync(TEntity department);
        Task<ValidationResult> UpdateAsync(TEntity department);
        Task<ValidationResult> DeleteAsync(TEntity entity);
    }
}