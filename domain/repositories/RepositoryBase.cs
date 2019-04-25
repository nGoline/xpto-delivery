using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using domain.contexts;
using domain.entities;
using Microsoft.EntityFrameworkCore;

namespace domain.repositories
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        protected DbSet<TEntity> DbSet;
        protected DbContext Context;

        /// <summary>
        /// Repository constructor takes Context needed from IoC injection 
        /// </summary>
        /// <param name="context">DbContext object</param>
        public RepositoryBase(MainContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
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

        public virtual async Task<TEntity> FindByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<TEntity, object>> include = null)
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
        public virtual Task RemoveByAsync(Func<TEntity, bool> where)
        {
            DbSet.RemoveRange(DbSet.ToList().Where(where));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Queues the deletion of an entry on the table
        /// </summary>
        /// <param name="entity">Entry to be removed</param>
        /// <returns>Async Task</returns>
        public virtual Task RemoveAsync(TEntity entity)
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
            await Context.SaveChangesAsync().ConfigureAwait(false);
            Context.ChangeTracker.AcceptAllChanges();
        }
    }
}