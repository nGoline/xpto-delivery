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

        public FakeRouteService(IMapPointRepository _fakeMapPointRepository = null)
        {
            _fakeRouteRepository = new FakeRouteRepository(_fakeMapPointRepository);
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

        public async Task CreateRoutesUsingNewPoint(IEnumerable<MapPoint> mapPoints, MapPoint newMapPoint)
        {
            foreach (var mapPoint in mapPoints)
            {
                var route = new Route(mapPoint.Id, newMapPoint.Id)
                {
                    Cost = mapPoint.Coordinate.GetDistanceTo(newMapPoint.Coordinate)
                };

                await _fakeRouteRepository.AddAsync(route);
            }
        }

        public Task<ValidationResult> DeleteAsync(Route entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Route>> FindAsync(Expression<Func<Route, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Route>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Route> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByIdAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> UpdateAsync(Route entity)
        {
            throw new NotImplementedException();
        }
    }
}