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
        private List<MapPoint> _mapPoints = new List<MapPoint>{
            new MapPoint("Test 1", 10, -10){ Id = Guid.NewGuid() },
            new MapPoint("Test 2", 11, -11){ Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200") },
            new MapPoint("Test 3", 13, -13){ Id = Guid.NewGuid() }
        };

        public FakeMapPointService()
        {
            _validationResult = new ValidationResult();
        }

        public Task<ValidationResult> AddAsync(MapPoint entity)
        {
             if (!_validationResult.IsValid)
                return Task.FromResult(_validationResult);

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return Task.FromResult(selfValidationEntity.ValidationResult);

            _mapPoints.Add(entity);
            return Task.FromResult(_validationResult);
        }

        public Task<ValidationResult> DeleteAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid mapPointId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapPoint>> FindAsync(Expression<Func<MapPoint, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapPoint>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<MapPoint>)_mapPoints);
        }

        public Task<MapPoint> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_mapPoints.SingleOrDefault(mp => mp.Id.Equals(id)));
        }

        public Task<ValidationResult> UpdateAsync(MapPoint department)
        {
            throw new NotImplementedException();
        }
    }
}