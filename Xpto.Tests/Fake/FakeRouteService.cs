using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Validation;

namespace Xpto.Tests.Fake
{
    public class FakeRouteService : IRouteService
    {
        private ValidationResult _validationResult;
        private FakeRouteRepository _fakeRouteRepository;
        private IMapPointRepository _fakeMapPointRepository;

        public FakeRouteService(IMapPointRepository fakeMapPointRepository = null)
        {
            _fakeRouteRepository = new FakeRouteRepository(fakeMapPointRepository);
            _fakeMapPointRepository = fakeMapPointRepository;
        }

        public async Task<ValidationResult> AddAsync(Route entity)
        {
            if (!_validationResult.IsValid)
                return _validationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;

            await _fakeRouteRepository.AddAsync(entity);

            return _validationResult;
        }

        public Task<ValidationResult> DeleteAsync(Route entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Route>> FindAsync(Expression<Func<Route, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MapPoint>> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id)
        {
            var mapPointIdList = await _fakeRouteRepository.FindBestRouteAsync(mapPoint1Id, mapPoint2Id);
            var mapPointList = await _fakeMapPointRepository.FindAsync(x=>mapPointIdList.Contains(x.Id));

            return mapPointList.ToList();
        }

        public Task<IEnumerable<Route>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Route> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> UpdateAsync(Route entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(Guid routeId)
        {
            await _fakeRouteRepository.DeleteByAsync(mp => mp.Id.Equals(routeId));
        }
    }
}