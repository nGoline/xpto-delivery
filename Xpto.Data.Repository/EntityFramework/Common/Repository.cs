using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Domain.Entities.Common;
using Xpto.Domain.Interfaces.Repository.Common;

namespace Xpto.Data.Repository.EntityFramework.Common
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : Entity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected DbContext Context
        {
            get { return _dbContext; }
        }

        protected DbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }

        /// <summary>
        /// Gets all entries from the table
        /// </summary>
        /// <param name="asNoTracking">True if the changes made to the entities should not be tracked</param>
        /// <returns>IEnumuerable containing all entries from the tables</returns>        
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, Expression<Func<TEntity, object>> include = null)
        {
            if (asNoTracking)
            {
                if (include == null)
                    return await DbSet.AsNoTracking()
                                      .ToListAsync()
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Include(include)
                                      .AsNoTracking()
                                      .ToListAsync()
                                      .ConfigureAwait(false);
            }
            else
            {
                if (include == null)
                    return await DbSet.ToListAsync()
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Include(include)
                                      .ToListAsync()
                                      .ConfigureAwait(false);
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<TEntity, object>> include = null)
        {
            if (asNoTracking)
            {
                if (include == null)
                    return await DbSet.AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.Id.Equals(id))
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Include(include)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.Id.Equals(id))
                                      .ConfigureAwait(false);
            }
            else
            {
                if (include == null)
                    return await DbSet.FirstOrDefaultAsync(x => x.Id.Equals(id))
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Include(include)
                                      .FirstOrDefaultAsync(x => x.Id.Equals(id))
                                      .ConfigureAwait(false);
            }
        }


        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, Expression<Func<TEntity, object>> include = null)
        {
            if (asNoTracking)
            {
                if (include == null)
                    return await DbSet.AsNoTracking()
                                      .Where(predicate)
                                      .ToListAsync()
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Include(include)
                                      .AsNoTracking()
                                      .Where(predicate)
                                      .ToListAsync()
                                      .ConfigureAwait(false);
            }
            else
            {
                if (include == null)
                    return await DbSet.Where(predicate)
                                      .ToListAsync()
                                      .ConfigureAwait(false);
                else
                    return await DbSet.Where(predicate)
                                      .ToListAsync()
                                      .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Queues the insert of a new entry to the table
        /// </summary>
        /// <param name="entity">New entry to add</param>
        /// <returns>Async Task</returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Queues the update of an entry on the table
        /// </summary>
        /// <param name="entity">Entry to be updated</param>
        /// <returns>Async Task</returns>
        public virtual Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Queues the deletion of an entry on the table using a lambda delegate
        /// </summary>
        /// <param name="entity">New entry to add</param>
        /// <returns>Async Task</returns>
        public virtual Task DeleteByAsync(Func<TEntity, bool> where)
        {
            DbSet.RemoveRange(DbSet.ToList().Where(where));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Queues the deletion of an entry on the table
        /// </summary>
        /// <param name="entity">Entry to be removed</param>
        /// <returns>Async Task</returns>
        public virtual Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves all entry changes queued to the database
        /// </summary>
        /// <returns>Async Task</returns>
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            _dbContext.ChangeTracker.AcceptAllChanges();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Context == null) return;
            Context.Dispose();
        }
        #endregion
    }
}