using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using domain.entities;
using domain.repositories;

namespace domain.services
{
    public abstract class ServiceBase<TEntity>
        where TEntity : EntityBase
    {
        protected readonly RepositoryBase<TEntity> _repository;

        /// <summary>
        /// Service constructor takes Repositories needed from IoC injection
        /// </summary>
        /// <param name="repository">Repository object</param>
        public ServiceBase(RepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Add entity to database
        /// </summary>
        /// <param name="entity">Entity object to be added</param>
        /// <returns>Returns a Task</returns>
        public async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        /// <summary>
        /// Get all entries from the database
        /// </summary>
        /// <returns>Returns a collection of TEntity</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Find Entity by its Id
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <returns>Returns a database entry</returns>
        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        /// <summary>
        /// Removes a database entry by its Id
        /// </summary>
        /// <param name="id">Entry Id</param>
        /// <returns>returns a Task</returns>
        public async Task RemoveByIdAsync(Guid id)
        {
            await _repository.RemoveByAsync(x => x.Id.Equals(id));
            await _repository.SaveChangesAsync();
        }
    }
}