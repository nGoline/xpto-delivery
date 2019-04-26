using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Validation;

namespace Xpto.Tests.Fake
{
    public class FakeRouteService : IRouteService
    {
        private ValidationResult _validationResult;
        private List<Route> _routes = new List<Route>();
        public Task<ValidationResult> AddAsync(Route entity)
        {
            if (!_validationResult.IsValid)
                return Task.FromResult(_validationResult);

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return Task.FromResult(selfValidationEntity.ValidationResult);

            _routes.Add(entity);
            return Task.FromResult(_validationResult);
        }

        public Task CreateRoutesUsingNewPoint(IEnumerable<MapPoint> mapPoints, MapPoint newMapPoint)
        {
            foreach (var mapPoint in mapPoints)
            {
                var route = new Route(mapPoint.Id, newMapPoint.Id)
                {
                    Cost = mapPoint.Coordinate.GetDistanceTo(newMapPoint.Coordinate)
                };

                _routes.Add(route);
            }

            return Task.CompletedTask;
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