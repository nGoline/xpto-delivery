using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Tests.Fake
{
    public class FakeRouteRepository : IRouteRepository
    {
        private Dictionary<Guid, Route> _routes = new Dictionary<Guid, Route>();

        private IMapPointRepository _fakeMapPointRepository;

        public FakeRouteRepository(IMapPointRepository fakeMapPointRepository = null)
        {
            if (fakeMapPointRepository == null)
                fakeMapPointRepository = new FakeMapPointRepository();

            _fakeMapPointRepository = fakeMapPointRepository;
        }

        public async Task AddAsync(Route entity)
        {
            if (entity.Id == null || entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            if ((await _fakeMapPointRepository.GetByIdAsync(entity.FromId)) == null)
            {
                if (entity.From == null)
                    entity.From = ((FakeMapPointRepository)_fakeMapPointRepository).GetNewMapPoint(entity.FromId);

                await _fakeMapPointRepository.AddAsync(entity.From);
            }

            if ((await _fakeMapPointRepository.GetByIdAsync(entity.ToId)) == null)
            {
                if (entity.To == null)
                    entity.To = ((FakeMapPointRepository)_fakeMapPointRepository).GetNewMapPoint(entity.ToId);

                await _fakeMapPointRepository.AddAsync(entity.To);
            }

            _routes.Add(entity.Id, entity);
        }

        public Task DeleteAsync(Route entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByAsync(Func<Route, bool> where)
        {
            var routes = _routes.Values.Where(where).ToList();
            foreach (var route in routes)
                _routes.Remove(route.Id);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Route>> FindAsync(Expression<Func<Route, bool>> predicate, bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Route>> GetAllAsync(bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            var routes = new List<Route>();
            foreach (var route in _routes.Values)
            {
                route.From = await _fakeMapPointRepository.GetByIdAsync(route.FromId);
                route.To = await _fakeMapPointRepository.GetByIdAsync(route.ToId);
            }

            return routes;
        }

        public async Task<Route> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            if (!_routes.ContainsKey(id))
                return null;

            var route = _routes[id];
            if (route != null)
            {
                route.From = await _fakeMapPointRepository.GetByIdAsync(route.FromId);
                route.To = await _fakeMapPointRepository.GetByIdAsync(route.ToId);
            }

            return route;
        }

        public Task UpdateAsync(Route entity)
        {
            _routes[entity.Id] = entity;

            return Task.CompletedTask;
        }
    }
}