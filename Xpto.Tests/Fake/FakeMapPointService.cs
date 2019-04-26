using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Validation;
using Xpto.Domain.Interfaces.Validation;

namespace Xpto.Tests.Fake
{
    public class FakeMapPointService : IMapPointService
    {
        private ValidationResult _validationResult;
        private FakeMapPointRepository _fakeMapPointRepository = new FakeMapPointRepository();

        public FakeMapPointService()
        {
            _validationResult = new ValidationResult();
        }

        public async Task<ValidationResult> AddAsync(MapPoint entity)
        {            
            if (!_validationResult.IsValid)
                return _validationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;
            
            await _fakeMapPointRepository.AddAsync(entity);
            return _validationResult;
        }

        public Task<ValidationResult> DeleteAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(Guid mapPointId)
        {
            await _fakeMapPointRepository.DeleteByAsync(mp => mp.Id.Equals(mapPointId));
        }

        public Task<IEnumerable<MapPoint>> FindAsync(Expression<Func<MapPoint, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapPoint>> GetAllAsync()
        {
            return _fakeMapPointRepository.GetAllAsync();
        }

        public Task<MapPoint> GetByIdAsync(Guid id)
        {
            return _fakeMapPointRepository.GetByIdAsync(id);
        }

        public Task<ValidationResult> UpdateAsync(MapPoint department)
        {
            throw new NotImplementedException();
        }
    }
}