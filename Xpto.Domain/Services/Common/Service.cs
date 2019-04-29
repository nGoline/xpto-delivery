using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xpto.Domain.Interfaces.Repository.Common;
using Xpto.Domain.Interfaces.Service.Common;
using Xpto.Domain.Validation;
using Xpto.Domain.Interfaces.Validation;
using System.Threading.Tasks;
using Xpto.Domain.Entities.Common;

namespace Xpto.Domain.Services.Common
{
    public class Service<TEntity> : IService<TEntity>
        where TEntity : Entity
    {
        #region Constructor
        private readonly IRepository<TEntity> _repository;
        private readonly ValidationResult _validationResult;

        public Service(IRepository<TEntity> repository)
        {
            _repository = repository;
            _validationResult = new ValidationResult();
        }
        #endregion

        #region Properties
        protected IRepository<TEntity> Repository
        {
            get { return _repository; }
        }

        protected ValidationResult ValidationResult
        {
            get { return _validationResult; }
        }
        #endregion

        #region Read Methods
        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }
        #endregion

        #region CRUD Methods

        public virtual async Task<ValidationResult> AddAsync(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;


            await _repository.AddAsync(entity);
            return _validationResult;
        }

        public virtual async Task<ValidationResult> UpdateAsync(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;

            await _repository.UpdateAsync(entity);
            return _validationResult;
        }

        public virtual async Task<ValidationResult> DeleteAsync(TEntity entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            await _repository.DeleteAsync(entity);
            return _validationResult;
        }        

        public async Task DeleteByIdAsync(Guid id)
        {
            await Repository.DeleteByAsync(e => e.Id.Equals(id));
        }
        #endregion
    }
}